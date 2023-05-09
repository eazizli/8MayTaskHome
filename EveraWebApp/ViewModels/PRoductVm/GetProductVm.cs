namespace EveraWebApp.ViewModels.PRoductVm
{
    public class GetProductVm
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; } = null!;
        public string ImageName { get; set; } = null!;
    }
}
