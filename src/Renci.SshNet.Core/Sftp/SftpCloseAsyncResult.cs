using Renci.SshNetCore.Common;
using System;

namespace Renci.SshNetCore.Sftp
{
    internal class SftpCloseAsyncResult : AsyncResult
    {
        public SftpCloseAsyncResult(AsyncCallback asyncCallback, object state) : base(asyncCallback, state)
        {
        }
    }
}
