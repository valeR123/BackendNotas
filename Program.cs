using BackendNotas.Models;
using BackendNotas.Services;
using FirebaseAdmin.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMongoDatabaseSettings>(sp =>
    new MongoDatabaseSettings
    {
        ConnectionString = "mongodb+srv://vrodriguezv:Valentina123@cluster0.knlvx22.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0",
        DatabaseName = "test"
    });

builder.Services.AddSingleton<FirebaseAuth>();
builder.Services.AddSingleton<NoteService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
