namespace Elevator.ViewModels
{
    public class BaseResponseWithData<ViewModel>
    {
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
     
        public ViewModel Data { get; set; }
    }
}
