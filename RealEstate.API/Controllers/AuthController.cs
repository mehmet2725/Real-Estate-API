using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstate.API.Tools;
using RealEstate.Business.Dtos.AuthDtos;
using RealEstate.Entity.Concrete;
using System.Threading.Tasks;

namespace RealEstate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    // REGISTER
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        // Böyle bir kullanıcı var mı bakalım
        var userExists = await _userManager.FindByNameAsync(registerDto.UserName);
        if (userExists != null)
            return BadRequest("Bu kullanıcı adı kullanımda");

        // 1. Kullanıcı nesnesini oluştur
        var user = new AppUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            IsAgent = registerDto.IsAgent // Emlakçı mı?
        };

        // 2. Kullanıcıyı kaydet (Şifreyi hashle)
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {

            var role = registerDto.IsAgent ? "Agent" : "User";

            // Rolü veritabanına işle
            await _userManager.AddToRoleAsync(user, role);

            return StatusCode(201, new { message = "Kullanıcı başarıyla oluşturuldu" });
        }

        return BadRequest(result.Errors);
    }

    // LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        // Kullanıcıyı bul 
        var user = await _userManager.FindByNameAsync(loginDto.UserNameOrEmail);
        if (user == null)
            user = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail);

        if (user == null)
            return Unauthorized("Kullanıcı adı bulunamadı");

        // Şifreyi kontrol et
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded)
            return Unauthorized("Hatalı şifre");

        // rolünü bul
        var userRoles = await _userManager.GetRolesAsync(user);
        var role = userRoles.FirstOrDefault() ?? "User";

        // Token Üret
        var token = JwtTokenGenerator.GenerateToken(user, role, _configuration);

        return Ok(new { token = token });
    }
}
