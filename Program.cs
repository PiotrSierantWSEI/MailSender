using MailSender;

var builder = WebApplication.CreateBuilder(args);
// Rejestracja usług - tutaj dodajemy wszystkie zależności (use case'y, infra, corsy... i tak dalej).
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

// Konfiguracja srodowiska - w środowisku deweloperskim włączamy Swagger
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // A w produkcyjnym wymuszamy HTTPS.
    app.UseHttpsRedirection();
}

// Middleware do obsługi CORS, uwierzytelniania i autoryzacji.
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// Mapowanie endpointów API.
app.MapApiEndpoints();

app.Run();
