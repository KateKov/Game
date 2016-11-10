using System.Collections.Generic;
using System.Linq;
using AutoMapper;

using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using NLog;
namespace GameStore.BLL.Services
{
    public class GameService : IService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        public GameService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        private ICollection<Genre> GetAllGenres(ICollection<GenreDTO> genres)
        {
            var allGenres = _unitOfWork.Repository<Genre>().GetAll().ToList();
            var newGenres = new List<Genre>();
            if (allGenres.Count > 0)
            {
                newGenres = (from g in genres
                    where allGenres.Exists(x => x.Name.Equals(g.Name))
                    select allGenres.First(x => x.Name.Equals(g.Name))).ToList();
                if (newGenres.Count != genres.Count)
                    throw new ValidationException("Cannot find some genres", "");
            }
           else if (genres != null && genres.Count > 0)
                throw new ValidationException("Cannot find some genres. There are no genres exists", "");
            return newGenres;       
        }

        private ICollection<PlatformType> GetAllTypes(ICollection<PlatformTypeDTO> types)
        {
            var newPlatforms = new List<PlatformType>();
            var allPlatforms = _unitOfWork.Repository<PlatformType>().GetAll().ToList();
            if (allPlatforms.Count > 0)
            {
                newPlatforms = (from g in types
                                    where allPlatforms.Exists(x => x.Type.Equals(g.Type))
                                    select allPlatforms.First(x => x.Type.Equals(g.Type))).ToList();
                if (newPlatforms.Count != types.Count)
                    throw new ValidationException("Cannot find some platforms", "");
            }
            else if (types != null && types.Count > 0)
                throw new ValidationException("Cannot find some platforms. There are no platforms exists", "");
            return newPlatforms;
        }

        public void AddGame(GameDTO gameDto)
        {
            Validator.ValidateGameModel(gameDto);
            if (_unitOfWork.Repository<Game>().FindBy(g => g.Key.Equals(gameDto.Key)).Any())
            {
                throw new ValidationException("Game with such key is already exists", "");
            }

            var game = Mapper.Map<GameDTO, Game>(gameDto);
            game.Comments.Clear();
            game.Genres.Clear();
            game.PlatformTypes.Clear();
            game.Genres = GetAllGenres(gameDto.Genres);
            game.PlatformTypes = GetAllTypes(gameDto.PlatformTypes);      
            _unitOfWork.Repository<Game>().Add(game);
            _logger.Debug("Adding game with Key={0}", gameDto.Key);
        }

        public void AddComment(CommentDTO commentDto, string gameKey)
        {
            Validator.ValidateCommentModel(commentDto);
            var comment = Mapper.Map<Comment>(commentDto);
            var game = _unitOfWork.Repository<Game>().FindBy(g => g.Key.Equals(gameKey)).FirstOrDefault();
            if (game == null)
                throw new ValidationException("Cannot find game for creating a comment", "");
            comment.Game = game;
            if (commentDto.ParentId != null)
            {
                var parentComment = _unitOfWork.Repository<Comment>().GetSingle((int)commentDto.ParentId);
                if (parentComment == null)
                    throw new ValidationException("Cannot find parent comment for creating a comment", "");
                comment.ParentComment = parentComment;
            }

            _unitOfWork.Repository<Comment>().Add(comment);
            _logger.Debug("Adding new comment with Author={0}, Id={1} to game with Key={2}", commentDto.Name, commentDto.Id, gameKey);
        }

        private Game GetGame(GameDTO gameDto)
        {
            var updatingGame = _unitOfWork.Repository<Game>().GetSingle(gameDto.Id);
            if (updatingGame == null)
                throw new ValidationException("Game wasn't found", "");
            if (!gameDto.Key.Equals(updatingGame.Key))
                if (_unitOfWork.Repository<Game>().FindBy(g => g.Key.Equals(gameDto.Key)).Any())
                    throw new ValidationException("Game with such key is already exists", "");
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
            return updatingGame;
        }

        public void EditGame(GameDTO gameDto)
        {
            Validator.ValidateGameModel(gameDto);
            var updatingGame = GetGame(gameDto);
            Mapper.Map(gameDto, updatingGame);
            updatingGame.Genres = GetAllGenres(gameDto.Genres);
            updatingGame.PlatformTypes = GetAllTypes(gameDto.PlatformTypes);
            _unitOfWork.Repository<Game>().Edit(updatingGame);
            _logger.Debug("Game updating gameKey={0}, Id={1} ", gameDto.Key, gameDto.Key);
        }
       
        public void DeleteGame(int id)
        {
            if (_unitOfWork.Repository<Game>().GetSingle(id) == null)
                throw new ValidationException("Game wasn't found", "");
            _unitOfWork.Repository<Game>().Delete(_unitOfWork.Repository<Game>().GetSingle(id));
            _unitOfWork.Save();
            _logger.Debug("Game deleting by id = {0} ", id);
        }

        public IEnumerable<GameDTO> GetGames()
        {
            var res = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().GetAll()).ToList();
            _logger.Debug("Getting all games. Returned {0} games.", res.Count);
            return res;
        }

        public IEnumerable<GameDTO> GetGamesByGenres(int genreId)
        {
            var res = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().FindBy(
                game => game.Genres.Select(
                    genre => genre.Id).ToList().Contains(genreId))).ToList();
            _logger.Debug("Getting games by name of genre id {0}. Returned {1} games", genreId, res.Count);
            return res;
        }

        public IEnumerable<GameDTO> GetGamesByGenresName(string genreName)
        {
            var res = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().FindBy(
                game => game.Genres.Select(
                    genre => genre.Name).ToList().Contains(genreName))).ToList();
            _logger.Debug("Getting games by name of genre {0}. Returned {1} games", genreName, res.Count);
            return res;
        }

        public IEnumerable<GameDTO> GetGamesByPlatformType(int platformType)
        {
            var res = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().FindBy(
                game => game.PlatformTypes.Select(
                    platform => platform.Id).ToList().Contains(platformType))).ToList();
            _logger.Debug("Getting games by platform id {0}. Returned {1} games", platformType, res.Count);
            return res;
        }

        public IEnumerable<GameDTO> GetGamesByPlatformTypeName(string platformType)
        {
            var res = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().FindBy(
                game => game.PlatformTypes.Select(
                    platform => platform.Type).ToList().Contains(platformType))).ToList();
            _logger.Debug("Getting games by platform {0}. Returned {1} games", platformType, res.Count);
            return res;
        }

        public GameDTO GetGameByKey(string key)
        {
            var game = _unitOfWork.Repository<Game>().FindBy(g => g.Key.Equals(key)).FirstOrDefault();
            if (game == null)
                throw new ValidationException("Game wasn't found", "");
            var res = Mapper.Map<GameDTO>(game);
            _logger.Debug("Getting game by key={0} ", key);
            return res;
        }

        public GameDTO GetGameById(int id)
        {
            var game = _unitOfWork.Repository<Game>().FindBy(g => g.Id.Equals(id)).FirstOrDefault();
            if (game == null)
                throw new ValidationException("Game wasn't found", "");
            var res = Mapper.Map<GameDTO>(game);
            _logger.Debug("Getting game by id={0}.", id);
            return res;
        }

        public IEnumerable<CommentDTO> GetCommentsByGame(int gameId)
        {
            var res = Mapper.Map<IEnumerable<CommentDTO>>(_unitOfWork.Repository<Comment>().FindBy(
                comment => comment.Game.Id.Equals(gameId))).ToList();
            _logger.Debug("Getting comments by id = {0}. Retured {1} comments", gameId, res.Count);
            return res;
        }

        public IEnumerable<CommentDTO> GetCommentsByGameKey(string gameKey)
        {
            var res = Mapper.Map<IEnumerable<CommentDTO>>(_unitOfWork.Repository<Comment>().FindBy(
                comment => comment.Game.Key.Equals(gameKey))).ToList();
            _logger.Debug("Getting comments by key = {0}. Retured {1} comments", gameKey, res.Count);
            return res;
        }
    }
}
