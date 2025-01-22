using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEvents.API.Helpers;

namespace ProEvents.API.helpers
{
    public class Util : IUtil
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public Util(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        //não é um endpoint, não poderá ser acessado por fora da API da mesma forma q post, get,...

        public async Task<string> SaveImage(IFormFile imageFile, string destino) {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                              .Take(10)
                                              .ToArray()
                                              ).Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}"; //serve para diferenciar imagens de mesmo nome

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/{destino}", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create)){
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
            }

        //não é um endpoint, não poderá ser acessado por fora da API da mesma forma q post, get,...

        public void DeleteImage(string imageName, string destino) {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/{destino}", imageName); //pega raiz atual do caminho e concatena com o diretório criado (resources/images)
            if(System.IO.File.Exists(imagePath)){
                System.IO.File.Delete(imagePath);
            }
        }
    }
}