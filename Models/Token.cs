using Microsoft.AspNetCore.Mvc;

namespace Workshop.Models
{
    public class Token
    {
        [FromForm(Name = "profile")]
        public string Profile { get; set; }

        [FromForm(Name = "client_id")]
        public string ClientId { get; set; }

        [FromForm(Name = "client_secret")]
        public string ClientSecret { get; set; }
    }
}
