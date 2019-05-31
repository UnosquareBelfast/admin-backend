using System;

namespace AdminCore.DTOs.LinkGenerator
{
    public class EventRequestDto
    {
        public int EventRequestId { get; set; }

        public int RequestTypeId { get; set; }

        public int EventDateId { get; set; }

        public string Salt { get; set; }

        public string Hash { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime TimeExpires { get; set; }

        public bool Expired { get; set; }

        public bool AutoApproved { get; set; }
    }
}
