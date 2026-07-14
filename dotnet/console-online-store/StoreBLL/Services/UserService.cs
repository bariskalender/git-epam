namespace StoreBLL.Services
{
    using System;
    using System.Linq;
    using StoreBLL.Interfaces;
    using StoreBLL.Models;
    using StoreDAL.Entities;
    using StoreDAL.Interfaces;

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public UserModel Login(string login, string password)
        {
            if (login == null || password == null)
            {
                throw new InvalidOperationException();
            }

            var user = this.userRepository
                .GetAll()
                .FirstOrDefault(x => x.Login == login && x.Password == password);

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            return new UserModel(user.Id, user.Name)
            {
                LastName = user.LastName ?? string.Empty,
                Login = user.Login ?? string.Empty,
                Password = user.Password ?? string.Empty,
                RoleId = user.RoleId,
            };
        }

        public void Register(UserModel model)
        {
            if (model == null)
            {
                throw new InvalidOperationException();
            }

            var user = new User
            {
                Name = model.Name ?? string.Empty,
                LastName = model.LastName ?? string.Empty,
                Login = model.Login ?? string.Empty,
                Password = model.Password ?? string.Empty,
                RoleId = 2,
            };

            this.userRepository.Add(user);
        }
    }
}