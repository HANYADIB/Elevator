using Elevator.IService;
using Elevator.Models.elevator;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Elevator.Models.Quartez
{
    public class DeleteExpiredDataJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteExpiredDataJob(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _unitOfWork.ElevatorClass.Add(new ElevatorClass { Name="Quart" });
            _unitOfWork.Complete();
            
            return Task.CompletedTask;
        }
    }
}
