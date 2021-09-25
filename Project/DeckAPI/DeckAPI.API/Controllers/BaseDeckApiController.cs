using DeckAPI.API.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace DeckAPI.API.Controllers
{
    [ApiController]
    public class BaseDeckApiController : ControllerBase
    {
        protected DeckNotFoundResult DeckNotFound() => new();
        //protected DeckIsEmptyResult DeckIsEmpty() => new();
    }
}
