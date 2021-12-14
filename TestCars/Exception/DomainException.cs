using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TestCars.Domain
{
    /// <summary>
    /// This is Business Logic Exception.
    /// Should be handled correctly.
    /// </summary>
    public class DomainException : Exception
    {
        public int Code { get; } = 0;
        public object[] Parameters = new object[] { };

        public string[] ParametersString
        {
            get { return Parameters.Select(x => x.ToString()).ToArray(); }
        }

        public DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DomainException(string message) : base(message)
        {
        }
    }
}
