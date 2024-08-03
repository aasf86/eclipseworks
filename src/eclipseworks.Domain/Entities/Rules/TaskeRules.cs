namespace eclipseworks.Domain.Entities
{
    public partial class Taske
    {
        public static class TaskeRule
        {
            public const int IdValueMinimalLenth = 1;
            public const int NameMinimalLenth = 3;
            public const int NameMaxLenth = 250;
        }

        public static class TaskeMsgDialog
        {
            public const string RequiredTitle = "Informe o nome um título para a tarefa.";
            public const string RequiredDescription = "Informe descrição para a tarefa.";
            public const string RequiredExpires = "Informe vencimento para a tarefa.";
            public const string RequiredStatus = "Informe o status da tarefa.";
            public const string RequiredPriority = "Informe o nível de prioridade da tarefa.";
            
            /*
            public const string InvalidName = "Informe o nome com até 250 caracteres e mínimo de 3.";
            public const string RequiredUserOwner = "Informe o usuário do projeto.";
            public const string InvalidId = "Informe id do projeto.";
            public const string NotFound = "Projeto não encontrado.";
            public const string RequiredUserEvent = "Informe o usuário para esta ação.";
            */
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
