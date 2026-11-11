namespace LMS.Domain.Entities;

public class UserPermission : BaseEntity
{
    public int UserId { get; set; }
    public int PermissionId { get; set; }
    public int AssignedByUserId { get; set; }
    public DateTime AssignedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public int? RevokedByUserId { get; set; }
    public DateTime? RevokedAt { get; set; }

    public User User { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
    public User AssignedByUser { get; set; } = null!;
    public User? RevokedByUser { get; set; }
}

