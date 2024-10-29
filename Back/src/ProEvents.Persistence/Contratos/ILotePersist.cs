using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEvents.Domain;

namespace ProEvents.Persistence.Contratos
{
    public interface ILotePersist
    {

        /// <summary>
        /// Método get que retornará uma lista de lotes por eventoId
        /// </summary>
        /// <param name="eventoId">Código chave da tabela evento</param>
        /// <returns>Array de lotes</returns>

        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
        /// <summary>
        /// Método get que retornará apenas um lote
        /// </summary>
        /// <param name="eventoId">Código chave da tabela Evento</param>
        /// <param name="loteId">Código chave da tabela lote</param>
        /// <returns>apenas um lote</returns>
        Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId); //tem que passar chave composta


    }
}