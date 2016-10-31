﻿
using GameStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.EF
{
    public class GameStoreDbInitializer: DropCreateDatabaseAlways<GameStoreContext>
    {
        protected override void Seed(GameStoreContext context)
        {
            
                GetGenres().ForEach(c => context.Genres.Add(c));
                GetComment().ForEach(c => context.Comments.Add(c));
                GetTypes().ForEach(c => context.PlatformTypes.Add(c));
                GetGames().ForEach(c => context.Games.Add(c));
            context.SaveChanges();

        }

        public static List<Game> GetGames()
        {
            return new List<Game>
            {
                new Game
                {
                    Id = 1,
                    Key="GTA6_NewEdition",
                    Name = "GTA6",
                    Description = "It's amazing game",
                    Genres = new List<Genre>() {GetGenres()[0], GetGenres()[2]},
                    
                    PlatformTypes = GetTypes()
                },
                  new Game
                {
                    Id = 2,
                    Key="Sims3_SecondEdition",
                    Name = "Sims3",
                    Description = "You'll spend on it a lot of time",
                    Genres = new List<Genre>() {GetGenres()[1], GetGenres()[2]},
              
                    PlatformTypes = GetTypes()
                }
            };
        }
        private static List<PlatformType> GetTypes()
        {
            return new List<PlatformType>
            {
                new PlatformType
                {
                    Id = 1,
                    Type = "Mobile"
                },
                 new PlatformType
                {
                     Id = 2,
                    Type = "Browser"
                },
                  new PlatformType
                {
                      Id = 3,
                    Type = "Desktop"
                },
                   new PlatformType
                {
                       Id = 4,
                    Type = "Console"
                }
            };
        }

        private static List<Comment> GetComment()
        {
            Comment first = new Comment
            {
                Id = 1,
                Name = "Marina",
                Body = "This is first comment",
                GameId = 1

            };
            return new List<Comment>
            {
               
               first,
                 new Comment() {Id = 2, Name = "Artur", GameId = 1, Body = "This is answer", ParentComment = first},
                  new Comment
                {
                    Id = 3,
                    Name="Nina",

                    Body="This is second comment",
                    GameId = 1
                }
            };
        }

       
        
        private static List<Genre> GetGenres()
        {
            Genre strategy = new Genre
            {
                Id = 1,
                Name = "Strategy"
            };
            Genre races = new Genre
            {
                Id = 2,
                Name = "Races"
            };
            Genre action = new Genre
            {
                Id = 3,
                Name = "Action"
            };
            return new List<Genre>
            {

                strategy,
                new Genre
                {
                    Id = 4,
                    Name = "RPG"
                },
                new Genre
                {
                    Id = 5,
                    Name = "Sports"
                },
                races,
               action,
                new Genre
                {
                    Id = 6,
                    Name = "Adventure"
                },
                 new Genre
                {
                     Id = 7,
                    Name = "Puzzle&Skill"
                },
                new Genre
                {
                    Id = 8,
                    Name = "Misc"
                },
                new Genre
                {
                    Id = 9,
                    Name = "RTS",
                    ParentGenre = strategy
                },
                 new Genre
                {
                     Id = 10,
                    Name = "TBS",
                    ParentGenre = strategy
                },
                  new Genre
                {
                      Id = 11,
                    Name = "Rally",
                   ParentGenre = races
                },
                 new Genre
                {
                     Id = 12,
                    Name= "Arcade",
                     ParentGenre = races
                },
                    new Genre
                {
                        Id = 13,
                   Name = "Formula",
                     ParentGenre = races
                },
                 new Genre
                {
                     Id = 14,
                    Name = "Off-road",
                    ParentGenre = races
                },
                    new Genre
                {
                        Id = 15,
                    Name = "FPS",
                     ParentGenre = action
                },
                 new Genre
                {
                     Id = 16,
                    Name = "TPS",
                    ParentGenre = action
                }

            };


        }
    }
    
}
