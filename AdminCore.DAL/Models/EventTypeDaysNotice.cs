using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("event_type_days_notice")]
  public class EventTypeDaysNotice
  {
    [Key]
    [Column("event_type_days_notice_id")]
    public int EventDaysNoticeId { get; set; }
   
    [Column("leave_length_days")]
    public virtual int LeaveLengthDays { get; set; }
    
    [Column("days_notice")]
    public virtual int DaysNotice { get; set; }
   
    [Column("time_notice")]
    public virtual TimeSpan? TimeNotice { get; set; }
    
    [Column("event_type_id")]
    public virtual int EventTypeId { get; set; }
    public virtual EventType EventType { get; set; }
  }
}