using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Infrastructure.Filters;
using GameStore.BLL.Infrastructure.Paging;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Interfaces;
using NLog;

namespace GameStore.BLL.Services
{
    public class GamesService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly DtoToDomain _dtoToDomain;

        public GamesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dtoToDomain = new DtoToDomain(unitOfWork);
        }

        public FilterResultDTO GetAllByFilter(FilterDTO filter, int page = 1,
            PageEnum pageSize = PageEnum.Ten)
        {
            if (filter == null)
            {
                throw new ValidationException("There is no filter", string.Empty);
            }
            var pipeline = new Pipeline<Game>();
            RegisterFilter(pipeline, filter, page, pageSize);
            var query = pipeline.Execute();
            var result = _unitOfWork.Repository<Game>().GetAll(query);
            var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(result.List.ToList());
            var filterResult = new FilterResultDTO
            {
                Games = gamesDto,
                Count = result.Count
            };
            return filterResult;
        }

   
        private void RegisterFilter(Pipeline<Game> pipeline, FilterDTO filter, int page = 1, PageEnum pageSize = PageEnum.Ten)
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