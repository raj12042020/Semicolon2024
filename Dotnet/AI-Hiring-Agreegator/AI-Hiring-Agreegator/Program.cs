using Microsoft.Extensions.Options;
using AIHiringPortal.Database;
using AIHiringPortal.DatabaseService;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseConnection"));
builder.Services.AddScoped<IDatabaseSettings>(db => db.GetRequiredService<IOptions<DatabaseSettings>>().Value);
//builder.Services.AddSingleton<PatientService>();
builder.Services.AddSingleton<SearchService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(origin => true)
                            .AllowCredentials());

app.UseAuthorization();

app.MapControllers();

app.Run();
