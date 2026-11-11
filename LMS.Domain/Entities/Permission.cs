namespace LMS.Domain.Entities;

public class Permission : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}

