using KmcAPI.Data;
using KmcAPI.Repos;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration
		.GetConnectionString("conn")));


builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler =
			System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
	});
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(Program));

// ← Register all Repos!
builder.Services.AddScoped<EventRepo>();
builder.Services.AddScoped<OrganizerRepo>();
builder.Services.AddScoped<ParticipantRepo>();
builder.Services.AddScoped<EventRegistrationRepo>();
builder.Services.AddScoped<AdminRepo>();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
		policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference();
}

app.UseCors("AllowAll");
app.UseStaticFiles(); // ← Add this so uploaded images are served!
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();