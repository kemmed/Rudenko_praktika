using System;
using System.Collections.Generic;

namespace Rudenko_praktika.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserLogin { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public virtual Role? Role { get; set; }
}
