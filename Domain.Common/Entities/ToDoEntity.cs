using System;
using Domain.Core.Model;
using Newtonsoft.Json;

namespace Domain.Common.Entities
{
    public class ToDoEntity : Entity<string>
    {
        [JsonProperty("toDoId")]
        public string ToDoId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public ToDoEntity() : base()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ToDoId = this.Id;
            this.Object = "ToDo";
        }
    }
}
