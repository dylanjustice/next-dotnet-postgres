using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DylanJustice.Demo.Business.Core.Models.Entities.Roles;

using DylanJustice.Demo.Business.Core.Models.Entities.Users;

using DylanJustice.Demo.Business.Core.Interfaces.Data;
using DylanJustice.Demo.Infrastructure.Data.PostgreSql.Extensions;
using DylanJustice.Demo.Infrastructure.Data.PostgreSql.Maps.Roles;
using DylanJustice.Demo.Infrastructure.Data.PostgreSql.Maps.Users;
using DylanJustice.Demo.Infrastructure.Data.PostgreSql.Maps.Users.Logins;
using DylanJustice.Demo.Infrastructure.Data.PostgreSql.Maps.Users.Roles;
using System;
using System.Linq;
using System.Linq.Expressions;
using DylanJustice.Demo.Business.Core.Models.Jobs;
using AndcultureCode.CSharp.Core.Models.Entities;
using AndcultureCode.CSharp.Data.Interfaces;

namespace DylanJustice.Demo.Infrastructure.Data.PostgreSql
{
    public class GBApiContext : Context, IDatabaseContext, IGBApiContext
    {
        #region Constructor

        #region Constructor


        public GBApiContext(string connectionString, ILoggerFactory loggerFactory)
        : base(connectionString, loggerFactory)
        {
            // Uncomment for debugging purposes only
            // Console.WriteLine($"GBApiContext () => {Configuration.GetConnectionString()}");
        }

        #endregion Constructor

        #endregion Constructor

        #region Entities

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        #endregion Entities

        #region IGBApiContext Implementation

        IQueryable<Role> IGBApiContext.Roles => Roles;
        IQueryable<User> IGBApiContext.Users => Users;
        IQueryable<UserLogin> IGBApiContext.UserLogins => UserLogins;
        IQueryable<UserRole> IGBApiContext.UserRoles => UserRoles;

        public IQueryable<Job> Jobs => throw new NotImplementedException();

        public IQueryable<UserMetadata> UserMetadata => throw new NotImplementedException();

        public IQueryable<Acl> Acls => throw new NotImplementedException();

        #endregion IGBApiContext Implementation

        #region DbContext Overrides

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureMappings(modelBuilder);
        }

        #endregion DbContext Overrides

        #region Private Methods

        public override void ConfigureMappings(ModelBuilder modelBuilder)
        {
            modelBuilder.AddMapping(new RoleMap());
            modelBuilder.AddMapping(new UserLoginMap());
            modelBuilder.AddMapping(new UserMap());
            modelBuilder.AddMapping(new UserRoleMap());
        }

        #endregion Private Methods


    }
}
