﻿using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Event
{
  public class HolidayStatsViewModel : ViewModel
  {
    public double AvailableHolidays { get; set; }

    public double PendingHolidays { get; set; }

    public double ApprovedHolidays { get; set; }

    public double TotalHolidays { get; set; }
  }
}