using System;
using Renci.SshNetCore.Messages.Connection;

namespace Renci.SshNetCore.Common
{
    /// <summary>
    /// Provides data for <see cref="Renci.SshNetCore.Channels.Channel.RequestReceived"/> event.
    /// </summary>
    internal class ChannelRequestEventArgs : EventArgs
    {
        /// <summary>
        /// Gets request information.
        /// </summary>
        public RequestInfo Info { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelRequestEventArgs"/> class.
        /// </summary>
        /// <param name="info">Request information.</param>
        public ChannelRequestEventArgs(RequestInfo info)
        {
            Info = info;
        }
    }
}
