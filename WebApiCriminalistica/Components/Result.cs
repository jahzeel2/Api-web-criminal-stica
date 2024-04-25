namespace WebApiCriminalistica.Components
{
    public class Result<T>
    {
        public object dato;
        public List<T> data;
        public int totalPaginas;
        public int totalRegistros;
        public string code;
        public string message;
        public string error;
    }
}
