using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark.Events
{
    interface IFactory
    {
        IEnumerable<IEvent> Create(string[] messages);
    }
}
