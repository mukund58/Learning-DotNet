using System;
using System.Collections.Generic;

namespace _1_efcore.Models;

public partial class EfmigrationsLock
{
    public int Id { get; set; }

    public string Timestamp { get; set; } = null!;
}
