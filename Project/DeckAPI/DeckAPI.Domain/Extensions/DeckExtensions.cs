using DeckAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeckAPI.Domain.Extensions
{
    public static class DeckExtensions
    {
        public static void Shuffle(this Deck deck)
        {
            var random = new Random();
            var cardsArray = deck.Cards.ToArray();
            var maxCount = cardsArray.Length - 1;
            for (var i = 0; i < maxCount; i++)
            {
                var j = random.Next(i, maxCount);
                var swap = cardsArray[j];
                cardsArray[j] = cardsArray[i];
                cardsArray[i] = swap;
            }
            deck.Cards = cardsArray;
        }

        public static Card Draw(this Deck deck)
        {
            if (deck.Cards is null || !deck.Cards.Any()) return null;

            var cards = new Stack<Card>(deck.Cards);
            var card = cards.Pop();
            deck.Cards = cards.ToList();
            return card;
        }

        public static IEnumerable<Card> DrawSet(this Deck deck, int amount)
        {
            if (deck.Cards is null || !deck.Cards.Any()) return Enumerable.Empty<Card>();

            var stack = new Stack<Card>(deck.Cards);
            var draws = new List<Card>();
            draws.AddRange(stack.PopRange(amount));
            deck.Cards = stack.ToList();
            return draws;
        }
    }
}
