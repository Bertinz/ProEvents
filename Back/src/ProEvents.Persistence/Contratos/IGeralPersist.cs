using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEvents.Domain;

namespace ProEvents.Persistence.Contratos
{
    public interface IGeralPersist
    {

        //camada de acesso a algo externo do software
        //GERAL - Todo add, upd ou delete, vai ser feito usando esses métodos, apenas os gets(selects) serão diferentes
        void Add<T>(T entity) where T: class;
        void Update<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        void DeleteRange<T>(T[] entity) where T: class;

        Task<bool> SaveChangesAsync();

    }
}