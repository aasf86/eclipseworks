namespace eclipseworks.Domain.Entities
{
    public partial class Project
    {
        public static class ProjectRule
        {
            public const int IdValueMinimalLenth = 1;
            public const int NameMinimalLenth = 3;
            public const int NameMaxLenth = 250;
        }

        public static class ProjectMsgDialog
        {            
            public const string RequiredName = "Informe o nome do projeto.";
            public const string InvalidName = "Informe o nome com até 250 caracteres e mínimo de 3.";
            public const string RequiredUserOwner = "Informe o usuário do projeto.";
            public const string InvalidId = "Informe id do projeto.";
            public const string NotFound = "Projeto não encontrado.";
            public const string RequiredUserEvent = "Informe o usuário para esta ação.";
        }
    }
}
