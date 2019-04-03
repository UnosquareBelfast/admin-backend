namespace AdminCore.DTOs
{
  public class ScheduleDto
  {
    public int ScheduleId { get; set; }

    public string ScheduleName { get; set; }

    public string ScheduleCronExpression { get; set; }

    public bool IsActive { get; set; }
  }
}