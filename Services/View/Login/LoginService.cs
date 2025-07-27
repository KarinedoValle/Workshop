using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Workshop.DB;
using Workshop.Models;

public class LoginService : ILoginService
{
    private readonly AppDbContext _context;
    private readonly SecurityKey _jwtKey;

    public LoginService(AppDbContext context, SecurityKey jwtKey)
    {
        _context = context;
        _jwtKey = jwtKey;
    }

    public (bool sucesso, string? token, string? mensagemErro) Autenticar(string username, string senha)
    {
        var usuario = _context.Usuario.FirstOrDefault(u => u.Login == username);
        if (usuario == null)
            return (false, null, "Usuário ou senha inválidos");

        var hasher = new PasswordHasher<Usuario>();
        var resultado = hasher.VerifyHashedPassword(usuario, usuario.Senha, senha);

        if (resultado != PasswordVerificationResult.Success)
            return (false, null, "Usuário ou senha inválidos");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.Login),
            new Claim("Cpf", usuario.Cpf),
            new Claim("Perfil", usuario.Perfil)
        };

        var creds = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "http://localhost:5001",
            audience: "http://localhost:5001",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return (true, tokenString, null);
    }
}
