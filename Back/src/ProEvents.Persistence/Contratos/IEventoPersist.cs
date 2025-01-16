using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEvents.Domain;
using ProEvents.Persistence.Models;

namespace ProEvents.Persistence.Contratos
{
    public interface IEventoPersist
    {

        //Eventos

        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);


    }
}