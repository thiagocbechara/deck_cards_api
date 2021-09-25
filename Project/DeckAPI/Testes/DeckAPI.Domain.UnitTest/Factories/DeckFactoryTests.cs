using DeckAPI.Domain.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace DeckAPI.Domain.UnitTest.Factories
{
    [TestClass]
    public class DeckFactoryTests
    {
        [TestMethod]
        public void CreateDefault_Should_Result_Ok()
        {
            //Arrange
            //Act
            var deck = DeckFactory.CreateDefault();

            //Assert
            Assert.AreEqual(0, deck.Id);
            Assert.IsNotNull(deck.Guid);
            Assert.AreNotEqual(Guid.Empty, deck.Guid);
            Assert.AreEqual(52, deck.Cards.Count());
        }
    }
}
