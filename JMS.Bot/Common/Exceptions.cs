using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMS
{
    internal class JMSException : Exception
    {
        public JMSException()
        { }

        public JMSException(string message)
            : base(message)
        { }

        public JMSException(string message, params object[] obj)
            : base(String.Format(message, obj))
        { }

        public JMSException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
