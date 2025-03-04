﻿namespace Renci.SshNetCore.Messages.Connection
{
    /// <summary>
    /// Represents "eow@openssh.com" type channel request information
    /// </summary>
    public class EndOfWriteRequestInfo : RequestInfo
    {
        /// <summary>
        /// Channel request name
        /// </summary>
        public const string Name = "eow@openssh.com";

        /// <summary>
        /// Gets the name of the request.
        /// </summary>
        /// <value>
        /// The name of the request.
        /// </value>
        public override string RequestName
        {
            get { return Name; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndOfWriteRequestInfo"/> class.
        /// </summary>
        public EndOfWriteRequestInfo()
        {
            WantReply = false;
        }
    }
}
