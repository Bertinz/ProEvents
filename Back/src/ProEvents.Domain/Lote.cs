using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEvents.Domain
{
    public class Lote
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int Quantidade { get; set; }

        //[ForeignKey("EventosDetalhes")] especifica que EventoId é foreign key da tabela eventos detalhes (EF)
        public int EventoId { get; set; }
        public Evento Evento { get; set; }
    }
}