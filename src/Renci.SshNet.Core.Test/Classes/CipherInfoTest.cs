using Microsoft.VisualStudio.TestTools.UnitTesting;
using Renci.SshNetCore.Common;
using Renci.SshNetCore.Security.Cryptography;
using Renci.SshNetCore.Tests.Common;
using System;

namespace Renci.SshNetCore.Tests.Classes
{
    /// <summary>
    /// Holds information about key size and cipher to use
    /// </summary>
    [TestClass]
    public class CipherInfoTest : TestBase
    {
        /// <summary>
        ///A test for CipherInfo Constructor
        ///</summary>
        [TestMethod]
        [Ignore] // placeholder
        public void CipherInfoConstructorTest()
        {
            int keySize = 0; // TODO: Initialize to an appropriate value
            Func<byte[], byte[], Cipher> cipher = null; // TODO: Initialize to an appropriate value
            CipherInfo target = new CipherInfo(keySize, cipher);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}