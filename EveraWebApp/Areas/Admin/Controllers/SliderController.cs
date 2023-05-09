using EveraWebApp.DataContext;
using EveraWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing.Text;

namespace EveraWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly EveraDbContext _everaDbContext;
        private readonly  IWebHostEnvironment _envoriment;

     

        public SliderController(EveraDbContext everaDbContext,IWebHostEnvironment envoriment)
        {
            _everaDbContext = everaDbContext;
            _envoriment = envoriment;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _everaDbContext.Sliders.ToListAsync();
            return View(sliders);
        }
      
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            string guid = Guid.NewGuid().ToString();
            string newFileName = guid + slider.Image.FileName;
            string path = Path.Combine(_envoriment.WebRootPath, "assets", "imgs", "slider");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, newFileName);

            using (FileStream filestream = new FileStream(path, FileMode.Create))
            {
                await slider.Image.CopyToAsync(filestream);
            }
            slider.ImageName = newFileName;

            if (_everaDbContext.Sliders.Any(s => s.Title.Trim().ToLower() == slider.Title.Trim().ToLower()))
            {
                ModelState.AddModelError("Title", "Already title exsit!");
                return View();
            }
            await _everaDbContext.Sliders.AddAsync(slider);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> Create(Slider slider)
        //{
        //    if (!ModelState.IsValid) return View();
        //    string guid= Guid.NewGuid().ToString();
        //    string newFileName = guid + slider.Image.FileName;
        //    string path = Path.Combine(_envoriment.WebRootPath,"assets","imgs","slider");

        //    using (FileStream filestream = new FileStream(path, FileMode.CreateNew))

        //    {
        //        await slider.Image.CopyToAsync(filestream);
        //    }
        //    slider.ImageName = newFileName;

        //    if(_everaDbContext.Sliders.Any(s=>s.Title.Trim().ToLower() == slider.Title.Trim().ToLower()))
        //    {
        //        ModelState.AddModelError("Title", "Already title exsit!");
        //        return View();
        //    }
        //    await _everaDbContext.Sliders.AddAsync(slider);
        //    await _everaDbContext.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> Read(int id)
        {
            Slider? slider = await _everaDbContext.Sliders.FindAsync(id);
            if(slider== null) return NotFound();
            return View(slider);
        }
     
        public async Task<IActionResult> Update(int id)
        {
            Slider? slider = await _everaDbContext.Sliders.FindAsync(id);
            if(slider==null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Slider newSlider)
        {
            if (!ModelState.IsValid) return View();

            Slider? slider = await _everaDbContext.Sliders.AsNoTracking().Where(c=>c.Id== id).FirstOrDefaultAsync();
            if (slider == null) return NotFound();

            if (_everaDbContext.Sliders.Any(s => s.Title.Trim().ToLower() == newSlider.Title.Trim().ToLower()))
            {
                ModelState.AddModelError("Title", "Already title exsit!");
                return View();
            }
             newSlider.Id= id;
            _everaDbContext.Sliders.Update(newSlider);
            //slider.Title = newSlider.Title;
            //slider.ImageName = newSlider.ImageName;
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Slider? slider = await _everaDbContext.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            if (slider.ImageName != null)
            {
            string filepath = Path.Combine(_envoriment.WebRootPath, "assets", "imgs", "slider",slider.ImageName);
                if(System.IO.File.Exists(filepath)) 
                {
                    System.IO.File.Delete(filepath);
                }
            }
            _everaDbContext.Sliders.Remove(slider);
            await _everaDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
