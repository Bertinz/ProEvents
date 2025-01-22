using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEvents.Domain;
using ProEvents.Persistence.Context;
using ProEvents.Persistence.Contratos;

namespace ProEvents.Persistence
{
    public class RedeSocialPersist : GeralPersist, IRedeSocialPersist
    {
        private readonly ProEventsContext _context;

        public RedeSocialPersist(ProEventsContext context): base(context)
        {
            _context = context;
        }
        public async Task<RedeSocial> GetRedeSocialEventoByIdsAsync(int eventoId, int id) 
        {
            IQueryable<RedeSocial> query = _context.RedesSociais;

            query = query.AsNoTracking() //nao travar banco de dados
                         .Where(rs => rs.EventoId == eventoId && rs.Id == id);

            return await query.FirstOrDefaultAsync();    
        }
        public async Task<RedeSocial> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int id) 
        {
            IQueryable<RedeSocial> query = _context.RedesSociais;

            query = query.AsNoTracking() //nao travar banco de dados
                         .Where(rs => rs.PalestranteId == palestranteId && rs.Id == id);
                         
            return await query.FirstOrDefaultAsync();      
        }
        public async Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId) 
        {
            IQueryable<RedeSocial> query = _context.RedesSociais;

            query = query.AsNoTracking() //nao travar banco de dados
                         .Where(rs => rs.EventoId == eventoId);

            return await query.ToArrayAsync();    
        }
        public async Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId) 
        {
            IQueryable<RedeSocial> query = _context.RedesSociais;

            query = query.AsNoTracking() //nao travar banco de dados
                         .Where(rs => rs.PalestranteId == palestranteId);

            return await query.ToArrayAsync();    
        }
    }
}