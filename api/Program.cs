using Microsoft.EntityFrameworkCore;
using quiz.Data;
using quiz.Handler;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, QuizAuthHandler>("Authentication", null);
builder.Services.AddDbContext<QuizDBContext>(
    options => options.UseSqlite(builder.Configuration["QuizDbConnection"])
);
builder.Services.AddControllers();
builder.Services.AddScoped<IQuizRepo, QuizRepo>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOnly", policy => policy.RequireClaim("userName"));
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("admin"));

    options.AddPolicy("Both", policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == "userName" || c.Type == "admin")));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
