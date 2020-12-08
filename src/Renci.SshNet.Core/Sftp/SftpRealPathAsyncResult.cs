using Renci.SshNetCore.Common;
using System;

namespace Renci.SshNetCore.Sftp
{
    internal class SftpRealPathAsyncResult : AsyncResult<string>
    {
        public SftpRealPathAsyncResult(AsyncCallback asyncCallback, object state) : base(asyncCallback, state)
        {
        }
    }
}
