using ChoETL;

namespace AdminCore.DataETL.Models
{
    [ChoCSVRecordObject(ErrorMode = ChoErrorMode.ThrowAndStop)]
    public class EmployeeChoEtl
    {
//        public int EmployeeId { get; set; }
//        public int CountryId { get; set; }
//        public string Email { get; set; }
//        public int EmployeeRoleId { get; set; }
//        public int EmployeeStatusId { get; set; }
        public string Forename { get; set; }
//        public DateTime StartDate { get; set; }
        public string Surname { get; set; }
//        public int TotalHolidays { get; set; }
//        public Country Country { get; set; }
//        public EmployeeRole EmployeeRole { get; set; }
//        public EmployeeStatus EmployeeStatus { get; set; }
//        public ICollection<Event> Events { get; set; }
//        public ICollection<Contract> Contracts { get; set; }

        public override string ToString()
        {
            return $"{Forename} {Surname}";
        }
    }
}