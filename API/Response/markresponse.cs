namespace HomeTheater.API.Response
{
    public class markresponse
    {
        public bool auth;
        public string msg;

        public bool Status()
        {
            return auth && "success" == msg;
        }
    }
}