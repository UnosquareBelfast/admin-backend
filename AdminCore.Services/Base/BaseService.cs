using AdminCore.DAL;

namespace AdminCore.Services.Base
{
  public abstract class BaseService
  {
    protected readonly IDatabaseContext DatabaseContext;

    protected BaseService(IDatabaseContext databaseContext)
    {
      DatabaseContext = databaseContext;
    }
  }
}