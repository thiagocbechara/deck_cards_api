using DeckAPI.API.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace DeckAPI.API.Extensions
{
    public static class ControllerBaseExtension
    {
        public static NotFoundObjectResult DeckNotFound(this ControllerBase _) => new("Deck no found");
        public static DeckIsEmptyResult DeckIsEmpty(this ControllerBase _) => new();
    }
}
