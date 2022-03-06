using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ETickets.Data.Base
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {

        private AppDbContext _db;
        public EntityBaseRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var data = await _db.Set<T>().ToListAsync();
            return data;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var data = await _db.Set<T>().FirstOrDefaultAsync(n => n.Id == id);
            return data;
        }

        public async Task AddAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(int id, T entity)
        {
            EntityEntry entityEntry = _db.Entry<T>(entity);
            entityEntry.State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            EntityEntry entityEntry = _db.Entry<T>(await _db.Set<T>().FirstOrDefaultAsync(n => n.Id == id));
            entityEntry.State = EntityState.Deleted;
            await _db.SaveChangesAsync();
           
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T,object>>[] inlcludeProperties)
        {
            IQueryable<T> query = _db.Set<T>();

            query = inlcludeProperties.Aggregate(query, (curr, includeProperty) => curr.Include(includeProperty));

            return await query.ToListAsync();
        }




    }
}
