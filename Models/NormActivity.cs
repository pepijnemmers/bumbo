namespace BumboApp.Models
{
    public enum NormActivity
    {
        ColiUitladen, VakkenVullen, Kassa, Vers, Spiegelen
    }

    public static class NormActivityExtensions
    {
        public static string ToFriendlyString(this NormActivity normActivity)
        {
            switch (normActivity)
            {
                case NormActivity.ColiUitladen:
                    return "Coli uitladen";
                case NormActivity.VakkenVullen:
                    return "Vakken vullen";
                case NormActivity.Kassa:
                    return "Kassa";
                case NormActivity.Vers:
                    return "Vers";
                case NormActivity.Spiegelen:
                    return "Spiegelen";
                default:
                    return "Onbekend";
            }
        }

    }
}
