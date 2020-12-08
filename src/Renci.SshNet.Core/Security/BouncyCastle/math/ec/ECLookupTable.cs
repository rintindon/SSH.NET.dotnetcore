using System;

namespace Renci.SshNetCore.Security.Org.BouncyCastle.Math.EC
{
    internal interface ECLookupTable
    {
        int Size { get; }
        ECPoint Lookup(int index);
    }
}
