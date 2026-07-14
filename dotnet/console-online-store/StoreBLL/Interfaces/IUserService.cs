namespace StoreBLL.Interfaces;

using StoreBLL.Models;

public interface IUserService
{
    UserModel Login(string login, string password);

    void Register(UserModel model);
}