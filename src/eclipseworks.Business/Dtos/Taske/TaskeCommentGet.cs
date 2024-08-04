namespace eclipseworks.Business.Dtos.Taske
{
    public class TaskeCommentGet
    {
        public long Id { get; set; }
        public string Comment { get; set; } = "";
        public string UserOwner { get; set; } = "";
    }
}
