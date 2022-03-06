using ETickets.Data;
using ETickets.Data.Services;
using ETickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ETickets.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMoviesService _service;

        public MovieController(IMoviesService service)
        {
            _service = service;
        }

       public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync(n => n.Cinema);
            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var data = await _service.GetMovieByIdAsync(id);
            return View(data);

        }

        public async Task FillDropdowns()
        {
            var dropdwons = await _service.GetMovieDropDownsVMAsync();
            ViewBag.Cinemas = new SelectList(dropdwons.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(dropdwons.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(dropdwons.Actors, "Id", "FullName");
        }

        public async Task<IActionResult> Create()
        {
            await FillDropdowns();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewMovieVM movie)
        {
            if (!ModelState.IsValid)
            {
                await FillDropdowns();
                return View(movie);
            }

            await _service.AddNewMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            await FillDropdowns();
            var data = await _service.GetMovieByIdAsync(id);
            if (data == null)
                return View("NotFound");

            var response = new NewMovieVM()
            {
                Id = data.Id,
                Name = data.Name,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                Description = data.Description,
                Price = data.Price,
                ImageUrl = data.ImageUrl,
                Category = data.Category,
                CinemaId = data.CinemaId,
                ProducerId = data.ProducerId,
                ActorIds = data.Actors_Movies.Select(a => a.ActorId).ToList()
            };

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewMovieVM newMovie)
        {

            if (id != newMovie.Id)
                return View("NotFound");

            if (!ModelState.IsValid)
            {
                await FillDropdowns();
                return View(newMovie);
            }

            await _service.UpdateNewMovieAsync(newMovie);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Filter(string searchString)
        {
            var data = await _service.GetAllAsync(n => n.Cinema);
            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResults = data.Where(n => n.Name.ToLower().Contains(searchString.ToLower()) || n.Description.ToLower().Contains(searchString.ToLower()));
                return View("Index", filteredResults);
            }
            return View("Index", data);
        }


    }
}
