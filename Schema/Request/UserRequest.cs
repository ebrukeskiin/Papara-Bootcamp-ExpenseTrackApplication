﻿namespace Schema.Request
{
    public class UserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Plain text olarak gelecek, hash'lenmeli
    }
}
