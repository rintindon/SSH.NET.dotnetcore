﻿using System;

namespace Renci.SshNetCore.Security.Org.BouncyCastle.Utilities
{
    internal sealed class Times
    {
        private static long NanosecondsPerTick = 100L;

        public static long NanoTime()
        {
            return DateTime.UtcNow.Ticks * NanosecondsPerTick;
        }
    }
}
