using System;

namespace Renci.SshNetCore.Security.Org.BouncyCastle.Math.Field
{
    internal interface IFiniteField
    {
        BigInteger Characteristic { get; }

        int Dimension { get; }
    }
}
