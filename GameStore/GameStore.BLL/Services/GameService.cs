using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Infrastracture;
using GameStore.BLL.Infrastracture;
using GameStore.DAL.EF;
using NLog;

namespace GameStore.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository<Game> _gamesRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<PlatformType> _platformTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ILogger logger;

        public GameService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _gamesRepository = new Repository<Game>(new GameStoreContext(), logger);
            _genreRepository = _unitOfWork.Repository<Genre>();
            _platformTypeRepository = _unitOfWork.Repository<PlatformType>();
            this.logger = logger;
        }
        public GameService()
        {
            try
            {
                _unitOfWork = new UnitOfWork();
                _gamesRepository = _unitOfWork.Repository<Game>();
                _genreRepository = _unitOfWork.Repository<Genre>();
                _platformTypeRepository = _unitOfWork.Repository<PlatformType>();
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to load repository from GameService failed: {0}", ex.StackTrace);
            }

        }

        public void AddGame(GameDTO gameDto)
        {
            try
            {
                AutoMapperConfiguration.Configure();
                Game game = Mapper.Map<GameDTO, Game>(gameDto);

                _gamesRepository.Add(game);
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to add game to repository from GameService failed: {0}", ex.StackTrace);
            }

        }

        public void DeleteGame(GameDTO gameDto)
        {
            try
            {
                AutoMapperConfiguration.Configure();
                Game game = Mapper.Map<GameDTO, Game>(gameDto);

                _gamesRepository.Delete(game);
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to delete game with id {0} from GameService failed: {1}", gameDto.Id, ex.StackTrace);
            }

        }

        public void EditGame(GameDTO gameDto)
        {
            try
            {
                AutoMapperConfiguration.Configure();
                Game game = Mapper.Map<GameDTO, Game>(gameDto);

                _gamesRepository.Edit(game);
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to edit game with id {0} from GameService failed: {1}", gameDto.Id, ex.StackTrace);
            }

        }

        public GameDTO GetGameById(int Id)
        {
            try
            {
                AutoMapperConfiguration.Configure();
                Game game = _gamesRepository.GetSingle(Id);
                GameDTO gameDto = Mapper.Map<Game, GameDTO>(game);
                return gameDto;
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to get game with id {0} from GameService failed: {1}", Id, ex.StackTrace);
                return new GameDTO();
            }
        }

        public GameDTO GetGameByKey(string key)
        {
            try
            {
                AutoMapperConfiguration.Configure();
                Game game = _gamesRepository.GetAll().FirstOrDefault(x => x.Key == key);
                GameDTO gameDto = Mapper.Map<Game, GameDTO>(game);
                return gameDto;
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to get game with key {0} from GameService failed: {1}", key, ex.StackTrace);
                return new GameDTO();
            }
        }
        public List<GameDTO> GetGames()
        {
            try
            {
                AutoMapperConfiguration.Configure();
                List<Game> games = _gamesRepository.GetAll().ToList();
                List<GameDTO> gamesDto = Mapper.Map<List<Game>, List<GameDTO>>(games);
                return gamesDto;
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to get  all games from GameService failed: {0}", ex.StackTrace);
                return new List<GameDTO>();
            }

        }
       
      
        public List<GameDTO> GetGamesByGenres(int genreId)
        {
            try
            {
                AutoMapperConfiguration.Configure();
                List<Game> games =
                    _gamesRepository.GetAll()
                        .Where(x => x.Genres.Contains(_genreRepository.GetSingle(genreId)))
                        .ToList();
                List<GameDTO> gamesDto = Mapper.Map<List<Game>, List<GameDTO>>(games);
                return gamesDto;
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to get games by GenreId {0} from GameService failed: {1}",genreId,  ex.StackTrace);
                return new List<GameDTO>();
            }
        }

        public List<GameDTO> GetGamesByPlatformType(int platformTypeId)
        {
            try
            {
                AutoMapperConfiguration.Configure();
                List<Game> games =
                    _gamesRepository.GetAll()
                        .Where(x => x.PlatformTypes.Contains(_platformTypeRepository.GetSingle(platformTypeId)))
                        .ToList();
                List<GameDTO> gamesDto = Mapper.Map<List<Game>, List<GameDTO>>(games);
                return gamesDto;
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to get games by PlatformType Id {0} from GameService failed: {1}", platformTypeId, ex.StackTrace);
                return new List<GameDTO>();
            }
        }
    }
}
