﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Renci.SshNetCore.Common;
using Renci.SshNetCore.Security.Cryptography.Ciphers.Paddings;

namespace Renci.SshNetCore.Tests.Classes.Security.Cryptography.Ciphers.Paddings
{
    [TestClass]
    public class PKCS7PaddingTest
    {
        private PKCS7Padding _padding;

        [TestInitialize]
        public void SetUp()
        {
            _padding = new PKCS7Padding();
        }

        [TestMethod]
        public void Pad_BlockSizeAndInput_LessThanBlockSize()
        {
            var input = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
            var expected = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x03, 0x03, 0x03 };

            var actual = _padding.Pad(8, input);

            Assert.IsTrue(expected.IsEqualTo(actual));
        }

        [TestMethod]
        public void Pad_BlockSizeAndInput_MoreThanBlockSizeButNoMultipleOfBlockSize()
        {
            var input = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 };
            var expected = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07 };

            var actual = _padding.Pad(8, input);

            Assert.IsTrue(expected.IsEqualTo(actual));
        }

        [TestMethod]
        public void Pad_BlockSizeAndInputAndOffsetAndLength_LessThanBlockSize()
        {
            var input = new byte[] { 0x0f, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
            var expected = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x03, 0x03, 0x03 };

            var actual = _padding.Pad(8, input, 1, input.Length - 2);

            Assert.IsTrue(expected.IsEqualTo(actual));
        }

        [TestMethod]
        public void Pad_BlockSizeAndInputAndOffsetAndLength_MoreThanBlockSizeButNoMultipleOfBlockSize()
        {
            var input = new byte[] { 0x0f, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10 };
            var expected = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07 };

            var actual = _padding.Pad(8, input, 1, input.Length - 2);

            Assert.IsTrue(expected.IsEqualTo(actual));
        }
    }
}
