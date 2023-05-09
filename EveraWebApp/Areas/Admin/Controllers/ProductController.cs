using EveraWebApp.DataContext;
using EveraWebApp.Models;
using EveraWebApp.ViewModels.PRoductVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;

namespace EveraWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly EveraDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(EveraDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!ModelState.IsValid) return View();

            List<Product> products = await _context.Products.Include(c=>c.Images).ToListAsync();
            List<GetProductVm> getProductVms = new List<GetProductVm>();
            foreach (Product product in products)
            {
                getProductVms.Add(new GetProductVm()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    ImageName = product.Images.FirstOrDefault().ImageName

                });

            }
            return View(getProductVms);
        }
        public async Task<IActionResult> Create()
        {
            List<Catagory> catagories = await _context.Catagories.ToListAsync();
            //CreateProductVm createProductVm = new CreateProductVm()
            //{
            //    Catagories = catagories,
            //};
            ViewData["Catagories"] = catagories;
          return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductVm createProductVm)
        {
            if (!ModelState.IsValid) 
            { 
                return View();
            }
            foreach (IFormFile item in createProductVm.Images) 
            { 
                string guid = Guid.NewGuid().ToString();
                string newFile = guid + item.FileName;
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "imgs", "shop", newFile);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
                path = Path.Combine(path, newFile);
                using (FileStream fileStream = new FileStream(path, FileMode.CreateNew))
                {
                    await item.CopyToAsync(fileStream);
                }

            }
            return View();

            
        }
    }
}
    

