using System.Threading.Tasks;
using Domain.Common.Entities;
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
    public class ToDoAdd
    {
        private readonly IToDoRepository _toDoEntityDataStore;

        public ToDoAdd(
            IToDoRepository toDoEntityDataStore)
        {
            _toDoEntityDataStore = toDoEntityDataStore;
        }

        [FunctionName(nameof(ToDoAdd))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(ToDoAdd)} processed a request.");

            var toDoAddOptions = await req.Body.DeserializeAsync<ToDoAddOptions>();

            var toDoEntity =
                new ToDoEntity
                {
                    Status = toDoAddOptions.Status,
                    Description = toDoAddOptions.Description
                };

            await _toDoEntityDataStore.AddAsync(toDoEntity);

            return new CreatedResult("", new ToDo(toDoEntity));
        }
    }
}
