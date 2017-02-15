using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Infrastructure.Filters;
using GameStore.BLL.Infrastructure.Paging;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Enums;
using GameStore.DAL.Interfaces;
using NLog;

namespace GameStore.BLL.Services
{
    public class GamesService : NameService<Game, GameDTO, GameTranslate, GameDTOTranslate>, IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IDtoToDomainMapping _dtoToDomain;
        private readonly ITranslateService<Game, GameDTO> _translateService;

        public GamesService(IUnitOfWork unitOfWork) :base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dtoToDomain = new DtoToDomainMapping(_unitOfWork);
            _translateService = new TranslateService<Game, GameDTO>(_unitOfWork);
        }

        public GameDTO GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ValidationException("Key isn't correct", "Key");
            }

            var entity = _unitOfWork.Repository<Game>().FindBy(x => x.Key == key).FirstOrDefault();
            if (entity == null)
            {
                throw new ValidationException(typeof(Game).Name + " wasn't found", string.Empty);
            }

            _logger.Debug("Getting {0} by key={1} ", typeof(Game).Name, key); 

            return Mapper.Map<GameDTO>(entity); 
        }

        public void DeleteByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ValidationException("Key isn't correct", "Key");
            }

            var entity = _unitOfWork.Repository<Game>().FindBy(x => x.Key == key).FirstOrDefault();
            if (entity == null)
            {
                throw new ValidationException(typeof(Game).Name + " wasn't found", string.Empty);
            }

            _logger.Debug("Deleting {0} by key={1} ", typeof(Game).Name, key);

            _unitOfWork.Repository<Game>().Delete(entity);
        }

        public override void AddEntity(GameDTO model)
        {
            Validator.Validate(model);
            base.AddEntity(model);
        }

        public override void EditEntity(GameDTO model)
        {
            Validator.Validate(model);
            var game = _unitOfWork.Repository<Game>().FindBy(x=>x.Key == model.Key).FirstOrDefault();
            if (game == null)
            {
                throw new ValidationException("There is no games in with such data", "Game");
            }

            game.Discountinues = model.Discountinues;
            game.UnitsInStock = model.UnitsInStock;
            game.DateOfAdding = DateTime.UtcNow;
            game.Price = model.Price;
            game.FilePath = model.FilePath;

            if (game.Genres != null)
            {
                game.Genres.Clear();
            }

            if (game.PlatformTypes != null)
            {
                game.PlatformTypes.Clear();
            }

            var result = (Game)_dtoToDomain.AddEntities(game, model);

            _unitOfWork.Repository<Game>().Edit(result);
            _translateService.EditTranslate(result, model);  
            _unitOfWork.Save();
        }

        public FilterResultDTO GetAllByFilter(FilterDTO filter, bool isWithDeleted = false, int page = 1, PageEnum pageSize = PageEnum.Ten) 
        {
            if (filter == null)
            {
                throw new ValidationException("There is no filter", string.Empty);
            }

            var pipeline = new Pipeline<Game>();
            RegisterFilter(pipeline, filter, page, pageSize);
            var query = pipeline.Execute();
            var result = _unitOfWork.Repository<Game>().GetAll(query, isWithDeleted);
            var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(result.List.ToList());
            var filterResult = new FilterResultDTO
            {
                Games = gamesDto,
                Count = result.Count
            };
            return filterResult;
        }

   
        private void RegisterFilter(IPipeLine<Game> pipeline, FilterDTO filter, int page = 1, PageEnum pageSize = PageEnum.Ten)
        {
            if (filter.SelectedGenresName != null && filter.SelectedGenresName.Any())
            {
                pipeline.Register(new GenreFilter(filter.SelectedGenresName));
            }

            if (filter.MaxPrice != null)
            {
                pipeline.Register(new MaxPriceFilter(filter.MaxPrice.Value));
            }

            if (filter.MinPrice != null)
            {
                pipeline.Register(new MinPriceFilter(filter.MinPrice.Value));
            }

            if (filter.SelectedTypesName != null && filter.SelectedTypesName.Any())
            {
                pipeline.Register(new PlatformTypeFilter(filter.SelectedTypesName));
            }

            if (filter.SelectedPublishersName != null && filter.SelectedPublishersName.Any())
            {
                pipeline.Register(new PublisherFilter(filter.SelectedPublishersName));
            }

            if (filter.Name != null && filter.Name.Length >= 3)
            {
                pipeline.Register(new SearchNameFilter(filter.Name));
            }            

            pipeline.Register(new DateFilter(filter.DateOfAdding))
                .Register(new FilterBy(filter.FilterBy))
                .Register(new PageFilter<Game>(page, pageSize));
        }
    }
}