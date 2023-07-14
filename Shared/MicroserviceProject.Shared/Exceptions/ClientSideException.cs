namespace MicroserviceProject.Shared.Exceptions
{
    public class ClientSideException : Exception
    {
        public ClientSideException():base()
        {
            
        }
        
        public ClientSideException(string message) : base(message)
        {

        }
        
        public ClientSideException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
