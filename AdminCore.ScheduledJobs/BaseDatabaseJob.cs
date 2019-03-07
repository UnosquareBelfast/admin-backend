using AdminCore.DAL;

namespace AdminCore.ScheduledJobs
{
  public class BaseDatabaseJob
  {
    public IDatabaseContext DatabaseContext { get; set; }
  }
}