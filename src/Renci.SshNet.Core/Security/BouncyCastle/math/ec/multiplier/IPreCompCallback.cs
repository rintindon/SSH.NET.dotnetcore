using System;

namespace Renci.SshNetCore.Security.Org.BouncyCastle.Math.EC.Multiplier
{
    internal interface IPreCompCallback
    {
        PreCompInfo Precompute(PreCompInfo existing);
    }
}
