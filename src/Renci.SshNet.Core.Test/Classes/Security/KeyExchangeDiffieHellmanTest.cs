using Microsoft.VisualStudio.TestTools.UnitTesting;
using Renci.SshNetCore.Messages.Transport;
using Renci.SshNetCore.Security;
using Renci.SshNetCore.Tests.Common;

namespace Renci.SshNetCore.Tests.Classes.Security
{
    /// <summary>
    ///This is a test class for KeyExchangeDiffieHellmanTest and is intended
    ///to contain all KeyExchangeDiffieHellmanTest Unit Tests
    ///</summary>
    [TestClass()]
    public class KeyExchangeDiffieHellmanTest : TestBase
    {
        internal virtual KeyExchangeDiffieHellman CreateKeyExchangeDiffieHellman()
        {
            // TODO: Instantiate an appropriate concrete class.
            KeyExchangeDiffieHellman target = null;
            return target;
        }

        /// <summary>
        ///A test for Start
        ///</summary>
        [TestMethod]
        [Ignore] // placeholder for actual test
        public void StartTest()
        {
            KeyExchangeDiffieHellman target = CreateKeyExchangeDiffieHellman(); // TODO: Initialize to an appropriate value
            Session session = null; // TODO: Initialize to an appropriate value
            KeyExchangeInitMessage message = null; // TODO: Initialize to an appropriate value
            target.Start(session, message);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
