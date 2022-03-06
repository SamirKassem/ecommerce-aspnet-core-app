using ETickets.Data.Base;
using ETickets.Data.ViewModel;
using ETickets.Models;
using System.Threading.Tasks;

namespace ETickets.Data.Services
{
    public interface IMoviesService:IEntityBaseRepository<Movie>
    {
        Task<Movie> GetMovieByIdAsync(int id);

        Task<MovieDropDownsVM> GetMovieDropDownsVMAsync();

        Task AddNewMovieAsync(NewMovieVM data);

        Task UpdateNewMovieAsync(NewMovieVM data);
    }
}
