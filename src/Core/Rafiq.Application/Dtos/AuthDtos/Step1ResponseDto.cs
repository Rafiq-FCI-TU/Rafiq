using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafiq.Application.Dtos.AuthDtos
{
   public class StepResponseDto
    {
        public string Token { get; set; } = null!;
        public int NextStep { get; set; }
    }

}
