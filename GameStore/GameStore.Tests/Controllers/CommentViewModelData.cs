using GameStore.ViewModels;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Tests.Controllers
{
    public class CommentViewModelData
    {
        public static IEnumerable CommentValid
        {
            get
            {
                yield return
                    new TestCaseData(new CommentViewModel { Id = 1, Name = "name", Body = "body", GameId = 1 },
                        "gamekey");
            }
        }

        public static IEnumerable CommentInvalid
        {
            get
            {
                yield return
                    new TestCaseData(new CommentViewModel { Id = 1, Name = null, Body = null, GameId = 1 }, "gamekey")
                    ;
            }
        }
    }
}
