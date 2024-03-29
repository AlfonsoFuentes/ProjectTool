using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.UserAccounts.Reponses
{
    public class UserReponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        public string OldPassword { get; set; } = string.Empty;
    }
}
