using System.Text;

namespace Renci.SshNetCore.Sftp
{
    internal interface ISftpResponseFactory
    {
        SftpMessage Create(uint protocolVersion, byte messageType, Encoding encoding);
    }
}
