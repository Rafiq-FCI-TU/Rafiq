using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafiq.Domain.Enums;

public enum GameQuestionType
{
    /// <summary>
    /// Default type — covers Sound Practice, Word Association,
    /// Emotion Explorer (emoji face), and Social Stories.
    /// </summary>
    Standard = 0,

    /// <summary>Memory Palace: flip two cards to find a matching emoji pair</summary>
    PairMatch = 1,

    /// <summary>Memory Palace: bonus awarded for finishing all pairs within the move limit</summary>
    CompletionBonus = 2,

    /// <summary>Math Quest: count the displayed emojis</summary>
    Counting = 3,

    /// <summary>Math Quest: add two groups of emojis</summary>
    Addition = 4,

    /// <summary>Math Quest: subtract one group from another</summary>
    Subtraction = 5
}
