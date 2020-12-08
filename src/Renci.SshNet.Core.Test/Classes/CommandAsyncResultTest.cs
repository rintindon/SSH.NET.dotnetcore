using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Renci.SshNetCore.Tests.Common;

namespace Renci.SshNetCore.Tests.Classes
{
    [TestClass()]
    public class CommandAsyncResultTest : TestBase
    {
        [TestMethod()]
        public void BytesSentTest()
        {
            var target = new CommandAsyncResult();
            int expected = new Random().Next();

            target.BytesSent = expected;

            Assert.AreEqual(expected, target.BytesSent);
        }

        [TestMethod()]
        public void BytesReceivedTest()
        {
            var target = new CommandAsyncResult();
            var expected = new Random().Next();

            target.BytesReceived = expected;

            Assert.AreEqual(expected, target.BytesReceived);
        }
    }
}
