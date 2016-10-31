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
using GameStore.BLL.Infrastructure;
using GameStore.DAL.EF;
using NLog;

namespace GameStore.BLL.Services
{
    public class GameService : IService
    {
        private readonly IRepository<Game> _gamesRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<PlatformType> _platformTypeRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ILogger _logger;

        public GameService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _gamesRepository = new Repository<Game>(new GameStoreContext(), logger);
            _genreRepository = _unitOfWork.Repository<Genre>();
            _platformTypeRepository = _unitOfWork.Repository<PlatformType>();
            _commentRepository = _unitOfWork.Repository<Comment>();
            this._logger = logger;
        }
       
        public void AddGame(GameDTO gameDto)
        {
            _logger.Debug("Game creation service is started. " + LoggerMessage.generateDataString_GameDTO(gameDto));
            try
            {
                var automapper = AutoMapperConfiguration.DtoToDomain().CreateMapper();

                Game game = automapper.Map<GameDTO, Game>(gameDto);
                game.Comments.Clear();
                game.Genres.Clear();
                game.PlatformTypes.Clear();
                _logger.Debug($"Game creation (key: {gameDto.Key}). Checking genres..");
                var allGenres = _genreRepository.GetAll().ToList();
                if (allGenres.Count > 0)
                {
                    var newGenres = (from g in gameDto.Genres
                                     where allGenres.Exists(x => x.Name.Equals(g.Name))
                                     select allGenres.First(x => x.Name.Equals(g.Name))).ToList();
                    if (newGenres.Count != gameDto.Genres.Count)
                        throw new ValidationException("Cannot find some genres", "");
                    game.Genres = newGenres;
                }
                else if (gameDto.Genres != null && gameDto.Genres.Count > 0)
                    throw new ValidationException("Cannot find some genres. There are no genres exists", "");
                _logger.Debug($"Game creation (key: {gameDto.Key}). Genres are successfully checked and assigned");

                _logger.Debug($"Game creation (key: {gameDto.Key}). Checking platforms..");
                var allPlatforms = _platformTypeRepository.GetAll().ToList();
                if (allPlatforms.Count > 0)
                {
                    var newPlatforms = (from g in gameDto.PlatformTypes
                                        where allPlatforms.Exists(x => x.Type.Equals(g.Type))
                                        select allPlatforms.First(x => x.Type.Equals(g.Type))).ToList();
                    if (newPlatforms.Count != gameDto.PlatformTypes.Count)
                        throw new ValidationException("Cannot find some platforms", "");
                    game.PlatformTypes = newPlatforms;
                }
                else if (gameDto.PlatformTypes != null && gameDto.PlatformTypes.Count > 0)
                    throw new ValidationException("Cannot find some platforms. There are no platforms exists", "");
                _logger.Debug($"Game creation (key: {gameDto.Key}). Platforms are successfully checked and assigned");
                _gamesRepository.Add(game);
                _logger.Debug("Game creation service is succesfully finished. " + LoggerMessage.generateDataString_GameDTO(gameDto));
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to add game to repository from GameService failed: {0}", ex.StackTrace);
            }

        }

        public void DeleteGame(int id)
        {
            try
            {

                _logger.Debug("Game deleting service is started. Id = " + id);
                _logger.Debug("Game deleting service. Getting game by id = " + id + "..");
                if (_gamesRepository.GetSingle(id) == null)
                    throw new ValidationException("Game wasn't found", "");  
                _gamesRepository.Delete(_gamesRepository.GetSingle(id));
                _logger.Debug("Game deleting service is successfuly finished. Id = " + id);
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to delete game with id {0} from GameService failed: {1}", id, ex.StackTrace);
            }

        }

        public void EditGame(GameDTO gameDto)
        {
            try
            {
                _logger.Debug("Game updating service is started. " + LoggerMessage.generateDataString_GameDTO(gameDto));
                var updatingGame = _gamesRepository.GetSingle(gameDto.Id);
                if (updatingGame == null)
                    throw new ValidationException("Game wasn't found", "");

                if (!gameDto.Key.Equals(updatingGame.Key))
                    if (_gamesRepository.FindBy(g => g.Key.Equals(gameDto.Key)).Any())
                        throw new ValidationException("Game with such key is already exists", "");
                var automapper = AutoMapperConfiguration.DtoToDomain().CreateMapper();

                if (updatingGame.Comments == null)
                    updatingGame.Comments = new List<Comment>();
                else
                    gameDto.Comments.Clear();
                if (updatingGame.Genres == null)
                    updatingGame.Genres = new List<Genre>();
                else
                    updatingGame.Genres.Clear();
                if (updatingGame.PlatformTypes == null)
                    updatingGame.PlatformTypes = new List<PlatformType>();
                else
                    updatingGame.PlatformTypes.Clear();
                Game game = automapper.Map<GameDTO, Game>(gameDto);
                _logger.Debug($"Game updating (id: {gameDto.Id}). Checking genres..");
                var allGenres =_genreRepository.GetAll().ToList();
                if (allGenres.Count > 0)
                {
                    var newGenres = (from g in updatingGame.Genres
                                     where allGenres.Exists(x => x.Name.Equals(g.Name))
                                     select allGenres.First(x => x.Name.Equals(g.Name))).ToList();
                    if (newGenres.Count != gameDto.Genres.Count)
                        throw new ValidationException("Cannot find some genres", "");
                    updatingGame.Genres = newGenres;
                }
                else if (gameDto.Genres != null && gameDto.Genres.Count > 0)
                    throw new ValidationException("Cannot find some genres. There are no genres exists", "");
                _logger.Debug($"Game updating (id: {gameDto.Id}). Genres are successfuly updated");

                _logger.Debug($"Game updating (id: {gameDto.Id}). Checking platforms..");
                var allPlatforms = _platformTypeRepository.GetAll().ToList();
                if (allPlatforms.Count > 0)
                {
                    var newPlatforms = (from g in updatingGame.PlatformTypes
                                        where allPlatforms.Exists(x => x.Type.Equals(g.Type))
                                        select allPlatforms.First(x => x.Type.Equals(g.Type))).ToList();
                    if (newPlatforms.Count != gameDto.PlatformTypes.Count)
                        throw new ValidationException("Cannot find some platforms", "");
                    updatingGame.PlatformTypes = newPlatforms;
                }
                else if (gameDto.PlatformTypes != null && gameDto.PlatformTypes.Count > 0)
                    throw new ValidationException("Cannot find some platforms. There are no platforms exists", "");
                _logger.Debug($"Game updating (id: {gameDto.Id}). platforms are successfuly updated");

                _gamesRepository.Edit(game);
                _logger.Debug("Game updating service is successfuly finished. " + LoggerMessage.generateDataString_GameDTO(gameDto));
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to edit game with id {0} from GameService failed: {1}", gameDto.Id, ex.StackTrace);
            }

        }

        public GameDTO GetGameById(int Id)
        {
            try
            {
                _logger.Debug("Getting game by id. Started. Id = " + Id);
                var game = _gamesRepository.GetSingle(Id);
                if (game == null)
                    throw new ValidationException("Game wasn't found", "");

                var automapper = AutoMapperConfiguration.DomaintToDto().CreateMapper();
                GameDTO gameDto = Mapper.Map<Game, GameDTO>(game);
                return gameDto;
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to get game with id {0} from GameService failed: {1}", Id, ex.StackTrace);
                return new GameDTO();
            }
        }

        public GameDTO GetGameByKey(string key)
        {
            try
            {
                _logger.Debug("Getting game by key. Started. Key = " + key);
                var game =_gamesRepository.FindBy(g => g.Key.Equals(key)).FirstOrDefault();
                if (game == null)
                    throw new ValidationException("Game wasn't found", "");

                var automapper = AutoMapperConfiguration.DomaintToDto().CreateMapper();
               
                GameDTO gameDto = Mapper.Map<Game, GameDTO>(game);
                _logger.Debug("Getting game by key. Finished. " + LoggerMessage.generateDataString_GameDTO(gameDto));
                return gameDto;
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to get game with key {0} from GameService failed: {1}", key, ex.StackTrace);
                return new GameDTO();
            }
        }
        public IEnumerable<GameDTO> GetGames()
        {
            try
            {
                _logger.Debug("Getting all games. Started.");
                var automapper = AutoMapperConfiguration.DomaintToDto().CreateMapper();
                List<Game> games = _gamesRepository.GetAll().ToList();
                List<GameDTO> gamesDto = automapper.Map<List<Game>, List<GameDTO>>(games);
                _logger.Debug("Getting all games. Finished. Returned " + games.Count + " games.");
                return gamesDto;
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to get  all games from GameService failed: {0}", ex.StackTrace);
                return new List<GameDTO>();
            }

        }
       
      
        public IEnumerable<GameDTO> GetGamesByGenres(int genreId)
        {
            try
            {
                _logger.Debug("Getting games by genre. Started. Genre = " + genreId + _genreRepository.GetSingle(genreId));
                var automapper = AutoMapperConfiguration.DomaintToDto().CreateMapper();
                List<Game> games =
                    _gamesRepository.FindBy(x => x.Genres.Contains(_genreRepository.GetSingle(genreId)))
                        .ToList();
                List<GameDTO> gamesDto = Mapper.Map<List<Game>, List<GameDTO>>(games);
                _logger.Debug("Getting games by genre. Finished. Genre = " + genreId+_genreRepository.GetSingle(genreId) + ". Returned " + games.Count + " games.");
                return gamesDto;
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to get games by GenreId {0} from GameService failed: {1}",genreId,  ex.StackTrace);
                return new List<GameDTO>();
            }
        }

        public IEnumerable<GameDTO> GetGamesByPlatformType(int platformTypeId)
        {
            try
            {
                _logger.Debug("Getting games by platform. Started. Platform = " + platformTypeId + _platformTypeRepository.GetSingle(platformTypeId));
                var mapper = AutoMapperConfiguration.DomaintToDto().CreateMapper();
                List<Game> games =
                    _gamesRepository.FindBy(x => x.PlatformTypes.Contains(_platformTypeRepository.GetSingle(platformTypeId)))
                        .ToList();
                List<GameDTO> gamesDto = Mapper.Map<List<Game>, List<GameDTO>>(games);
                _logger.Debug("Getting games by platform. Finished. Platform = " + platformTypeId + ". Returned " + games.Count + " games.");
                return gamesDto;
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to get games by PlatformType Id {0} from GameService failed: {1}", platformTypeId, ex.StackTrace);
                return new List<GameDTO>();
            }
        }
        public void AddComment(CommentDTO commentDto, string gameKey)
        {
            try
            {
                _logger.Debug($"Comment creation service is started. Comment for game with key = {gameKey}. " + LoggerMessage.generateDataString_CommentDTO(commentDto));
                var automapper = AutoMapperConfiguration.DtoToDomain().CreateMapper();
                Comment comment = automapper.Map<CommentDTO, Comment>(commentDto);
                var game = _gamesRepository.FindBy(g => g.Key.Equals(gameKey)).FirstOrDefault();

                _logger.Debug($"Comment creation (id: {commentDto.Id}). Checking game..");
                if (game == null)
                    throw new ValidationException("Cannot find game for creating a comment", "");
                comment.Game = game;
                _logger.Debug($"Comment creation (id: {commentDto.Id}). Game is successfuly checked and assigned");

                _logger.Debug($"Comment creation (id: {commentDto.Id}). Checking parent comment..");
                if (commentDto.ParentId != null)
                {
                    var parentComment = _commentRepository.GetSingle((int)commentDto.ParentId);
                    if (parentComment == null)
                        throw new ValidationException("Cannot find parent comment for creating a comment", "");
                    comment.ParentComment = parentComment;
                }
                _logger.Debug($"Comment creation (id: {commentDto.Id}). Perent comment is successfuly processed");

                _commentRepository.Add(comment);
                _logger.Debug($"Comment creation service is successfuly finished. Comment for game with key = {gameKey}. " + LoggerMessage.generateDataString_CommentDTO(commentDto));
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to add comment  to the game with Id {0} from CommentService failed: {1}", gameKey, ex.StackTrace);
            }

        }

        public IEnumerable<CommentDTO> GetCommentsByGame(int gameId)
        {
            try
            {
                _logger.Debug("Getting comments by game id. Started. Id = " + gameId);
                var mapper = AutoMapperConfiguration.DomaintToDto().CreateMapper();
                List<Comment> comments = _commentRepository.FindBy(x => x.Game.Id == gameId).ToList();
                List<CommentDTO> commentsDto = Mapper.Map<List<Comment>, List<CommentDTO>>(comments);
                _logger.Debug($"Getting comments by by game id. Finished. Id = {gameId}. Retured {comments.Count} comments");
                return commentsDto;
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to get comments by game Id {0} from CommentService failed: {1}", gameId, ex.StackTrace);
                return new List<CommentDTO>();
            }
        }
    }
}
