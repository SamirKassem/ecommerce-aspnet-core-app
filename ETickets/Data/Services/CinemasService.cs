using ETickets.Data.Base;
using ETickets.Models;

namespace ETickets.Data.Services
{
    public class CinemasService:EntityBaseRepository<Cinema>, ICinemasService
    {
        public CinemasService(AppDbContext db):base(db)
        {

        }
    }
}
