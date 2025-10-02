using System;
using System.Collections.Generic;
using System.Data;
using Domain.Entities;

namespace Domain.Models;

public partial class user
{
    public int userid { get; set; }

    public string? avatar { get; set; }

    public string name { get; set; } = null!;

    public string? surname { get; set; }

    public string? patronymic { get; set; }

    public string email { get; set; } = null!;

    public string telephonnumber { get; set; } = null!;

    public string passwordhash { get; set; } = null!;

    public string role { get; set; } = null!;

    public bool? isactive { get; set; }

    public DateTime? createdat { get; set; }

    public bool AcceptTerms { get; set; }

    public RoleT RoleT { get; set; }

    public string? VerificationToken { get; set; }

    public DateTime? Verifaied { get; set; }

    public bool IsVerified => Verifaied.HasValue || PasswordReset.HasValue;
    public string? ResetToken { get; set; }

    public DateTime? ResetTokenExpires { get; set; }

    public DateTime? PasswordReset { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Updated { get; set; }

    public List<RefreshToken> RefreshTokens { get; set; }

    public bool OwnsToken(string token)
    {
        return this.RefreshTokens?.Find(x => x.Token == token) != null;
    }
    public virtual ICollection<attendance> attendances { get; set; } = new List<attendance>();

    //public virtual ICollection<group> groups { get; set; } = new List<group>();

    public virtual ICollection<lecture> lectures { get; set; } = new List<lecture>();

    public virtual ICollection<notification> notifications { get; set; } = new List<notification>();

    public virtual ICollection<portfolio> portfolios { get; set; } = new List<portfolio>();
}