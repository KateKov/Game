using GameStore.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class FilterDTO
    {
        public Date DateOfAdding { get; set; }
        public Filter FilterBy { get; set; }
        public string Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public IEnumerable<string> SelectedGenresName { get; set; }
        public IEnumerable<string> SelectedTypesName { get; set; }
        public IEnumerable<string> SelectedPublishersName { get; set; }
    }
}
