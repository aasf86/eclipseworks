namespace eclipseworks.Domain.Entities
{
    public partial class Project : EntityBase
    {
        public Project() { }

        public Project(string name, string userOwner)
        {
            SetName(name);
            if (string.IsNullOrEmpty(userOwner)) throw new InvalidDataException(ProjectMsgDialog.RequiredUserOwner);

            Name = name;
            UserOwner = userOwner;
        }
        public string Name { get; private set; }
        public string UserOwner { get; private set; }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new InvalidDataException(ProjectMsgDialog.RequiredName);
            Name = name;
            ToUpdate();
        }
    }
}
