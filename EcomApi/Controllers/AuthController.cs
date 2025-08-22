using BCrypt.Net;
using EcomApi.DTOs;
using EcomApi.InMemory;
using EcomApi.Models;
using EcomApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(JwtService jwt) : ControllerBase
{
    [HttpPost("register")]
    public ActionResult<AuthResponse> Register(RegisterRequest dto)
    {
        if (StaticStore.Users.Any(u => u.Email == dto.Email))
            return Conflict("Email ya existe.");

        var user = new User
        {
            Id = StaticStore.UserSeq++,
            Email = dto.Email,
            FullName = dto.FullName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = string.IsNullOrWhiteSpace(dto.Role) ? "Customer" : dto.Role
        };
        StaticStore.Users.Add(user);
        var token = jwt.Create(user);
        return Ok(new AuthResponse(token, user.Email, user.FullName, user.Role));
    }

    [HttpPost("login")]
    public ActionResult<AuthResponse> Login(LoginRequest dto)
    {
        var user = StaticStore.Users.FirstOrDefault(u => u.Email == dto.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized();

        var token = jwt.Create(user);
        return Ok(new AuthResponse(token, user.Email, user.FullName, user.Role));
    }
}
