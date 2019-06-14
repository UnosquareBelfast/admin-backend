using AdminCore.DAL;

namespace AdminCore.Services.Base
{
  public abstract class BaseService
  {
    protected IDatabaseContext DatabaseContext { get; }

    protected BaseService(IDatabaseContext databaseContext)
    {
      DatabaseContext = databaseContext;
    }
  }
}
