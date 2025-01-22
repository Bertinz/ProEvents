using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEvents.Application.Dtos
{
    public class PalestranteAddDto
    {
        public int Id { get; set; } //nao precisa do Id
        public string MiniCurriculo { get; set; }
        public int UserId { get; set; }
        
    }
}