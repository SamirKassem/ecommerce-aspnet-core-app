using ETickets.Data.Base;
using ETickets.Data.ViewModel;
using ETickets.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ETickets.Data.Services
{
    public class MoviesService:EntityBaseRepository<Movie>,IMoviesService
    {
        private readonly AppDbContext _db;
        public MoviesService(AppDbContext db): base(db)
        {
            _db = db;
        }

        public async Task AddNewMovieAsync(NewMovieVM data)
        {
            var newData = new Movie()
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                ImageUrl = data.ImageUrl,
                CinemaId = data.CinemaId,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                Category = data.Category,
                ProducerId = data.ProducerId,
            };

            await _db.Movies.AddAsync(newData);
            await _db.SaveChangesAsync(); 

            // add movie_actors
            foreach(var actorId in data.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    ActorId = actorId,
                    MovieId = newData.Id
                };
                await _db.Actors_Movies.AddAsync(newActorMovie);
            }
            await _db.SaveChangesAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var data = await _db.Movies
                .Include(n => n.Cinema)
                .Include(n => n.Producer)
                .Include(n => n.Actors_Movies).ThenInclude(n => n.Actor)
                .FirstOrDefaultAsync(n => n.Id == id);
            return data;
        }

        public async Task<MovieDropDownsVM> GetMovieDropDownsVMAsync()
        {
            return new MovieDropDownsVM()
            {
                Producers = await _db.Producers.OrderBy(n => n.FullName).ToListAsync(),
                Actors = await _db.Actors.OrderBy(n => n.FullName).ToListAsync(),
                Cinemas = await _db.Cinemas.OrderBy(n => n.Name).ToListAsync(),
            };
        }

        public async Task UpdateNewMovieAsync(NewMovieVM data)
        {
            var movie = await _db.Movies.FirstOrDefaultAsync(n => n.Id == data.Id);
            if(movie != null)
            {
                movie.Name = data.Name;
                movie.Description = data.Description;
                movie.Price = data.Price;
                movie.ImageUrl = data.ImageUrl;
                movie.CinemaId = data.CinemaId;
                movie.StartDate = data.StartDate;
                movie.EndDate = data.EndDate;
                movie.Category = data.Category;
                movie.ProducerId = data.ProducerId;
                await _db.SaveChangesAsync();

            }
            // Remove existing actors 
            var existingActors = await _db.Actors_Movies.Where(n => n.Movie.Id == data.Id).ToListAsync();
            _db.Actors_Movies.RemoveRange(existingActors);
            await _db.SaveChangesAsync();



            // add movie_actors
            foreach (var actorId in data.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    ActorId = actorId,
                    MovieId = data.Id
                };
                await _db.Actors_Movies.AddAsync(newActorMovie);
            }
            await _db.SaveChangesAsync();
        }
    }
}
