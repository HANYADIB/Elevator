using Elevator.IService;
using Elevator.Models.auth;
using Elevator.Models.elevator;

namespace Elevator.Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
       
        public IBaseRepository<ElevatorClass , int> ElevatorClass { get; private set; }

        public IBaseRepository<ApplicationUser, int> ApplicationUsers { get; private set; }

        public UnitOfWork(ApplicationDbContext context )
        {
            _context=context;
            ElevatorClass=new BaseRepository<ElevatorClass , int>(_context);
           
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
