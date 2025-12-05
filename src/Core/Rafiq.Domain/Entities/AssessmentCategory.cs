using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafiq.Domain.Entities;
public class AssessmentCategory
{
    public int Id { get; set; }
    public int AssessmentId { get; set; }
    public int Score { get; set; }
    public int QuestionsNumber { get; set; } = 10;
    public int CurrentQuestion { get; set; } = 1;
    public int Sequence { get; set; } // Assuming sequence for ordering categories
    public Assessment Assessment { get; set; } = null!;
    public List<Question> Questions { get; set; } = new List<Question>();
}