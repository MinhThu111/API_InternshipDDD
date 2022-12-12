using System.Linq.Expressions;

namespace API.Internship.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        IQueryable<TEntity> GetAll();
        Task<ICollection<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity Find(int id);
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindAsync(int id);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        Task<IQueryable<TEntity>> FindAsyncBy(Expression<Func<TEntity, bool>> predicate);

        //
        IEnumerable<TEntity> FindAll();
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAllAsync();
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
        //
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        //
        void Update(object id, TEntity entity);
        Task UpdateAsync(object id, TEntity entity);
        Task UpdateAsync(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
        //
        void Remove(TEntity entity);
        Task RemoveAsync(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);
        //         public IEnumerable<TEntity> List();
        public IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> predicate);
        public Task<IEnumerable<TEntity>> ListAsync();
        public Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate);

        int Count();
        Task<int> CountAsync();
        void Dispose();
        //public Task<TEntity> AddAsync(TEntity entity);
        //public Task<TEntity> Update(TEntity entity);
        //public Task<TEntity> Delete(TEntity entity); 
        //public Task<TEntity> Remove2(TEntity entity);


    }
}
