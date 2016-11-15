using System.Collections.Generic;

namespace GameStore.Web.ViewModels
{
    public class BanViewModel
    {
        public string Name { get; set; }

        public static ICollection<string> Durations
        {
            get { return new List<string> {"1 hour", "1 day", "1 week", "1 month", "permanent"}; }
        }

        public string SelectedDuration { get; set; }
    }
}