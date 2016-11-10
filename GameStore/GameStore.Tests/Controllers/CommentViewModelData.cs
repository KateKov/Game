using System.Collections;
using GameStore.Web.ViewModels;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    public class CommentViewModelData
    {
        public static IEnumerable CommentValid
        {
            get
            {
                yield return new TestCaseData(new CommentViewModel {Id = 9, Name = "name", Body = "body", GameId = 1 }, "gamekey");
            }
        }

        public static IEnumerable CommentInvalid
        {
            get
            {
                yield return new TestCaseData(new CommentViewModel {Id = 10, Name = null, Body = null, GameId = 1 }, "gamekey");
            }
        }
    }
}
