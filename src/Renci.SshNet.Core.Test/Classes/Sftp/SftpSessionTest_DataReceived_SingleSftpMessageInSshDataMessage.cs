﻿using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Renci.SshNetCore.Channels;
using Renci.SshNetCore.Common;
using Renci.SshNetCore.Sftp;
using Renci.SshNetCore.Sftp.Responses;
using Renci.SshNetCore.Abstractions;

namespace Renci.SshNetCore.Tests.Classes.Sftp
{
    [TestClass]
    public class SftpSessionTest_DataReceived_SingleSftpMessageInSshDataMessage
    {
        #region SftpSession.Connect()

        private Mock<ISession> _sessionMock;
        private Mock<IChannelSession> _channelSessionMock;
        private Mock<ISftpResponseFactory> _sftpResponseFactoryMock;
        private SftpSession _sftpSession;
        private int _operationTimeout;
        private Encoding _encoding;
        private uint _protocolVersion;
        private byte[] _sftpInitRequestBytes;
        private SftpVersionResponse _sftpVersionResponse;
        private byte[] _sftpRealPathRequestBytes;
        private SftpNameResponse _sftpNameResponse;
        private byte[] _sftpReadRequestBytes;
        private byte[] _sftpDataResponseBytes;
        private byte[] _handle;
        private uint _offset;
        private uint _length;
        private byte[] _data;
        private byte[] _actual;

        #endregion SftpSession.Connect()

        [TestInitialize]
        public void Setup()
        {
            Arrange();
            Act();
        }

        private void SetupData()
        {
            var random = new Random();

            #region SftpSession.Connect()

            _operationTimeout = random.Next(100, 500);
            _protocolVersion = (uint) random.Next(0, 3);
            _encoding = Encoding.UTF8;

            _sftpInitRequestBytes = new SftpInitRequestBuilder().WithVersion(SftpSession.MaximumSupportedVersion)
                                                                .Build()
                                                                .GetBytes();
            _sftpVersionResponse = new SftpVersionResponseBuilder().WithVersion(_protocolVersion)
                                                                   .Build();
            _sftpRealPathRequestBytes = new SftpRealPathRequestBuilder().WithProtocolVersion(_protocolVersion)
                                                                        .WithRequestId(1)
                                                                        .WithPath(".")
                                                                        .WithEncoding(_encoding)
                                                                        .Build()
                                                                        .GetBytes();
            _sftpNameResponse = new SftpNameResponseBuilder().WithProtocolVersion(_protocolVersion)
                                                             .WithResponseId(1)
                                                             .WithEncoding(_encoding)
                                                             .WithFile("/ABC", SftpFileAttributes.Empty)
                                                             .Build();

            #endregion SftpSession.Connect()

            _handle = CryptoAbstraction.GenerateRandom(4);
            _offset = (uint) random.Next(1, 5);
            _length = (uint) random.Next(30, 50);
            _data = CryptoAbstraction.GenerateRandom(200);
            _sftpReadRequestBytes = new SftpReadRequestBuilder().WithProtocolVersion(_protocolVersion)
                                                                .WithRequestId(2)
                                                                .WithHandle(_handle)
                                                                .WithOffset(_offset)
                                                                .WithLength(_length)
                                                                .Build()
                                                                .GetBytes();
            _sftpDataResponseBytes = new SftpDataResponseBuilder().WithProtocolVersion(_protocolVersion)
                                                                  .WithResponseId(2)
                                                                  .WithData(_data)
                                                                  .Build()
                                                                  .GetBytes();
        }

        private void CreateMocks()
        {
            _sessionMock = new Mock<ISession>(MockBehavior.Strict);
            _channelSessionMock = new Mock<IChannelSession>(MockBehavior.Strict);
            _sftpResponseFactoryMock = new Mock<ISftpResponseFactory>(MockBehavior.Strict);
        }

        private void SetupMocks()
        {
            var sequence = new MockSequence();

            #region SftpSession.Connect()

            _sessionMock.InSequence(sequence).Setup(p => p.CreateChannelSession()).Returns(_channelSessionMock.Object);
            _channelSessionMock.InSequence(sequence).Setup(p => p.Open());
            _channelSessionMock.InSequence(sequence).Setup(p => p.SendSubsystemRequest("sftp")).Returns(true);
            _channelSessionMock.InSequence(sequence).Setup(p => p.IsOpen).Returns(true);
            _channelSessionMock.InSequence(sequence).Setup(p => p.SendData(_sftpInitRequestBytes))
                                                    .Callback(() =>
                {
                    _channelSessionMock.Raise(c => c.DataReceived += null,
                                              new ChannelDataEventArgs(0, _sftpVersionResponse.GetBytes()));
                });
            _sftpResponseFactoryMock.InSequence(sequence)
                                   .Setup(p => p.Create(0U, (byte) SftpMessageTypes.Version, _encoding))
                                   .Returns(_sftpVersionResponse);
            _channelSessionMock.InSequence(sequence).Setup(p => p.IsOpen).Returns(true);
            _channelSessionMock.InSequence(sequence).Setup(p => p.SendData(_sftpRealPathRequestBytes))
                                                    .Callback(() =>
                {
                    _channelSessionMock.Raise(c => c.DataReceived += null,
                                              new ChannelDataEventArgs(0, _sftpNameResponse.GetBytes()));
                });
            _sftpResponseFactoryMock.InSequence(sequence)
                                   .Setup(p => p.Create(_protocolVersion, (byte) SftpMessageTypes.Name, _encoding))
                                   .Returns(_sftpNameResponse);

            #endregion SftpSession.Connect()

            _channelSessionMock.InSequence(sequence).Setup(p => p.IsOpen).Returns(true);
            _channelSessionMock.InSequence(sequence).Setup(p => p.SendData(_sftpReadRequestBytes))
                                                    .Callback(() =>
                {
                    _channelSessionMock.Raise(c => c.DataReceived += null,
                                              new ChannelDataEventArgs(0, _sftpDataResponseBytes));
                });
            _sftpResponseFactoryMock.InSequence(sequence)
                                   .Setup(p => p.Create(_protocolVersion, (byte) SftpMessageTypes.Data, _encoding))
                                   .Returns(new SftpDataResponse(_protocolVersion));
        }

        protected void Arrange()
        {
            SetupData();
            CreateMocks();
            SetupMocks();

            _sftpSession = new SftpSession(_sessionMock.Object, _operationTimeout, _encoding, _sftpResponseFactoryMock.Object);
            _sftpSession.Connect();
        }

        protected void Act()
        {
            _actual = _sftpSession.RequestRead(_handle, _offset, _length);
        }

        [TestMethod]
        public void ReturnedValueShouldBeDataOfSftpDataResponse()
        {
            Assert.IsTrue(_data.IsEqualTo(_actual));
        }
    }
}