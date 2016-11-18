using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.DTO;
using GameStore.DAL.Interfaces;
using NLog;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Services
{
    public class PagingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public PagingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public IEnumerable<GameDTO> GetGames(int page, int countPerPage)
        //{
        //    var gamesCount = _unitOfWork.Repository<Game>().GetAll().Count()/countPerPage;
        //    var games= _unitOfWork.Repository<Game>().GetAll().
        //}
    }
}
