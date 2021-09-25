using DeckAPI.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DeckAPI.Domain.Factories
{
    public static class DeckFactory
    {
        private static readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public static Deck CreateDefault()
        {
            return new Deck { Cards = GenerateCards(), Guid = Guid.NewGuid() };
        }

        private static Card[] GenerateCards()
        {
            var result = _cache.GetOrCreate("DeckFactoryCards", context => {

                // Define o tempo de expiração para o cache
                context.SetAbsoluteExpiration(TimeSpan.FromHours(1));

                var cardNotation = new[] { "A", "K", "Q", "J", "T", "9", "8", "7", "6", "5", "4", "3", "2" };
                var cardSuitedness = new[] { "s", "c", "h", "d" };

                var cards = new Card[cardNotation.Length * cardSuitedness.Length];
                var position = 0;
                for (var i = 0; i < cardNotation.Length; i++)
                {
                    var notation = cardNotation[i];
                    for (var j = 0; j < cardSuitedness.Length; j++)
                    {
                        var suitedness = cardSuitedness[j];
                        cards[position++] = new Card { Value = $"{notation}{suitedness}" };
                    }
                }

                return cards;
            });

            return result;
        }
    }
}
