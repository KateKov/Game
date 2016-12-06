using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.DAL.EF;
using GameStore.DAL.Entities;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Enums;
using GameStore.DAL.Infrastracture;
using GameStore.DAL.MongoEntities;
using GameStore.DAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameStore.DAL.Mapping
{
    public class MongoToSql : Profile
    {
        private static readonly IMongoRepository<Category> _categoryRepository;
        private static readonly IMongoRepository<Supplier> _supplierRepository;
        private static readonly IMongoRepository<OrderDetailMongo> _orderDetailsRepository;
        private static readonly IMongoRepository<OrderMongo> _ordersRepository;
        private static readonly IMongoRepository<Product> _productRepository;
        public override string ProfileName
        {
            get { return "MongoToSql"; }
        }

        static MongoToSql()
        {
            _categoryRepository = new MongoRepository<Category>();
            _supplierRepository = new MongoRepository<Supplier>();
            _orderDetailsRepository = new MongoRepository<OrderDetailMongo>();
            _ordersRepository = new MongoRepository<OrderMongo>();
            _productRepository =new MongoRepository<Product>();
        }

        private List<OrderDetail> GetListOrderDetail(int orderId, Guid orderGuid)
        {
            var orderDetailsMongo = _orderDetailsRepository.FindBy(x=>x.OrderID == orderId).ToList();
            var orderDetails = new List<OrderDetail>();
            orderDetailsMongo.ForEach(x=>orderDetails.Add(new OrderDetail() { GameId = Guid.NewGuid() , Discount = (float) x.Discount, EntityId = Guid.NewGuid(), Id = x.Id, Price = x.UnitPrice, Quantity = x.Quantity, OrderId = orderGuid}));
            return orderDetails;
        }

        public T GetEntityFromMongo<T, TD>(TD entity) where T : class, IEntityBase, new() where TD: class, IMongoEntity, new()
        {
            switch (typeof(TD).Name)
            {
                case "Product":
                    return GetGame(entity as Product) as T;
                case "OrderMongo":
                    return GetOrder(entity as OrderMongo) as T;
                default:
                    return null;
            }
        }

        public Game GetGame(Product product)
        {
            var game = new Game()
            {
                EntityId = Guid.NewGuid(),
                Key = product.Id,
                DateOfAdding = DateTime.UtcNow,
                Id = product.Id,
                Discountinues = product.Discountinued,
                Translates = new List<GameTranslate>()
                {
                    new GameTranslate() {EntityId = Guid.NewGuid(), Language = Language.En, Name = product.ProductName}
                },
                Price = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                Viewing = 10,
                Publisher = GetPublisher(product.SupplierID),
                Genres = new List<Genre>() {GetGenre(product.CategoryID)}
            };
            game.PublisherId = game.Publisher.EntityId;
            return game;
        }

        public Order GetOrder(OrderMongo orderMongo)
        {
            var id = Guid.NewGuid();
            var order = new Order()
            {
                EntityId = id,
                CustomerId = orderMongo.CustomerID,
                Date = GetRandomDate(),
                Id = orderMongo.Id,
                IsConfirmed = true,
                OrderDetails = GetListOrderDetail(orderMongo.OrderID, id)
            };
            order.Sum = order.OrderDetails.Select(x => x.Price).Sum();
            return order;
        }

        private Game GetGameByProductId(int productId)
        {
            var gameMongo = _productRepository.FindBy(x => x.ProductID == productId).FirstOrDefault();
            if (gameMongo != null)
            {
                var game = new CommonRepository<Game, Product>(new GameStoreContext()).FindBy(x=>x.Key==gameMongo.Id).FirstOrDefault();
                return game;
            }
            return new Game() {EntityId = Guid.NewGuid()};
        }

        private Order GetOrder(int orderId)
        {
            var orderMongo = _ordersRepository.FindBy(x => x.OrderID == orderId).FirstOrDefault();
            if (orderMongo != null)
            {
                var order =
                    new CommonRepository<Order, OrderMongo>(new GameStoreContext()).FindBy(
                        x => x.Id == orderMongo.Id).FirstOrDefault();
                return order;
            }
            return new Order() { EntityId = Guid.NewGuid() };
        }

        private DateTime GetRandomDate()
        {
            var random = new Random();
            var years = random.Next(2000, 2016);
            var days = random.Next(1, 28);
            var month = random.Next(1, 12);
            return DateTime.Parse(years+"-"+month+"-"+days).Date;
        }

        private void CategoryToGenre()
        {
            CreateMap<Category, Genre>()
                .ForMember(dm => dm.Translates, map => map.MapFrom(dm => new List<GenreTranslate>() {new GenreTranslate() {EntityId = Guid.NewGuid(), Language = Language.En, Name = dm.CategoryName} }))
                .ForMember(dm => dm.Id, map => map.MapFrom(dm => dm.MongoId.ToString()));
        }

        private void GenreToCategory()
        {
            CreateMap<Genre, Category>()
                .ForMember(dm => dm.CategoryName, map => map.MapFrom(dm => (dm.Translates.FirstOrDefault(x => x.Language == Language.En) !=null)?dm.Translates.FirstOrDefault(x => x.Language == Language.En).Name:""))
                .ForMember(dm => dm.CategoryID, map => map.MapFrom(dm =>  _supplierRepository.GetSingle(dm.Id).SupplierID))
                .ForMember(dm => dm.MongoId, map => map.MapFrom(dm => new ObjectId(dm.Id)));
        }

        private void OrderDetailToSql()
        {
            CreateMap<OrderDetailMongo, OrderDetail>()
                .ForMember(dm => dm.Price, map => map.MapFrom(dm => dm.UnitPrice))
                .ForMember(dm => dm.Id, map => map.MapFrom(dm => dm.MongoId.ToString()))
                .ForMember(dm => dm.Quantity, map => map.MapFrom(dm => dm.Quantity))
                .ForMember(dm => dm.Game, map => map.MapFrom(dm => GetGameByProductId(dm.ProductId)))
                .ForMember(dm => dm.EntityId, map => map.MapFrom(dm => Guid.NewGuid()))
                .ForMember(dm => dm.OrderId, map => map.MapFrom(dm => GetOrder(dm.OrderID).EntityId))
                 .ForMember(dm => dm.Order, map => map.MapFrom(dm => GetOrder(dm.OrderID)))
                .ForMember(dm => dm.Discount, map => map.MapFrom(dm => dm.Discount));
        }

        private void OrderToSql()
        {
            var id = Guid.NewGuid();
            CreateMap<OrderMongo, Order>()
                .ForMember(dm => dm.CustomerId, map => map.MapFrom(dm => dm.CustomerID))
                .ForMember(dm => dm.Date, map => map.MapFrom(dm => GetRandomDate()))
                .ForMember(dm => dm.Id, map => map.MapFrom(dm => dm.MongoId.ToString()))
                .ForMember(dm => dm.IsConfirmed, map => map.MapFrom(dm => true))
                .ForMember(dm => dm.OrderDetails, map => map.MapFrom(dm => GetListOrderDetail(dm.OrderID, id)))
                .ForMember(dm => dm.Sum, map => map.MapFrom(dm => GetListOrderDetail(dm.OrderID, id).Select(x => x.Price).Sum()))
                .ForMember(dm => dm.EntityId, map => map.MapFrom(dm => Guid.NewGuid()));
        }

        private void PublisherToSupplier()
        {
            CreateMap<Publisher, Supplier>()
                .ForMember(dm => dm.CompanyName, map => map.MapFrom(dm => (dm.Translates.FirstOrDefault(x => x.Language == Language.En) != null) ? dm.Translates.FirstOrDefault(x => x.Language == Language.En).Name : ""))
                .ForMember(dm => dm.MongoId, map => map.MapFrom(dm => new ObjectId(dm.Id)))
                .ForMember(dm => dm.SupplierID,
                    map => map.MapFrom(dm => _supplierRepository.GetSingle(dm.Id).SupplierID));
        }

        private void SupplierToPublisher()
        {
            CreateMap<Supplier, Publisher>()
               .ForMember(dm => dm.Translates, map => map.MapFrom(dm => new List<PublisherTranslate>() { new PublisherTranslate() { EntityId = Guid.NewGuid(), Language = Language.En, Name = dm.CompanyName } }))
                .ForMember(dm => dm.Id, map => map.MapFrom(dm => dm.MongoId.ToString()));
        }

        private void GameToProduct()
        {
            CreateMap<Game, Product>()
                .ForMember(dm => dm.MongoId, map => map.MapFrom(dm => new ObjectId(dm.Id)))
                .ForMember(dm => dm.Discountinued, map => map.MapFrom(dm => dm.Discountinues))
                .ForMember(dm => dm.UnitsInStock, map => map.MapFrom(dm => dm.UnitsInStock))
                .ForMember(dm => dm.UnitPrice, map => map.MapFrom(dm => dm.Price))
                .ForMember(dm => dm.CategoryID,
                 map => map.MapFrom(dm => (_productRepository.GetSingle(dm.Key)!=null)?_productRepository.GetSingle(dm.Key).CategoryID:0))
                .ForMember(dm => dm.SupplierID,
                 map => map.MapFrom(dm => (_productRepository.GetSingle(dm.Key)!=null)? _productRepository.GetSingle(dm.Key).SupplierID:0));
        }

        private Publisher GetPublisher(int supplierId)
        {
            return (_supplierRepository.FindBy(x=>x.SupplierID==supplierId).Any())? new Publisher()
            {
                Id = _supplierRepository.FindBy(x => x.SupplierID == supplierId).First().MongoId.ToString(),
                Translates = new List<PublisherTranslate>() { new PublisherTranslate() { EntityId = Guid.NewGuid(), Language = Language.En, Name = _supplierRepository.FindBy(x => x.SupplierID == supplierId).First().CompanyName } },
                HomePage = _supplierRepository.FindBy(x => x.SupplierID == supplierId).First().HomePage ?? ""
            } : new Publisher() {EntityId = Guid.NewGuid()};
        }

        private Genre GetGenre(int categoryId)
        {
            return (_categoryRepository.FindBy(x => x.CategoryID == categoryId).Any())
                ? new Genre()
                {
                    Id = _categoryRepository.FindBy(x => x.CategoryID == categoryId).First().MongoId.ToString(),
                    Translates = new List<GenreTranslate>() { new GenreTranslate() { EntityId = Guid.NewGuid(), Language = Language.En, Name = _categoryRepository.FindBy(x => x.CategoryID == categoryId).First().CategoryName } },
                    EntityId = Guid.NewGuid()
                }
               : new Genre() {EntityId = Guid.NewGuid()};
        }

        private void ProductToGame()
        {
            CreateMap<Product, Game>()
               .ForMember(dm => dm.Id, map => map.MapFrom(dm => dm.MongoId.ToString()))
               .ForMember(dm => dm.DateOfAdding, map => map.MapFrom(dm => DateTime.UtcNow))
               .ForMember(dm => dm.Translates, map => map.MapFrom(dm => new List<GameTranslate>() {new GameTranslate() {EntityId = Guid.NewGuid(), Language = Language.En, Name = dm.ProductName} }))
               .ForMember(dm => dm.UnitsInStock, map => map.MapFrom(dm => dm.UnitsInStock))
               .ForMember(dm => dm.Viewing, map => map.MapFrom(dm => 10))
               .ForMember(dm => dm.Publisher,
                   map =>
                       map.MapFrom(
                           dm => GetPublisher(dm.SupplierID)))
               .ForMember(dm => dm.Genres, map => map.MapFrom(dm => new List<Genre>()
               { GetGenre(dm.CategoryID)}))
               .ForMember(dm => dm.Discountinues, map => map.MapFrom(dm => dm.Discountinued))
               .ForMember(dm => dm.Price, map => map.MapFrom(dm => dm.UnitPrice));
        }

        [Obsolete]
        protected override void Configure()
        {
            CategoryToGenre();
            GenreToCategory();
            OrderDetailToSql();
            OrderToSql();
            PublisherToSupplier();
            SupplierToPublisher();
            GameToProduct();
            ProductToGame();
        }
    }
}
