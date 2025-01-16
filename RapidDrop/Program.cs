using RapidDrop.Service;

var builder = WebApplication.CreateBuilder(args);

// Add CORS to allow requests from the frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:8080") // Add your frontend's URL here
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Configure MongoDB settings from `appsettings.json`
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

// Register services
builder.Services.AddScoped<DeliveryService>();
builder.Services.AddScoped<WhatsAppService>();

// Add controllers
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline for development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS in the middleware pipeline
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
