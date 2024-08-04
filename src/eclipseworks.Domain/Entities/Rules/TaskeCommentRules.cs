namespace eclipseworks.Domain.Entities
{
    public partial class TaskeComment
    {
        public static class TaskeCommentRule
        {
            //aasf86
            public const int IdValueMinimalLenth = 1;
            public const int TitleMinimalLenth = 3;
            public const int TitleMaxLenth = 250;

            public const int DescriptionMinimalLenth = 3;
            public const int DescriptionMaxLenth = 250;

            public const int MaximumNumberTaskesPerProject = 20;

            /*
            public const int IdValueMinimalLenth = 1;
            public const int NameMinimalLenth = 3;
            public const int NameMaxLenth = 250;
            */
        }

        public static class TaskeCommentMsgDialog
        {
            public const string RequiredComment = "Informe um comentário.";
            public const string RequiredUserOwner = "Informe o usuário do comentário.";
        }

        public enum eStatus
        {
            Nulo = 0,
            Pending = 1,
            Progress = 2,
            Finished = 3
        }

        public enum ePriority
        {
            Nulo = 0,
            Average = 1,
            Low = 2,
            High = 3
        }
    }
}
