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
    public class GeralPersist : IGeralPersist
    {
        private readonly ProEventsContext _context;
        public GeralPersist(ProEventsContext context)
        {
            _context = context;
            
        }

         public void Add<T>(T entity) where T: class{
            _context.Add(entity);
         }

        public void Update<T>(T entity) where T: class{
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T: class{
            _context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entityArray) where T: class{
            _context.RemoveRange(entityArray);
        }


        public async Task<bool> SaveChangesAsync(){
            return (await _context.SaveChangesAsync()) > 0;
        }
        
    }
}