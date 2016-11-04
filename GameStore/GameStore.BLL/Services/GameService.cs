using System;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public GameService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private ICollection<T> GetAllInstansesOfType<T, TD>(ICollection<TD> types) where TD : class, IDtoBase, new() where T: class, IEntityBase, new() 
        {
            var type = typeof(T).Name;
            var allTypes = _unitOfWork.Repository<T>().GetAll().ToList();
            var newTypes = new List<T>();
            if (allTypes.Count > 0)
            {
                newTypes = types
                    .Where(x => allTypes
                        .Exists(g => g.Id.Equals(x.Id)))
                    .Select(x => allTypes
                        .First(g => g.Id.Equals(x.Id))).ToList();
                if(newTypes.Count != types.Count)
                    throw new ValidationException(@"Cannot find "+type, string.Empty);
            }
            else if(types != null && types.Count > 0)
                throw new ValidationException("Cannot find "+ type +". There are no "+type+" exists", string.Empty);
            return newTypes;
        }

        public void AddGame(GameDTO gameDto)
        {
            Validator.ValidateGameModel(gameDto);
            if (_unitOfWork.Repository<Game>().FindBy(g => g.Key.Equals(gameDto.Key)).Any())
            {
                throw new ValidationException("Game with such key is already exists", string.Empty);
            }

            var game = Mapper.Map<GameDTO, Game>(gameDto);
            game.OrderDetails = GetAllInstansesOfType<OrderDetail, OrderDetailDTO>(gameDto.OrderDetails);
            game.Genres = GetAllInstansesOfType<Genre, GenreDTO>(gameDto.Genres);
            game.PlatformTypes = GetAllInstansesOfType<PlatformType, PlatformTypeDTO>(gameDto.PlatformTypes);
            _unitOfWork.Repository<Game>().Add(game);
            _logger.Debug("Adding game with Key={0}", gameDto.Key);
        }

        public void AddComment(CommentDTO commentDto, string gameKey)
        {
            Validator.ValidateCommentModel(commentDto);
            var comment = Mapper.Map<CommentDTO, Comment>(commentDto);
            var game = _unitOfWork.Repository<Game>().FindBy(g => g.Key.Equals(gameKey)).FirstOrDefault();
            if (game == null)
                throw new ValidationException("Cannot find game for creating a comment", string.Empty);
            comment.Game = game;
            if (commentDto.ParentId != null)
            {
                var parentComment = _unitOfWork.Repository<Comment>().GetSingle((int)commentDto.ParentId);
                if (parentComment == null)
                    throw new ValidationException("Cannot find parent comment for creating a comment", string.Empty);
                comment.ParentComment = parentComment;
            }

            _unitOfWork.Repository<Comment>().Add(comment);
            _logger.Debug("Adding new comment with Author={0}, Id={1} to game with Key={2}", commentDto.Name, commentDto.Id, gameKey);
        }

        private Game GetGame(GameDTO gameDto)
        {
            var updatingGame = _unitOfWork.Repository<Game>().GetSingle(gameDto.Id);
            updatingGame.Comments = new List<Comment>();
            updatingGame.Genres = new List<Genre>();
            updatingGame.PlatformTypes = new List<PlatformType>();
            return updatingGame;
        }

        public void EditGame(GameDTO gameDto)
        {
            Validator.ValidateGameModel(gameDto);
            var updatingGame = GetGame(gameDto);
            Mapper.Map(gameDto, updatingGame);
            updatingGame.Genres = GetAllInstansesOfType<Genre, GenreDTO>(gameDto.Genres);
            updatingGame.PlatformTypes = GetAllInstansesOfType<PlatformType, PlatformTypeDTO>(gameDto.PlatformTypes);
            updatingGame.OrderDetails = GetAllInstansesOfType<OrderDetail, OrderDetailDTO>(gameDto.OrderDetails);
            _unitOfWork.Repository<Game>().Edit(updatingGame);
            _logger.Debug("Game updating gameKey={0}, Id={1} ", gameDto.Key, gameDto.Key);
        }
       
        public void DeleteGame(int id)
        {
            if (_unitOfWork.Repository<Game>().GetSingle(id) == null)
                throw new ValidationException("Game wasn't found", string.Empty);
            _unitOfWork.Repository<Game>().Delete(_unitOfWork.Repository<Game>().GetSingle(id));
            _unitOfWork.Save();
            _logger.Debug("Game deleting by id = {0} ", id);
        }

        public IEnumerable<GameDTO> GetGames()
        {
            var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().GetAll()).ToList();
            _logger.Debug("Getting all games. Returned {0} games.", gamesDto.Count);
            return gamesDto;
        }

        public IEnumerable<GameDTO> GetGamesByGenres(int genreId)
        {
            var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().FindBy(
                game => game.Genres.Select(
                    genre => genre.Id).ToList().Contains(genreId))).ToList();
            _logger.Debug("Getting games by name of genre id {0}. Returned {1} games", genreId, gamesDto.Count);
            return gamesDto;
        }

        public IEnumerable<GameDTO> GetGamesByGenresName(string genreName)
        {
            var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().FindBy(
                game => game.Genres.Select(
                    genre => genre.Name).ToList().Contains(genreName))).ToList();
            _logger.Debug("Getting games by name of genre {0}. Returned {1} games", genreName, gamesDto.Count);
            return gamesDto;
        }

        public IEnumerable<GameDTO> GetGamesByPlatformType(int platformType)
        {
            var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().FindBy(
                game => game.PlatformTypes.Select(
                    platform => platform.Id).ToList().Contains(platformType))).ToList();
            _logger.Debug("Getting games by platform id {0}. Returned {1} games", platformType, gamesDto.Count);
            return gamesDto;
        }

        public IEnumerable<GameDTO> GetGamesByPlatformTypeName(string platformType)
        {
            var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().FindBy(
                game => game.PlatformTypes.Select(
                    platform => platform.Type).ToList().Contains(platformType))).ToList();
            _logger.Debug("Getting games by platform {0}. Returned {1} games", platformType, gamesDto.Count);
            return gamesDto;
        }

        public GameDTO GetGameByKey(string key)
        {
            var game = _unitOfWork.Repository<Game>().FindBy(g => g.Key.Equals(key)).FirstOrDefault();
            if (game == null)
                throw new ValidationException("Game wasn't found", string.Empty);
            var gameDto = Mapper.Map<GameDTO>(game);
            _logger.Debug("Getting game by key={0} ", key);
            return gameDto;
        }

        public GameDTO GetGameById(int id)
        {
            var game = _unitOfWork.Repository<Game>().FindBy(g => g.Id.Equals(id)).FirstOrDefault();
            if (game == null)
                throw new ValidationException("Game wasn't found", string.Empty);
            var gameDto = Mapper.Map<GameDTO>(game);
            _logger.Debug("Getting game by id={0}.", id);
            return gameDto;
        }

        public IEnumerable<CommentDTO> GetCommentsByGame(int gameId)
        { 
            var commentsDto = Mapper.Map<IEnumerable<CommentDTO>>(_unitOfWork.Repository<Comment>().FindBy(
                comment => comment.Game.Id.Equals(gameId))).ToList();
            _logger.Debug("Getting comments by id = {0}. Retured {1} comments", gameId, commentsDto.Count);
            return commentsDto;
        }

        public IEnumerable<CommentDTO> GetCommentsByGameKey(string gameKey)
        {
            var commentsDto = Mapper.Map<IEnumerable<CommentDTO>>(_unitOfWork.Repository<Comment>().FindBy(
                comment => comment.Game.Key.Equals(gameKey))).ToList();
            _logger.Debug("Getting comments by key = {0}. Retured {1} comments", gameKey, commentsDto.Count);
            return commentsDto;
        }

        public PublisherDTO GetPublisher(string companyName)
        {
            var publisherDto = Mapper.Map<PublisherDTO>(_unitOfWork.Repository<Publisher>().FindBy(
                publisher => publisher.CompanyName.Equals(companyName)));
            _logger.Debug("Getting publisher by companyName = {0}.", companyName);
            return publisherDto;
        }

        public IEnumerable<PublisherDTO> GetPublishers()
        {
            var publishersDto = Mapper.Map<IEnumerable<PublisherDTO>>(_unitOfWork.Repository<Publisher>().GetAll()).ToList();
            _logger.Debug("Getting all publishers with count: {0}.", publishersDto.Count);
            return publishersDto;
        }

        public void AddPublisher(PublisherDTO publisherDto)
        {
            Validator.ValidatePublisherModel(publisherDto);
            if (_unitOfWork.Repository<Publisher>().FindBy(g => g.CompanyName.Equals(publisherDto.CompanyName)).Any())
            {
                throw new ValidationException("Publisher with such Company Name is already exists", string.Empty);
            }

            var publisher = Mapper.Map<PublisherDTO, Publisher>(publisherDto);
            _unitOfWork.Repository<Publisher>().Add(publisher);
            _logger.Debug("Adding publisher with CompanyName: {0}", publisherDto.CompanyName);
        }

        public void DeletePublisher(string companyName)
        {
            if (_unitOfWork.Repository<Publisher>().FindBy(x => x.CompanyName.Equals(companyName)) == null)
                throw new ValidationException("Publisher wasn't found", string.Empty);
            _unitOfWork.Repository<Publisher>().Delete(_unitOfWork.Repository<Publisher>().FindBy(x => x.CompanyName.Equals(companyName)).FirstOrDefault());
            _logger.Debug("Publisher deleting by CompanyName = {0} ", companyName);
        }

        public void DeletePublisherById(int id)
        {
            if (_unitOfWork.Repository<Publisher>().GetSingle(id) == null)
                throw new ValidationException("Publisher wasn't found", string.Empty);
            _unitOfWork.Repository<Publisher>().Delete(_unitOfWork.Repository<Publisher>().GetSingle(id));
            _logger.Debug("Publisher deleting by id = {0} ", id);
        }

        public PublisherDTO GetPublisherById(int id)
        {
            var publisher = _unitOfWork.Repository<Game>().GetSingle(id);
            if (publisher == null)
                throw new ValidationException("Publisher wasn't found", string.Empty);
            var publisherDto = Mapper.Map<PublisherDTO>(publisher);
            _logger.Debug("Getting publisher by id={0} ", id);
            return publisherDto;
        }
    }
}
