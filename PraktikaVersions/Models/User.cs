using System;
using System.Collections.Generic;

namespace PraktikaVersions.Models;

public partial class User
{
    public Guid id { get; set; }

    public string surname { get; set; } = null!;

    public string name { get; set; } = null!;

    public string patronymic { get; set; } = null!;

    public string email { get; set; } = null!;
}
