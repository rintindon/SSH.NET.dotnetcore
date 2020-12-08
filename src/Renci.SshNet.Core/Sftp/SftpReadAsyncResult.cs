using Renci.SshNetCore.Common;
using System;

namespace Renci.SshNetCore.Sftp
{
    internal class SftpReadAsyncResult : AsyncResult<byte[]>
    {
        public SftpReadAsyncResult(AsyncCallback asyncCallback, object state) : base(asyncCallback, state)
        {
        }
    }
}
