﻿namespace APIGatewayService.Models
{
    public class APIGateway
    {

    }
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = "User";  
    }

    public class RegisterRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class MicroserviceSettings
    {
        public string Category { get; set; }
        public string Product { get; set; }
        public string Company { get; set; }
    }
}
