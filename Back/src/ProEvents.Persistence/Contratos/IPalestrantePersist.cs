using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEvents.Domain;
using ProEvents.Persistence.Models;

namespace ProEvents.Persistence.Contratos
{
    public interface IPalestrantePersist : IGeralPersist
    {

        Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);
        Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);

    }
}