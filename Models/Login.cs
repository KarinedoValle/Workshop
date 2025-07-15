using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

public class Login
{
    [BindProperty]
    [Required(ErrorMessage = "O nome de usuário é obrigatório")]
    public string Username { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Password { get; set; }

    public string Message { get; set; }
}
