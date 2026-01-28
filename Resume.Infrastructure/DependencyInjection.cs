using Microsoft.Extensions.DependencyInjection;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;
using Resume.Infrastructure.Repositories;

namespace Resume.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Método de extensión para agregar servicios de infraestructura al contenedor de inyección de dependencias
    /// </summary>
    /// <param name="services">El contenedor de servicios de inyección de dependencias</param>
    /// <returns>El contenedor de servicios de inyección de dependencias con los servicios de infraestructura agregados</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // TO DO: Agregar servicios al contenedor IoC
        // Los servicios de infraestructura a menudo incluyen acceso a datos, almacenamiento en caché y otros componentes de bajo nivel.

        services.AddTransient<IResumeRepository, ResumeRepository>();
        services.AddTransient<IPersonalInfoRepository, PersonalInfoRepository>();
        services.AddTransient<IProfessionalResumeRepository, ProfessionalResumeRepository>();
        services.AddTransient<ISportsResumeRepository, SportsResumeRepository>();
        services.AddTransient<IInternshipResumeRepository, InternshipResumeRepository>();
        services.AddTransient<IWorkExperienceRepository, WorkExperienceRepository>();
        services.AddTransient<ISoftSkillRepository, SoftSkillRepository>();
        services.AddTransient<ITechnicalSkillRepository, TechnicalSkillRepository>();
        services.AddTransient<ILanguageRepository, LanguageRepository>();
        services.AddTransient<IAcademicEducationRepository, AcademicEducationRepository>();
        services.AddTransient<IInadehCourseRepository, InadehCourseRepository>();
        services.AddTransient<IInternshipTypeRepository, InternshipTypeRepository>();
        services.AddTransient<IResumeDocumentRepository, ResumeDocumentRepository>();
        services.AddTransient<IDocumentTypeRepository, DocumentTypeRepository>();
        services.AddTransient<IDisabilityTypeRepository, DisabilityTypeRepository>();
        services.AddTransient<IGenderRepository, GenderRepository>();
        services.AddTransient<ILanguageCatalogRepository, LanguageCatalogRepository>();
        services.AddTransient<ISkillCatalogRepository, SkillCatalogRepository>();
        services.AddTransient<IProfessionalSkillRepository, ProfessionalSkillRepository>();
        services.AddTransient<IApiLogRepository, ApiLogRepository>();
        services.AddTransient<IScheduleCounterRepository, ScheduleCounterRepository>();
        services.AddTransient<IScheduleRepository, ScheduleRepository>();
        services.AddTransient<IEventResumeRepository, EventResumeRepository>();

        services.AddTransient<ResumeDbContext>();
        services.AddTransient<RecruitmentSharedDbContext>();
        services.AddTransient<CompanyRecruitmentDbContext>();

        return services;
    }
}
