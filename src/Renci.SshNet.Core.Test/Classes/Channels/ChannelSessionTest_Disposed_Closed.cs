using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Renci.SshNetCore.Tests.Classes.Channels
{
    [TestClass]
    public class ChannelSessionTest_Disposed_Closed : ChannelSessionTest_Dispose_Disposed
    {
        protected override void Act()
        {
            Channel.Dispose();
        }
    }
}
