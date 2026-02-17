using System;
using System.Collections.Generic;

namespace _1_efcore.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
