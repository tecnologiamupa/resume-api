using Microsoft.Extensions.DependencyInjection;
using Resume.Core.ServiceContracts;
using Resume.Core.Services;

namespace Resume.Core;

public static class DependencyInjection
{
    /// <summary>
    /// Método de extensión para agregar servicios principales al contenedor de inyección de dependencias.
    /// </summary>
    /// <param name="services">El contenedor de servicios al que se agregarán los servicios.</param>
    /// <returns>El contenedor de servicios con los servicios principales agregados.</returns>
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        // TO DO: Agregar servicios al contenedor IoC
        // Los servicios principales a menudo incluyen validación, almacenamiento en caché y otros componentes de negocio.

        services.AddTransient<IResumeService, ResumeService>();
        services.AddTransient<IPersonalInfoService, PersonalInfoService>();
        services.AddTransient<IProfessionalResumeService, ProfessionalResumeService>();
        services.AddTransient<ISportsResumeService, SportsResumeService>();
        services.AddTransient<IInternshipResumeService, InternshipResumeService>();
        services.AddTransient<IWorkExperienceService, WorkExperienceService>();
        services.AddTransient<ISoftSkillService, SoftSkillService>();
        services.AddTransient<ITechnicalSkillService, TechnicalSkillService>();
        services.AddTransient<ILanguageService, LanguageService>();
        services.AddTransient<IAcademicEducationService, AcademicEducationService>();
        services.AddTransient<IProfileImageService, ProfileImageService>();
        services.AddTransient<IInadehCourseService, InadehCourseService>();
        services.AddTransient<IInternshipTypeService, InternshipTypeService>();
        services.AddTransient<IResumeDocumentService, ResumeDocumentService>();
        services.AddTransient<ISftpFileService, SftpFileService>();
        services.AddTransient<IDocumentTypeService, DocumentTypeService>();
        services.AddTransient<IDisabilityTypeService, DisabilityTypeService>();
        services.AddTransient<IGenderService, GenderService>();
        services.AddTransient<ILanguageCatalogService, LanguageCatalogService>();
        services.AddTransient<ISkillCatalogService, SkillCatalogService>();
        services.AddTransient<IProfessionalSkillService, ProfessionalSkillService>();
        services.AddTransient<IScheduleService, ScheduleService>();
        services.AddTransient<IEventResumeService, EventResumeService>();

        return services;
    }
}
