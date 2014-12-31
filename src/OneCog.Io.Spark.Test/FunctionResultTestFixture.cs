using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark.Test
{
    [TestFixture]
    public class FunctionResultTestFixture
    {
        [Test]
        public void CanReadFunctionResultFromStream()
        {
            Stream stream = Resources.JsonFunctionResult.ToStream();

            IFunctionResult functionResult = Function.FromJsonStream(stream);

            Assert.That(functionResult.Name, Is.EqualTo("prototype99"));
            Assert.That(functionResult.DeviceId, Is.EqualTo("0123456789abcdef01234567"));
            Assert.That(functionResult.ReturnValue, Is.InstanceOf<long>());
            Assert.That(functionResult.ReturnValue, Is.EqualTo(42));
            Assert.That(functionResult.Connected, Is.True);
        }
    }
}
