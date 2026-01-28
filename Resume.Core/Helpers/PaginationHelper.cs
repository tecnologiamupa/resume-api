namespace Resume.Core.Helpers;

/// <summary>
/// Helper para manejar la lógica de paginación.
/// </summary>
public static class PaginationHelper
{
    /// <summary>
    /// Ajusta los valores de número de página y tamaño de página a valores válidos.
    /// </summary>
    /// <param name="pageNumber">Número de página.</param>
    /// <param name="pageSize">Tamaño de página.</param>
    /// <param name="defaultPageSize">Tamaño de página por defecto si no se especifica.</param>
    public static void NormalizePagination(ref int pageNumber, ref int pageSize, int defaultPageSize = 10)
    {
        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }

        if (pageSize <= 0)
        {
            pageSize = defaultPageSize;
        }
    }
}
