using System;

namespace RealEstate.Business.Dtos.AuthDtos;

public class LoginDto
{
    public string UserNameOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
