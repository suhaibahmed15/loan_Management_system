namespace LMS.Domain.Entities;

public class LoanApplication : BaseEntity
{
    public string ApplicationNumber { get; set; } = string.Empty;
    public int ApplicantUserId { get; set; }
    public int LoanTypeId { get; set; }
    public int StatusId { get; set; }
    public decimal RequestedAmount { get; set; }
    public int RequestedTermMonths { get; set; }
    public string? Purpose { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public int? ReviewedByUserId { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewRemarks { get; set; }

    public User ApplicantUser { get; set; } = null!;
    public User? ReviewedByUser { get; set; }
    public LoanType LoanType { get; set; } = null!;
    public Status Status { get; set; } = null!;

    public ICollection<LoanDocument> Documents { get; set; } = new List<LoanDocument>();
    public ICollection<LoanGuarantor> Guarantors { get; set; } = new List<LoanGuarantor>();
    public ICollection<LoanApproval> Approvals { get; set; } = new List<LoanApproval>();
    public Loan? Loan { get; set; }
}

