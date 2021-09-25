using AutoMapper;
using DeckAPI.API.Automapper;
using DeckAPI.API.Controllers;
using DeckAPI.API.Dtos;
using DeckAPI.API.HttpResponse;
using DeckAPI.Domain.Entities;
using DeckAPI.Domain.Factories;
using DeckAPI.Infra.Db;
using DeckAPI.Infra.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckAPI.Api.IntegrationTests.Controllers
{
    [TestClass]
    public class DeckControllerTests
    {
        private static (DeckController controller, UnitOfWork uow) CreateController()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new EntityToApiDtoProfile()));
            var dbContextOption = new DbContextOptionsBuilder<ApplicationDbContext>()
                                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                        .Options;
            var unitOfWork = new UnitOfWork(new ApplicationDbContext(dbContextOption));
            var controller = new DeckController(unitOfWork, new Mapper(mapperConfig));
            return (controller, unitOfWork);
        }

        [TestMethod]
        public async Task Shuffle_ShouldReturnOk()
        {
            //Arrange
            var controller = CreateController().controller;

            //Act
            var actionResult = await controller.Shuffle();
            var okResult = actionResult as OkObjectResult;

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.IsFalse(string.IsNullOrWhiteSpace(okResult.Value.ToString()));
            Assert.AreNotEqual(Guid.Empty.ToString(), okResult.Value.ToString());
        }

        [TestMethod]
        public async Task Get_ShouldReturnDeck()
        {
            //Arrange
            var controller = CreateController().controller;
            var guid = ((await controller.Shuffle()) as OkObjectResult).Value.ToString();

            //Act
            var actionResult = await controller.Get(guid);
            var okResult = actionResult as OkObjectResult;
            var deck = okResult.Value as DeckDto;

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.AreEqual(guid, deck.Guid);
            Assert.AreEqual(DeckFactory.CreateDefault().Cards.Count(), deck.Cards.Length);
        }

        [TestMethod]
        public async Task Get_ShouldReturnDeckNotFound()
        {
            var controller = CreateController().controller;

            var actionResult = await controller.Get(Guid.Empty.ToString());

            Assert.IsInstanceOfType(actionResult, typeof(DeckNotFoundResult));
        }

        [TestMethod]
        public async Task Draw_ShouldReturnOk()
        {
            var controller = CreateController().controller;
            var guid = ((await controller.Shuffle()) as OkObjectResult).Value.ToString();

            var actionResult = await controller.Draw(guid);
            var drawDto = (actionResult as OkObjectResult).Value as DrawDto;

            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.IsNotNull(drawDto);
            Assert.IsFalse(string.IsNullOrWhiteSpace(drawDto.Value));
        }

        [TestMethod]
        public async Task Draw_ShouldReturnDeckIsEmpty()
        {
            var tuple = CreateController();
            var controller = tuple.controller;
            var uow = tuple.uow;
            var guid = Guid.NewGuid();
            await uow.Decks.InsertAsync(new Deck { Guid = guid, Cards = new List<Card>() });
            await uow.CommitAsync();

            var actionResult = await controller.Draw(guid.ToString());

            Assert.IsInstanceOfType(actionResult, typeof(DeckIsEmptyResult));
        }

        [TestMethod]
        public async Task SetDraw_ShouldReturnOk()
        {
            var controller = CreateController().controller;
            var guid = ((await controller.Shuffle()) as OkObjectResult).Value.ToString();

            var amount = 2;
            var actionResult = await controller.SetDraw(guid, amount);
            var drawsDto = (actionResult as OkObjectResult).Value as IEnumerable<DrawDto>;

            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.IsNotNull(drawsDto);
            Assert.AreEqual(amount, drawsDto.Count());
        }

        [TestMethod]
        public async Task SetDraw_ShouldReturnDeckIsEmpty()
        {
            var tuple = CreateController();
            var controller = tuple.controller;
            var uow = tuple.uow;
            var guid = Guid.NewGuid();
            await uow.Decks.InsertAsync(new Deck { Guid = guid, Cards = new List<Card>() });
            await uow.CommitAsync();

            var amount = 2;
            var actionResult = await controller.SetDraw(guid.ToString(), amount);

            Assert.IsInstanceOfType(actionResult, typeof(DeckIsEmptyResult));
        }
    }
}
