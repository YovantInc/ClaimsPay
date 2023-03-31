using System.Text.Json.Serialization;

namespace ClaimsPay.Modules.ClaimsPay.Models
{
    
    public class user_auth
    {
        public string? user_name { get; set; }
        public string? password { get; set; }
        public string? version { get; set; }
    }
    public class UserAuthenticationModel
    {
        public user_auth? user_auth { get; set; }
    }
}
