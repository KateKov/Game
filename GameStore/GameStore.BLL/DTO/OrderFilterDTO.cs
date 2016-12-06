using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Enums;

namespace GameStore.BLL.DTO
{
    public class OrderFilterDTO
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Filter FilterBy { get; set; }
    }
}
