using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using Renci.SshNet.Common;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

internal class SftpFileService : ISftpFileService
{
    private readonly string _sftpHost;
    private readonly int _sftpPort;
    private readonly string _username;
    private readonly string _password;
    private readonly string _basePath;

    public SftpFileService(IConfiguration configuration)
    {
        _sftpHost = configuration["Sftp:Host"] ?? throw new ArgumentNullException("Sftp:Host");
        _sftpPort = int.Parse(configuration["Sftp:Port"] ?? "22");
        _username = configuration["Sftp:Username"] ?? throw new ArgumentNullException("Sftp:Username");
        _password = configuration["Sftp:Password"] ?? throw new ArgumentNullException("Sftp:Password");
        _basePath = configuration["Sftp:BasePath"] ?? "/var/uploads/resume";
    }

    public async Task<string> UploadFile(IFormFile file, string relativePath)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Archivo no proporcionado");

        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        string remoteFilePath = $"{_basePath}/{relativePath}/{fileName}";

        SftpClient sftp = null;
        try
        {
            sftp = new SftpClient(_sftpHost, _sftpPort, _username, _password);
            sftp.Connect();

            string fullRemotePath = $"{_basePath}/{relativePath}";
            if (!sftp.Exists(fullRemotePath))
                sftp.CreateDirectory(fullRemotePath);

            using var stream = file.OpenReadStream();
            sftp.UploadFile(stream, remoteFilePath);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al subir el archivo al SFTP: {ex.Message}", ex);
        }
        finally
        {
            if (sftp.IsConnected)
                sftp.Disconnect();
        }

        return $"{relativePath}/{fileName}";
    }

    public async Task<byte[]> DownloadFile(string relativePath)
    {
        string decodedRelativePath = Uri.UnescapeDataString(relativePath);
        string remoteFilePath = $"{_basePath}/{decodedRelativePath}";

        using var sftp = new SftpClient(_sftpHost, _sftpPort, _username, _password);
        try
        {
            sftp.Connect();

            if (!sftp.Exists(remoteFilePath))
                throw new FileNotFoundException("Archivo no encontrado en el servidor SFTP.");

            using var memoryStream = new MemoryStream();
            sftp.DownloadFile(remoteFilePath, memoryStream);
            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inesperado al descargar el archivo: {ex.Message}", ex);
        }
        finally
        {
            if (sftp.IsConnected)
                sftp.Disconnect();
        }
    }
}
