namespace eclipseworks.Domain.Entities
{
    public partial class TaskeComment
    {
        public static class TaskeCommentRule
        {
            public const int IdValueMinimalLenth = 1;
            public const int CommentMinimalLenth = 3;
            public const int CommentMaxLenth = 500;
        }

        public static class TaskeCommentMsgDialog
        {
            public const string InvalidId = "Informe id do comentário.";            
            public const string InvalidComment = "Informe um commentário com até 500 caracteres e mínimo de 3.";
            public const string RequiredComment = "Informe um comentário.";
            public const string RequiredUser = "Informe o usuário do comentário.";
            public const string NotFound = "Comentário não encontada.";
        }
    }
}
