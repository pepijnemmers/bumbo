using BumboApp.Models;

namespace BumboApp.ViewModels
{
    public class NormsViewModel
    {
        public required List<Norm> NormsList { get; set; }
        public required List<Norm> LatestNormsList { get; set; }
    }

    public class AddNormViewModel
    {
        public NormActivity NormActivity { get; set; }
        public int Value { get; set; }
        public NormType NormType { get; set; }
    }
}
