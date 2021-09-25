using DeckAPI.Domain.Entities;
using DeckAPI.Domain.Extensions;
using DeckAPI.Domain.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckAPI.Domain.UnitTest.Extensions
{
    [TestClass]
    public class DeckExtensionsTests
    {
        private static void AssertDeck(Deck deck, long assertId, int assertCardsCount)
        {
            Assert.AreEqual(assertId, deck.Id);
            Assert.IsNotNull(deck.Guid);
            Assert.AreNotEqual(Guid.Empty, deck.Guid);
            Assert.AreEqual(assertCardsCount, deck.Cards.Count());
        }

        #region Shuffle
        [TestMethod]
        public void Shuffle_Should_ResultOk()
        {
            var deck = DeckFactory.CreateDefault();
            var deckAssert = DeckFactory.CreateDefault();

            deck.Shuffle();

            AssertDeck(deck, deckAssert.Id, deckAssert.Cards.Count());
        }
        #endregion

        #region Draw
        [TestMethod]
        public void Draw_WhenCardsIsNull_ShouldReturnNull()
        {
            var deck = new Deck();

            var draw = deck.Draw();

            Assert.IsNull(draw);
        }

        [TestMethod]
        public void Draw_WhenCardsIsEmpty_ShouldReturnNull()
        {
            var deck = new Deck { Cards = Enumerable.Empty<Card>() };

            var draw = deck.Draw();

            Assert.IsNull(draw);
        }

        [TestMethod]
        public void Draw_WhenCards_ShouldReturnDraw()
        {
            var deck = DeckFactory.CreateDefault();
            var deckAssert = DeckFactory.CreateDefault();

            var draw = deck.Draw();

            AssertDeck(deck, deckAssert.Id, deckAssert.Cards.Count() - 1);
            Assert.IsNotNull(draw);
        }
        #endregion Draw

        #region DrawSet
        [TestMethod]
        public void DrawSet_WhenCardsIsNull_ShouldReturnNull()
        {
            var deck = new Deck();

            var draw = deck.DrawSet(2);

            Assert.IsFalse(draw.Any());
        }

        [TestMethod]
        public void DrawSet_WhenCardsIsEmpty_ShouldReturnNull()
        {
            var deck = new Deck { Cards = Enumerable.Empty<Card>() };

            var draw = deck.DrawSet(2);

            Assert.IsFalse(draw.Any());
        }

        [TestMethod]
        public void DrawSet_WhenCards_ShouldReturnDraws()
        {
            const int set = 2;
            var deck = DeckFactory.CreateDefault();
            var deckAssert = DeckFactory.CreateDefault();

            var draws = deck.DrawSet(set);

            AssertDeck(deck, deckAssert.Id, deckAssert.Cards.Count() - set);
            Assert.AreEqual(set, draws.Count());
        }
        #endregion
    }
}
