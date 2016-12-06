using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameStore.DAL.Enums;

namespace GameStore.Web.ViewModels.Translation
{
    public class ViewModelTranslate 
    {
        public string Id { get; set; }
        public Language Language;
    }
}