﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Homeclick.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string Name { get; set; }
        public string Organisation { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
        [Required(ErrorMessage = "Answer is required")]
        public virtual int CaptchaResult { get; set; }
    }
}