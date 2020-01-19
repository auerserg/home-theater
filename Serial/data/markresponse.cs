namespace HomeTheater.Serial.data
{
    internal class markresponse
    {
#pragma warning disable CS0649 // Полю "data4play.secureMark" нигде не присваивается значение, поэтому оно всегда будет иметь значение по умолчанию null.
        public string msg;
        public bool auth;
#pragma warning restore CS0649 // Полю "data4play.addr" нигде не присваивается значение, поэтому оно всегда будет иметь значение по умолчанию 0.
        public bool Status()
        {
            return auth && "success" == msg;
        }
    }
}
