using BuberBreakfast.Contracts.Breakfast.BreakfastResponse;
using BuberBreakfast.Contracts.Breakfast.CreateBreakfastRequest;
using BuberBreakfast.Contracts.Breakfast.UpsertBreakfastRequest;
using BuberBreakfast.Models;
using BuberBreakfast.Services.Breakfasts;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using BuberBreakfast.ServiceErrors;

namespace BuberBreakfast.Controllers;

// the class BreakfastsController is being inherited by the ControllerBase
//[ApiController] is built in with asp.net

public class BreakfastsController : ApiController
{
    private readonly IBreakfastService _breakfastService;

    // dependency injection
    public BreakfastsController(IBreakfastService breakfastService)
    {
        _breakfastService = breakfastService;
    }

    [HttpPost]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        // map the data in the request to the "language" or format that our application speaks
        // take in all the values and:
            // create a new id
            // change the date last modified to UtcNow
        ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.From(request);
            
            if(requestToBreakfastResult.IsError)
            {
                return Problem(requestToBreakfastResult.Errors);
            }
            var breakfast = requestToBreakfastResult.Value;
            ErrorOr<Created> createBreakfastResult = _breakfastService.CreateBreakfast(breakfast);


        return createBreakfastResult.Match(
            created => CreatedAtGetBreakfast(breakfast),
            errors => Problem(errors)
        );
    }
    

    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);

        return getBreakfastResult.Match(
            breakfast => Ok(MapBreakfastResponse(breakfast)),
            errors => Problem(errors)
        );

    }

        

    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        // similar to GET, but we're using the id we get from the route
        ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.From(id, request);

        if(requestToBreakfastResult.IsError)
        {
            return Problem(requestToBreakfastResult.Errors);
        }
        var breakfast = requestToBreakfastResult.Value;
        // need to generate the method.
        ErrorOr<UpsertedBreakfast> upsertBreakfastResult = _breakfastService.UpsertBreakfast(breakfast);

        // return no content if the id is of a breakfast that already exists in our db
        // otherwise, we want a new breakfast

        return upsertBreakfastResult.Match(
            upserted => upserted.IsNewlyCreated ? CreatedAtGetBreakfast(breakfast) : NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {

        ErrorOr<Deleted> deletedBreakfastResult = _breakfastService.DeleteBreakfast(id);

        return deletedBreakfastResult.Match(
            deleted => NoContent(),
            errors => Problem(errors)
        );
    }

// This is our method to map our responses
    private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
        {
            return new BreakfastResponse(
            breakfast.Id,
            breakfast.Name,
            breakfast.Description,
            breakfast.StartDateTime,
            breakfast.EndDateTime,
            breakfast.LastModifiedDateTime,
            breakfast.Savory,
            breakfast.Sweet
            );
        }

// Method
        private CreatedAtActionResult CreatedAtGetBreakfast(Breakfast breakfast)
    {
        return CreatedAtAction(
            //action in which client can retrieve the resource
            actionName: nameof(GetBreakfast),
            // pass an object with the id
            routeValues: new{id = breakfast.Id},
            // the content of the response
            value: MapBreakfastResponse(breakfast)
        );
    }
}