// New Template for .net 6
//--------------------------------------
// builder variable for dependency injection and configuration
using BuberBreakfast.Services.Breakfasts;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    // Every time it recieves a request for IBreakfastService in the constructor,
    // then use the BreakfastService project as the implimentation of this interface

    // Using AddSingleton means that the first time the request comes in for
    // IBreakfastService interface, we create a new object
    // But, for now on, every time the the interface is requested, use the existing object

    // AddScoped means a new object is created at the initial use of the interface
    // within the lifetime of a request

    //Add Transient means every time the interface is requested, we create a new
    // BreakfastService object
    builder.Services.AddScoped<IBreakfastService, BreakfastService>();
}

// app variable manages the request pipeline
// we have a pipeline here that determines what middleware our
// program will go through, and in what order

// UseExeptionHandler will cath the exeption, 
// and changes the route to one we define, and reexecutes the request
var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}