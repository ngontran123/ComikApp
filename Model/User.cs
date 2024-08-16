using System;
using System.Collections.Generic;

namespace TestComikApp.Model;

public partial class User
{
    /// <summary>
    /// Identifier for username
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Password for the user
    /// </summary>
    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public string? Avatar { get; set; }

    public string? Phonenumber { get; set; }
}
