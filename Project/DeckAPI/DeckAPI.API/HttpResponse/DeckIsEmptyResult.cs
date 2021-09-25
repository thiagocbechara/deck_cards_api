using Microsoft.AspNetCore.Mvc;

namespace DeckAPI.API.HttpResponse
{
    public class DeckIsEmptyResult : NotFoundObjectResult
    {
        public DeckIsEmptyResult() : base("Deck is empty")
        {
        }
    }
}
