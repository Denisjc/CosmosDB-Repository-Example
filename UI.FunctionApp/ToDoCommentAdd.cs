using System;
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
    public class ToDoCommentAdd
    {
        private readonly IToDoCommentChildRepository _toDoCommentChildRepository;

        public ToDoCommentAdd(IToDoCommentChildRepository toDoCommentEntityDataStore) 
        {

            _toDoCommentChildRepository = toDoCommentEntityDataStore;
        }

        [FunctionName(nameof(ToDoCommentAdd))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{nameof(ToDoCommentAdd)} processed a request.");

            string toDoId = req.Query["toDoId"];

            if (string.IsNullOrWhiteSpace(toDoId))
            {
                throw new ArgumentNullException(nameof(toDoId));
            }

            var toDoCommentAddOptions =
                await req.Body.DeserializeAsync<ToDoCommentAddOptions>();

            var toDoCommentEntity =
                new ToDoCommentEntity
                {
                    Body = toDoCommentAddOptions.Body,
                    CreatedOn = DateTime.UtcNow
                };

            await _toDoCommentChildRepository.AddAsync(
                toDoId,
                toDoCommentEntity);

            return new CreatedResult("", new ToDoComment(toDoCommentEntity));
        }
    }
}
