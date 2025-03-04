﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Renci.SshNetCore.Channels;
using Renci.SshNetCore.Common;

namespace Renci.SshNetCore.Tests.Classes
{
    [TestClass]
    public class SubsystemSession_OnSessionDisconnected_Disposed
    {
        private Mock<ISession> _sessionMock;
        private Mock<IChannelSession> _channelMock;
        private string _subsystemName;
        private SubsystemSessionStub _subsystemSession;
        private int _operationTimeout;
        private IList<EventArgs> _disconnectedRegister;
        private IList<ExceptionEventArgs> _errorOccurredRegister;
            
        [TestInitialize]
        public void Setup()
        {
            Arrange();
            Act();
        }

        protected void Arrange()
        {
            var random = new Random();
            _subsystemName = random.Next().ToString(CultureInfo.InvariantCulture);
            _operationTimeout = 30000;
            _disconnectedRegister = new List<EventArgs>();
            _errorOccurredRegister = new List<ExceptionEventArgs>();

            _sessionMock = new Mock<ISession>(MockBehavior.Strict);
            _channelMock = new Mock<IChannelSession>(MockBehavior.Strict);

            var sequence = new MockSequence();
            _sessionMock.InSequence(sequence).Setup(p => p.CreateChannelSession()).Returns(_channelMock.Object);
            _channelMock.InSequence(sequence).Setup(p => p.Open());
            _channelMock.InSequence(sequence).Setup(p => p.SendSubsystemRequest(_subsystemName)).Returns(true);
            _channelMock.InSequence(sequence).Setup(p => p.Dispose());

            _subsystemSession = new SubsystemSessionStub(
                _sessionMock.Object,
                _subsystemName,
                _operationTimeout);
            _subsystemSession.Disconnected += (sender, args) => _disconnectedRegister.Add(args);
            _subsystemSession.ErrorOccurred += (sender, args) => _errorOccurredRegister.Add(args);
            _subsystemSession.Connect();
            _subsystemSession.Dispose();
        }

        protected void Act()
        {
            _sessionMock.Raise(s => s.Disconnected += null, EventArgs.Empty);
        }

        [TestMethod]
        public void DisconnectHasNeverFired()
        {
            Assert.AreEqual(0, _disconnectedRegister.Count);
        }

        [TestMethod]
        public void ErrorOccurredHasNeverFired()
        {
            Assert.AreEqual(0, _errorOccurredRegister.Count);
        }

        [TestMethod]
        public void IsOpenShouldReturnFalse()
        {
            Assert.IsFalse(_subsystemSession.IsOpen);
        }
    }
}
