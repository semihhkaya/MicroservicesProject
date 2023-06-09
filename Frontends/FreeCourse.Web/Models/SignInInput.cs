﻿using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models
{
    public class SigninInput //kullanıcdan data alcaksak input kullanıcya data göstereceksek viev model türünde class isimlendirmesi yapalaım
    {
        [Required]
        [Display(Name = "Email adresiniz")] 
        public string Email { get; set; }
        [Required]
        [Display(Name = "Şifreniz")]
        public string Password { get; set; }

        [Display(Name = "Beni hatırla")]
        public bool IsRemember { get; set; }
    }
}
