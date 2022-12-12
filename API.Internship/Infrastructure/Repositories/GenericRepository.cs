using API.Internship.Domain.Interfaces;
using API.Internship.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace API.Internship.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly InternshipContext _context;
        private bool disposed = false;
        //private DbSet<TEntity> entities; 
        public GenericRepository(InternshipContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //entities = context.Set<TEntity>();
        }

        public virtual TEntity Get(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }
        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }
        public virtual async Task<ICollection<TEntity>> GetAllAsync()
        {

            return await _context.Set<TEntity>().ToListAsync();
        }
        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {

            IQueryable<TEntity> queryable = GetAll();
            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include<TEntity, object>(includeProperty);
            }

            return queryable;
        }
        //
        public TEntity Find(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }
        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().SingleOrDefault(predicate);
        }
        public async Task<TEntity> FindAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();
        }
        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(predicate);
            return query;
        }
        public async virtual Task<IQueryable<TEntity>> FindAsyncBy(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(predicate);
            return await Task.Run(() => query);
        }

        //
        public IEnumerable<TEntity> FindAll()
        {
            return _context.Set<TEntity>().AsNoTracking().ToList();
        }
        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().AsNoTracking().Where(predicate).ToList();
        }
        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            //predicate.Compile();
            return await _context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();
        }
        //
        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }
        //
        public virtual void Update(object id, TEntity entity)
        {
            if (entity != null)
            {
                TEntity existing = _context.Set<TEntity>().Find(id);
                if (existing != null)
                    _context.Entry<TEntity>(existing).CurrentValues.SetValues(entity);
            }
        }
        public virtual async Task UpdateAsync(object id, TEntity entity)
        {
            if (entity != null)
            {
                TEntity existing = _context.Set<TEntity>().Find(id);
                if (existing != null)
                    await Task.Run(() => _context.Entry<TEntity>(existing).CurrentValues.SetValues(entity));
            }
        }
        public async Task UpdateAsync(TEntity entity)
        {
            try
            {
                //_context.Set<TEntity>().Attach(entity);
                _context.Entry(entity).State = EntityState.Detached;
                await Task.Run(() => _context.Set<TEntity>().Update(entity));
                //await Task.Run(() => _context.Entry<TEntity>(entity).CurrentValues.SetValues(entity) ; 
            }
            catch (Exception)
            {

            }

        }
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            try
            {
                _context.Set<TEntity>().UpdateRange(entities);
            }
            catch (Exception)
            {

            }
        }
        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                await Task.Run(() => _context.Set<TEntity>().UpdateRange(entities));
            }
            catch (Exception)
            {

            }
        }
        //
        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }


        public async Task RemoveAsync(TEntity entity)
        {
            await Task.Run(() => _context.Set<TEntity>().Remove(entity));
        }
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }
        public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _context.Set<TEntity>().RemoveRange(entities));
        }
        // 
        public IEnumerable<TEntity> List()
        {
            return _context.Set<TEntity>().AsNoTracking().ToList();
        }
        public IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).AsNoTracking().ToList();
        }
        public async Task<IEnumerable<TEntity>> ListAsync()
        {
            return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).AsNoTracking().ToListAsync();
        }


        public int Count()
        {
            return _context.Set<TEntity>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<TEntity>().CountAsync();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
