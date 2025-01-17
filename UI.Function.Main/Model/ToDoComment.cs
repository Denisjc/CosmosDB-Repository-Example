﻿using System;
using Domain.Common.Entities;

namespace UI.FunctionApp.Model
{
    public class ToDoComment
    {
        public string Id { get; set; }
        public string ToDoId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }

        public ToDoComment()
        {
        }

        public ToDoComment(
            ToDoCommentEntity toDoCommentEntity)
        {
            this.Id = toDoCommentEntity.Id;
            this.ToDoId = toDoCommentEntity.ToDoId;
            this.Body = toDoCommentEntity.Body;
            this.CreatedOn = toDoCommentEntity.CreatedOn;
        }
    }
}
