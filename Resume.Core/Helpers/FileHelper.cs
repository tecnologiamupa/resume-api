using Microsoft.AspNetCore.Http;
using Renci.SshNet;

namespace Resume.Core.Helpers;

public static class FileHelper
{
    /// <summary>
    /// Guarda un archivo en una carpeta local.
    /// </summary>
    /// <param name="file">El archivo a subir.</param>
    /// <param name="localPath">La ruta local donde se guardará el archivo.</param>
    /// <returns>La ruta del archivo en la carpeta local.</returns>
    /// <exception cref="ArgumentException">Se lanza si el archivo de imagen no es proporcionado.</exception>
    /// <exception cref="Exception">Se lanza si ocurre un error al subir el archivo.</exception>
    public static async Task<string> SaveFileToLocal(IFormFile file, string localPath)
    {
        // Verifica si el archivo es nulo o está vacío
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Archivo no proporcionado");
        }

        string originalName = Path.GetFileNameWithoutExtension(file.FileName);
        if (originalName.Length > 50) // Limita el nombre original a 50 caracteres
        {
            originalName = originalName.Substring(0, 50);
        }

        // Genera un nombre único para el archivo y construye la ruta completa en la carpeta local
        string shortGuid = Math.Abs(Guid.NewGuid().GetHashCode()).ToString();
        string fileName = $"{shortGuid}_{originalName}{Path.GetExtension(file.FileName)}";
        string localFilePath = Path.Combine(localPath, fileName);

        // Crea el directorio si no existe
        if (!Directory.Exists(localPath))
        {
            Directory.CreateDirectory(localPath);
        }

        // Guarda el archivo en la ruta local
        using (var fileStream = new FileStream(localFilePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return fileName;
    }
}
