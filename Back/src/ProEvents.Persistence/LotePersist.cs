using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEvents.Persistence.Contratos;
using ProEvents.Domain;
using ProEvents.Persistence.Context;

namespace ProEvents.Persistence
{
    public class LotePersist : ILotePersist
    {
        private readonly ProEventsContext _context;
        public  LotePersist(ProEventsContext context)
        {
            _context = context;
            
            
        }

        public async Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query.AsNoTracking()
                         .Where(lote => lote.EventoId == eventoId
                                     && lote.Id == loteId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query.AsNoTracking()
                         .Where(lote => lote.EventoId == eventoId); //não vai buscar pelo Id, vai retornar todos os lotes do evento

            return await query.ToArrayAsync();
        }
    }
}