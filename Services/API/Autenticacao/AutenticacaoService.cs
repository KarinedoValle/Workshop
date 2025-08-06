using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Workshop.DB;
using Workshop.Models;
using Workshop.Services.API.Usuario;

public class AutenticacaoService : IAutenticacaoService
{
    private readonly AppDbContext _context;
    private readonly SecurityKey _jwtKey;

    public AutenticacaoService(AppDbContext context, SecurityKey jwtKey)
    {
        _context = context;
        _jwtKey = jwtKey;
    }

    public (bool sucesso, object? resultado, string? erro) GerarToken(Token request)
    {
        var usuario = _context.Usuario.FirstOrDefault(u => u.Login == request.ClientId);
        if (usuario == null)
            return(false, null, "Usuário ou perfil inválido.");

        var perfilcorreto = usuario.Perfil == request.Profile;
        var verificacaoSenha = new PasswordHasher<Usuario>().VerifyHashedPassword(usuario, usuario.Senha, request.ClientSecret);

        if (!perfilcorreto || (verificacaoSenha != PasswordVerificationResult.Success))
        {
            return (false, null, "Usuário ou perfil inválido.");
        }

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

        var result = new
        {
            access_token = tokenString,
            token_type = "Bearer",
            expires_in = 3600
        };

        return (true, result, null);
    }
}
