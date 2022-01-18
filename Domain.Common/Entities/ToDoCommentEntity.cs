using System;
using Domain.Core.Model;
using Newtonsoft.Json;

namespace Domain.Common.Entities
{
    public class ToDoCommentEntity : ChildEntity<string>
    {
        [JsonProperty("toDoId")]
        public string ToDoId { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }


        public ToDoCommentEntity() : base()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Object = "Comment";
        }
    }
}
