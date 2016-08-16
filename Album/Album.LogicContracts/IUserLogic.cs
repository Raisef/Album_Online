using Album.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Album.LogicContracts
{
    public interface IUserLogic
    {
        int Create(string userName, string password);
        User SignIn(string userName, string password);
        User GetById(int id);
        User GetByName(string userName);
        List<User> GetAll();
        List<User> GetAllByNamePath(string userName);
        bool Delete(int id);
        bool ChangePassword(int id, string oldPass, string newPass);
        bool IsUserInRole(string userName, string roleName);
        string GetRole(string userName);
        List<string> GetAllRoles();
        bool ChangeRole(int id, string roleName);
        Image GetAvatar(int id);
        bool SetAvatar(int id, Image avatar);
    }
}
