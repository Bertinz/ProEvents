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
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ProEvents.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
           
        private readonly IEventoService _eventoService;
        private readonly ProEventsContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public EventosController(IEventoService eventoService, IWebHostEnvironment hostEnvironment)
        {
            _eventoService = eventoService;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Get() //IActionResult permite retornar os status code do http
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync(true); //sempre retornar com os participantes
                if (eventos == null) return NoContent(); //erro 404

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
                var evento = await _eventoService.GetEventoByIdAsync(id, true);
                if (evento == null) return NoContent(); //NotFound("Mensagem") = erro 404

                return Ok(evento);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{tema}/tema")] //colocar /tema porque o http pode confundir, consertando a rota
        public async Task<IActionResult> GetByTema(string tema) //ActionResult<Evento> consegue trabalhar com detalhes de evento
        {
            try
            {
                var evento = await _eventoService.GetAllEventosByTemaAsync(tema, true);
                if (evento == null) return NoContent(); 

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
                var evento = await _eventoService.GetEventoByIdAsync(eventoId, true);
                if (evento == null) return NoContent();


                var file = Request.Form.Files[0];
                if(file.Length > 0) {
                    DeleteImage(evento.ImagemURL);      
                    evento.ImagemURL = await SaveImage(file);
                }
                var EventoRetorno = await _eventoService.UpdateEvento(eventoId, evento); //atualizar no BD

                return Ok(evento);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var eventos = await _eventoService.AddEventos(model);
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
                var eventos = await _eventoService.UpdateEvento(id, model);
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
                var evento = await _eventoService.GetEventoByIdAsync(id, true);
                if (evento == null) return NoContent();

                if (await _eventoService.DeleteEvento(id)){
                    DeleteImage(evento.ImagemURL);
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

        [NonAction] //não é um endpoint, não poderá ser acessado por fora da API da mesma forma q post, get,...

        public async Task<string> SaveImage(IFormFile imageFile) {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                              .Take(10)
                                              .ToArray()
                                              ).Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}"; //serve para diferenciar imagens de mesmo nome

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create)){
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
            }

        [NonAction] //não é um endpoint, não poderá ser acessado por fora da API da mesma forma q post, get,...

        public void DeleteImage(string imageName) {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName); //pega raiz atual do caminho e concatena com o diretório criado (resources/images)
            if(System.IO.File.Exists(imagePath)){
                System.IO.File.Delete(imagePath);
            }
        }
    }
}
