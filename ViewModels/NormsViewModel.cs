using BumboApp.Models;

namespace BumboApp.ViewModels
{
    public class NormsViewModel
    {
        public required List<Norm> NormsList { get; set; }
        public required List<Norm> LatestNormsList { get; set; }
    }

}
