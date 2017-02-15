using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Enums;
using GameStore.Web.Infrastructure;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Infrastructure.AuthorizeAttribute;
using GameStore.Web.Interfaces;
using GameStore.Web.Providers;
using GameStore.Web.ViewModels.Games;
using GameStore.Web.ViewModels.Translates;
using GameStore.Web.ViewModels.Genres;
using GameStore.Web.ViewModels.Orders;
using GameStore.Web.ViewModels.PlatformTypes;
using GameStore.Web.ViewModels.Publishers;
using NLog;
using GlobalRes = GameStore.Web.App_LocalResources.GlobalRes;

namespace GameStore.Web.Controllers
{
    public class GamesController : BaseController
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IGameService _gameService;
        private readonly INamedService<PublisherDTO, PublisherDTOTranslate> _publisherService;
        private readonly INamedService<GenreDTO, GenreDTOTranslate> _genreService;
        private readonly INamedService<PlatformTypeDTO, PlatformTypeDTOTranslate> _typeService;
        private readonly IOrderService _orderService;

        private static string DefaultPicture = "default.jpg";
        private static string PatternPicture = @"^[\w]+.(jpg|gif|png)$";

        public GamesController(IGameService gameService, INamedService<PlatformTypeDTO, PlatformTypeDTOTranslate> type,
            INamedService<GenreDTO, GenreDTOTranslate> genre, IOrderService orders,
            INamedService<PublisherDTO, PublisherDTOTranslate> publisher, IAuthenticationManager authentication)
            : base(authentication)
        {
            _genreService = genre;
            _publisherService = publisher;
            _typeService = type;
            _orderService = orders;
            _gameService = gameService;
        }

        [HttpGet]
        public ActionResult Index(FilterViewModel gameFilterViewModel)
        {
            var page = (gameFilterViewModel.Page >= 1) ? gameFilterViewModel.Page : 1;
            var filterDto = Mapper.Map<FilterDTO>(gameFilterViewModel);
            var isWithDeleted = false;
            if (User.Identity.IsAuthenticated)
            {
                isWithDeleted = User.IsInRole("Administrator") ||
                                User.IsInRole("Manager")
                                || User.IsInRole("Moderator");
            }
            gameFilterViewModel.Page = page;
            gameFilterViewModel.PageSize = PageEnum.Ten;
            var filterResult = _gameService.GetAllByFilter(filterDto, isWithDeleted, gameFilterViewModel.Page,
                gameFilterViewModel.PageSize);
            gameFilterViewModel.TotalItemsCount = filterResult.Count;
            return View(gameFilterViewModel);
        }

        [HttpGet]
        public ActionResult Games(FilterViewModel gameFilterViewModel)
        {
            var filterDto = Mapper.Map<FilterDTO>(gameFilterViewModel);
            var isWithDeleted = false;
            if (User.Identity.IsAuthenticated)
            {
                isWithDeleted = User.IsInRole("Administrator") ||
                                User.IsInRole("Manager")
                                || User.IsInRole("Moderator");
            }
            var filterResult = _gameService.GetAllByFilter(filterDto, isWithDeleted, gameFilterViewModel.Page,
                gameFilterViewModel.PageSize);
            var gameViewModel = Mapper.Map<IEnumerable<GameViewModel>>(filterResult.Games);
            var gamePaging = new GamePagingViewModel
            {
                Games = gameViewModel,
                Page = gameFilterViewModel.Page,
                PageSize = gameFilterViewModel.PageSize,
                TotalItemsCount = filterResult.Count
            };

            return PartialView(gamePaging);
        }

        public FileResult GameImage(string key)
        {
            var imageData = _gameService.GetByKey(key).FilePath;
            var imagePath = Server.MapPath($"~/Content/images/{imageData}");
            var file = File(imagePath, "image/jpeg");
            return file;
        }

        public async Task<FileResult> GameImageAsync(string key)
        {
            var imageData = await Task.Factory.StartNew(() => _gameService.GetByKey(key).FilePath);
            var imagePath = Server.MapPath($"../../Content/images/{imageData}");
            var file = File(imagePath, "image/jpeg");
            return file;
        }

        [HttpGet]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult New()
        {
            var game = new CreateGameViewModel
            {
                Id = Guid.NewGuid().ToString(),
                DateOfAdding = DateTime.UtcNow
            };

            game.AllGenres = GetAllGenres().ToList();
            game.AllPublishers = GetAllPublishers().ToList();
            game.AllTypes = GetAllTypes().ToList();
            game.PublisherName = GetDefaultPublisher().Name;
            game.GenresName = new List<string> {GetDefaultGenre().Name};
            game.Translates = new List<TranslateViewModelDescription>
            {
                new TranslateViewModelDescription
                {
                    Id = Guid.NewGuid().ToString(),
                    Language = Language.ru
                }
            };

            return View(game);
        }

        [HttpPost]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult New(CreateGameViewModel game, HttpPostedFileBase image)
        {
            if (image != null && !Regex.IsMatch(image.FileName, PatternPicture))
            {
                ModelState.AddModelError("FilePath", @GlobalRes.FilePathError);
            }

            if (!ModelState.IsValid)
            {
                game.AllGenres = GetAllGenres().ToList();
                game.AllPublishers = GetAllPublishers().ToList();
                game.AllTypes = GetAllTypes().ToList();
                game.Translates = new List<TranslateViewModelDescription>
                {
                    new TranslateViewModelDescription
                    {
                        Id = Guid.NewGuid().ToString(),
                        Language = Language.ru
                    }
                };

                return View("New", game);
            }

            var gameViewModel = game;
            var gameDto = Mapper.Map<GameDTO>(gameViewModel);
            if (image != null)
            {
                string fileName = image.FileName;
                image.SaveAs(Server.MapPath($"~/Content/images/{fileName}"));
                gameDto.FilePath = fileName;
            }
            else
            {
                gameDto.FilePath = DefaultPicture;
            }

            _gameService.AddEntity(gameDto);

            _logger.Info("Game is created. Id {0} Key {1} ", gameViewModel.Id, gameViewModel.Key);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult Update(string key)
        {
            var game = _gameService.GetByKey(key);
            var gameUpdate = Mapper.Map<GameDTO, CreateGameViewModel>(game);
            gameUpdate.AllGenres = GetAllGenres().ToList();
            gameUpdate.AllPublishers = GetAllPublishers().ToList();
            gameUpdate.AllTypes = GetAllTypes().ToList();
            gameUpdate.PublisherName = (string.IsNullOrEmpty(gameUpdate.PublisherName))
                ? GetDefaultPublisher().Name
                : gameUpdate.PublisherName;
            gameUpdate.GenresName = gameUpdate.GenresName.Any()
                ? gameUpdate.GenresName
                : new List<string> {GetDefaultGenre().Name};
            return View(gameUpdate);
        }

        [HttpPost]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult Update(CreateGameViewModel game, HttpPostedFileBase image)
        {
            _logger.Info("Request to GamesController.Update");
            if (image != null && !Regex.IsMatch(image.FileName, PatternPicture))
            {
                ModelState.AddModelError("FilePath", @GlobalRes.FilePathError);
            }

            if (!ModelState.IsValid)
            {
                var gameUpdate = game;
                gameUpdate.AllGenres = GetAllGenres().ToList();
                gameUpdate.AllTypes = GetAllTypes().ToList();
                gameUpdate.AllPublishers = GetAllPublishers().ToList();
                gameUpdate.PublisherName = string.IsNullOrEmpty(gameUpdate.PublisherName)
                    ? GetDefaultPublisher().Name
                    : gameUpdate.PublisherName;
                gameUpdate.GenresName = gameUpdate.GenresName.Any()
                    ? gameUpdate.GenresName
                    : new List<string> {GetDefaultGenre().Name};

                return View("Update", game);
            }

            var gameViewModel = game;
            var gameDto = Mapper.Map<GameDTO>(gameViewModel);
            if (image != null)
            {
                string fileName = image.FileName;
                image.SaveAs(Server.MapPath("~/Content/images/" + fileName));
                gameDto.FilePath = fileName;
            }
            else
            {
                gameDto.FilePath = DefaultPicture;
            }

            _gameService.EditEntity(gameDto);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddToBasket(string gameKey, short unitsInStock)
        {
            var basket = new BasketViewModel()
            {
                GameKey = gameKey,
                CustomerId = User.Current.Username,
                UnitInStock = unitsInStock
            };
            return PartialView("AddToBasket", basket);
        }

        [HttpPost]
        public ActionResult AddToBasket(BasketViewModel basketModel)
        {
            if (ModelState.IsValid)
            {
                var basket = basketModel;
                _orderService.AddOrderDetail(basket.GameKey, short.Parse(basket.Quantity), basketModel.CustomerId, true);
                return PartialView("Success");
            }

            return PartialView("AddToBasket", basketModel);
        }

        [HttpGet]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult Delete(string key)
        {
            TempData["Delete"] = GlobalRes.DeleteMessage;
            _gameService.DeleteByKey(key);
            return RedirectToAction("Index");
        }

        [OutputCache(Duration = 60)]
        [ChildActionOnly]
        public ActionResult CountGames()
        {
            var count = _gameService.GetAll(false).Count();
            return PartialView("CountGames", count);
        }

        public PartialViewResult Filters(FilterViewModel model)
        {
            var filterViewModel = GetFilterViewModel(model.SelectedGenresName, model.SelectedTypesName,
                model.SelectedPublishersName);
            filterViewModel.Page = model.Page;
            filterViewModel.PageSize = PageEnum.Ten;
            filterViewModel.TotalItemsCount = model.TotalItemsCount;
            return PartialView("Filter", filterViewModel);
        }

        private FilterViewModel GetFilterViewModel(IEnumerable<string> genresName = null,
            IEnumerable<string> typesName = null, IEnumerable<string> publishersName = null)
        {
            var dtoToViewModel = new DtoToViewModelMapping();
            var genres = _genreService.GetAll(false).ToList();
            var genreTranslates = genres.Select(x => dtoToViewModel.GetDefaultTranslate(x.Translates.ToList()));
            var types = _typeService.GetAll(false).ToList();
            var typesTranslate = types.Select(x => dtoToViewModel.GetDefaultTranslate(x.Translates.ToList()));
            var publishers = _publisherService.GetAll(false).ToList();
            var publishersTranslate = publishers.Select(x => dtoToViewModel.GetDefaultTranslate(x.Translates.ToList()));
            var model = new FilterViewModel()
            {
                ListGenres = GetListOfItems(genreTranslates),
                ListTypes = GetListOfItems(typesTranslate),
                ListPublishers = GetListOfItems(publishersTranslate)
            };
            if (genresName != null)
            {
                model.SelectedGenres = model.ListGenres.Where(x => genresName.Contains(x.Text));
                model.SelectedGenresName = genresName;
            }

            if (typesName != null)
            {
                model.SelectedType = model.ListTypes.Where(x => typesName.Contains(x.Text));
                model.SelectedTypesName = typesName;
            }

            if (publishersName != null)
            {
                model.SelectedPublishers = model.ListPublishers.Where(x => publishersName.Contains(x.Text));
                model.SelectedPublishersName = publishersName;
            }

            return model;
        }

        private List<CheckBox> GetListOfItems<T>(IEnumerable<T> entities)
            where T : class, IDtoNamed, IDTOTranslate, new()
        {
            var list = new List<CheckBox>();
            entities.ToList().ForEach(x => list.Add(new CheckBox()
            {
                Text = x.Name,
                Checked = false
            }));
            return list;
        }

        private IEnumerable<PublisherViewModel> GetAllPublishers()
        {
            var publishersDto = _publisherService.GetAll(false);
            var publishers =
                Mapper.Map<IEnumerable<PublisherDTO>, IEnumerable<PublisherViewModel>>(publishersDto).ToList();
            publishers.Add(GetDefaultPublisher());
            return publishers;
        }

        private IEnumerable<GenreViewModel> GetAllGenres()
        {
            var genres = _genreService.GetAll(false);
            var genresViewModel = Mapper.Map<IEnumerable<GenreDTO>, IEnumerable<GenreViewModel>>(genres).ToList();

            genresViewModel.Add(GetDefaultGenre());

            return genresViewModel;
        }

        private IEnumerable<PlatformTypeViewModel> GetAllTypes()
        {
            return Mapper.Map<IEnumerable<PlatformTypeDTO>, IEnumerable<PlatformTypeViewModel>>(
                _typeService.GetAll(false)).ToList();
        }

        private PublisherViewModel GetDefaultPublisher()
        {
            var publisher = new PublisherDTO
            {
                Translates = new List<PublisherDTOTranslate>
                {
                    new PublisherDTOTranslate
                    {
                        Language = Language.en,
                        Name = "unknown"
                    },
                    new PublisherDTOTranslate
                    {
                        Language = Language.ru,
                        Name = "неизвестный"
                    }
                }
            };

            return Mapper.Map<PublisherViewModel>(publisher);
        }

        private GenreViewModel GetDefaultGenre()
        {
            var genre = new GenreDTO
            {
                Translates = new List<GenreDTOTranslate>
                {
                    new GenreDTOTranslate
                    {
                        Language = Language.en,
                        Name = "other"
                    },
                    new GenreDTOTranslate
                    {
                        Language = Language.ru,
                        Name = "другой"
                    }
                }
            };

            return Mapper.Map<GenreViewModel>(genre);
        }
    }
}