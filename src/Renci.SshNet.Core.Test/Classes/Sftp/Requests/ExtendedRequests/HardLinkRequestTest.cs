﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Renci.SshNetCore.Common;
using Renci.SshNetCore.Sftp;
using Renci.SshNetCore.Sftp.Requests;
using Renci.SshNetCore.Sftp.Responses;

namespace Renci.SshNetCore.Tests.Classes.Sftp.Requests.ExtendedRequests
{
    [TestClass]
    public class HardLinkRequestTest
    {
        private uint _protocolVersion;
        private uint _requestId;
        private string _name;
        private string _oldPath;
        private byte[] _oldPathBytes;
        private string _newPath;
        private byte[] _newPathBytes;
        private byte[] _nameBytes;

        [TestInitialize]
        public void Init()
        {
            var random = new Random();
            _protocolVersion = (uint)random.Next(0, int.MaxValue);
            _requestId = (uint)random.Next(0, int.MaxValue);
            _oldPath = random.Next().ToString(CultureInfo.InvariantCulture);
            _oldPathBytes = Encoding.UTF8.GetBytes(_oldPath);
            _newPath = random.Next().ToString(CultureInfo.InvariantCulture);
            _newPathBytes = Encoding.UTF8.GetBytes(_newPath);

            _name = "hardlink@openssh.com";
            _nameBytes = Encoding.UTF8.GetBytes(_name);
        }

        [TestMethod]
        public void Constructor()
        {
            var request = new HardLinkRequest(_protocolVersion, _requestId, _oldPath, _newPath, null);

            Assert.AreEqual(_name, request.Name);
            Assert.AreEqual(_newPath, request.NewPath);
            Assert.AreEqual(_oldPath, request.OldPath);
            Assert.AreEqual(_protocolVersion, request.ProtocolVersion);
            Assert.AreEqual(_requestId, request.RequestId);
            Assert.AreEqual(SftpMessageTypes.Extended, request.SftpMessageType);
        }

        [TestMethod]
        public void Complete_SftpStatusResponse()
        {
            IList<SftpStatusResponse> statusActionInvocations = new List<SftpStatusResponse>();

            Action<SftpStatusResponse> statusAction = statusActionInvocations.Add;
            var statusResponse = new SftpStatusResponse(_protocolVersion);

            var request = new HardLinkRequest(_protocolVersion, _requestId, _oldPath, _newPath, statusAction);

            request.Complete(statusResponse);

            Assert.AreEqual(1, statusActionInvocations.Count);
            Assert.AreSame(statusResponse, statusActionInvocations[0]);
        }

        [TestMethod]
        public void GetBytes()
        {
            var request = new HardLinkRequest(_protocolVersion, _requestId, _oldPath, _newPath, null);

            var bytes = request.GetBytes();

            var expectedBytesLength = 0;
            expectedBytesLength += 4; // Length
            expectedBytesLength += 1; // Type
            expectedBytesLength += 4; // RequestId
            expectedBytesLength += 4; // Name length
            expectedBytesLength += _nameBytes.Length; // Name
            expectedBytesLength += 4; // OldPath length
            expectedBytesLength += _oldPathBytes.Length; // OldPath
            expectedBytesLength += 4; // NewPath length
            expectedBytesLength += _newPathBytes.Length; // NewPath

            Assert.AreEqual(expectedBytesLength, bytes.Length);

            var sshDataStream = new SshDataStream(bytes);

            Assert.AreEqual((uint) bytes.Length - 4, sshDataStream.ReadUInt32());
            Assert.AreEqual((byte) SftpMessageTypes.Extended, sshDataStream.ReadByte());
            Assert.AreEqual(_requestId, sshDataStream.ReadUInt32());
            Assert.AreEqual((uint) _nameBytes.Length, sshDataStream.ReadUInt32());

            var actualNameBytes = new byte[_nameBytes.Length];
            sshDataStream.Read(actualNameBytes, 0, actualNameBytes.Length);
            Assert.IsTrue(_nameBytes.SequenceEqual(actualNameBytes));

            Assert.AreEqual((uint) _oldPathBytes.Length, sshDataStream.ReadUInt32());

            var actualOldPath = new byte[_oldPathBytes.Length];
            sshDataStream.Read(actualOldPath, 0, actualOldPath.Length);
            Assert.IsTrue(_oldPathBytes.SequenceEqual(actualOldPath));

            Assert.AreEqual((uint) _newPathBytes.Length, sshDataStream.ReadUInt32());

            var actualNewPath = new byte[_newPathBytes.Length];
            sshDataStream.Read(actualNewPath, 0, actualNewPath.Length);
            Assert.IsTrue(_newPathBytes.SequenceEqual(actualNewPath));

            Assert.IsTrue(sshDataStream.IsEndOfData);
        }
    }
}