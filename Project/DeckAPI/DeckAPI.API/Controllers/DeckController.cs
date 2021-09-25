using AutoMapper;
using DeckAPI.API.Dtos;
using DeckAPI.API.Extensions;
using DeckAPI.Domain.Extensions;
using DeckAPI.Domain.Factories;
using DeckAPI.Infra.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeckAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckController : BaseDeckApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeckController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("new/shuffle")]
        public async Task<IActionResult> Shuffle()
        {
            // Criar o baralho
            var defaultDeck = DeckFactory.CreateDefault();
            // Embaralhar
            defaultDeck.Shuffle();
            // Salvar no banco de dados
            await _unitOfWork.Decks.InsertAsync(defaultDeck);
            await _unitOfWork.CommitAsync();

            // retornar o Guid
            return Ok(defaultDeck.Guid.ToString());
        }

        [HttpGet]
        [Route("{deckGuid}")]
        public async Task<IActionResult> Get(string deckGuid)
        {
            var guid = new Guid(deckGuid);
            var deck = await _unitOfWork.Decks.GetAsync(x => x.Guid == guid);
            // Se não achar o deck, return NotFound
            if (deck is null)
            {
                return DeckNotFound();
            }

            return Ok(_mapper.Map<DeckDto>(deck));
        }

        [HttpGet]
        [Route("{deckGuid}/draw")]
        public async Task<IActionResult> Draw(string deckGuid)
        {
            // Recuperar o deck, via guid
            var guid = new Guid(deckGuid);
            var deck = await _unitOfWork.Decks.GetAsync(x => x.Guid == guid);
            // Se não achar o deck, return NotFound
            if(deck is null)
            {
                return DeckNotFound();
            }

            // Remover a carta do baralho
            var card = deck.Draw();

            if(card is null)
            {
                return this.DeckIsEmpty();
            }

            _unitOfWork.Decks.Update(deck);
            await _unitOfWork.CommitAsync();

            var draw = _mapper.Map<DrawDto>(card);
            //var draw = new DrawDto { Value = card.Value };
            // Retorna carta removida
            return Ok(draw);
        }

        [HttpGet]
        [Route("{deckGuid}/set/draw/{amount}")]
        public async Task<IActionResult> SetDraw(string deckGuid, int amount)
        {
            // Recuperar o deck, via guid
            var guid = new Guid(deckGuid);
            var deck = await _unitOfWork.Decks.GetAsync(x => x.Guid == guid);

            // Se não achar o deck, return NotFound
            if (deck is null)
            {
                return DeckNotFound();
            }

            // Remover a quantidade de cartas do baralho
            var cards = deck.DrawSet(amount);

            if(cards?.Any() == false)
            {
                return this.DeckIsEmpty();
            }

            _unitOfWork.Decks.Update(deck);
            await _unitOfWork.CommitAsync();

            var draws = cards.Select(_mapper.Map<DrawDto>);
            // Retorna essas cartas
            return Ok(draws);
        }
    }
}
