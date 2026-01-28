using Microsoft.AspNetCore.Http;

namespace Resume.Core.Helpers;

public static class FileValidationHelper
{
    public static bool IsValidFileContent(IFormFile file, string fileExtension)
    {
        try
        {
            using var stream = file.OpenReadStream();
            byte[] buffer = new byte[8]; // Leer los primeros 8 bytes
            stream.Read(buffer, 0, buffer.Length);

            // Validar PDF
            if (fileExtension == ".pdf")
            {
                string fileSignature = System.Text.Encoding.ASCII.GetString(buffer, 0, 4);
                return fileSignature.StartsWith("%PDF");
            }

            // Validar JPEG
            if (fileExtension == ".jpg" || fileExtension == ".jpeg")
            {
                return buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF;
            }

            // Validar PNG
            if (fileExtension == ".png")
            {
                return buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47 &&
                       buffer[4] == 0x0D && buffer[5] == 0x0A && buffer[6] == 0x1A && buffer[7] == 0x0A;
            }
        }
        catch
        {
            // Si ocurre un error al leer el archivo, asumimos que no es válido
            return false;
        }

        return false;
    }
}
