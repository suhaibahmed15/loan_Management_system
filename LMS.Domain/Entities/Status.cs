namespace LMS.Domain.Entities;

public class Status : BaseEntity
{
    public string Category { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<LoanApplication> LoanApplications { get; set; } = new List<LoanApplication>();
    public ICollection<LoanGuarantor> LoanGuarantors { get; set; } = new List<LoanGuarantor>();
    public ICollection<LoanApproval> LoanApprovals { get; set; } = new List<LoanApproval>();
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    public ICollection<RepaymentSchedule> RepaymentSchedules { get; set; } = new List<RepaymentSchedule>();
    public ICollection<Repayment> Repayments { get; set; } = new List<Repayment>();
}

