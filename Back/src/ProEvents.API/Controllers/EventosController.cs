using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProEvents.Persistence;
using ProEvents.Persistence.Context;
using ProEvents.Application.Contratos;
using ProEvents.Application.Dtos;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ProEvents.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using ProEvents.Persistence.Models;
using ProEvents.API.Helpers;

namespace ProEvents.API.Controllers
{
    [Authorize] //sempre que tiver um metodo tentando acessar um extension method, precisa autorizar a requisicao (autorizou todas)
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
           
        private readonly IEventoService _eventoService;
        private readonly IUtil _util;
        private readonly IAccountService _accountService;
        private readonly string _destino = "Images";

        public EventosController(IEventoService eventoService, IUtil util, IAccountService accountService)
        {
            _eventoService = eventoService;
            _util = util;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams) //IActionResult permite retornar os status code do http
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), pageParams, true); //sempre retornar com os participantes //Pega id a partir do token (User.Get), retornando os eventos apenas de quem ta passando o token
                if (eventos == null) return NoContent(); //erro 404

                Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);

                return Ok(eventos);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) //ActionResult<Evento> consegue trabalhar com detalhes de evento
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
                if (evento == null) return NoContent(); //NotFound("Mensagem") = erro 404

                return Ok(evento);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }


        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {//endpoint
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, true);
                if (evento == null) return NoContent();


                var file = Request.Form.Files[0];
                if(file.Length > 0) {
                    _util.DeleteImage(evento.ImagemURL, _destino);      
                    evento.ImagemURL = await _util.SaveImage(file, _destino);
                }
                var EventoRetorno = await _eventoService.UpdateEvento(User.GetUserId(), eventoId, evento); //atualizar no BD

                return Ok(evento);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar realizar upload de foto de eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var eventos = await _eventoService.AddEventos(User.GetUserId(), model);
                if (eventos == null) return NoContent();

                return Ok(eventos);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var eventos = await _eventoService.UpdateEvento(User.GetUserId(), id, model);
                if (eventos == null) return NoContent();

                return Ok(eventos);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar eventos. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
                if (evento == null) return NoContent();

                if (await _eventoService.DeleteEvento(User.GetUserId(), id)){
                    _util.DeleteImage(evento.ImagemURL, _destino);
                    return Ok(new { message  = "Deletado"});
                } 
                else{
                 throw new Exception("Ocorreu um problema não especificado.");
                }
                    
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar eventos. Erro: {ex.Message}");
            }
        }

        
    }
}
