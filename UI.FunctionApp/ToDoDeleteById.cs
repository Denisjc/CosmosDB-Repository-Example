using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using UI.FunctionApp.Data;

namespace UI.FunctionApp
{
    public class ToDoDeleteById
    {
        private readonly IToDoRepository _toDoEntityDataStore;

        public ToDoDeleteById(
            IToDoRepository toDoEntityDataStore)
        {
            _toDoEntityDataStore = toDoEntityDataStore;
        }

        [FunctionName(nameof(ToDoDeleteById))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(ToDoDeleteById)} function processed a request.");

            string id = req.Query["id"];

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _toDoEntityDataStore.DeleteByIdAsync(
                id);

            return new NoContentResult();
        }
    }
}
