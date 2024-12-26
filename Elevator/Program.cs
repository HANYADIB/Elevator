using Elevator.IService;
using Elevator.Models.auth;
using Elevator.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Quartz;
using Elevator.Models.Quartez;
using Quartz.Impl;
using static Quartz.Logging.OperationName;
using Elevator.Models.elevator;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddIdentity<ApplicationUser , IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata=false;
    o.SaveToken=false;
    o.TokenValidationParameters=new TokenValidationParameters
    {
        ValidateIssuerSigningKey=true ,
        ValidateIssuer=true ,
        ValidateAudience=true ,
        ValidateLifetime=true ,
        ValidIssuer=builder.Configuration ["JWT:Issuer"] ,
        ValidAudience=builder.Configuration ["JWT:Audience"] ,
    };
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit=false;
    options.Password.RequireLowercase=false;
    options.Password.RequireNonAlphanumeric=false;
    options.Password.RequireUppercase=false;
    options.Password.RequiredLength=3;
    //options.Password.RequiredUniqueChars = 0;
});
builder.Services.AddScoped<IAuthService , AuthService>();
builder.Services.AddTransient<IUnitOfWork , UnitOfWork>();
builder.Services.AddScoped<DeleteExpiredDataJob>();     // Register your Quartz Job

// Ensure this class is properly defined and implements IJob

// Add Quartz hosted service

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(80);  // Configure Kestrel to listen on port 80
//});





var app = builder.Build();
//StdSchedulerFactory factory = new StdSchedulerFactory();

//// Get a scheduler
//IScheduler scheduler = await factory.GetScheduler();
//await scheduler.Start();

//// Define the job and tie it to the MyJob class
//IJobDetail job = JobBuilder.Create<DeleteExpiredDataJob>()
//    .WithIdentity("deleteExpiredDataJob", "group1")
//    .Build();

//// Trigger the job to run every 5 seconds
//ITrigger trigger = TriggerBuilder.Create()
//    .WithIdentity("myTrigger", "group1")
//    .StartNow()
//    .WithSimpleSchedule(x => x
//        .WithIntervalInSeconds(5)  // Every 5 seconds
//        .RepeatForever())          // Keep repeating
//    .Build();

//// Schedule the job using the trigger
//await scheduler.ScheduleJob(job , trigger);
using(var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    // Here is the migration executed
    dbContext.Database.Migrate();
    if (!dbContext.ElevatorClass.Any()) 
    {
        dbContext.ElevatorClass.AddRange(

            new ElevatorClass { Name = "????" },
            new ElevatorClass { Name = "???" }
            );
        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
