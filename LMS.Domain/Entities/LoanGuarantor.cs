namespace LMS.Domain.Entities;

public class LoanGuarantor : BaseEntity
{
    public int LoanApplicationId { get; set; }
    public int StatusId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? NationalIdNumber { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? RelationshipToApplicant { get; set; }
    public string? Address { get; set; }
    public int? VerifiedByUserId { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? VerificationRemarks { get; set; }

    public LoanApplication LoanApplication { get; set; } = null!;
    public Status Status { get; set; } = null!;
    public User? VerifiedByUser { get; set; }
}

