using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ninject.Infrastructure.Language;
using GameStore.BLL.Infrastructure;

namespace GameStore.BLL.Services
{
    public class GamesService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IEnumerable<GameDTO> _gamesDto;

        public GamesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _gamesDto = Mapper.Map<IEnumerable<GameDTO>>(
                    _unitOfWork.Repository<Game>().GetAll());
        }

        private static object GetPropertyValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private IEnumerable<Game> GetGamesByListObject<T>(string name) where T : class, IDtoNamed, new()
        {
            var modelName = typeof(T).Name;
            var games = _unitOfWork.Repository<Game>().GetAll();
            var modelList = games.Where(x =>
                    ((IEnumerable<IEntityNamed>)
                            GetPropertyValue(x, modelName.Substring(0, modelName.Length - 3) + "s"))
                        .Select(m => m.Name)
                        .Contains(name)).ToList();
            return modelList;
        }

        private IEnumerable<Game> GetGamesByObject<T>(string name) where T : class, IDtoNamed, new()
        {
            var modelName = typeof(T).Name;
            var games = _unitOfWork.Repository<Game>().GetAll();
            var modelList = games.Where(x =>
                       ((IEntityNamed)GetPropertyValue(x, modelName.Substring(0, modelName.Length - 3))).Name
                           .Contains(name)).ToList();
            return modelList;
        }

        public IEnumerable<GameDTO> GetGamesByNameOfProperty<T>(IEnumerable<GameDTO> gamesDto, string name) where T : class, IDtoNamed, new()
        {
            var modelList = ((typeof(T).Name) == "PublisherDTO")
                ? GetGamesByObject<T>(name)
                : GetGamesByListObject<T>(name);
            var dtoList = Mapper.Map<IEnumerable<GameDTO>>(modelList).ToList();
            if (gamesDto != null)
            {
                dtoList.Union(gamesDto);
            }
            return dtoList;
        }

        public IEnumerable<GameDTO> GetGamesByModelId<T>(string id) where T: class, IDtoBase, new()
        {
            var modelName = typeof(T).Name;
            var modelList =
                _unitOfWork.Repository<Game>()
                    .FindBy(
                        x =>
                            ((IEnumerable<IEntityBase>) GetPropertyValue(x, modelName.Substring(0, modelName.Length-3))).Select(m => m.Id)
                                .Contains(Guid.Parse(id))).ToList();
            return Mapper.Map<IEnumerable<GameDTO>>(modelList);
        }

        public IEnumerable<GameDTO> GetGamesByNameOfProperty<T>( string name) where T : class, IDtoNamed, new()
        {
            if (string.IsNullOrEmpty(name))
            {
                return Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().GetAll());
            }
            var modelName = typeof(T).Name;
            IEnumerable<Game> modelList;
            if (modelName == "PublisherDTO")
            {
                modelList = _unitOfWork.Repository<Game>().GetAll().Where(x =>
                    ((IEntityNamed)GetPropertyValue(x, modelName.Substring(0, modelName.Length - 3))).Name
                        .Contains(name)).ToList();
            }
            else
            {
                modelList = _unitOfWork.Repository<Game>().GetAll().Where(x =>
                    ((IEnumerable<IEntityNamed>)
                            GetPropertyValue(x, modelName.Substring(0, modelName.Length - 3) + "s"))
                        .Select(m => m.Name)
                        .Contains(name)).ToList();
            }
            return Mapper.Map<IEnumerable<GameDTO>>(modelList);
        }

        public IEnumerable<GameDTO> MostViewed(IEnumerable<GameDTO> games)
        {
            var gamesDto = games.OrderByDescending(x => x.Viewing).Take(8);
            _logger.Debug("Getting most viewed games");
            return gamesDto;
        }

        public IEnumerable<GameDTO> Filter(IEnumerable<GameDTO> games, string filter)
        {
            var gamesDto = (games==null && !games.Any()) ? Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().GetAll()) : games ;
            switch (filter)
            {
                case "1":
                    return MostViewed(gamesDto);
                case "2":
                    return MostCommented(gamesDto);
                case "3":
                    return GetGamesByPriceFilter(gamesDto, true);
                case "4":
                    return GetGamesByPriceFilter(gamesDto, false);
                case "5":
                    return GetNewGames(gamesDto);
                default:
                    return gamesDto;
            }
        }

        public IEnumerable<GameDTO> MostCommented(IEnumerable<GameDTO> games)
        {
            var gamesDto = games.OrderByDescending(x => x.Comments.Count).Take(8);
            _logger.Debug("Getting most commented games");
            return gamesDto;
        }

        public IEnumerable<GameDTO> GetGamesByPriceFilter(IEnumerable<GameDTO> gamesDto, bool isAsc)
        {
            var games = (isAsc)
                ? gamesDto.OrderBy(x => x.Price)
                : gamesDto.OrderByDescending(x => x.Price);
            _logger.Debug("Getting games by price");
            return games;
        }

        public IEnumerable<GameDTO> GetNewGames(IEnumerable<GameDTO> gamesDto)
        {
            var games = gamesDto.OrderBy(x => x.DateOfAdding).Take(8);
            _logger.Debug("Getting new games");
            return games;
        }

        public IEnumerable<GameDTO> GetGamesByPriceRange(IEnumerable<GameDTO> gamesDto, decimal from, decimal to)
        {
            if (from>to)
            {
                throw new ValidationException("The range isn't correct", string.Empty);
            }
            var games =
                gamesDto.Where(x => x.Price > from && x.Price < to);
            _logger.Debug("Getting games by price < {0} and price > {1}", from, to);
            return games;
        }

        private DateTime GetDate(string date)
        {
            var dateNow = DateTime.UtcNow;
            switch (date)
            {
                case "last week":
                    return dateNow.AddDays(-7);
                case "last month":
                    return dateNow.AddMonths(-1);
                case "2 year":
                    return dateNow.AddYears(-2);
                case "3 year":
                    return dateNow.AddYears(-3);
            }
            throw new ValidationException("The date isn't correct", string.Empty);
        }

        public IEnumerable<GameDTO> GetGamesByDate(IEnumerable<GameDTO> gamesDto, string date)
        {
            return gamesDto.Where(x => x.DateOfAdding >= GetDate(date));           
        }

        public IEnumerable<GameDTO> GetGamesByName(IEnumerable<GameDTO> gamesDto, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ValidationException("The name is empty", string.Empty);
            }
            return gamesDto.Where(x => x.Name.ToLower().Contains(name.ToLower()));
        }
    }
}
