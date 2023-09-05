using BuberBreakfast.Models;
using ErrorOr;
using BuberBreakfast.ServiceErrors;

namespace BuberBreakfast.Services.Breakfasts;

public class BreakfastService : IBreakfastService
{
    private static readonly Dictionary<Guid, Breakfast> _breakfasts = new();

    // implimented interface for the POST
    public ErrorOr<Created> CreateBreakfast(Breakfast breakfast)
    {
        // breakfast.Id is the key, breakfast is the value
        _breakfasts.Add(breakfast.Id, breakfast);

        return Result.Created;
    }

    // implimented interface for the DELETE
    public ErrorOr<Deleted> DeleteBreakfast(Guid id)
    {
        // remove the breakfast with the given id
        _breakfasts.Remove(id);

        return Result.Deleted;
    }

    // implimented interface for GET
    public ErrorOr<Breakfast> GetBreakfast(Guid id)
    {
        if (_breakfasts.TryGetValue(id, out var breakfast))
        {
            return breakfast;
        }
        return Errors.Breakfast.NotFound;
    }

    // implimented interface for the PUT
    public ErrorOr<UpsertedBreakfast> UpsertBreakfast(Breakfast breakfast)
    {
        var IsNewlyCreated = !_breakfasts.ContainsKey(breakfast.Id);
        _breakfasts[breakfast.Id] = breakfast;

        return new UpsertedBreakfast(IsNewlyCreated);
    }



}