﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fullstack.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(30)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only letters and numbers are allowed.")]
        public string? Username { get; set; }
    }
}
