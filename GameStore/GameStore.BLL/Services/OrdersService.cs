using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Infrastructure.MailServer;
using GameStore.BLL.Infrastructure.OrderFilter;
using GameStore.BLL.Infrastructure.Paging;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.MailServer;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Services
{
    public class OrdersService : Service<Order, OrderDTO>, IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IDtoToDomainMapping _dtoToDomain;
        private readonly IGameService _gameService;
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryption;

        public OrdersService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dtoToDomain = new DtoToDomainMapping(_unitOfWork);
            _gameService = new GamesService(_unitOfWork);
            _encryption = new EncryptionService();
            _userService = new UserService(_unitOfWork, _encryption);
        }

        #region Get

        public OrderDTO GetBasket(string username)
        {
            var userId = _userService.GetUserEntityByName(username).Id;
            var basket =
                _unitOfWork.Repository<Order>()
                    .FindBy(x => !x.IsConfirmed)
                    .FirstOrDefault(x => x.UserId == userId && !x.IsPayed);

            _logger.Debug("Getting basket by Customer id={0} ", userId);
            return Mapper.Map<OrderDTO>(basket);
        }

        public OrderDTO GetOrders(string username)
        {
            var orders =
                _unitOfWork.Repository<Order>()
                    .FindBy(x => x.IsConfirmed && x.IsPayed == false && !x.IsDeleted)
                    .FirstOrDefault(
                        x => x.User != null && x.User.Username == username && x.OrderDetails!=null);
            if (orders == null)
            {
                orders = new Order() {Id = Guid.NewGuid()};
            }
            orders.OrderDetails = orders.OrderDetails.Where(x => !x.IsDeleted).ToList();
            _logger.Debug("Getting order by Customer id={0} ");
            return Mapper.Map<OrderDTO>(orders);
        }

        public OrderDetailDTO GetOrderDetail(string gameKey, short quantity, bool isOrderExist)
        {
            //Adding order to sql database, if game is situated in mongobase
            if (gameKey.Length == 24 && _gameService.GetByKey(gameKey) != null)
            {
                var gameMongo = _gameService.GetByKey(gameKey);
                _gameService.AddEntity(gameMongo);
            }

            //Change total numbers of game
            var game = _gameService.GetByKey(gameKey);
            if (game == null)
            {
                throw new ValidationException("There is no game with such key", "GameNotFound");
            }

            game.UnitsInStock -= quantity;
            _gameService.EditEntity(game);


            //Get OrderDetail (new or existing)
            var orderDetailEntity =
                _unitOfWork.Repository<OrderDetail>()
                    .FindBy(x => x.Game.Key == game.Key && !x.IsDeleted)
                    .FirstOrDefault(x => x.IsPayed == false);
            var orderDetailDto = (orderDetailEntity != null && isOrderExist)
                ? GetExistedOrderDetail(orderDetailEntity, game, quantity)
                : GetNewOrderDetail(game, quantity);

            return orderDetailDto;
        }

        public OrderDetailDTO GetExistedOrderDetail(OrderDetail orderDetailEntity, GameDTO game, short quantity)
        {
            var orderDetailDto = Mapper.Map<OrderDetailDTO>(orderDetailEntity);
            orderDetailDto.Quantity += quantity;
            orderDetailDto.Price += quantity*game.Price;
            return orderDetailDto;
        }

        public OrderDetailDTO GetNewOrderDetail(GameDTO game,
            short quantity)
        {
            var orderDetailDto = new OrderDetailDTO()
            {
                Id = Guid.NewGuid().ToString(),
                Discount = 0,
                GameId = game.Id,
                GameKey = game.Key,
                Price = game.Price*quantity,
                Quantity = quantity
            };
            return orderDetailDto;
        }

        public OrderFilterResultDTO GetOrdersByFilter(OrderFilterDTO filter, bool isHistory, int page = 1,
            PageEnum pageSize = PageEnum.Ten)
        {
            if (filter == null)
            {
                throw new ValidationException("There is no filter", string.Empty);
            }

            var pipeline = new Pipeline<Order>();
            if (isHistory)
            {
                filter.DateTo = DateTime.UtcNow.AddDays(-30);
            }
            else
            {
                filter.DateFrom = DateTime.UtcNow.AddDays(-30);
            }

            RegisterFilter(pipeline, filter, page, pageSize);

            var query = pipeline.Execute();
            var result = _unitOfWork.Repository<Order>().GetAll(query, false);

            var ordersDto = Mapper.Map<IEnumerable<OrderDTO>>(result.List).AsQueryable();
            var filterResult = new OrderFilterResultDTO
            {
                Orders = ordersDto,
                Count = result.Count
            };
            return filterResult;
        }

        #endregion

        #region Add

        public void AddOrderDetail(string gameKey, short quantity, string username, bool isBasket)
        {
          
            var order =
                _unitOfWork.Repository<Order>()
                    .FindBy(x => x.User != null && x.User.Username == username).FirstOrDefault(x => x.IsConfirmed == !isBasket && !x.IsPayed);
            var isOrderExist = order != null;
            var orderDetail = GetOrderDetail(gameKey, quantity, isOrderExist);
            if (order==null)
            {
                AddOrder(orderDetail, username, isBasket);
            }
            else
            {
                UpdateOrder(username, orderDetail, isBasket);
            }

            _logger.Debug("Adding product with Game {0} to the busket of Customer {1} ", orderDetail.GameId, username);
        }

        public void AddOrder(OrderDetailDTO product, string username, bool isBasket)
        {
            var user = _userService.GetUserEntityByName(username);
            var order = new OrderDTO()
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.UtcNow,
                IsConfirmed = !isBasket,
                CustomerId = user.Id.ToString()
            };

            var productOrder = product;     

            var orderDetailEntity = Mapper.Map<OrderDetail>(productOrder);
            orderDetailEntity = (OrderDetail) _dtoToDomain.AddEntities(orderDetailEntity, productOrder);

            var orderEntity = Mapper.Map<Order>(order);
            orderEntity.User = user;
            orderEntity.OrderDetails = new List<OrderDetail>
            {
                orderDetailEntity
            };

            _unitOfWork.Repository<Order>().Add(orderEntity);
        }

        #endregion

        #region Delete

        public void DeleteOrder(string username, bool isBasket)
        {
            var orders =
                _unitOfWork.Repository<Order>().FindBy(x => x.User != null).Where(x => x.User.Username == username);
            var order = (isBasket)
                ? orders.FirstOrDefault(x => !x.IsConfirmed && !x.IsPayed)
                : orders.FirstOrDefault(x => x.IsConfirmed);
            if (order == null)
            {
                throw new ValidationException("Order wasn't found", "OrderNotFound");
            }

            _unitOfWork.Repository<Order>().Delete(order);

            //Change total number of game
            foreach (var item in order.OrderDetails)
            {
                var game = _gameService.GetById(item.GameId.ToString());
                if (game == null)
                {
                    throw new ValidationException("There is no game with such id", "GameNotFound");
                }

                game.UnitsInStock += item.Quantity;
                _gameService.EditEntity(game);
            }

            _logger.Debug("Deleting busket by Customer ={0} ", username);
        }

        #endregion

        #region Edit

        public void ChangeStatus(string username, bool isShipped)
        {
            var payedOrder =
                _unitOfWork.Repository<Order>()
                    .FindBy(x => x.User!=null && x.User.Username == username)
                    .FirstOrDefault(x => x.IsConfirmed);

            if (payedOrder == null)
            {
                throw new ValidationException("There is no order for payed", "Order");
            }

            payedOrder.IsShipped = isShipped;
            _unitOfWork.Repository<Order>().Edit(payedOrder);
        }

        public void Pay(string username)
        {
            var payedOrder =
                _unitOfWork.Repository<Order>()
                    .FindBy(x => x.User != null && x.User.Username == username)
                    .FirstOrDefault(x => x.IsConfirmed && !x.IsShipped);

            if (payedOrder == null)
            {
                throw new ValidationException("There is no order for payed", "Order");
            }

            payedOrder.IsPayed = true;
            _unitOfWork.Repository<Order>().Edit(payedOrder);
            IObservable observable = MailServer.CreateServer(_unitOfWork);
            observable.NotifyObserver(payedOrder);
        }

        public OrderDTO ConfirmeOrder(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ValidationException("The id isn't correct", "Id");
            }

            var order = _unitOfWork.Repository<Order>()
                .FindBy(x => x.User != null && x.User.Username == username)
                .FirstOrDefault(x => !x.IsConfirmed);

            if (order == null)
            {
                throw new ValidationException("Order wasn't found", "OrderNotFound");
            }

            _logger.Debug("Confirming order by usern={0} ", username);
            order.IsConfirmed = true;
            _unitOfWork.Repository<Order>().Edit(order);
            return Mapper.Map<OrderDTO>(order);
        }

        public void UpdateOrder(string username, OrderDetailDTO product, bool isBasket)
        {
            var productEntity = Mapper.Map<OrderDetail>(product);
            var user = _userService.GetUserEntityByName(username);
            var orders =
                _unitOfWork.Repository<Order>().FindBy(x => x.User != null && x.User.Username == username);
            var order = (isBasket)
                ? orders.FirstOrDefault(x => !x.IsConfirmed && !x.IsPayed)
                : orders.FirstOrDefault(x => x.IsConfirmed);
            if (order == null)
            {
                throw new ValidationException("Order wasn't found", "OrderNotFound");
            }

            var game = _unitOfWork.Repository<Game>().FindBy(x => x.Key == product.GameKey).First();
            productEntity.OrderId = order.Id;
            productEntity.Game = game;
            if (order.OrderDetails.Any(x=>x.GameId==productEntity.GameId))
            {
                var orderDetail = order.OrderDetails.First(x => x.Id == productEntity.Id);
                orderDetail.Price = productEntity.Price;
                orderDetail.Quantity = productEntity.Quantity;
                orderDetail.Game = productEntity.Game;
                _unitOfWork.Repository<OrderDetail>().Edit(orderDetail);
            }
            else
            {
                order.OrderDetails.Add(productEntity);
                var orderDetails = order.OrderDetails;
                foreach (var item in orderDetails)
                {
                    item.Game = item.Game ?? _unitOfWork.Repository<Game>().GetSingle(item.GameId.ToString());
                }
                order.User = user;
                _unitOfWork.Repository<Order>().Edit(order);
            }
        }

        #endregion

        private void RegisterFilter(Pipeline<Order> pipeline, OrderFilterDTO filter, int page = 1,
            PageEnum pageSize = PageEnum.Ten)
        {
            if (filter.DateFrom != new DateTime())
            {
                pipeline.Register(new DateFromFilter(filter.DateFrom));
            }

            if (filter.DateTo != new DateTime())
            {
                pipeline.Register(new DateToFilter(filter.DateTo));
            }

            pipeline.Register(new FilterBy(Filter.New))
                .Register(new PageFilter<Order>(page, pageSize));
        }
    }
}