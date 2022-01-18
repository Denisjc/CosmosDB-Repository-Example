using System.Linq;
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
    public class ToDoList
    {
        private readonly IToDoRepository _toDoEntityDataStore;

        public ToDoList(
            IToDoRepository toDoEntityDataStore)
        {
            _toDoEntityDataStore = toDoEntityDataStore;
        }

        [FunctionName(nameof(ToDoList))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(ToDoList)} function processed a request.");

            var toDoEntityList =
                await _toDoEntityDataStore.ListAsync();

            var toDoList =
                toDoEntityList.Select(
                    x => new ToDo(x));

            return new OkObjectResult(toDoList);
        }
    }
}
