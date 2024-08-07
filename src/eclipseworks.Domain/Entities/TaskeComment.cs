﻿namespace eclipseworks.Domain.Entities
{
    public partial class TaskeComment : EntityBase
    {
        public TaskeComment() { }

        public TaskeComment(string comment, string userOwner, long taskeId)
        {
            SetComment(comment);
            if (string.IsNullOrEmpty(userOwner)) throw new InvalidDataException(TaskeCommentMsgDialog.RequiredUser);

            Comment = comment;
            UserOwner = userOwner;
            TaskeId = taskeId;
        }

        public long TaskeId { get; private set; }
        public string? Comment { get; private set; }
        public string? UserOwner { get; private set; }

        public void SetComment(string comment)
        {
            if (string.IsNullOrEmpty(comment)) throw new InvalidDataException(TaskeCommentMsgDialog.RequiredComment);
            Comment = comment;
            ToUpdate();
        }
    }
}
