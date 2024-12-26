using Elevator.consts;
using System.Linq.Expressions;

namespace Elevator.IService
{
    public interface IBaseRepository<T, DT> where T : class
    {
        T GetById(DT id);
        Task<T> GetByIdAsync(DT id);
        IEnumerable<T> GetAll();
        IQueryable<T> GetAllQuerable();

        Task<IEnumerable<T>> GetAllAsync();
        T Find(Expression<Func<T , bool>> criteria , string [] includes = null);
        Task<T> FindAsync(Expression<Func<T , bool>> criteria , string [] includes = null);
        IEnumerable<T> FindAll(Expression<Func<T , bool>> criteria , string [] includes = null);
        IEnumerable<T> FindAll(Expression<Func<T , bool>> criteria , int take , int skip);
        IEnumerable<T> FindAll(Expression<Func<T , bool>> criteria , int? take , int? skip ,
            Expression<Func<T , object>> orderBy = null , string orderByDirection = ApplicationConsts.OrderByAscending);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T , bool>> criteria , string [] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T , bool>> criteria , int skip , int take);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T , bool>> criteria , int? skip , int? take ,
            Expression<Func<T , object>> orderBy = null , string orderByDirection = ApplicationConsts.OrderByAscending);


        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T , bool>> criteria , int? skip , int? take , string [] includes = null ,
    Expression<Func<T , object>> orderBy = null , string orderByDirection = ApplicationConsts.OrderByAscending);

       
        T Add(T entity);
        Task<T> AddAsync(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);
        int Count();
        int Count(Expression<Func<T , bool>> criteria);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T , bool>> criteria);
        IQueryable<T> GetAsQueryable();
        IQueryable<T> FindAllAsQueryable(Expression<Func<T , bool>> criteria , string [] includes = null);
    }
}
