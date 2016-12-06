using GameStore.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameStore.Web.ViewModels
{
    public class OrderFilterViewModel
    {
        [DisplayName("From Date Of Order")]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }
        [DisplayName("To Date Of Order")]
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; }
        public Filter FilterBy { get; set; }
    }
}