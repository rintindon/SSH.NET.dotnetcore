﻿using Renci.SshNetCore.NetConf;

namespace Renci.SshNetCore
{
    internal partial interface IServiceFactory
    {
        /// <summary>
        /// Creates a new <see cref="INetConfSession"/> in a given <see cref="ISession"/>
        /// and with the specified operation timeout.
        /// </summary>
        /// <param name="session">The <see cref="ISession"/> to create the <see cref="INetConfSession"/> in.</param>
        /// <param name="operationTimeout">The number of milliseconds to wait for an operation to complete, or -1 to wait indefinitely.</param>
        /// <returns>
        /// An <see cref="INetConfSession"/>.
        /// </returns>
        INetConfSession CreateNetConfSession(ISession session, int operationTimeout);
    }
}
