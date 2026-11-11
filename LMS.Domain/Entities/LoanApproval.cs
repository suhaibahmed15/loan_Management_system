namespace LMS.Domain.Entities;

public class LoanApproval : BaseEntity
{
    public int LoanApplicationId { get; set; }
    public int DecisionByUserId { get; set; }
    public int StatusId { get; set; }
    public DateTime DecisionAt { get; set; }
    public string? Remarks { get; set; }

    public LoanApplication LoanApplication { get; set; } = null!;
    public User DecisionByUser { get; set; } = null!;
    public Status Status { get; set; } = null!;
}

