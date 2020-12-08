using System;

namespace Renci.SshNetCore.Sftp
{
    internal interface ISftpFileReader : IDisposable
    {
        byte[] Read();
    }
}
