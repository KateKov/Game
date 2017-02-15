using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Services
{
    public class UserService : Service<User, UserDTO>, IUserService
    {
        private readonly IUnitOfWork _unitOfWorks;
        private readonly IEncryptionService _encryptionService;

        public UserService(IUnitOfWork unitOfWorks, IEncryptionService encryption) :base(unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
            _encryptionService = encryption;
        }

        public UserDTO GetUserByName(string username)
        {
            var user = GetUserEntityByName(username);
            var userDto = Mapper.Map<UserDTO>(user);
            return userDto;
        }

        public UserDTO Login(string username, string password)
        {
            User user;
            if (IsExistByUsername(username))
            {
                user = GetUserEntityByName(username);
            }
            else if (IsExistByEmail(username))
            {
                user = _unitOfWorks.Repository<User>().FindBy(x => x.Email == username).FirstOrDefault();
            }
            else
            {
                throw new ValidationException("There is no user with such username", "");
            }

            var encryptedPassword = _encryptionService.EncryptPassword(password, user.PasswordSalt);
            return (string.Equals(encryptedPassword, user.PasswordHash))
                ? Mapper.Map<UserDTO>(user)
                : null;
        }

        public bool IsExistByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ValidationException("The username should not be null or empty.", string.Empty);
            }


            return _unitOfWorks.Repository<User>().FindBy(x=>x.Username == username).Any(); 
        }

        public bool IsExistByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ValidationException("The email should not be null or empty.", string.Empty);
            }

            return _unitOfWorks.Repository<User>().FindBy(x => x.Email == email).Any(); 
        }

        public bool IsInRole(string username, string rolename)
        {
            if (string.IsNullOrEmpty(rolename))
            {
                throw new ValidationException("The values should not be null or empty.", string.Empty);
            }

            var user = GetUserEntityByName(username);
            var result = user.Roles != null && user.Roles.Any(x => x.Translates.Any(z=>z.Name==rolename));
            return result;
        }

        public User GetUserEntityByName(string username) 
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ValidationException("The username should not be null or empty.", string.Empty);
            }

            var user = _unitOfWorks.Repository<User>().FindBy(x => x.Username == username).FirstOrDefault();

            return user;
        }

        public User GetUserEntityByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ValidationException("The username should not be null or empty.", string.Empty);
            }

            var user = _unitOfWorks.Repository<User>().FindBy(x => x.Email == email).FirstOrDefault();

            return user;
        }

        public bool IsBanned(string username)
        {
            User user;
            if (IsExistByUsername(username))
            {
                user = GetUserEntityByName(username);
            }
            else if (IsExistByEmail(username))
            {
                user = GetUserEntityByEmail(username);
            }
            else
            {
                throw new ValidationException("There is no user", "User");
            }

            return user.Bans != null && user.Bans.Any(x => x.EndDate >= DateTime.UtcNow && !x.IsCanceled);
        }

        public void Ban(string username, string reason, Duration duration)
        {
            var user = GetUserEntityByName(username);
            var ban = new Ban {Id = Guid.NewGuid(), StartDate = DateTime.UtcNow, Reason = reason, User = user};
            switch (duration)
            {
                case Duration.Day:
                    ban.EndDate = ban.StartDate.AddDays(1);
                    break;
                case Duration.Hour:
                    ban.EndDate = ban.StartDate.AddHours(1);
                    break;
                case Duration.Month:
                    ban.EndDate = ban.StartDate.AddMonths(1);
                    break;
                case Duration.Week:
                    ban.EndDate = ban.StartDate.AddDays(7);
                    break;
            }
            _unitOfWorks.Repository<Ban>().Add(ban);
        }

        public void Unban(string username)
        {
            var user = GetUserEntityByName(username);
            var ban = _unitOfWorks.Repository<Ban>().FindBy(x => x.UserId == user.Id).FirstOrDefault();
            if (ban != null)
            {
                ban.IsCanceled = true;
                _unitOfWorks.Repository<Ban>().Edit(ban);
            }
        }

        public void ChangeNotificationMethod(string username, NotificationMethod method)
        {
            var user = GetUserEntityByName(username);
            var managerProfile = _unitOfWorks.Repository<ManagerProfile>().GetSingle(user.Id.ToString());
            managerProfile.Method = method;
            _unitOfWorks.Repository<ManagerProfile>().Edit(managerProfile);           
        }

        public void Register(UserDTO userDto)
        {
            if (IsExistByEmail(userDto.Email) && IsExistByUsername(userDto.Username))
            {
                throw new ValidationException("User with such email or username already exists", string.Empty);
            }
            var user = Mapper.Map<User>(userDto);
            user.Id = Guid.NewGuid();
            if (!string.IsNullOrEmpty(userDto.PasswordHash)) 
            {
                user.PasswordSalt = _encryptionService.CreateSalt();
                user.PasswordHash = _encryptionService.EncryptPassword(userDto.PasswordHash, user.PasswordSalt);
            }

            user.CreateDate = DateTime.UtcNow;

            user.Roles = (userDto.Translates==null)
                ? new List<Role>()
                {
                    _unitOfWorks.Repository<Role>().FindBy(x => x.Translates.Any()).First(x=>x.Translates.Any(z => z.Name == "User"))
                }
                : _unitOfWorks.Repository<Role>()
                    .FindBy(x => x.Translates.Any()).Where(x=>x.Translates.Any(z => userDto.Translates.Any(t=>t.RolesName.Contains(z.Name))))
                    .ToList();
            _unitOfWorks.Repository<User>().Add(user);
        }

        public void Edit(UserDTO userDto)
        {
            if (userDto == null)
            {
                throw new ValidationException("The user is empty", string.Empty);
            }

            var user = _unitOfWorks.Repository<User>().GetSingle(userDto.Id);
            if (user == null)
            {
                throw new ValidationException("There is no user in database", string.Empty);
            }
            user.Username = userDto.Username;
            user.Bans.Clear();
            if (userDto.Bans != null)
            {
                user.Bans =
                    _unitOfWorks.Repository<Ban>()
                        .GetAll()
                        .Where(x => userDto.Bans.Contains(x.Id.ToString()))
                        .ToList();
            }

            user.Email = userDto.Email;
            user.IsLocked = userDto.IsLocked;
            user.IsDeleted = userDto.IsDeleted;
            user.Orders.Clear();
            if (userDto.Orders != null)
            {
                user.Orders =
                    _unitOfWorks.Repository<Order>()
                        .GetAll()
                        .Where(x => userDto.Orders.Contains(x.Id.ToString()))
                        .ToList();
            }

            user.Roles.Clear();
            user.Roles =
                _unitOfWorks.Repository<Role>()
                    .FindBy(x => x.Translates.Any()).Where(x=>x.Translates.Any(z => userDto.Translates.Any(t=>t.RolesName.Contains(z.Name))))
                    .ToList();
            _unitOfWorks.Repository<User>().Edit(user);
        }
    }
}
