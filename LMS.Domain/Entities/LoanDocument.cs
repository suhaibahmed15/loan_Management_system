namespace LMS.Domain.Entities;

public class LoanDocument : BaseEntity
{
    public int LoanApplicationId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public int UploadedByUserId { get; set; }
    public bool IsVerified { get; set; }
    public int? VerifiedByUserId { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? VerificationRemarks { get; set; }

    public LoanApplication LoanApplication { get; set; } = null!;
    public User UploadedByUser { get; set; } = null!;
    public User? VerifiedByUser { get; set; }
}

