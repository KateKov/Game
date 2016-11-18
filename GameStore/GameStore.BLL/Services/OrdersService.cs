using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using NLog;
using System;
using System.Linq;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.Services
{
    public class OrdersService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly DtoToDomain _dtoToDomain;

        public OrdersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dtoToDomain = new DtoToDomain(unitOfWork);
        }

        public void GetOrderDetail(string gameId, short quantity, string customerId = "")
        {
            var game = new GameStoreService(_unitOfWork).GetById<GameDTO>(gameId);
            if (game == null)
            {
                throw new ValidationException("There is no game with such id", string.Empty);
            }
            var orderDetailEntity =
                _unitOfWork.Repository<OrderDetail>().FindBy(x => x.GameId.ToString() == gameId).FirstOrDefault();
            var orderDetailDto = (orderDetailEntity != null)
                ? GetExistingOrder(orderDetailEntity, game, quantity)
                : GetNewOrder(game, quantity, gameId);
            AddToBusket(orderDetailDto, customerId);
        }

        private OrderDetailDTO GetExistingOrder(OrderDetail orderDetailEntity, GameDTO game, short quantity)
        {
            var orderDetailDto = Mapper.Map<OrderDetailDTO>(orderDetailEntity);
            orderDetailDto.Quantity += quantity;
            orderDetailDto.Price += quantity * game.Price;
            return orderDetailDto;
        }

        private OrderDetailDTO GetNewOrder( GameDTO game,
            short quantity, string gameId)
        {
            var orderDetailDto = new OrderDetailDTO()
            {
                Id = Guid.NewGuid().ToString(),
                Discount = 0,
                GameId = gameId,
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
            var guidId = Guid.Parse(id);
            var order = _unitOfWork.Repository<Order>().GetSingle(guidId);
            if (order == null)
            {
                throw new ValidationException("Order wasn't found", string.Empty);
            }
            _logger.Debug("Confirming order by id={0} ", id);
            order.IsConfirmed = true;
            _unitOfWork.Repository<Order>().Edit(order);
            return Mapper.Map<OrderDTO>(order);
        }

        public OrderDTO GetBusket(string customerId)
        {
            var basket = _unitOfWork.Repository<Order>().FindBy(x => !x.IsConfirmed).FirstOrDefault();
            _logger.Debug("Getting basket by Customer id={0} ", customerId);
            return Mapper.Map<OrderDTO>(basket);
        }

        public void DeleteBusket(string customerId)
        {
            var busket = _unitOfWork.Repository<Order>().FindBy(x => !x.IsConfirmed).FirstOrDefault();
            if (busket == null)
            {
                throw new ValidationException("Busket wasn't found", string.Empty);
            }
            _unitOfWork.Repository<Order>().Delete(busket);
            _logger.Debug("Deleting busket by Customer id={0} ", customerId);
        }

        public OrderDTO GetOrders(string customerId)
        {
            var orders = _unitOfWork.Repository<Order>().FindBy(x => x.IsConfirmed).First();
            if (orders == null)
            {
                throw new ValidationException("Busket wasn't found", string.Empty);
            }
            _logger.Debug("Getting order by Customer id={0} ", customerId);
            return Mapper.Map<OrderDTO>(orders);
        }

        private void AddToBusket(OrderDetailDTO product, string customerId)
        {
            Validator<OrderDetailDTO>.ValidateModel(product);
            var busket = _unitOfWork.Repository<Order>().GetAll().FirstOrDefault();
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
            productEntity.OrderId = busket.Id;
            if (busket.OrderDetails.Count(x => x.Id == productEntity.Id) > 0)
            {
                var orderDetail = busket.OrderDetails.First(x => x.Id == productEntity.Id);
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
        private void AddNewOrderToBasket(OrderDetailDTO product)
        {
            var order = new OrderDTO() { Id = Guid.NewGuid().ToString(), Date = DateTime.UtcNow, IsConfirmed = false };
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
