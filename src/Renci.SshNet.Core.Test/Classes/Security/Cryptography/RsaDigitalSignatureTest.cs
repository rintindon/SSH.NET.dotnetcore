﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Renci.SshNetCore.Security;
using Renci.SshNetCore.Security.Cryptography;
using Renci.SshNetCore.Tests.Common;

namespace Renci.SshNetCore.Tests.Classes.Security.Cryptography
{
    /// <summary>
    /// Implements RSA digital signature algorithm.
    /// </summary>
    [TestClass]
    public class RsaDigitalSignatureTest : TestBase
    {

        /// <summary>
        ///A test for RsaDigitalSignature Constructor
        ///</summary>
        [TestMethod]
        [Ignore] // placeholder for actual test
        public void RsaDigitalSignatureConstructorTest()
        {
            RsaKey rsaKey = null; // TODO: Initialize to an appropriate value
            RsaDigitalSignature target = new RsaDigitalSignature(rsaKey);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod]
        [Ignore] // placeholder for actual test
        public void DisposeTest()
        {
            RsaKey rsaKey = null; // TODO: Initialize to an appropriate value
            RsaDigitalSignature target = new RsaDigitalSignature(rsaKey); // TODO: Initialize to an appropriate value
            target.Dispose();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}