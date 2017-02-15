using System;
using System.Collections.Generic;
using GameStore.BLL.Interfaces.Services;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.DAL.Enums;
using GameStore.Web.ViewModels.Genres;
using GameStore.Web.ViewModels.Translates;
using GameStore.Web.Interfaces;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Infrastructure.AuthorizeAttribute;

namespace GameStore.Web.Controllers
{
    public class GenresController : BaseController
    {
        private readonly INamedService<GenreDTO, GenreDTOTranslate> _genreService;

        public GenresController(INamedService<GenreDTO, GenreDTOTranslate> genre, IAuthenticationManager authentication) : base(authentication)
        {
            _genreService = genre;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var genresDto = _genreService.GetAll(false);
            var genres = Mapper.Map<IEnumerable<GenreViewModel>>(genresDto);
            return View(genres);
        }

        [HttpGet]
        public ActionResult Details(string name)
        {
            var publisher = _genreService.GetByName(name);
            return View(Mapper.Map<GenreViewModel>(publisher));
        }

        [HttpGet]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult New()
        {
            var genre = new CreateGenreViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Translates = new List<TranslateViewModel>
                {
                     new TranslateViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Language = Language.ru
                }
                }
            };

            genre.AllGenres = Mapper.Map<IEnumerable<GenreViewModel>>(_genreService.GetAll(false));

            return View(genre);
        }

        [HttpPost]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult New(CreateGenreViewModel createGenre)
        {
            if (ModelState.IsValid)
            {
                var genreDto = Mapper.Map<GenreDTO>(createGenre);
                _genreService.AddEntity(genreDto);
                return RedirectToAction("Index");
            }
            createGenre.Translates = new List<TranslateViewModel>
            {
                new TranslateViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Language = Language.ru
                }
            };
            createGenre.AllGenres = Mapper.Map<IEnumerable<GenreViewModel>>(_genreService.GetAll(false));
            return View("New", createGenre);
        }

        [HttpGet]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult Update(string name)
        {
            var genreDto = _genreService.GetByName(name);
            var genre = Mapper.Map<CreateGenreViewModel>(genreDto);
            genre.AllGenres = Mapper.Map<IEnumerable<GenreViewModel>>(_genreService.GetAll(false));
            return View(genre);
        }

        [HttpPost]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult Update(CreateGenreViewModel createGenre)
        {
            if (ModelState.IsValid)
            {
                var genreDto = Mapper.Map<GenreDTO>(createGenre);
                _genreService.EditEntity(genreDto);
                return RedirectToAction("Index");
            }
            createGenre.AllGenres = Mapper.Map<IEnumerable<GenreViewModel>>(_genreService.GetAll(false));
            return View("Update", createGenre);
        }

        [HttpGet]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult Delete(string name)
        {
            _genreService.DeleteByName(name);
            return RedirectToAction("Index");
        }
    }
}