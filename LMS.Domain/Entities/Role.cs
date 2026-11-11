namespace LMS.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

