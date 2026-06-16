using Rafiq.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafiq.Application.Dtos.PracticeCategoryDtos;
public class PracticeCategoryResponse
{
    public double Score { get; set; }
    public double QuestionsEarnedPoints { get; set; }
    public double QuestionsMaxPoints { get; set; } // dynamic: Current question * 10 (question points - static)
    public int QuestionsNumber { get; set; } = 10;
    public int CurrentQuestion { get; set; } = 1;
    public int AssessmentId { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}
