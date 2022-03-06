using ETickets.Data.Base;
using ETickets.Models;

namespace ETickets.Data.Services
{
    public class ProducersService:EntityBaseRepository<Producer>, IProducersService 
    {
        public ProducersService(AppDbContext db) : base(db) { }
    }
}
