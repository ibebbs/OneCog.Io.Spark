using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneCog.Io.Spark;
using FakeItEasy;

namespace OneCog.Io.Spark.Test
{
    [TestFixture]
    public class VariableTestFixture
    {
        [Test]
        public void CanReadVariableFromStream()
        {
            Stream stream = Resources.JsonVariable.ToStream();

            IVariable variable = Variable.FromJsonStream(stream);

            Assert.That(variable.Name, Is.EqualTo("temperature"));
            Assert.That(variable.Command, Is.EqualTo("VarReturn"));
            Assert.That(variable.Result, Is.InstanceOf<long>());
            Assert.That(variable.Result, Is.EqualTo(42));
            Assert.That(variable.As<int>(), Is.InstanceOf<int>());
            Assert.That(variable.As<int>(), Is.EqualTo(42));
            Assert.That(variable.CoreInfo, Is.Not.Null);
            Assert.That(variable.CoreInfo.DeviceId, Is.EqualTo("53ff6c065075535119511687"));
            Assert.That(variable.CoreInfo.Connected, Is.True);
            Assert.That(variable.CoreInfo.LastHeard, Is.EqualTo(new DateTime(2014, 08, 22, 22, 33, 25, 407)));
            Assert.That(variable.CoreInfo.LastApp, Is.EqualTo(string.Empty));
        }

        [Test]
        [Ignore]
        public void CanWriteVariableToStream()
        {
            ICoreInfo coreInfo = A.Fake<ICoreInfo>();
            A.CallTo(() => coreInfo.DeviceId).Returns("53ff6c065075535119511687");
            A.CallTo(() => coreInfo.Connected).Returns(true);
            A.CallTo(() => coreInfo.LastHeard).Returns(new DateTime(2014, 08, 22, 22, 33, 25, 407));
            A.CallTo(() => coreInfo.LastApp).Returns(string.Empty);
            IVariable variable = A.Fake<IVariable>();
            A.CallTo(() => variable.Name).Returns("temperature");
            A.CallTo(() => variable.Command).Returns("VarReturn");
            A.CallTo(() => variable.Result).Returns(42);
            A.CallTo(() => variable.CoreInfo).Returns(coreInfo);

            string actual = Variable.ToJsonString(variable);

            Assert.That(actual, Is.EqualTo(Resources.JsonVariable));
        }
    }
}
