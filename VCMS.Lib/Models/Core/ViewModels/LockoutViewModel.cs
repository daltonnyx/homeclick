using System;

namespace VCMS.Lib.Models
{
    public class LockoutViewModel
    {
        public string UserName { get; set; }
        public DateTime EndLockoutDate { get; set; }
    }
}
