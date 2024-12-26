using Elevator.consts;
using Elevator.IService;
using Elevator.Models.auth;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Elevator.Service
{
    public class BaseRepository<T, DT> : IBaseRepository <T , DT> where T : class
    {
        protected ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context=context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public IQueryable<T> GetAllQuerable()
        {
            return _context.Set<T>().AsQueryable();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public T GetById(DT id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(DT id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T Find(Expression<Func<T , bool>> criteria , string [] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if(includes!=null)
                foreach(var incluse in includes)
                    query=query.Include(incluse);

            return query.SingleOrDefault(criteria);
        }

        public async Task<T> FindAsync(Expression<Func<T , bool>> criteria , string [] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if(includes!=null)
                foreach(var incluse in includes)
                    query=query.Include(incluse);

            return await query.SingleOrDefaultAsync(criteria);
        }

        public IEnumerable<T> FindAll(Expression<Func<T , bool>> criteria , string [] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if(includes!=null)
                foreach(var include in includes)
                    query=query.Include(include);

            return query.Where(criteria).ToList();
        }
        public IQueryable<T> FindAllAsQueryable(Expression<Func<T , bool>> criteria , string [] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if(includes!=null)
                foreach(var include in includes)
                    query=query.Include(include);

            return query.Where(criteria).AsQueryable();
        }

        public IEnumerable<T> FindAll(Expression<Func<T , bool>> criteria , int skip , int take)
        {
            return _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToList();
        }

        public IEnumerable<T> FindAll(Expression<Func<T , bool>> criteria , int? skip , int? take ,
            Expression<Func<T , object>> orderBy = null , string orderByDirection = ApplicationConsts.OrderByAscending)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if(skip.HasValue)
                query=query.Skip(skip.Value);

            if(take.HasValue)
                query=query.Take(take.Value);

            if(orderBy!=null)
            {
                if(orderByDirection==ApplicationConsts.OrderByAscending)
                    query=query.OrderBy(orderBy);
                else
                    query=query.OrderByDescending(orderBy);
            }

            return query.ToList();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T , bool>> criteria , string [] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if(includes!=null)
                foreach(var include in includes)
                    query=query.Include(include);

            return await query.Where(criteria).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T , bool>> criteria , int take , int skip)
        {
            return await _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T , bool>> criteria , int? skip , int? take ,
            Expression<Func<T , object>> orderBy = null , string orderByDirection = ApplicationConsts.OrderByAscending)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if(take.HasValue)
                query=query.Take(take.Value);

            if(skip.HasValue)
                query=query.Skip(skip.Value);

            if(orderBy!=null)
            {
                if(orderByDirection==ApplicationConsts.OrderByAscending)
                    query=query.OrderBy(orderBy);
                else
                    query=query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T , bool>> criteria , int? skip , int? take , string [] includes = null ,
    Expression<Func<T , object>> orderBy = null , string orderByDirection = ApplicationConsts.OrderByAscending)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if(includes!=null)
                foreach(var include in includes)
                    query=query.Include(include);

            if(orderBy!=null)
            {
                if(orderByDirection==ApplicationConsts.OrderByAscending)
                    query=query.OrderBy(orderBy);
                else
                    query=query.OrderByDescending(orderBy);
            }
            if(take.HasValue)
                query=query.Take(take.Value);

            if(skip.HasValue)
                query=query.Skip(skip.Value);


            return await query.ToListAsync();
        }







      



        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            return entities;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public T Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Attach(T entity)
        {
            _context.Set<T>().Attach(entity);
        }

        public void AttachRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AttachRange(entities);
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public int Count(Expression<Func<T , bool>> criteria)
        {
            return _context.Set<T>().Count(criteria);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T , bool>> criteria)
        {
            return await _context.Set<T>().CountAsync(criteria);
        }
        public IQueryable<T> GetAsQueryable()
        {
            return _context.Set<T>().AsQueryable();
        }
    }
}

