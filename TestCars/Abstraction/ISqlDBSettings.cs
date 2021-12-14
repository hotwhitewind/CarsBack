using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCars.Abstraction
{
    public interface ISqlDBSettings
    {
        string ConnectionString { get; set; }
    }

    public class SqlDBSettings : ISqlDBSettings
    {
        public string ConnectionString { get; set; }
    }
}
