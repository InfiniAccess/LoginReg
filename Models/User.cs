using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginReg.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "First Name:")]
        [MinLength(2, ErrorMessage = "Requires atleast two characters")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name:")]
        [MinLength(2, ErrorMessage = "Requires atleast two characters")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address:")]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be atleast 8 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password must match.")]
        [NotMapped]
        [Display(Name = "Confirm Password:")]
        public string Confirm { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}