﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Renci.SshNetCore.Tests.Classes
{
    [TestClass]
    public class ForwardedPortRemoteTest_Stop_PortStopped : ForwardedPortRemoteTest_Dispose_PortStopped
    {
        protected override void Act()
        {
            ForwardedPort.Stop();
        }
    }
}
