using System;
using System.Collections.Generic;

namespace GameStore.WcfService.Models
{
    public class User
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<Card> Cards { get; set; }
    }
}