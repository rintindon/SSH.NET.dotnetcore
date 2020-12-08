using System;

namespace Renci.SshNetCore.Security.Org.BouncyCastle.Math.EC
{
    internal interface ECPointMap
    {
        ECPoint Map(ECPoint p);
    }
}
