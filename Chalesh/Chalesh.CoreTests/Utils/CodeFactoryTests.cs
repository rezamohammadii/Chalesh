using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chalesh.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chalesh.Core.Services;

namespace Chalesh.Core.Utils.Tests
{
    [TestClass()]
    public class CodeFactoryTests
    {
        [TestMethod()]
        public void GenerateGuidFromMacAddressTest()
        {
            var guid = CodeFactory.GenerateGuidFromMacAddress();
              Assert.AreNotEqual(guid, Guid.Empty);
        }
        [TestMethod()]
        public void ProducerTest()
        {
            KafkaService kafka = new KafkaService();
            string msg = "salam";
            ;
            kafka.Producer("my-app", msg);
        }

        [TestMethod()]
        public void ConsumerTest()
        {
            KafkaService kafka = new KafkaService();
            kafka.Listen(Console.WriteLine, "my-app");
        }

    }
}