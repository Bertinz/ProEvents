using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEvents.Domain;
using AutoMapper;
using ProEvents.Persistence.Contratos;
using ProEvents.Application.Contratos;
using ProEvents.Application.Dtos;


namespace ProEvents.Application
{
    public class EventoService : IEventoService
    {
        private readonly IMapper _mapper;
        private readonly IGeralPersist _geralPersist;
        private readonly IEventoPersist _eventoPersist;
        public EventoService(IGeralPersist geralPersist, IMapper mapper, IEventoPersist eventoPersist)
        {
            _eventoPersist = eventoPersist;
            _geralPersist = geralPersist;            
            _mapper = mapper;
        }

        public async Task<EventoDto> AddEventos(EventoDto model) //tratar o erro fora da Persistence, mas dentro da camada Service/App
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);

                _geralPersist.Add<Evento>(evento); //o elemento adicionado é alterado, o Id é alterado quando é realizado um Save
                if (await _geralPersist.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(evento.Id, false); 

                    return _mapper.Map<EventoDto>(eventoRetorno); 
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false); //pegar o evento Id, sem nenhum palestrante
                if (evento == null) return null;
                
                model.Id = evento.Id;

                _mapper.Map(model, evento); //do model para o destino evento

                _geralPersist.Update<Evento>(evento);
                if (await _geralPersist.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(evento.Id, false); 

                    return _mapper.Map<EventoDto>(eventoRetorno); 
                }
                return null;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false); //pegar o evento Id, sem nenhum palestrante
                if (evento == null) throw new Exception("Evento para delete não encontrado."); //mandar mensagem para Controller, que irá tratar a exceção
                

                _geralPersist.Delete<Evento>(evento); //Especificando que está deletando um evento
                return await _geralPersist.SaveChangesAsync();
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;
                
                
                var resultado = _mapper.Map<EventoDto[]>(eventos); 

                return resultado;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (eventos == null) 
                {
                    return null;
                }
                
                var resultado = _mapper.Map<EventoDto[]>(eventos); 

                return resultado;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);
                if (evento == null) 
                {
                    return null;
                }

                var resultado = _mapper.Map<EventoDto>(evento); 

                return resultado;

                //Substituído pelo código acima


                //var eventoRetorno = new EventoDto(){
                //        Id = evento.Id,                       
                //        Local = evento.Local,
                //        DataEvento = evento.DataEvento.ToString(),
                //        Tema = evento.Tema,
                //        QtdPessoas = evento.QtdPessoas,
                //        ImagemURL = evento.ImagemURL,
                //        Telefone = evento.Telefone,
                //        Email = evento.Email};
     
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
