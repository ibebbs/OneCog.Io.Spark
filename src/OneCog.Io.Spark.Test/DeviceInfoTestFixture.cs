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
    public class DeviceInfoTestFixture
    {
        [Test]
        public void CanReadDeviceInfoFromStream()
        {
            Stream stream = Resources.JsonDeviceInfo.ToStream();

            IDeviceInfo deviceInfo = DeviceInfo.FromJsonStream(stream);

            Assert.That(deviceInfo.Name, Is.EqualTo("FirstCore"));
            Assert.That(deviceInfo.Id, Is.EqualTo("53ff6c065075535119511687"));
            Assert.That(deviceInfo.Connected, Is.True);
        }
    }
}
