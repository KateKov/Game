using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Enums
{
    public enum PageEnum
    {
        [Description("PageSize_TenItems")]
        Ten = 10,
        [Description("PageSize_TwentyItems")]
        Twenty = 20,
        [Description("PageSize_FiftyItems")]
        Fifty = 50,
        [Description("PageSize_OneHundred")]
        OneHundred = 100,
        [Description("PageSize_All")]
        All = 0
    }
}
