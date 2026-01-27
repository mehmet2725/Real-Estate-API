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

        // kullanıcı nesnesini olutşturalım
        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            UserName =registerDto.UserName,
            Email = registerDto.Email,
            IsAgent = registerDto.IsAgent // emlakçı mı olmak istiyor
        };

        // veritabanına kaydet
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if(!result.Succeeded)
            return BadRequest(result.Errors);

        var role = registerDto.IsAgent ? "Agent" : "User";

        // Eğer veritabanında bu rol yoksa, önce oluştur (İlk çalışmada lazım olur)
        if(!await _roleManager.RoleExistsAsync(role))
            await _roleManager.CreateAsync(new AppRole { Name = role });

        // kullanıcıya rolü ver
        await _userManager.AddToRoleAsync(user, role);

        return StatusCode(201, new { message = "Kullanıcı başarıyla oluşturuldu" });
    }

    // LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        // Kullanıcıyı bul 
        var user = await _userManager.FindByNameAsync(loginDto.UserNameOrEmail);
        if(user == null)
            user = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail);

        if(user == null)
            return Unauthorized("Kullanıcı adı bulunamadı");

        // Şifreyi kontrol et
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);
        if(!result.Succeeded)
            return Unauthorized("Hatalı şifre");

        // rolünü bul
        var userRoles = await _userManager.GetRolesAsync(user);
        var role = userRoles.FirstOrDefault() ?? "User";

        // Token Üret
        var token = JwtTokenGenerator.GenerateToken(user, role, _configuration);

        return Ok(new { token = token });
    }
}
