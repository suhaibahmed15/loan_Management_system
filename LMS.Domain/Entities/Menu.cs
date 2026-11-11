namespace LMS.Domain.Entities;

public class Menu : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string? Route { get; set; }
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public int? ParentMenuId { get; set; }
    public bool IsActive { get; set; } = true;

    public Menu? ParentMenu { get; set; }
    public ICollection<Menu> ChildMenus { get; set; } = new List<Menu>();
    public ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
}

