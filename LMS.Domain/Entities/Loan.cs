namespace LMS.Domain.Entities;

public class Loan : BaseEntity
{
    public string LoanNumber { get; set; } = string.Empty;
    public int LoanApplicationId { get; set; }
    public int BorrowerUserId { get; set; }
    public int StatusId { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TermMonths { get; set; }
    public decimal TotalPayableAmount { get; set; }
    public decimal OutstandingBalance { get; set; }
    public DateTime? DisbursedAt { get; set; }
    public int? DisbursedByUserId { get; set; }
    public DateTime? ClosedAt { get; set; }

    public LoanApplication LoanApplication { get; set; } = null!;
    public User BorrowerUser { get; set; } = null!;
    public User? DisbursedByUser { get; set; }
    public Status Status { get; set; } = null!;

    public ICollection<RepaymentSchedule> RepaymentSchedules { get; set; } = new List<RepaymentSchedule>();
    public ICollection<Repayment> Repayments { get; set; } = new List<Repayment>();
}

