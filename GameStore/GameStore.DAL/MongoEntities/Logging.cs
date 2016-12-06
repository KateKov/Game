using System;
using System.Collections.Generic;

namespace GameStore.DAL.MongoEntities
{
    public class Logging : MongoBase
    {
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public string Version { get; set; }

        public Logging(DateTime date, string action, string entityType, string version)
        {
            Date = date;
            Action = action;
            EntityType = entityType;
            Version = version;
        }

        public Logging()
        { }
    }
}
