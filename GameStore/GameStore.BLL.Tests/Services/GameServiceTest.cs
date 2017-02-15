using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.BLL.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Enums;
using GameStore.DAL.Interfaces;
using GameStore.Web.Infrastracture;
using Moq;
using NUnit.Framework;

namespace GameStore.BLL.Tests.Services
{
    [TestFixture]
    public class StoreServiceTest
    {
        private Mock<IUnitOfWork> _mock;
        private IGameService _gameService;
        private ICommentService _commentService;
        private IOrderService _orderService;
        private ITranslateService<Game, GameDTO> _translateService;
        private ITranslateService<Genre, GenreDTO> _translateGenreService;
        private INamedService<GenreDTO, GenreDTOTranslate> _genreService;
        private INamedService<PlatformTypeDTO, PlatformTypeDTOTranslate> _typeService;
        private IService<GameDTO> _service;
        private INamedService<RoleDTO, RoleDTOTranslate> _roleService;
        private Mock<IUserService> _userServiceMock;

        [SetUp]
        public void SetUpService()
        {
            AutoMapperConfiguration.Configure();
            _mock = new Mock<IUnitOfWork>();
            _userServiceMock = new Mock<IUserService>();
            _genreService = new GenresService(_mock.Object);
            _gameService = new GamesService(_mock.Object);
            _typeService = new PlatformTypesService(_mock.Object);
            _roleService =new RolesService(_mock.Object);
            _commentService = new CommentsService(_mock.Object, _userServiceMock.Object);
            _orderService = new OrdersService(_mock.Object);
            _translateService = new TranslateService<Game, GameDTO>(_mock.Object);
            _translateGenreService = new TranslateService<Genre, GenreDTO>(_mock.Object);
            _service = new Service<Game, GameDTO>(_mock.Object);
       
            _mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(new List<Genre>());
            _mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(new List<PlatformType>());
            _mock.Setup(a => a.Repository<Genre>().Edit(It.IsAny<Genre>()));
            _mock.Setup(a => a.Repository<GenreTranslate>().GetAll()).Returns(new List<GenreTranslate>());
            _mock.Setup(a => a.Repository<Comment>().Add(It.IsAny<Comment>()));

            _mock.Setup(a => a.Repository<Game>().Add(It.IsAny<Game>()));
            _mock.Setup(a => a.Repository<Game>().Edit(It.IsAny<Game>()));
            _mock.Setup(a => a.Repository<Game>().GetSingle(It.IsAny<string>())).Returns(new Game());
            _mock.Setup(a => a.Repository<Game>().Delete(It.IsAny<Game>()));
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>()))
              .Returns(new List<Game>());
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidWithLists))]
        public void CreateGame_ItemSentToDAL_InvalidItemWithLists(GameDTO gameDto, List<Genre> genresFromDb,
            List<PlatformType> platformsFromDb)
        {
            // Arrange     
            _mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(genresFromDb);
            _mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(platformsFromDb);

            // Act
            Assert.Throws<NullReferenceException>(
                () => _gameService.AddEntity(gameDto));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidNoLists))]
        public void CreateGame_ValidationException_ThrownInvalidItemNoLists(GameDTO gameDto)
        {
            // Arrange
            _mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(new List<Genre>());
            _mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(new List<PlatformType>());

            // Act & Assert
            Assert.Throws<ValidationException>(
                () => _gameService.AddEntity(gameDto));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidNoLists))]
        public void EditGame_ValidationException_ThrownInvalidItemNoLists(GameDTO gameDto)
        {
        
            // Act & Assert
            Assert.Throws<ValidationException>(() => _gameService.EditEntity(gameDto));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidWithLists))]
        public void EditGame_ItemSentToDAL_InvalidItemWithLists(GameDTO gameDto, List<Genre> genresFromDb,
            List<PlatformType> platformsFromDb)
        {
            // Arrange
            _mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(genresFromDb);
            _mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(platformsFromDb);

            // Act
            Assert.Throws<ValidationException>(() => _gameService.EditEntity(gameDto));
        }

        [Test]
        public void EditGenreItemSentToDAL_InvalidItemWithLists()
        {
            // Act
            Assert.Throws<ArgumentNullException>(() => _genreService.EditEntity(new GenreDTO()));
        }

        [Test]
        public void EditPlatform_ItemSentToDAL_ValidItemWithLists()
        {
            // Arrange
            var translate = new PlatformTypeTranslate
            {
                Id = Guid.NewGuid(),
                IsDeleted = false,
                Language = Language.en,
                Name = "name"
            };
            var type = new PlatformType
            {
                Id = Guid.NewGuid(),
                IsDeleted = false,
                Translates = new List<PlatformTypeTranslate>
                {
                    translate
                }
            };
            _mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(new List<PlatformType>() {type});
            _mock.Setup(a => a.Repository<PlatformTypeTranslate>().GetAll())
                .Returns(new List<PlatformTypeTranslate> {translate});
            _mock.Setup(a => a.Repository<PlatformType>().Edit(It.IsAny<PlatformType>())).Verifiable();
            _mock.Setup(a => a.Repository<PlatformTypeTranslate>().Edit(It.IsAny<PlatformTypeTranslate>()));
            _mock.Setup(a => a.Repository<PlatformType>().GetSingle(type.Id.ToString())).Returns(type);

            // Act
            _typeService.EditEntity(new PlatformTypeDTO
            {
                Id = type.Id.ToString(),
                Translates =
                    new List<PlatformTypeDTOTranslate>
                    {
                        new PlatformTypeDTOTranslate
                        {
                            Id = translate.Id.ToString(),
                            Name = translate.Name,
                            Language = Language.en
                        }
                    }
            });
            Mock.Verify(_mock);
        }

        [Test]
        public void EditRole_ItemSentToDAL_ValidItemWithLists()
        {
            // Arrange
            var translate = new RoleTranslate
            {
                Id = Guid.NewGuid(),
                IsDeleted = false,
                Language = Language.en,
                Name = "name"
            };
            var role = new Role
            {
                Id = Guid.NewGuid(),
                IsDeleted = false,
                Translates = new List<RoleTranslate>
                {
                    translate
                }
            };
            _mock.Setup(a => a.Repository<Role>().GetAll()).Returns(new List<Role>() {role});
            _mock.Setup(a => a.Repository<RoleTranslate>().GetAll()).Returns(new List<RoleTranslate> {translate});
            _mock.Setup(a => a.Repository<RoleTranslate>().Edit(It.IsAny<RoleTranslate>()));
            _mock.Setup(a => a.Repository<Role>().Edit(It.IsAny<Role>()));
            _mock.Setup(a => a.Repository<Role>().GetSingle(role.Id.ToString())).Returns(role);
            // Act
           _roleService.EditEntity(new RoleDTO
            {
                Id = role.Id.ToString(),
                Translates =
                    new List<RoleDTOTranslate>
                    {
                        new RoleDTOTranslate
                        {
                            Id = translate.Id.ToString(),
                            Name = translate.Name,
                            Language = Language.en
                        }
                    }
            });
            Mock.Verify(_mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentValidNoParent))]
        public void AddComment_ItemSentToDAL_ValidItemNoParrent(CommentDTO commentDto, Game game)
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game> {game});

            // Act
            _commentService.AddEntity(commentDto, game.Key);

            // Assert
            Mock.Verify(_mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentValidWithParent))]
        public void AddComment_ItemSentToDAL_ValidItemWithParrent(CommentDTO commentDto, Comment parentComment,
            Game game)
        {
            // Arrange
            _mock.Setup(a => a.Repository<Comment>().GetSingle(It.IsAny<string>())).Returns(parentComment);
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game> {game});

            // Act
            _commentService.AddEntity(commentDto, game.Key);

            // Assert
            Mock.Verify(_mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentInvalidNoParrent))]
        public void AddComment_ValidationExceptionThrown_InvalidItemNoParrent(CommentDTO commentDto, Game game)
        {
            // Arrange     
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>()))
                    .Returns(new List<Game> {game});

            // Act & Assert
            Assert.Throws<ValidationException>(() => _commentService.AddEntity(commentDto, game.Key));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentInvalidWithParrent))]
        public void AddComment_ValidationExceptionThrown_InvalidItemWithParrent(CommentDTO commentDto,
            Comment parentComment, Game game)
        {
            // Arrange
            _mock.Setup(a => a.Repository<Comment>().GetSingle(It.IsAny<string>())).Returns((Comment) null);

            // Act & Assert
            Assert.Throws<ValidationException>(() => _commentService.AddEntity(commentDto, game.Key));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.OrderValid))]
        public void ConfirmeOrder_ItemChangeInDAL_ValidItem(OrderDTO orderDto)
        {
            //Arange
            _mock.Setup(a => a.Repository<Order>().Edit(It.IsAny<Order>())).Verifiable();
            _mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(new List<Order>
                {
                    new Order
                    {
                        //CustomerId = Guid.Parse(orderDto.Id),
                        Id = Guid.Parse(orderDto.Id),
                        Date = orderDto.Date,
                        IsConfirmed = false,
                        User = new User
                        {
                            Username = "user",
                            Id = Guid.NewGuid()
                        },
                        IsPayed = false,
                        IsDeleted = false,
                        IsShipped = false
                    }
                });

            //Act
            _orderService.ConfirmeOrder(orderDto.Id);

            //Assert
            Mock.Verify(_mock);
        }

        [Test]
        public void AddNewOrder_ItemChangeInDAL_ValidItem()
        {
            //Arange
            _mock.Setup(a => a.Repository<Order>().Add(It.IsAny<Order>()));
            _mock.Setup(a => a.Repository<OrderDetail>().GetAll()).Returns(new List<OrderDetail>());
            _mock.Setup(a => a.Repository<OrderDetail>().FindBy(It.IsAny<Expression<Func<OrderDetail, bool>>>()))
                .Returns(new List<OrderDetail>());

            _mock.Setup(a => a.Repository<OrderDetail>().Add(It.IsAny<OrderDetail>()));
            _mock.Setup(a => a.Repository<User>().FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(new List<User>() {new User() {Username = "name", Id = Guid.NewGuid()} });
            //Act
            _orderService.AddOrder(new OrderDetailDTO()
            {
                Discount = 0,
                GameId = Guid.NewGuid().ToString(),
                GameKey = "key",
                Id = Guid.NewGuid().ToString(),
                OrderId = Guid.NewGuid().ToString(),
                Price = 2
            }, "username", true);

            //Assert
            Mock.Verify(_mock);
        }

        [Test]
        public void PayOrder_ItemChangeInDAL_ValidItem()
        {
            //Arange
            var order = new Order()
            {
                User = new User {Id = Guid.NewGuid(), Username = "name"},
                IsPayed = false,
                IsConfirmed = true,
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                OrderDetails = new List<OrderDetail>()
                {
                    new OrderDetail()
                    {
                        Id = Guid.NewGuid(),
                        GameId = Guid.NewGuid(),
                        IsPayed = false,
                        Price = 2,
                        Quantity = 2
                    }
                }
            };
            _mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(new List<Order>()
                {
                    order
                });
            var id = Guid.NewGuid();
            _mock.Setup(a => a.Repository<Order>().Edit(It.IsAny<Order>()));
            _mock.Setup(a => a.Repository<OrderDetail>().Edit(It.IsAny<OrderDetail>()));
            _mock.Setup(a => a.Repository<User>().FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(new List<User>
                {
                    new User
                    {
                        Id = id,
                        CreateDate = DateTime.UtcNow,
                        Email = "katerynakovalenko96@gmail.com",
                        IsDeleted = false,
                        IsLocked = false,
                        Orders = new List<Order> {order }
                    }
                });
            //Act
            _orderService.Pay("name");

            //Assert
            Mock.Verify(_mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.OrderInvalid))]
        public void ConfirmeOrder_ItemChangeInDAL_InvalidItem(OrderDTO orderDto)
        {
            //Arange
            _mock.Setup(a => a.Repository<Order>().Edit(It.IsAny<Order>())).Verifiable();

            //Act && Assert
            Assert.Throws<ValidationException>(() => _orderService.ConfirmeOrder(orderDto.Id));
        }

        [Test]
        public void GetBasket_ReturnsOrder_ValidItem()
        {
            var userId = Guid.NewGuid();
            var user = new User() {Username = "username", Id = userId};
            // Arrange
            _mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(new List<Order>
                {
                    new Order()
                    {
                        User = user,
                        UserId = userId,
                        Id = Guid.NewGuid(),
                        IsDeleted = false,
                        IsConfirmed = true,
                        IsShipped = false,
                        IsPayed = false
                    },
                    new Order() {User = new User() {Username = "user2", Id = Guid.NewGuid()}}
                });
            _mock.Setup(a => a.Repository<User>().FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(new List<User> {user});

            // Act
            var res = _orderService.GetBasket("username");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(OrderDTO)));
        }

        [Test]
        public void GetOrderDetail_ReturnsOrder_ValidItem()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Username = "user",
                Orders = new List<Order>
                {
                    new Order
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.UtcNow,
                        IsDeleted = false,
                        IsConfirmed = false,
                        IsShipped = false,
                        IsPayed = false,
                        OrderDetails = new List<OrderDetail>
                        {
                            new OrderDetail
                            {
                                Id = Guid.NewGuid(),
                                IsDeleted = false,
                                IsPayed = false
                            }
                        }
                    }
                },
                CreateDate = DateTime.UtcNow
            };
            // Arrange
            _mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(new List<Order>
                {
                    new Order
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.UtcNow,
                        IsDeleted = false,
                        IsConfirmed = false,
                        IsShipped = false,
                        IsPayed = false,
                        User = user,
                        UserId = userId,
                        OrderDetails = new List<OrderDetail>
                        {
                            new OrderDetail
                            {
                                Id = Guid.NewGuid(),
                                IsDeleted = false,
                                IsPayed = false
                            }
                        }
                    },
                    new Order()
                });
            _mock.Setup(a => a.Repository<User>().FindBy(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(new List<User> {user});

            // Act
            var res = _orderService.GetBasket("user");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(OrderDTO)));
        }


        [Test]
        public void GetOrderDetail_ReturnsOrder_InvalidItem()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(new List<Order>
                {
                    new Order() {User = new User {Id = Guid.NewGuid(), Username = "name"}},
                    new Order()
                });

            // Act
            var res = _orderService.GetOrders("name");

            //// Assert
            Assert.That(res, Is.TypeOf(typeof(OrderDTO)));
        }


        [Test]
        public void GetOrders_ReturnsOrder_ValidItem()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(new List<Order>
                {
                    new Order() {User = new User {Id = Guid.NewGuid(), Username = "name"}},
                    new Order()
                });

            // Act
            var res = _orderService.GetOrders("name");

            //// Assert
            Assert.That(res, Is.TypeOf(typeof(OrderDTO)));
        }

        [Test]
        public void GetGame_ReturnsGameDTO_DALReturnsValue()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game> {new Game()});

            // Act
            var res = _gameService.GetByKey("stub-key");

            // Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.TypeOf(typeof(GameDTO)));
        }

        [Test]
        public void EditUser_ReturnsNothing_DALChangeValue()
        {
            // Arrange
            var role = new Role
            {
                Id = Guid.NewGuid(),
                Translates = new List<RoleTranslate>
                {
                    new RoleTranslate
                    {
                        Id = Guid.NewGuid(),
                        Language = Language.en,
                        Name = "role"
                    }
                }
            };
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "name",
                Roles = new List<Role>
                {
                    role
                }
            };

            _mock.Setup(a => a.Repository<User>().GetSingle(It.IsAny<Guid>().ToString())).Returns(user);
            _mock.Setup(a => a.Repository<Ban>().GetAll());
            _mock.Setup(a => a.Repository<Role>().FindBy(It.IsAny<Expression<Func<Role, bool>>>()))
                .Returns(new List<Role> {role});
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game> { new Game() });
            _mock.Setup(a => a.Repository<User>().Edit(It.IsAny<User>()));
            // Act
            var res = _gameService.GetByKey("stub-key");

            // Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.TypeOf(typeof(GameDTO)));
        }

        [Test]
        public void GetGameByName_ReturnsGameDTO_DALReturnsValue()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var translateId = Guid.NewGuid();
            _mock.Setup(a => a.Repository<GameTranslate>().FindBy(It.IsAny<Expression<Func<GameTranslate, bool>>>()))
                .Returns(new List<GameTranslate>
                {
                    new GameTranslate()
                    {
                        Id = translateId,
                        BaseEntityId = entityId,
                        Name = "name",
                        Language = Language.en
                    }
                });
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game>
                {
                    new Game
                    {
                        Id = entityId,
                        Translates =
                            new List<GameTranslate>()
                            {
                                new GameTranslate()
                                {
                                    Name = "name",
                                    BaseEntityId = entityId,
                                    Id = Guid.NewGuid(),
                                    Language = Language.en
                                }
                            }
                    }
                });

            // Act
            var res = _gameService.GetByName("name");

            // Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.TypeOf(typeof(GameDTO)));
        }

        [Test]
        public void GetAllGame_ReturnsGameDTO_DALReturnsValue()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().GetAll()).Returns(new List<Game> {new Game()});

            // Act
            var res = _gameService.GetAll(false);

            // Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.TypeOf(typeof(List<GameDTO>)));
        }


        [Test]
        public void GetGameById_ReturnsGameDTO_DALReturnsValue()
        {
            // Act
            var res = _gameService.GetById(Guid.NewGuid().ToString());

            // Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.TypeOf(typeof(GameDTO)));
        }

        [Test]
        public void GetGame_ReturnsNull_DALReturnsNothing()
        {
            // Arrange
            _mock.Setup(a => a.Repository<GameTranslate>().FindBy(It.IsAny<Expression<Func<GameTranslate, bool>>>()));
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game>
                {
                    new Game
                    {
                        Id = Guid.NewGuid(),
                        Key = "key",
                        Translates = new List<GameTranslate>
                        {
                            new GameTranslate()
                            {
                                Id = Guid.NewGuid(),
                                Language = Language.en,
                                Name = "name"
                            }
                        }
                    }
                });

            // Act & Assert
            var res = _gameService.GetByKey("key");
            Assert.That(res, Is.TypeOf<GameDTO>());
        }


        [Test]
        public void GetGamesByFilter_ReturnsGames_DALReturnsValues()
        {
            // Arrange
            var mockPipeline = new Mock<IPipeLine<Game>>();
            mockPipeline.Setup(x => x.Execute());
            mockPipeline.Setup(x => x.Register(It.IsAny<IOperation<Game>>()));
            _mock.Setup(a => a.Repository<Game>().GetAll(It.IsAny<Func<Game, bool>>())).Returns(new List<Game>());

            // Act & Assert
            Assert.Throws<NullReferenceException>(
                () =>
                    _gameService.GetAllByFilter(
                        new FilterDTO() {FilterBy = Filter.Comments, Name = "name", DateOfAdding = Date.month}, false, 1,
                        PageEnum.All));
        }

        [Test]
        public void GetOrdersByFilter_ReturnsOrders_DALReturnsValues()
        {
            // Arrange
            var mockPipeline = new Mock<IPipeLine<Order>>();
            mockPipeline.Setup(x => x.Execute());
            mockPipeline.Setup(x => x.Register(It.IsAny<IOperation<Order>>()));
            _mock.Setup(a => a.Repository<Order>().GetAll(It.IsAny<Func<Order, bool>>())).Returns(new List<Order>());
            // Act & Assert
            Assert.Throws<NullReferenceException>(
                () =>
                    _orderService.GetOrdersByFilter(
                        new OrderFilterDTO()
                        {
                            DateFrom = DateTime.UtcNow,
                            DateTo = DateTime.UtcNow,
                            FilterBy = Filter.New
                        }, false, 1, PageEnum.All));
        }

        [Test]
        public void GetCommentsByGameKey_ReturnsComments_DALReturnsValues()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            _mock.Setup(a => a.Repository<Game>().GetAll());
            _mock.Setup(a => a.Repository<Comment>().FindBy(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(new List<Comment>
                {
                    new Comment
                    {
                        Id = Guid.NewGuid(),
                        Name = "name",
                        Body = "name",
                        Game = new Game {Id = gameId}
                    },
                    new Comment
                    {
                        Id = Guid.NewGuid(),
                        Name = "anotherName",
                        Body = "name",
                        Game = new Game {Id = gameId}
                    }
                });

            // Act
            var res = _commentService.GetCommentsByGameKey("key");

            // Assert
            var commentsDto = res as IList<CommentDTO> ?? res.ToList();
            Assert.That(commentsDto, Is.TypeOf(typeof(List<CommentDTO>)));
            Assert.That(commentsDto.Count, Is.EqualTo(2));
        }

        [Test]
        public void AddGenreTranslate_DALNotChange_DALReturnsNothing()
        {
            // Act
            var res =
                _translateGenreService.AddTranslate(
                    new Genre()
                    {
                        Id = Guid.NewGuid(),
                        Translates =
                            new List<GenreTranslate>()
                            {
                                new GenreTranslate() {Id = Guid.NewGuid(), Language = Language.en, Name = "name"}
                            }
                    },
                    new GenreDTO()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Translates =
                            new List<GenreDTOTranslate>()
                            {
                                new GenreDTOTranslate()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Language = Language.en,
                                    Name = "name"
                                }
                            }
                    });

            // Assert          
            Assert.That(res, Is.TypeOf(typeof(Genre)));
            Assert.That(res.Translates.ToList().Count, Is.EqualTo(1));
        }


        [Test]
        public void AddGameTranslate_DALNotChange_DALReturnsNothing()
        {
            // Act
            var res =
                _translateService.AddTranslate(
                    new Game()
                    {
                        Id = Guid.NewGuid(),
                        Translates =
                            new List<GameTranslate>()
                            {
                                new GameTranslate() {Id = Guid.NewGuid(), Language = Language.en, Name = "name"}
                            }
                    },
                    new GameDTO()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Translates =
                            new List<GameDTOTranslate>()
                            {
                                new GameDTOTranslate()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Language = Language.en,
                                    Name = "name"
                                }
                            }
                    });
            // Assert          
            Assert.That(res, Is.TypeOf(typeof(Game)));
            Assert.That(res.Translates.First().Name, Is.EqualTo("name"));
            Assert.That(res.Translates.ToList().Count, Is.EqualTo(1));
        }


        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameForOrderDetails))]
        public void GetOrderDetail_ReturnsNothing_DALReturnsValues(Game game)
        {
            // Arrange
            _mock.Setup(a => a.Repository<Comment>().FindBy(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(new List<Comment>());
            _mock.Setup(a => a.Repository<Genre>().FindBy(It.IsAny<Expression<Func<Genre, bool>>>()))
                .Returns(new List<Genre>());
            _mock.Setup(a => a.Repository<PlatformType>().FindBy(It.IsAny<Expression<Func<PlatformType, bool>>>()))
                .Returns(new List<PlatformType>());
            _mock.Setup(a => a.Repository<GameTranslate>().FindBy(It.IsAny<Expression<Func<GameTranslate, bool>>>()))
                .Returns(new List<GameTranslate>());
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game>() {game});

            _mock.Setup(a => a.Repository<PlatformTypeTranslate>().GetAll())
                .Returns(new List<PlatformTypeTranslate>()
                {
                    new PlatformTypeTranslate() {Id = Guid.NewGuid(), Language = Language.en, Name = "name"}
                });
            _mock.Setup(a => a.Repository<Publisher>().GetAll())
                .Returns(new List<Publisher>()
                {
                    new Publisher()
                    {
                        Id = Guid.NewGuid(),
                        Translates =
                            new List<PublisherTranslate>()
                            {
                                new PublisherTranslate()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "name",
                                    Language = Language.en
                                }
                            }
                    }
                });
            _mock.Setup(a => a.Repository<GenreTranslate>().Add(It.IsAny<GenreTranslate>()));
            _mock.Setup(a => a.Repository<OrderDetail>().FindBy(It.IsAny<Expression<Func<OrderDetail, bool>>>()))
                .Returns(new List<OrderDetail>());
            _mock.Setup(a => a.Repository<Order>().GetAll()).Returns(new List<Order>());
            _mock.Setup(a => a.Repository<Order>().Add(It.IsAny<Order>()));
            _mock.Setup(a => a.Repository<OrderDetail>().Add(It.IsAny<OrderDetail>()));

            _orderService.GetOrderDetail("key", 1, true);
            Mock.Verify(_mock);
        }


        [Test]
        public void GetCommentsByGameKey_ReturnsNoComments_DALReturnsNothing()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().GetAll());
            _mock.Setup(a => a.Repository<Comment>().FindBy(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(new List<Comment>());

            // Act
            var res = _commentService.GetCommentsByGameKey("key");

            // Assert
            var commentsDto = res as IList<CommentDTO> ?? res.ToList();
            Assert.That(commentsDto, Is.TypeOf(typeof(List<CommentDTO>)));
            Assert.That(commentsDto.Count, Is.EqualTo(0));
        }

        [Test]
        public void DeleteGameByName_ReturnsValidationException_DALChangeValue()
        {
            // Arrange
            var sut = new NameService<Game, GameDTO, GameTranslate, GameDTOTranslate>(_mock.Object);

            // Act
            Assert.Throws<ValidationException>(() => sut.DeleteByName("name"));
        }

        [Test]
        public void DeleteGameById_ReturnsValidationException_DALChangeValue()
        {
            // Act
            _service.DeleteById(Guid.NewGuid().ToString());
            Mock.Verify(_mock);
        }


        [Test, TestCaseSource(typeof(TestData), nameof(TestData.OrderValidItems))]
        public void DeleteBusket_ChangeOrderRepository_DALReturnsNothing(Order order,
            Game game, Guid gameId,
            PlatformType platformType, PlatformTypeTranslate platformTypeTranslate,
            GameTranslate gameTranslate, Genre genre, GenreTranslate genreTranslate,
            Publisher publisher, PublisherTranslate publisherTranslate)
        {
            _mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(new List<Order>
                    {
                        order
                    }
                );
            _mock.Setup(a => a.Repository<Game>().GetSingle(gameId.ToString()))
                .Returns(game);
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>()))
              .Returns(new List<Game>() { game});

            _mock.Setup(a => a.Repository<Comment>().GetAll(It.IsAny<Func<Comment, bool>>()))
                .Returns(new List<Comment>());
            _mock.Setup(a => a.Repository<PlatformTypeTranslate>().GetAll())
                .Returns(new List<PlatformTypeTranslate>()
                {
                    platformTypeTranslate
                });
            _mock.Setup(a => a.Repository<PlatformType>().GetAll())
                .Returns(new List<PlatformType>()
                {
                    platformType
                });
            _mock.Setup(a => a.Repository<GenreTranslate>().GetAll())
                .Returns(new List<GenreTranslate>()
                {
                    genreTranslate
                });
            _mock.Setup(a => a.Repository<Genre>().GetAll())
                .Returns(new List<Genre>()
                {
                    genre
                });
            _mock.Setup(a => a.Repository<PublisherTranslate>().GetAll())
                .Returns(new List<PublisherTranslate>()
                {
                    publisherTranslate
                });
            _mock.Setup(a => a.Repository<Publisher>().GetAll())
                .Returns(new List<Publisher>()
                {
                    publisher
                });
            _mock.Setup(a => a.Repository<Order>().Delete(It.IsAny<Order>()));
            _mock.Setup(a => a.Repository<GameTranslate>().FindBy(It.IsAny<Expression<Func<GameTranslate, bool>>>()))
                .Returns(new List<GameTranslate> {gameTranslate});
            _orderService.DeleteOrder("name", true);

            // Assert
            Mock.Verify(_mock);
        }
    }
}