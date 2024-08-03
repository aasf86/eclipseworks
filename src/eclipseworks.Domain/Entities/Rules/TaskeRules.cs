using System.Runtime.ConstrainedExecution;

namespace eclipseworks.Domain.Entities
{
    public partial class Taske
    {
        public static class TaskeRule
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

        public static class TaskeMsgDialog
        {
            public const string RequiredTitle = "Informe o nome um título para a tarefa.";
            public const string InvalidTitle = "Título inválido.";
            public const string RequiredDescription = "Informe descrição para a tarefa.";
            public const string InvalidDescription = "Título inválido.";
            public const string RequiredExpires = "Informe vencimento para a tarefa.";
            public const string RequiredStatus = "Informe o status da tarefa.";
            public const string RequiredPriority = "Informe o nível de prioridade da tarefa.";
            public const string RequiredProjectId = "Informe o projeto da tarefa.";
            public const string InvalidProjectId = "Informe o projeto válido.";
            public const string RequiredUser = "Informe o usuário da tarefa.";
            public const string NotFound = "Tarefa não encontada.";
            public const string NotAllowedChangePriority = "Não é permitido alterar prioridade.";
            public const string InvalidId = "Informe id da tarefa.";
            public const string LimitReached = "Limite de tarefas atingido para o projeto. Quantidade permitida: {0}.";

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
