namespace Elevator.Models.auth
{
    public class AuthModel
    {
        public string UserId { get; set; }
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
        public bool? IsAuthenticated { get; set; }
        public bool Active { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public List<string> Roles { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresOn { get; set; }
        public string? ImagePath { get; set; }
        public string? Phone { get; set; }
        public string? DateTimeEGP { get; set; }
      
    }
}
