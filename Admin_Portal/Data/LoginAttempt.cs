using System;
using System.ComponentModel.DataAnnotations;

namespace Admin_Portal.Data
{
    public class LoginAttempt
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
    }
}