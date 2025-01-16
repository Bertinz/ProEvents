using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProEvents.API.Models;

namespace ProEvents.API.Extensions
{
    public static class Pagination
    {
        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var pagination = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(
                pagination, options //serializar um novo objeto com base no options (em camelcase)
            ));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination"); //expor a paginacao
        }
    }
}