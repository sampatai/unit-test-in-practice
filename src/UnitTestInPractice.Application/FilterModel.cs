using UnitTestInPractice.Domain.Enum;

namespace UnitTestInPractice.Application.Projections;

public record FilterModel
{
    public string Name { get; set; } = string.Empty;
    public DateTime? DateCreated { get; set; } = null!; 
    public AssessmentStatus? Status { get; set; }
    public required int PageNumber { get; set; } = 1;
    public required int PageSize { get; set; } = 10;
}