using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities
{
    public class Answer:BaseEntity
    {

        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        ///TODO:DELET IT
        public DateTime AnsweredAt { get; set; }
        public int AssessmentId { get; set; }
        public Assessment Assessment { get; set; } = null!;

        public Frequency? FrequencyAnswer { get; set; }
        public DifficultyLevel? DifficultyAnswer { get; set; }

        // Calculate Score automatic
        /*  public int GetScore()
          {
              if (FrequencyAnswer.HasValue)
              {
                  return FrequencyAnswer.Value switch
                  {
                      Frequency.NotYet => 0,
                      Frequency.Sometimes => 1,
                      Frequency.Often => 2,
                      Frequency.Always => 3,
                      _ => 0
                  };
              }

              if (DifficultyAnswer.HasValue)
              {
                  return DifficultyAnswer.Value switch
                  {
                     DifficultyLevel.Low => 1,
                     DifficultyLevel.Middle => 2,
                     DifficultyLevel.High => 3,
                      _ => 0
                  };
              }

              return 0;
          }*/
    }


}
