namespace LMS.Domain.Entities;

public class RepaymentSchedule : BaseEntity
{
    public int LoanId { get; set; }
    public int InstallmentNumber { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PrincipalDue { get; set; }
    public decimal InterestDue { get; set; }
    public decimal TotalDue { get; set; }
    public decimal PaidAmount { get; set; }
    public int StatusId { get; set; }
    public DateTime? PaidAt { get; set; }

    public Loan Loan { get; set; } = null!;
    public Status Status { get; set; } = null!;
    public ICollection<Repayment> Repayments { get; set; } = new List<Repayment>();
}

