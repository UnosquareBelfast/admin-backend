﻿using AdminCore.WebApi.Models.Base;
using System;

namespace AdminCore.WebApi.Models.Event
{
  public class CreatePublicHolidayViewModel : ViewModel
  {
    public int CountryId { get; set; }
    public DateTime Date { get; set; }
  }
}