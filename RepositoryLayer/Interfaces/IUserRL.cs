using CommonLayer.Model;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRL
    {
        public UserEntity Register(UserRegistrationModel userRegistrationModel);
        public string Login(UserLoginModel userLoginModel);
        public string ForgetPassword(string EmailID);

    }
}
