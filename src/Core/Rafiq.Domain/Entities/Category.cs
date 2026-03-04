using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities;
public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int QuestionsNumber => Questions?.Count ?? 0;

    public List<Question> Questions { get; set; } = [];
}