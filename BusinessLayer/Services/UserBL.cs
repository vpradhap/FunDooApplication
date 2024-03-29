﻿using BusinessLayer.Interfaces;
using CommonLayer.Model;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBL:IUserBL
    {
        private readonly IUserRL userRL;

        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        public UserEntity Register(UserRegistrationModel userRegistrationModel)
        {
            try
            {
                return userRL.Register(userRegistrationModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string Login(UserLoginModel userLoginModel)
        {
            try
            {
                return userRL.Login(userLoginModel);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string ForgetPassword(UserForgetModel userForgetModel)
        {
            try
            {
                return userRL.ForgetPassword(userForgetModel);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string ResetPassword(UserResetModel userResetModel)
        {
            try
            {
                return userRL.ResetPassword(userResetModel);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
