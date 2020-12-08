using Renci.SshNetCore.Common;

namespace Renci.SshNetCore.Sftp.Responses
{
    internal abstract class ExtendedReplyInfo
    {
        public abstract void LoadData(SshDataStream stream);
    }
}
