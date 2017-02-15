using System;
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
