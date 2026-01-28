using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

public interface IApiLogRepository
{
    Task InsertApiLog(ApiLog logEntry);
}
