namespace LMS.Domain.Entities;

public class RoleMenu : BaseEntity
{
    public int RoleId { get; set; }
    public int MenuId { get; set; }

    public Role Role { get; set; } = null!;
    public Menu Menu { get; set; } = null!;
}

