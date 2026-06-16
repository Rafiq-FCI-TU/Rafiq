using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities;

public class GameQuestion: BaseEntity
{
    public int GameLevelId { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string? TargetLabel { get; set; }
    public string? TargetEmoji { get; set; }
    public string? MetaJson { get; set; }

    // Navigation

    public GameLevel GameLevel { get; set; } = null!;
    public ICollection<GameOption> Options { get; set; } = [];
}
