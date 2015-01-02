using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark.Test
{
    [TestFixture]
    public class DevicesTestFixture
    {
        [Test]
        public void CanReadDevicesFromStream()
        {
            Stream stream = Resources.JsonDevices.ToStream();

            IEnumerable<IDevicesInfo> cores = Devices.FromJsonStream(stream);

            Assert.That(cores.Count(), Is.EqualTo(1));

            IDevicesInfo core = cores.First();

            Assert.That(core.Name, Is.EqualTo("FirstCore"));
            Assert.That(core.DeviceId, Is.EqualTo("53ff6c065075535119511687"));
            Assert.That(core.Connected, Is.True);
            Assert.That(core.LastHeard, Is.EqualTo(new DateTime(2014, 08, 22, 22, 33, 25, 407)));
            Assert.That(core.LastApp, Is.Null);
        }
    }
}
