using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using UI.FunctionApp.Data;
using UI.FunctionApp.Helpers;
using UI.FunctionApp.Model;
using UI.FunctionApp.Options;

namespace UI.FunctionApp
{
    public class ToDoUpdateById
    {
        private readonly IToDoRepository _toDoEntityDataStore;

        public ToDoUpdateById(
            IToDoRepository toDoEntityDataStore)
        {
            _toDoEntityDataStore = toDoEntityDataStore;
        }

        [FunctionName(nameof(ToDoUpdateById))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(ToDoUpdateById)} function processed a request.");

            string id = req.Query["id"];

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var toDoUpdateOptions =
                await req.Body.DeserializeAsync<ToDoUpdateOptions>();

            var toDoEntity =
                await _toDoEntityDataStore.GetByIdAsync(
                    id);

            if (toDoEntity == null) return new NotFoundResult();

            toDoEntity.Status = toDoUpdateOptions.Status;
            toDoEntity.Description = toDoUpdateOptions.Description;

            await _toDoEntityDataStore.UpdateAsync(
                toDoEntity);

            var toDo =
                new ToDo(
                    toDoEntity);

            return new OkObjectResult(toDo);
        }
    }
}
