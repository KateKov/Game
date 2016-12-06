using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Infrastructure.OrderFilter;
using GameStore.BLL.Infrastructure.Paging;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.EF;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Services
{
    public class OrdersService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly DtoToDomain _dtoToDomain;
        private readonly IGameStoreService _gameStoreService;

        public OrdersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dtoToDomain = new DtoToDomain(unitOfWork);
            _gameStoreService = new GameStoreService(_unitOfWork);
        }

        public void GetOrderDetail(string gameKey, short quantity, string customerId = "")
        {
            if (gameKey.Length == 24 && _gameStoreService.KeyService<GameDTO>().GetByKey(gameKey)!=null && _gameStoreService.KeyService<GameDTO>().GetByKey(gameKey).Id.Length!=Guid.Empty.ToString().Length)
            {
                var gameMongo = _gameStoreService.GenericService<GameDTO>().GetById(gameKey);
                gameMongo.Id = Guid.NewGuid().ToString();
                _gameStoreService.GenericService<GameDTO>().AddOrUpdate(gameMongo, true);
            }
            var game = _gameStoreService.KeyService<GameDTO>().GetByKey(gameKey);
            game.UnitsInStock -= quantity;
             _gameStoreService.GenericService<GameDTO>().AddOrUpdate(game, false);
            if (game == null)
            {
                throw new ValidationException("There is no game with such key", string.Empty);
            }
            var orderDetailEntity =
                _unitOfWork.Repository<OrderDetail>().FindBy(x => x.Game.Key == gameKey).FirstOrDefault(x=>x.IsPayed==false);
            var orderDetailDto = (orderDetailEntity != null)
                ? GetExistingOrder(orderDetailEntity, game, quantity)
                : GetNewOrder(game, quantity);
            AddToBusket(orderDetailDto, customerId);
        }

        public OrderDetailDTO GetExistingOrder(OrderDetail orderDetailEntity, GameDTO game, short quantity)
        {
            var orderDetailDto = Mapper.Map<OrderDetailDTO>(orderDetailEntity);
            orderDetailDto.Quantity += quantity;
            orderDetailDto.Price += quantity * game.Price;
            return orderDetailDto;
        }

        private OrderDetailDTO GetNewOrder( GameDTO game,
            short quantity)
        {
            var orderDetailDto = new OrderDetailDTO()
            {
                Id = Guid.NewGuid().ToString(),
                Discount = 0,
                GameId = game.Id,
                Price = game.Price * quantity,
                Quantity = quantity
            };
            return orderDetailDto;
        }

        public OrderDTO ConfirmeOrder(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ValidationException("The id isn't correct", string.Empty);
            }
            var order = _unitOfWork.Repository<Order>().GetSingle(id);
            if (order == null)
            {
                throw new ValidationException("Order wasn't found", string.Empty);
            }
            _logger.Debug("Confirming order by id={0} ", id);
            order.IsConfirmed = true;
            _unitOfWork.Repository<Order>().Edit(order);
            return Mapper.Map<OrderDTO>(order);
        }

        private void RegisterFilter(Pipeline<Order> pipeline, OrderFilterDTO filter, int page = 1, PageEnum pageSize = PageEnum.Ten)
        {
            if (filter.DateFrom!= new DateTime())
            {
                pipeline.Register(new DateFromFilter(filter.DateFrom));
            }

            if (filter.DateTo != new DateTime())
            {
                pipeline.Register(new DateToFilter(filter.DateTo));
            }
            pipeline.Register(new Infrastructure.OrderFilter.FilterBy(Filter.New))
                .Register(new PageFilter<Order>(page, pageSize));
        }

        public OrderFilterResultDTO GetOrdersByFilter(OrderFilterDTO filter, int page = 1,
          PageEnum pageSize = PageEnum.Ten)
        {
            if (filter == null)
            {
                throw new ValidationException("There is no filter", string.Empty);
            }
            var pipeline = new Pipeline<Order>();
            RegisterFilter(pipeline, filter, page, pageSize);
            var query = pipeline.Execute();
            var result = _unitOfWork.Repository<Order>().GetAll(query);
            var ordersDto = Mapper.Map<IEnumerable<OrderDTO>>(result.List.ToList());
            var filterResult = new OrderFilterResultDTO
            {
                Orders = ordersDto,
                Count = result.Count
            };
            return filterResult;
        }

        public OrderDTO GetBusket(string customerId)
        {
            var basket =
                _unitOfWork.Repository<Order>().FindBy(x => !x.IsConfirmed).FirstOrDefault(x => (x.CustomerId == "" || string.IsNullOrEmpty(x.CustomerId))&& !x.IsPayed);
            _logger.Debug("Getting basket by Customer id={0} ", customerId);
            return Mapper.Map<OrderDTO>(basket);
        }

        public void DeleteBusket(string customerId)
        {
            var busket = _unitOfWork.Repository<Order>().FindBy(x => !x.IsConfirmed).FirstOrDefault(x => !x.IsPayed);
            if (busket == null)
            {
                throw new ValidationException("Busket wasn't found", string.Empty);
            }
            _unitOfWork.Repository<Order>().Delete(busket);
            _logger.Debug("Deleting busket by Customer id={0} ", customerId);
        }

        public OrderDTO GetOrders()
        {
            var orders = _unitOfWork.Repository<Order>().FindBy(x => x.IsConfirmed).FirstOrDefault(x => (x.CustomerId == "" || string.IsNullOrEmpty(x.CustomerId)) &&x.IsPayed==false);
            if (orders == null)
            {
                orders = new Order() {EntityId = Guid.NewGuid()};
            }
            _logger.Debug("Getting order by Customer id={0} ");
            return Mapper.Map<OrderDTO>(orders);
        }

        public void AddToBusket(OrderDetailDTO product, string customerId)
        {
            Validator<OrderDetailDTO>.ValidateModel(product);
            var busket = _unitOfWork.Repository<Order>().FindBy(x=>x.CustomerId=="").FirstOrDefault(x=>x.IsPayed==false);
            if (busket == null)
            {
                AddNewOrderToBasket(product);
            }
            else
            {
                UpdateOrderInBasket(busket, product);
            }
            _logger.Debug("Adding product with Game {0} to the busket of Customer {1} ", product.GameId, customerId);

        }

        private void UpdateOrderInBasket(Order busket, OrderDetailDTO product)
        {
            var productEntity = Mapper.Map<OrderDetail>(product);
            productEntity.OrderId = busket.EntityId;
            if (busket.OrderDetails.Count(x => x.EntityId == productEntity.EntityId) > 0)
            {
                var orderDetail = busket.OrderDetails.First(x => x.EntityId == productEntity.EntityId);
                orderDetail.Price = productEntity.Price;
                orderDetail.Quantity = productEntity.Quantity;
                _unitOfWork.Repository<OrderDetail>().Edit(orderDetail);
            }
            else
            {
                busket.OrderDetails.Add(productEntity);
                _unitOfWork.Repository<Order>().Edit(busket);
            }
        }

        public void PayOrder()
        {
            var payedOrder = _unitOfWork.Repository<Order>().FindBy(x => x.CustomerId == "").FirstOrDefault(x => !x.IsPayed && x.IsConfirmed);
            var orderDetails = payedOrder.OrderDetails;
            orderDetails.ToList().ForEach(x=>x.IsPayed=true);
            payedOrder.IsPayed = true;
            _unitOfWork.Repository<Order>().Edit(payedOrder);
            orderDetails.ToList().ForEach(x=>_unitOfWork.Repository<OrderDetail>().Edit(x));
        }

        public void AddNewOrderToBasket(OrderDetailDTO product)
        {
            var order = new OrderDTO() { Id = Guid.NewGuid().ToString(), Date = DateTime.UtcNow, IsConfirmed = false, CustomerId = ""};
            var productOrder = product;
            productOrder.OrderId = order.Id;
            var orderEntity = Mapper.Map<Order>(order);
            _unitOfWork.Repository<Order>().Add(orderEntity);
            var orderDetailEntity = Mapper.Map<OrderDetail>(productOrder);
            orderDetailEntity = (OrderDetail) _dtoToDomain.AddEntities(orderDetailEntity, productOrder);
            _unitOfWork.Repository<OrderDetail>().Add(orderDetailEntity);
        }
    }
}
