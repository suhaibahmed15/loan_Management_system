namespace LMS.Domain.Entities;

public class User : BaseEntity
{
    public int RoleId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public Role Role { get; set; } = null!;
    public ICollection<LoanApplication> LoanApplications { get; set; } = new List<LoanApplication>();
    public ICollection<LoanApplication> ReviewedLoanApplications { get; set; } = new List<LoanApplication>();
    public ICollection<LoanDocument> UploadedLoanDocuments { get; set; } = new List<LoanDocument>();
    public ICollection<LoanDocument> VerifiedLoanDocuments { get; set; } = new List<LoanDocument>();
    public ICollection<LoanGuarantor> VerifiedGuarantors { get; set; } = new List<LoanGuarantor>();
    public ICollection<LoanApproval> LoanApprovals { get; set; } = new List<LoanApproval>();
    public ICollection<Loan> BorrowerLoans { get; set; } = new List<Loan>();
    public ICollection<Loan> DisbursedLoans { get; set; } = new List<Loan>();
    public ICollection<Repayment> RecordedRepayments { get; set; } = new List<Repayment>();
    public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    public ICollection<UserPermission> AssignedUserPermissions { get; set; } = new List<UserPermission>();
    public ICollection<UserPermission> RevokedUserPermissions { get; set; } = new List<UserPermission>();
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}

