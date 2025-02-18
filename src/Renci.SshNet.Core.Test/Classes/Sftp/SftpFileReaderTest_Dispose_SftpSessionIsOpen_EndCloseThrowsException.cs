﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
#if !FEATURE_EVENTWAITHANDLE_DISPOSE
using Renci.SshNetCore.Common;
#endif // !FEATURE_EVENTWAITHANDLE_DISPOSE
using Renci.SshNetCore.Sftp;
using System;
using System.Threading;
using BufferedRead = Renci.SshNetCore.Sftp.SftpFileReader.BufferedRead;

namespace Renci.SshNetCore.Tests.Classes.Sftp
{
    [TestClass]
    public class SftpFileReaderTest_Dispose_SftpSessionIsOpen_EndCloseThrowsException : SftpFileReaderTestBase
    {
        private const int ChunkLength = 32 * 1024;

        private MockSequence _seq;
        private byte[] _handle;
        private int _fileSize;
        private WaitHandle[] _waitHandleArray;
        private int _operationTimeout;
        private SftpCloseAsyncResult _closeAsyncResult;
        private SftpFileReader _reader;
        private AsyncCallback _readAsyncCallback;
        private ManualResetEvent _beginReadInvoked;
        private EventWaitHandle _disposeCompleted;

        [TestCleanup]
        public void TearDown()
        {
            if (_beginReadInvoked != null)
            {
                _beginReadInvoked.Dispose();
            }

            if (_disposeCompleted != null)
            {
                _disposeCompleted.Dispose();
            }
        }

        protected override void SetupData()
        {
            var random = new Random();

            _handle = CreateByteArray(random, 5);
            _fileSize = 5000;
            _waitHandleArray = new WaitHandle[2];
            _operationTimeout = random.Next(10000, 20000);
            _closeAsyncResult = new SftpCloseAsyncResult(null, null);
            _beginReadInvoked = new ManualResetEvent(false);
            _disposeCompleted = new ManualResetEvent(false);
            _readAsyncCallback = null;
        }

        protected override void SetupMocks()
        {
            _seq = new MockSequence();

            SftpSessionMock.InSequence(_seq)
                           .Setup(p => p.CreateWaitHandleArray(It.IsNotNull<WaitHandle>(), It.IsNotNull<WaitHandle>()))
                           .Returns<WaitHandle, WaitHandle>((disposingWaitHandle, semaphoreAvailableWaitHandle) =>
                                {
                                    _waitHandleArray[0] = disposingWaitHandle;
                                    _waitHandleArray[1] = semaphoreAvailableWaitHandle;
                                    return _waitHandleArray;
                                });
            SftpSessionMock.InSequence(_seq)
                           .Setup(p => p.OperationTimeout)
                           .Returns(_operationTimeout);
            SftpSessionMock.InSequence(_seq)
                           .Setup(p => p.WaitAny(_waitHandleArray, _operationTimeout))
                           .Returns(() => WaitAny(_waitHandleArray, _operationTimeout));
            SftpSessionMock.InSequence(_seq)
                           .Setup(p => p.BeginRead(_handle, 0, ChunkLength, It.IsNotNull<AsyncCallback>(), It.IsAny<BufferedRead>()))
                           .Callback(() =>
                                {
                                    // harden test by making sure that we've invoked BeginRead before Dispose is invoked
                                    _beginReadInvoked.Set();
                                })
                           .Returns<byte[], ulong, uint, AsyncCallback, object>((handle, offset, length, callback, state) =>
                                {
                                    _readAsyncCallback = callback;
                                    return null;
                                })
                           .Callback(() =>
                                {
                                    // wait until Dispose has been invoked on reader to allow us to harden test, and
                                    // verify whether Dispose will prevent us from entering the read-ahead loop again
                                    _waitHandleArray[0].WaitOne();
                                });
            SftpSessionMock.InSequence(_seq)
                           .Setup(p => p.IsOpen)
                           .Returns(true);
            SftpSessionMock.InSequence(_seq)
                           .Setup(p => p.BeginClose(_handle, null, null))
                           .Returns(_closeAsyncResult);
            SftpSessionMock.InSequence(_seq)
                           .Setup(p => p.EndClose(_closeAsyncResult))
                           .Throws(new SshException());
        }

        protected override void Arrange()
        {
            base.Arrange();

            _reader = new SftpFileReader(_handle, SftpSessionMock.Object, ChunkLength, 1, _fileSize);
        }

        protected override void Act()
        {
            Assert.IsTrue(_beginReadInvoked.WaitOne(5000));
            _reader.Dispose();
        }

        [TestMethod]
        public void ReadAfterDisposeShouldThrowObjectDisposedException()
        {
            try
            {
                _reader.Read();
                Assert.Fail();
            }
            catch (ObjectDisposedException ex)
            {
                Assert.IsNull(ex.InnerException);
                Assert.AreEqual(typeof(SftpFileReader).FullName, ex.ObjectName);
            }
        }

        [TestMethod]
        public void InvokeOfReadAheadCallbackShouldCompleteImmediately()
        {
            Assert.IsNotNull(_readAsyncCallback);

            _readAsyncCallback(new SftpReadAsyncResult(null, null));
        }

        [TestMethod]
        public void EndCloseOnSftpSessionShouldHaveBeenInvokedOnce()
        {
            SftpSessionMock.Verify(p => p.EndClose(_closeAsyncResult), Times.Once);
        }
    }
}
