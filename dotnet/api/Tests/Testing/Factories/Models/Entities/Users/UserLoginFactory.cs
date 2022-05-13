using AndcultureCode.CSharp.Testing.Factories;
using DylanJustice.Demo.Business.Core.Models.Entities.Users;
using AndcultureCode.CSharp.Testing.Tests;
using System;

namespace DylanJustice.Demo.Testing.Factories.Models.Entities.Users
{
    public class UserLoginFactory : Factory
    {
        public override void Define()
        {
            this.DefineFactory(() => new UserLogin
            {
                FailedAttemptCount = 0,
                Ip = "127.0.0.1",
                IsSuccessful = true,
                ServerName = Environment.MachineName,
                UserAgent = "user-agent",
                UserName = $"TestUserName{Milliseconds}"
            });
        }
    }
}