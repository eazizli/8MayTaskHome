using EveraWebApp.Models;

namespace EveraWebApp.ViewModels.PRoductVm
{
    public class CreateProductVm
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; } 
        public int CatagoryId { get; set; }
        public List<Catagory> Catagories { get; set; }
        public IFormFileCollection Images { get; set; } = null!;
    }
}
