using Elevator.Models.auth;
using Elevator.Models.elevator;

namespace Elevator.IService
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<ApplicationUser , int> ApplicationUsers { get; }
        IBaseRepository<ElevatorClass , int> ElevatorClass { get; }
      
        int Complete();

    }
}
