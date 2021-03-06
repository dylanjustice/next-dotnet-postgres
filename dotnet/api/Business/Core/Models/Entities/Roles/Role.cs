using System.Collections.Generic;
using AndcultureCode.CSharp.Core.Interfaces.Entity;
using AndcultureCode.CSharp.Core.Models;
using DylanJustice.Demo.Business.Core.Models.Entities.Users;

namespace DylanJustice.Demo.Business.Core.Models.Entities.Roles
{
    public class Role : Auditable, IRole
    {
        #region Properties

        public string Description { get; set; }
        public string Name { get; set; }

        #endregion Properties


        #region Navigation Properties

        public List<UserRole> UserRoles { get; set; }

        #endregion Navigation Properties
    }
}
