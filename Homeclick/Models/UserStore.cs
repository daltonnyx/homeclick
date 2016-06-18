using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Homeclick.Models
{
    public class UserStore<TUser> : Microsoft.AspNet.Identity.EntityFramework.UserStore<TUser> where TUser : ApplicationUser
    {

        vinabits_homeclickEntities db = new vinabits_homeclickEntities();
       
        public UserStore() : base()
        {

        }

        public UserStore(System.Data.Entity.DbContext applicationDBContext) : base(applicationDBContext)
        {
            
        }

        #region IUserLoginStore
        

        

        public override Task<TUser> FindAsync(UserLoginInfo login)
        {
            User dbUser = db.Users.SingleOrDefault<User>(u => u.UserName == login.LoginProvider);
            if (dbUser == null)
                return null;
            ApplicationUser user = new ApplicationUser();
            user.UserName = dbUser.UserName;
            user.PasswordHash = dbUser.StoredPassword;
            user.Id = dbUser.ID.ToString();
            return Task.FromResult<TUser>((TUser)user);
        }

       public TUser FindByID(string userID)
        {
            int uID = Convert.ToInt32(userID);
            User dbUser = db.Users.SingleOrDefault<User>(u => u.ID == uID);
            if (dbUser == null)
                return null;
            ApplicationUser user = new ApplicationUser();
            user.UserName = dbUser.UserName;
            user.PasswordHash = dbUser.StoredPassword;
            user.Id = dbUser.ID.ToString();
            return user as TUser;
        }


        #endregion
    }

    public class CustomUserManager<TUser> : Microsoft.AspNet.Identity.UserManager<TUser> where TUser : ApplicationUser
    {
        public CustomUserManager(IUserStore<TUser> store) : base(store)
        {

        }

        public override Task<TUser> FindAsync(string userName, string password)
        {
            UserLoginInfo model = new UserLoginInfo(userName, password);
            return ((IUserLoginStore<TUser>)this.Store).FindAsync(model);
            
        }

        public override Task<ClaimsIdentity> CreateIdentityAsync(TUser user, string authenticationType)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.CookiePath, ClaimTypes.Role);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), "http://www.w3.org/2001/XMLSchema#string"));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName, "http://www.w3.org/2001/XMLSchema#string"));
            claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/IdentityProvider", "Custom Identity", "http://www.w3.org/2001/XMLSchema#string"));
            return Task.FromResult<ClaimsIdentity>(claimsIdentity);
        }

        public new TUser FindById(string userId)
        {
            return ((UserStore<TUser>)this.Store).FindByID(userId);
        }
    }
}