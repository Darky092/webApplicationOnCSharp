using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public user Account { get; set; }
        public string Token {get; set;}
        public DateTime Expires { get; set;}
        public DateTime Created { get; set;}
        public string CreatedByIp { get; set;}
        public DateTime? Revoked { get; set;}
        public string? RevokedByIp { get;set;}
        public string? ReplaceByToken { get; set; }

        public string? ReasonRevoked { get; set; }

        public bool IsExpired => DateTime.UtcNow > Expires;
        public bool IsRevoked => Revoked != null;
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
