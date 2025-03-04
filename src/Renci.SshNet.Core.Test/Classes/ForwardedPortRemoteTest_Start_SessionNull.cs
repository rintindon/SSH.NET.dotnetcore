﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Renci.SshNetCore.Channels;
using Renci.SshNetCore.Common;
using Renci.SshNetCore.Messages.Connection;

namespace Renci.SshNetCore.Tests.Classes
{
    [TestClass]
    public class ForwardedPortRemoteTest_Start_SessionNull
    {
        private ForwardedPortRemote _forwardedPort;
        private IPEndPoint _bindEndpoint;
        private IPEndPoint _remoteEndpoint;
        private IList<EventArgs> _closingRegister;
        private IList<ExceptionEventArgs> _exceptionRegister;
        private InvalidOperationException _actualException;

        [TestInitialize]
        public void Setup()
        {
            Arrange();
            Act();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_forwardedPort != null)
            {
                _forwardedPort.Dispose();
                _forwardedPort = null;
            }
        }

        protected void Arrange()
        {
            var random = new Random();
            _closingRegister = new List<EventArgs>();
            _exceptionRegister = new List<ExceptionEventArgs>();
            _bindEndpoint = new IPEndPoint(IPAddress.Any, random.Next(IPEndPoint.MinPort, 1000));
            _remoteEndpoint = new IPEndPoint(IPAddress.Parse("193.168.1.5"), random.Next(IPEndPoint.MinPort, IPEndPoint.MaxPort));

            _forwardedPort = new ForwardedPortRemote(_bindEndpoint.Address, (uint)_bindEndpoint.Port, _remoteEndpoint.Address, (uint)_remoteEndpoint.Port);
            _forwardedPort.Closing += (sender, args) => _closingRegister.Add(args);
            _forwardedPort.Exception += (sender, args) => _exceptionRegister.Add(args);
        }

        protected void Act()
        {
            try
            {
                _forwardedPort.Start();
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                _actualException = ex;
            }
        }

        [TestMethod]
        public void StartShouldThrowSshConnectionException()
        {
            Assert.IsNotNull(_actualException);
            Assert.AreEqual("Forwarded port is not added to a client.", _actualException.Message);
        }

        [TestMethod]
        public void IsStartedShouldReturnFalse()
        {
            Assert.IsFalse(_forwardedPort.IsStarted);
        }

        [TestMethod]
        public void ClosingShouldNotHaveFired()
        {
            Assert.AreEqual(0, _closingRegister.Count);
        }

        [TestMethod]
        public void ExceptionShouldNotHaveFired()
        {
            Assert.AreEqual(0, _exceptionRegister.Count);
        }
    }
}

