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

        [TestMethod()]
        public void GetSpecificStingWithRegexTest()
        {
            Dictionary<string, string> results = new();
            results["test@example.com"] = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            results["http://example.com"] = @"^(http|https):\/\/[a-zA-Z0-9]+([\-\.]{1}[a-zA-Z0-9]+)*\.[a-zA-Z]{2,}(:[0-9]{1,5})?(\/.*)?$";
            results["abc123xyz"] = @"\d+";
            foreach (var input in results)
            {
                Assert.IsNotNull(CodeFactory.GetSpecificStingWithRegex(input.Key, input.Value));
            }
        }
    }
}