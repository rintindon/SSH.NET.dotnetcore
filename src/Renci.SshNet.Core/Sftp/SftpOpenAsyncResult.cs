using Renci.SshNetCore.Common;
using System;

namespace Renci.SshNetCore.Sftp
{
    internal class SftpOpenAsyncResult : AsyncResult<byte[]>
    {
        public SftpOpenAsyncResult(AsyncCallback asyncCallback, object state) : base(asyncCallback, state)
        {
        }
    }
}
