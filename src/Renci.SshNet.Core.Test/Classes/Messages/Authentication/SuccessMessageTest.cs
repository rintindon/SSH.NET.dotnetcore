﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Renci.SshNetCore.Messages.Authentication;
using Renci.SshNetCore.Tests.Common;

namespace Renci.SshNetCore.Tests.Classes.Messages.Authentication
{
    /// <summary>
    ///This is a test class for SuccessMessageTest and is intended
    ///to contain all SuccessMessageTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SuccessMessageTest : TestBase
    {
        /// <summary>
        ///A test for SuccessMessage Constructor
        ///</summary>
        [TestMethod]
        [Ignore] // placeholder
        public void SuccessMessageConstructorTest()
        {
            SuccessMessage target = new SuccessMessage();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
