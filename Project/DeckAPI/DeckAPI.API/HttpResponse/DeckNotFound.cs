using Microsoft.AspNetCore.Mvc;
namespace DeckAPI.API.HttpResponse
{
    public class DeckNotFoundResult : NotFoundObjectResult
    {
        public DeckNotFoundResult() : base("Deck not found")
        {
        }
    }
}
