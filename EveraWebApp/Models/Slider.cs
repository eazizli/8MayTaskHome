using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace EveraWebApp.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; }= null!;
        public string? ImageName { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; } = null!;


    }
}
