namespace AdminCore.Constants.Enums
{
  public enum EmployeeRoles
  {
    [AzureRole("TeamLeader")]
    TeamLeader = 1,
    [AzureRole("Admin")]
    SystemAdministrator = 2,
    [AzureRole("Standard")]
    User = 3
  }
}
