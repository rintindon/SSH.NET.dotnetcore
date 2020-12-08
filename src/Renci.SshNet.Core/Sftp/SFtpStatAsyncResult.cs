using Renci.SshNetCore.Common;
using System;

namespace Renci.SshNetCore.Sftp
{
    internal class SFtpStatAsyncResult : AsyncResult<SftpFileAttributes>
    {
        public SFtpStatAsyncResult(AsyncCallback asyncCallback, object state) : base(asyncCallback, state)
        {
        }
    }
}
