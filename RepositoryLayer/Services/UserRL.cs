﻿using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRL:IUserRL
    {
        private readonly FundooContext fundooContext;
        private readonly IConfiguration config;
        public UserRL(FundooContext fundooContext , IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.config = configuration;
        }
        public UserEntity Register(UserRegistrationModel userRegistrationModel)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = userRegistrationModel.FirstName;
                userEntity.LastName = userRegistrationModel.LastName;
                userEntity.Email = userRegistrationModel.Email;
                userEntity.Password = userRegistrationModel.Password;

                fundooContext.UserTable.Add(userEntity);
                int result=fundooContext.SaveChanges();
                if (result > 0)
                {
                    return userEntity;
                }
                else
                {
                    return null;
                }
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
                var login = fundooContext.UserTable.FirstOrDefault(x => x.Email == userLoginModel.Email && x.Password == userLoginModel.Password);
                if (login != null)
                {
                    var token = JwtMethod(login.Email, login.UserId);
                    return token;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string JwtMethod(string email, long userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.config[("Jwt:Secretkey")]));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                new Claim[]
                {
                        new Claim(ClaimTypes.Email, email),
                        new Claim("userId", userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        public string ForgetPassword(UserForgetModel userForgetModel)
        {
            var emailCheck = fundooContext.UserTable.FirstOrDefault(x => x.Email == userForgetModel.Email);
            if (emailCheck != null)
            {
                var token = JwtMethod(emailCheck.Email, emailCheck.UserId);
                MSMQ_Model msmq_Model = new MSMQ_Model();
                msmq_Model.sendData2Queue(token);
                return token;
            }
            else
                return null;
        }
        public string ResetPassword(UserResetModel userResetModel)
        {
            try
            {
                if (userResetModel.Password.Equals(userResetModel.ConfirmPassword))
                {
                    // var emailCheck = fundooContext.User.Where(x => x.Email == email);
                    UserEntity user = fundooContext.UserTable.Where(x => x.Email == userResetModel.Email).FirstOrDefault();;
                    user.Password = userResetModel.ConfirmPassword;
                    //fundooContext.User.Update(user);
                    fundooContext.SaveChanges();
                    return "Reset success";
                }
                else
                {
                    return "Reset failed";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
