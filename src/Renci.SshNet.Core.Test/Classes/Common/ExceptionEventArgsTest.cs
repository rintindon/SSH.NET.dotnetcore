﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Renci.SshNetCore.Common;
using Renci.SshNetCore.Tests.Common;

namespace Renci.SshNetCore.Tests.Classes.Common
{
    /// <summary>
    ///This is a test class for ExceptionEventArgsTest and is intended
    ///to contain all ExceptionEventArgsTest Unit Tests
    ///</summary>
    [TestClass]
    public class ExceptionEventArgsTest : TestBase
    {
        /// <summary>
        ///A test for ExceptionEventArgs Constructor
        ///</summary>
        [TestMethod]
        public void ExceptionEventArgsConstructorTest()
        {
            Exception exception = null; // TODO: Initialize to an appropriate value
            ExceptionEventArgs target = new ExceptionEventArgs(exception);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
