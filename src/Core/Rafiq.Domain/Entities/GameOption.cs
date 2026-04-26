using Rafiq.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafiq.Domain.Entities;

public class GameOption: BaseEntity
{
    public int GameQuestionId { get; set; }
    
    public string Label { get; set; } = string.Empty;

    public string? Emoji { get; set; }

    public string? ColorHex { get; set; }

    public bool IsCorrect { get; set; }

    // Navigation
    public GameQuestion GameQuestion { get; set; } = null!;
}