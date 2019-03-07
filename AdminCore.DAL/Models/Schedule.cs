using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("scheduled_job")]
  public class Schedule
  {
    [Key]
    [Column("scheduled_job_id")]
    public int ScheduleJobId { get; set; }

    [Column("schedule_job_name")]
    public string ScheduleJobName { get; set; }

    [Column("schedule_cron_expression")]
    public string ScheduleCronExpression { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }
  }
}