namespace BumboApp.Models
{
    public enum Department
    {
        Vers, Vakkenvullen, Kassa
    }
    
    public static class DepartmentExtensions
    {
        public static string ToFriendlyString(this Department department)
        {
            return department switch
            {
                Department.Vers => "Vers",
                Department.Vakkenvullen => "Vakkenvullen",
                Department.Kassa => "Kassa",
                _ => "Unknown"
            };
        }
    }
}
