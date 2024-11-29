namespace BumboApp.Models
{
    public enum Role
    {
        Employee, Manager, Unknown
    }
    
    public static class RoleExtensions
    {
        public static string ToFriendlyString(this Role role)
        {
            return role switch
            {
                Role.Employee => "Werknemer",
                Role.Manager => "Manager",
                _ => "Gebruiker"
            };
        }
    }
}
