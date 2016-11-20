using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Homeclick.Models
{
    public class IdentifyUser : IUser
    {
        public IdentifyUser()
        {

        }

        public string Id
        {
            get;
            set;
        }

        public string UserName
        {
            get;

            set;
        }

        public string StoredPassword
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }
    }
}