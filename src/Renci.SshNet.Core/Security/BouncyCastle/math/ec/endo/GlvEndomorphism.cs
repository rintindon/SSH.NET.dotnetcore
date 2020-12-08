using System;

namespace Renci.SshNetCore.Security.Org.BouncyCastle.Math.EC.Endo
{
    internal interface GlvEndomorphism
        :   ECEndomorphism
    {
        BigInteger[] DecomposeScalar(BigInteger k);
    }
}
