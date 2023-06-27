using DU_Summer_2023_Capstone.Data;
using DU_Summer_2023_Capstone.Data.Interfaces;
using DU_Summer_2023_Capstone.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DU_Summer_2023_Capstone.Controllers
{
    public class PizzaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPizzaRepository _pizzaRepository;

        public PizzaController(ApplicationDbContext context, IPizzaRepository pizzaRepository)
        {
            _context = context;
            _pizzaRepository = pizzaRepository;

        }


        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var pizzas = _context.Pizzas.ToList();
            return View(pizzas);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
        {
            var entry = _context.Entry(new Pizza { PizzaId = id });

            if (entry.State == EntityState.Detached)
            {
                var pizza = _context.Set<Pizza>().Find(id);
                if (pizza == null)
                {
                    return NotFound();
                }
                entry.CurrentValues.SetValues(pizza);
            }

            return View(entry.Entity);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Enable(int id)
        {
            _pizzaRepository.EnablePizza(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DisAble(int id)
        {
            _pizzaRepository.DisablePizza(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Pizza pizza)
        {
            if (ModelState.IsValid)
            {
                // Find the existing pizza entity
                var existingPizza = _context.Pizzas.Find(pizza.PizzaId);
                if (existingPizza == null)
                {
                    return NotFound();
                }

                // Detach the existing entity from the DbContext
                _context.Entry(existingPizza).State = EntityState.Detached;

                if (pizza.UploadedImage != null)
                {
                    // Get the file name without extension, then append the timestamp and finally the extension
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pizza.UploadedImage.FileName);
                    var extension = Path.GetExtension(pizza.UploadedImage.FileName);
                    var fileName = $"{fileNameWithoutExtension}_{DateTime.Now.Ticks}{extension}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await pizza.UploadedImage.CopyToAsync(stream);
                    }
                    pizza.ImageUrl = "/images/" + fileName;  // Update the pizza to use the new filename as the image URL.
                }
                else
                {
                    pizza.ImageUrl = existingPizza.ImageUrl;
                }

                // Update the detached entity
                _context.Update(pizza);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(pizza);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Pizza pizza)
        {
            if (ModelState.IsValid)
            {

                if (pizza.UploadedImage != null)
                {
                    // Get the file name without extension, then append the timestamp and finally the extension
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pizza.UploadedImage.FileName);
                    var extension = Path.GetExtension(pizza.UploadedImage.FileName);
                    var fileName = $"{fileNameWithoutExtension}_{DateTime.Now.Ticks}{extension}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await pizza.UploadedImage.CopyToAsync(stream);
                    }
                    pizza.ImageUrl = "/images/" + fileName;  // Update the pizza to use the filename as the image URL.
                }

                _context.Add(pizza);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pizza);
        }


    }
}
