using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEvents.Domain.Identity;

namespace ProEvents.Domain
{
    public class Palestrante
    {
        public int Id { get; set; }
        //public string Nome { get; set; } removido pq vai vir de User
        public string MiniCurriculo { get; set; }
        //public string ImagemURL { get; set; } removido pq vai vir de User
        // public string Telefone { get; set; } removido pq vai vir de User
        // public string Email { get; set; } removido pq vai vir de User
        public int UserId { get; set; } 
        public User User { get; set; } //o palestrante eh um usuario com a permissao
        public IEnumerable<RedeSocial> RedesSociais { get; set; }
        public IEnumerable<PalestranteEvento> PalestrantesEventos{ get; set; }
    }
}