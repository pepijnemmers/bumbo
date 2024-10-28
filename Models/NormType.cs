namespace BumboApp.Models
{
    public enum NormType
    {
        Minutes, MinutesPerColi, CustomersPerHour, SecondsPerMeter
    }

    public static class NormTypeExtensions
    {
        public static string ToFriendlyString(this NormType normType)
        {
            switch (normType)
            {
                case NormType.Minutes:
                    return "minuten";
                case NormType.MinutesPerColi:
                    return "minuten per coli";
                case NormType.CustomersPerHour:
                    return "klanten per uur";
                case NormType.SecondsPerMeter:
                    return "seconden per meter";
                default:
                    return "Onbekend";
            }
        }

    }
}
