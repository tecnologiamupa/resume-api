using Microsoft.AspNetCore.Http;

namespace Resume.Core.ServiceContracts;

public interface ISftpFileService
{
    /// <summary>
    /// Sube un archivo al servidor SFTP.
    /// </summary>
    /// <param name="file">El archivo a subir.</param>
    /// <param name="remotePath">La ruta remota donde se subirá el archivo.</param>
    /// <returns>El nombre del archivo subido.</returns>
    Task<string> UploadFile(IFormFile file, string remotePath);

    /// <summary>
    /// Descarga un archivo desde el servidor SFTP.
    /// </summary>
    /// <param name="fileName">El nombre del archivo a descargar.</param>
    /// <returns>Un arreglo de bytes que representa el contenido del archivo.</returns>
    Task<byte[]> DownloadFile(string relativePath);
}
