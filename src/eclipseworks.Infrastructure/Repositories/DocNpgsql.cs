namespace eclipseworks.Infrastructure.Repositories
{
    public class DocPostgresql
    {
        public class CodeErrors
        {
            //https://www.postgresql.org/docs/8.2/errcodes-appendix.html
            public const string FOREIGN_KEY_VIOLATION = "23503";
        }
    }
}
