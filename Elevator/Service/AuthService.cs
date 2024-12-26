using Elevator.IService;
using Elevator.Models.auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
namespace Elevator.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png", ".jpeg", ".svg" };
        private new List<string> _allowedResourcesExtenstions = new List<string> { ".pdf", ".docs" };
        private long _maxAllowedPosterSize = 15728640;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IHostingEnvironment _environment;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager
            , IOptions<JWT> jwt , IHttpContextAccessor httpContextAccessor , IHostingEnvironment Environment , IUnitOfWork unitOfWork)
        //IHostingEnvironment Environment , IHttpContextAccessor httpContextAccessor , IUnitOfWork unitOfWork ,
        //IMapper mapper , IOptions<MailSettings> mailSettingsOptions , IWhatsAppService whatsAppService)
        {
            _userManager=userManager;
            _roleManager=roleManager;
            _jwt=jwt.Value;
            _environment=Environment;
            _httpContextAccessor=httpContextAccessor;
            _unitOfWork=unitOfWork;
            //_mapper=mapper;
            //_mailSettings=mailSettingsOptions.Value;
            //_whatsAppService=whatsAppService;

        }
        private string BaseURL
        {
            get
            {
                var uri = _httpContextAccessor?.HttpContext?.Request;
                string Host = uri?.Scheme + "://" + uri?.Host.Value.ToString();
                return Host;
            }
        }


        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            List<string> errors = new List<string>();
        
            string ImagePath = null;
            if(model.Image!=null)
            {

                if(!_allowedExtenstions.Contains(Path.GetExtension(model.Image.FileName).ToLower()))
                {
                    errors.Add("Logo Only .png , .jpg , .jpeg and .svg images are allowed!");
                    return new AuthModel { Result=false , Errors=errors };

                }
                if(model.Image.Length>_maxAllowedPosterSize)
                {
                    errors.Add("Max allowed size for Cover Image greater than 10MB!");
                    return new AuthModel { Result=false , Errors=errors };

                }

                string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + model.Image.FileName.Trim().Replace(" ", "");
                ImagePath="Users/";
                string SaveImagePath = Path.Combine(this._environment.WebRootPath, ImagePath);
                if(!System.IO.Directory.Exists(SaveImagePath))
                {
                    System.IO.Directory.CreateDirectory(SaveImagePath); //Create directory if it doesn't exist
                }
                SaveImagePath=SaveImagePath+FileName;
                using FileStream fileStream = new(SaveImagePath, FileMode.Create);
                ImagePath="/"+ImagePath+FileName;
                model.Image.CopyTo(fileStream);
            }
         
            var user = new ApplicationUser
            {
                CreationDate = DateTime.Now,
                UserName = model.Username,
                Active = false,
                Email = model.Email,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                NationalId = model.NationalId,
                AddressArea = model.AddressArea,
                Street = model.Street,
                BuildingNumber = model.BuildingNumber,
                FloorNumber = model.FloorNumber,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
              
                PhoneNumber = model.PhoneNumber,
                LandLine = model.LandLine,
                ImagePath = ImagePath,
                Age = model.Age
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if(!result.Succeeded)
            {
                //var errors = string.Empty;
                errors.AddRange(result.Errors.Select(x => x.Description).ToList());
                // foreach (var error in result.Errors)
                //errors += $"{error.Description},";

                return new AuthModel { Result=false , Errors=errors };
            }

           

          
              
            _unitOfWork.Complete();

            return new AuthModel
            {
                Result=true ,
                UserId=user.Id ,
                ImagePath=user.ImagePath!=null ? BaseURL+user.ImagePath : null ,
                Email=user.Email ,
                Active=false ,
              
                Username=user.UserName ,
                Phone=user.PhoneNumber

            };
        }




        public async Task<AuthModel> GetTokenAsync(LoginRequestModel model)
        {
            var authModel = new AuthModel();
            authModel.Errors=new List<string>();

            var user = await _userManager.Users.Where(u => u.UserName == model.UserName || u.Email == model.UserName || u.PhoneNumber == model.UserName || u.NationalId == model.UserName).FirstOrDefaultAsync();

            if(user is null||!await _userManager.CheckPasswordAsync(user , model.Password))
            {
                authModel.Errors.Add("خطاء فى اسم المستخدم او كلمة السر");
                return authModel;
            }
            if(user.Active==false)
            {
                authModel.Errors.Add(" يرجى التواصل مع المسئول لتفعيل الحساب");
                return authModel;
            }
            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.ImagePath=user.ImagePath!=null ? BaseURL+user.ImagePath : null;
            authModel.UserId=user.Id;
            authModel.Result=true;
            authModel.IsAuthenticated=true;
            authModel.Token=new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email=user.Email;
            authModel.Phone=user.PhoneNumber;
            authModel.Username=user.UserName;
            authModel.Active=user.Active;
            authModel.ExpiresOn=jwtSecurityToken.ValidTo;
            authModel.Roles=rolesList.ToList();
            // Get the timezone information for Egypt
            TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            // Get the current datetime in Egypt
            DateTime egyptDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
            // Alternatively, you can specify a custom format
            string formattedDateTime = egyptDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            authModel.DateTimeEGP=formattedDateTime;
           

            return authModel;
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach(var role in roles)
                roleClaims.Add(new Claim("roles" , role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("Pass", user.PasswordHash),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }


    }
}
