﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Renci.SshNetCore.NetConf;
using System;

namespace Renci.SshNetCore.Tests.Classes
{
    [TestClass]
    public class NetConfClientTest_Dispose_Disposed
    {
        private Mock<IServiceFactory> _serviceFactoryMock;
        private Mock<ISession> _sessionMock;
        private Mock<INetConfSession> _netConfSessionMock;
        private NetConfClient _netConfClient;
        private ConnectionInfo _connectionInfo;
        private int _operationTimeout;

        [TestInitialize]
        public void Setup()
        {
            Arrange();
            Act();
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        protected void Arrange()
        {
            _serviceFactoryMock = new Mock<IServiceFactory>(MockBehavior.Strict);
            _sessionMock = new Mock<ISession>(MockBehavior.Strict);
            _netConfSessionMock = new Mock<INetConfSession>(MockBehavior.Strict);

            _connectionInfo = new ConnectionInfo("host", "user", new NoneAuthenticationMethod("userauth"));
            _operationTimeout = new Random().Next(1000, 10000);
            _netConfClient = new NetConfClient(_connectionInfo, false, _serviceFactoryMock.Object);
            _netConfClient.OperationTimeout = TimeSpan.FromMilliseconds(_operationTimeout);

            var sequence = new MockSequence();
            _serviceFactoryMock.InSequence(sequence)
                .Setup(p => p.CreateSession(_connectionInfo))
                .Returns(_sessionMock.Object);
            _sessionMock.InSequence(sequence).Setup(p => p.Connect());
            _serviceFactoryMock.InSequence(sequence)
                .Setup(p => p.CreateNetConfSession(_sessionMock.Object, _operationTimeout))
                .Returns(_netConfSessionMock.Object);
            _netConfSessionMock.InSequence(sequence).Setup(p => p.Connect());
            _sessionMock.InSequence(sequence).Setup(p => p.OnDisconnecting());
            _netConfSessionMock.InSequence(sequence).Setup(p => p.Disconnect());
            _sessionMock.InSequence(sequence).Setup(p => p.Dispose());
            _netConfSessionMock.InSequence(sequence).Setup(p => p.Dispose());

            _netConfClient.Connect();
            _netConfClient.Dispose();
        }

        protected void Act()
        {
            _netConfClient.Dispose();
        }

        [TestMethod]
        public void CreateNetConfSessionOnServiceFactoryShouldBeInvokedOnce()
        {
            _serviceFactoryMock.Verify(p => p.CreateNetConfSession(_sessionMock.Object, _operationTimeout), Times.Once);
        }

        [TestMethod]
        public void CreateSessionOnServiceFactoryShouldBeInvokedOnce()
        {
            _serviceFactoryMock.Verify(p => p.CreateSession(_connectionInfo), Times.Once);
        }

        [TestMethod]
        public void DisconnectOnNetConfSessionShouldBeInvokedOnce()
        {
            _netConfSessionMock.Verify(p => p.Disconnect(), Times.Once);
        }

        [TestMethod]
        public void DisconnectOnSessionShouldNeverBeInvoked()
        {
            _sessionMock.Verify(p => p.Disconnect(), Times.Never);
        }

        [TestMethod]
        public void DisposeOnNetConfSessionShouldBeInvokedOnce()
        {
            _netConfSessionMock.Verify(p => p.Dispose(), Times.Once);
        }

        [TestMethod]
        public void DisposeOnSessionShouldBeInvokedOnce()
        {
            _sessionMock.Verify(p => p.Dispose(), Times.Once);
        }

        [TestMethod]
        public void OnDisconnectingOnSessionShouldBeInvokedOnce()
        {
            _sessionMock.Verify(p => p.OnDisconnecting(), Times.Once);
        }
    }
}
