namespace LMS.Domain.Entities;

public class Repayment : BaseEntity
{
    public int LoanId { get; set; }
    public int? RepaymentScheduleId { get; set; }
    public int StatusId { get; set; }
    public string PaymentReference { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal PrincipalPaid { get; set; }
    public decimal InterestPaid { get; set; }
    public DateTime PaidAt { get; set; }
    public string? PaymentMethod { get; set; }
    public int? RecordedByUserId { get; set; }
    public string? Remarks { get; set; }

    public Loan Loan { get; set; } = null!;
    public RepaymentSchedule? RepaymentSchedule { get; set; }
    public Status Status { get; set; } = null!;
    public User? RecordedByUser { get; set; }
}

