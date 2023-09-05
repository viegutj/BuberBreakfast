using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;

// reexecutes the request to the error route
public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()

    // here, we can have whatever error handling logic we want
    {
        // using the Problem method from the ControllerBase
        // will return 500, internal server error
        return Problem();
    }
}