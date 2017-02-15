using System;
using System.Collections.Generic;
using GameStore.WcfService.Models;

namespace GameStore.WcfService.Repositories
{
    public class PaymentInitializer
    {
        public static void Seed()
        {
            var users = new List<User>
            {
                new User
                {
                    UserId = Guid.NewGuid(),
                    Name = "Name1",
                    Surname = "Surname1",
                    Email = "user1@email.com",
                    Password = "aaaaa"
                },
                new User
                {
                    UserId = Guid.NewGuid(),
                    Name = "Name2",
                    Surname = "Suranme2",
                    Email = "demo@demo.com",
                    Password = "aaaaa"
                }
            };
         
            var cards = new List<Card>
            {
                new Card
                {
                    CardNumber = "11111111111111111",
                    AmountOfMoney = 134m,
                    CvvCode = 243,
                    User = users[0],
                    ExpirationMonth = 1,
                    ExpirationYear = 17
                },
                //Default card for user
                new Card
                {
                    CardNumber = "22222222222222222",
                    AmountOfMoney = 872.23m,
                    CvvCode = 274,
                    User = users[1],
                    ExpirationMonth = 12,
                    ExpirationYear = 17
                }
            };
            users[0].Cards = new List<Card> {cards[0]};
            users[1].Cards = new List<Card> {cards[1]};

            PaymentContext.Users.AddRange(users);
            PaymentContext.Cards.AddRange(cards);
        }
    }
}