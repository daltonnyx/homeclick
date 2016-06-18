using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base.Security;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;

namespace Homeclick.vn.Module.BusinessObjects {
    [MetadataType(typeof(UserMetadata))]
    [ImageName("BO_User"), System.ComponentModel.DefaultProperty("UserName"),XafDisplayName("Người dùng")]
    public partial class User : ISecurityUser, IAuthenticationActiveDirectoryUser, IAuthenticationStandardUser, IOperationPermissionProvider, ISecurityUserWithRoles {
        // ISecurityUser

        public User(DevExpress.Xpo.Session session) : base(session) { }

        Boolean ISecurityUser.IsActive {
            get { return IsActive; }
        }
        String ISecurityUser.UserName {
            get { return UserName; }
        }

        // IAuthenticationActiveDirectoryUser
        String IAuthenticationActiveDirectoryUser.UserName {
            get { return UserName; }
            set { UserName = value; }
        }
        // IAuthenticationStandardUser
        Boolean IAuthenticationStandardUser.ComparePassword(String password) {
            PasswordCryptographer passwordCryptographer = new PasswordCryptographer();
            return passwordCryptographer.AreEqual(StoredPassword, password);
        }
        public void SetPassword(String password) {
            PasswordCryptographer passwordCryptographer = new PasswordCryptographer();
            StoredPassword = passwordCryptographer.GenerateSaltedPassword(password);
        }
        Boolean IAuthenticationStandardUser.ChangePasswordOnFirstLogon {
            get { return ChangePasswordOnFirstLogon; }
            set { ChangePasswordOnFirstLogon = value; }
        }
        String IAuthenticationStandardUser.UserName {
            get { return UserName; }
        }

        IList<ISecurityRole> ISecurityUserWithRoles.Roles
        {
            get
            {
                IList<ISecurityRole> result = new List<ISecurityRole>();
                foreach (CustomSecurityRole role in SecurityRoles)
                {
                    result.Add(role);
                }
                return result;
            }
        }
        [DevExpress.Xpo.Association("Users-Roles"), XafDisplayName("Quyền truy nhập")]
        public XPCollection<CustomSecurityRole> SecurityRoles
        {
            get
            {
                return GetCollection<CustomSecurityRole>("SecurityRoles");
            }
        }

        IEnumerable<IOperationPermission> IOperationPermissionProvider.GetPermissions()
        {
            return new IOperationPermission[0];
        }
        IEnumerable<IOperationPermissionProvider> IOperationPermissionProvider.GetChildren()
        {
            return new EnumerableConverter<IOperationPermissionProvider, CustomSecurityRole>(SecurityRoles);
        }
    }

    public class UserMetadata {
        [Browsable(false)]
        public Int32 ID { get; set; }
        [Browsable(false), SecurityBrowsable]
        protected String StoredPassword { get; set; }
    }

    public class CustomSecurityRole : SecuritySystemRoleBase
    {
        public CustomSecurityRole(Session session)
            : base(session)
        {
        }
        [DevExpress.Xpo.Association("Users-Roles")]
        public XPCollection<User> Users
        {
            get
            {
                return GetCollection<User>("Users");
            }
        }
    }
}
