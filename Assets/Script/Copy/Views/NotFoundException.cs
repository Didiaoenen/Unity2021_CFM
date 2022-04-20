using System;

namespace CFM.Framework.Views
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {

        }

        public NotFoundException(string message) : base(message)
        {

        }

        public NotFoundException(Exception e) : base("", e)
        {

        }

        public NotFoundException(string message, Exception e): base(message, e)
        {
            
        }
    }
}

