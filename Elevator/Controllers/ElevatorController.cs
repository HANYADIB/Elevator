using Elevator.Dtos;
using Elevator.IService;
using Elevator.Models.elevator;
using Elevator.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace Elevator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElevatorController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;


        public ElevatorController(IAuthService authService , IUnitOfWork unitOfWork)
        {
            _authService=authService;
            _unitOfWork=unitOfWork;

        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            BaseResponseWithData<List<ElevatorClass>> Response = new BaseResponseWithData<List<ElevatorClass>>();
            Response.Result=true;
            Response.Errors=new List<string>();


            try
            {
                var TempData = await _unitOfWork.ElevatorClass.FindAllAsync(x => x.Id > 0);
                Response.Result=true;
                Response.Data=(List<ElevatorClass>)TempData;
                return Ok(Response);
            }
            catch(Exception ex)
            {
                Response.Result=false;
                string ErrorMSG = "Exception :" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                Response.Errors.Add(ErrorMSG);
                return BadRequest(Response);
            }
        }

       

   
        [HttpPost]
        public IActionResult Adddept(ElevatorClass dept)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            BaseResponse Response = new BaseResponse();
            Response.Result=true;
            Response.Errors=new List<string>();
            try
            {
                var newcatagory = _unitOfWork.ElevatorClass.Add(dept);
                _unitOfWork.Complete();

                Response.Result=true;
                return Ok(Response);
            }
            catch(Exception ex)
            {
                Response.Result=false;
                string ErrorMSG = "Exception :" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                Response.Errors.Add(ErrorMSG);
                return BadRequest(Response);
            }
        }


        [HttpPost("Remove")]
        public IActionResult RemoveDept([FromHeader] int Id)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors=new List<string>();
            Response.Result=false;

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _unitOfWork.ElevatorClass.Delete(_unitOfWork.ElevatorClass.GetById(Id));
                _unitOfWork.Complete();
                Response.Result=true;
                return Ok(Response);
            }
            catch(Exception ex)
            {
                Response.Result=false;
                string ErrorMSG = "Exception :" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                Response.Errors.Add(ErrorMSG);
                return BadRequest(Response);
            }
        }



        [HttpPost("Update")]
        public IActionResult Update([FromBody] ElevatorClass dept)
        {
            BaseResponse Response = new BaseResponse();
            Response.Errors=new List<string>();
            Response.Result=false;
            try
            {

                var oldItem = _unitOfWork.ElevatorClass.GetById(dept.Id) ;
                if(oldItem!=null)
                {
                    oldItem.Name=dept.Name;

                    _unitOfWork.ElevatorClass.Update(oldItem);
                    _unitOfWork.Complete();
                }
                else
                {
                    Response.Result=false;
                    Response.Errors.Add("Invalid Id");
                    return BadRequest();
                }
                Response.Result=true;
                return Ok(Response);
            }
            catch(Exception ex)
            {
                Response.Result=false;
                string ErrorMSG = "Exception :" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                Response.Errors.Add(ErrorMSG);
                return BadRequest(Response);
            }
        }
    }
}
