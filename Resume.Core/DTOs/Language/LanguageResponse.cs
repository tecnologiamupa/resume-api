namespace Resume.Core.DTOs;

/// <summary>
/// Representa una respuesta que contiene información sobre un idioma.
/// </summary>
public class LanguageResponse
{
    public string? Name { get; set; }
    public LanguageCatalogResponse? LanguageCatalog { get; set; } = null;
}