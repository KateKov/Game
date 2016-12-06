using System.ComponentModel;


namespace GameStore.DAL.Enums
{
    public enum Date
    {
        [Description("last week")]
        week,
        [Description("last month")]
        month,
        [Description("last year")]
        year,
        [Description("2 year")]
        twoyear,
        [Description("3 year")]
        threeyear
    };

    public enum Filter
    {
        [Description("Most popular")]
        Popularity = 1,
        [Description("Most commented")]
        Comments = 2,
        [Description("By price ascending")]
        PriceAsc = 3,
        [Description("By price descending")]
        PriceDesc = 4,
        [Description("New")]
        New = 5
    }
}
