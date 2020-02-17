using System;

namespace HomeTheater.API.Response
{
    internal class header
    {
        public string ContentLength, ContentType, ETag, LastModified;

        public int Size => Convert.ToInt32(ContentLength);

        public string Tag => ETag.Replace(Convert.ToString('"'), "");

        public DateTime Date => Convert.ToDateTime(LastModified.Replace(" GMT", ""));
    }
}