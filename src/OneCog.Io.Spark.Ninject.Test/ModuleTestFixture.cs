using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark.Ninject.Test
{
    [TestFixture]
    public class ModuleTestFixture
    {
        [Test]
        public void ShouldBeAbleToResolveApi()
        {
            IKernel kernel = new StandardKernel(new Io.Spark.Ninject.Module("TEST"));

            IApi api = kernel.Get<IApi>();

            Assert.That(api, Is.Not.Null);
        }

        [Test]
        public void ShouldBeAbleToResolveRestClient()
        {
            IKernel kernel = new StandardKernel(new Io.Spark.Ninject.Module("TEST"));

            IRestClient restClient = kernel.Get<IRestClient>();

            Assert.That(restClient, Is.Not.Null);
        }
    }
}
