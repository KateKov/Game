using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CSharp.RuntimeBinder;
using GameStore.DAL.Entities;
using Moq;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastracture;
using GameStore.BLL.Services;
using GameStore.DAL.EF;
using GameStore.DAL.Infrastracture;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace GameStore.Tests
{
    [TestFixture]
    public class ServicesTests
    {
        IGameService _gameService;
        IRepository<Game> _gameRepository;
        IUnitOfWork _unitOfWork;
        List<Game> _randomGames;

        [SetUp]
        public void SetUp()
        {
            _randomGames = SetupGames();
            _gameRepository = SetupGameRepository();
            _unitOfWork = new Mock<IUnitOfWork>().Object;
            _gameService = new GameService( _unitOfWork);
        }
        public List<Game> SetupGames()
        {

            int _counter = new int();
            List<Game> _games = GameStoreDbInitializer.GetGames();

            foreach (Game _game in _games)
                _game.Id = ++_counter;

            return _games;
        }
        public IRepository<Game> SetupGameRepository()
        {
            // Init repository
            var repo = new Mock<IRepository<Game>>();

            // Setup mocking behavior
            repo.Setup(r => r.GetAll()).Returns(_randomGames);

            repo.Setup(r => r.GetSingle(It.IsAny<int>()))
            .Returns(new Func<int, Game>(
            id => _randomGames.Find(a => a.Id.Equals(id))));

            repo.Setup(r => r.Add(It.IsAny<Game>()))
            .Callback(new Action<Game>(newGame =>
            {
              
                dynamic maxGameId = _randomGames.Last().Id;

                dynamic nextGameId = maxGameId + 1;
                newGame.Id = nextGameId;
                newGame.Key = "some_game";
                newGame.Name = "some_game";
                newGame.Description = "some interesting game";
                _randomGames.Add(newGame);
            }));

            repo.Setup(r => r.Edit(It.IsAny<Game>()))
            .Callback(new Action<Game>(x =>
            {
                var oldArticle = _randomGames.Find(a => a.Id == x.Id);
                oldArticle = x;
            }));

            repo.Setup(r => r.Delete(It.IsAny<Game>()))
            .Callback(new Action<Game>(x =>
            {
                var _gameToRemove = _randomGames.Find(a => a.Id == x.Id);

                if (_gameToRemove != null)
                    _randomGames.Remove(_gameToRemove);
            }));

            // Return mock implementation
            return repo.Object;
        }
        [Test]
        public void ServiceShouldReturnAllArticles()
        {
            AutoMapperConfiguration.Configure();
            var articles = _gameService.GetGames();
            
            
            Assert.That(Mapper.Map<List<GameDTO>, List<Game>>(articles), Is.EqualTo(_randomGames));
        }
    }
}
