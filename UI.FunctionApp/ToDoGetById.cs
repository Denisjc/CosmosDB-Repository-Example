using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using UI.FunctionApp.Data;
using UI.FunctionApp.Model;

namespace UI.FunctionApp
{
    public class ToDoGetById
    {
        private readonly IToDoRepository _toDoEntityDataStore;

        public ToDoGetById(
            IToDoRepository toDoEntityDataStore)
        {
            _toDoEntityDataStore = toDoEntityDataStore;
        }

        [FunctionName(nameof(ToDoGetById))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(ToDoGetById)} function processed a request.");

            string id = req.Query["id"];

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }


            var toDoEntity =
                await _toDoEntityDataStore.GetByIdAsync(
                    id);

            if (toDoEntity == null) return new NotFoundResult();

            var toDo =
                new ToDo(
                    toDoEntity);

            return new OkObjectResult(toDo);
        }
    }
}
