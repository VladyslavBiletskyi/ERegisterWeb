﻿namespace MVCClient.Models
{
    public class SignUpViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Group { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}