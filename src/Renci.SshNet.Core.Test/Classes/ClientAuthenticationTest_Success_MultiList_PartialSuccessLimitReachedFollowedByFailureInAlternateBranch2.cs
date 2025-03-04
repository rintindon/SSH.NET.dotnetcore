﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Renci.SshNetCore.Common;

namespace Renci.SshNetCore.Tests.Classes
{
    /// <summary>
    /// * ConnectionInfo provides the following authentication methods (in order):
    ///     o password
    ///     o publickey
    ///     o keyboard-interactive
    /// * Partial success limit is 2
    /// 
    ///                    none
    ///                  (1=FAIL)
    ///                     |
    ///             +-------------------+
    ///             |                   |
    ///         publickey      keyboard-interactive
    ///          (2=PS)          ^   (6=PS)
    ///             |            |      |
    ///         password         |      +-----------+
    ///          (3=PS)          |      |           |
    ///             |            |  password     publickey
    ///         password         |   (7=SKIP)    (8=FAIL)
    ///          (4=PS)          |
    ///             |            |
    ///         password         |
    ///         (5=SKIP)         |
    ///             +------------+
    /// </summary>
    [TestClass]
    public class ClientAuthenticationTest_Success_MultiList_PartialSuccessLimitReachedFollowedByFailureInAlternateBranch2 : ClientAuthenticationTestBase
    {
        private int _partialSuccessLimit;
        private ClientAuthentication _clientAuthentication;
        private SshAuthenticationException _actualException;

        protected override void SetupData()
        {
            _partialSuccessLimit = 2;
        }

        protected override void SetupMocks()
        {
            var seq = new MockSequence();

            SessionMock.InSequence(seq).Setup(p => p.RegisterMessage("SSH_MSG_USERAUTH_FAILURE"));
            SessionMock.InSequence(seq).Setup(p => p.RegisterMessage("SSH_MSG_USERAUTH_SUCCESS"));
            SessionMock.InSequence(seq).Setup(p => p.RegisterMessage("SSH_MSG_USERAUTH_BANNER"));

            ConnectionInfoMock.InSequence(seq).Setup(p => p.CreateNoneAuthenticationMethod())
                              .Returns(NoneAuthenticationMethodMock.Object);

            /* 1 */

            NoneAuthenticationMethodMock.InSequence(seq).Setup(p => p.Authenticate(SessionMock.Object))
                                        .Returns(AuthenticationResult.Failure);
            ConnectionInfoMock.InSequence(seq)
                              .Setup(p => p.AuthenticationMethods)
                              .Returns(new List<IAuthenticationMethod>
                                  {
                                      PasswordAuthenticationMethodMock.Object,
                                      PublicKeyAuthenticationMethodMock.Object,
                                      KeyboardInteractiveAuthenticationMethodMock.Object,
                                  });
            NoneAuthenticationMethodMock.InSequence(seq)
                                        .Setup(p => p.AllowedAuthentications)
                                        .Returns(new[] { "publickey", "keyboard-interactive" });

            /* Enumerate supported authentication methods */

            PasswordAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("password");
            PublicKeyAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("publickey");
            KeyboardInteractiveAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("keyboard-interactive");

            /* 2 */

            PublicKeyAuthenticationMethodMock.InSequence(seq)
                                             .Setup(p => p.Authenticate(SessionMock.Object))
                                             .Returns(AuthenticationResult.PartialSuccess);
            PublicKeyAuthenticationMethodMock.InSequence(seq)
                                             .Setup(p => p.AllowedAuthentications)
                                             .Returns(new[] { "password" });

            /* Enumerate supported authentication methods */

            PasswordAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("password");
            PublicKeyAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("publickey");
            KeyboardInteractiveAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("keyboard-interactive");

            /* 3 */

            PasswordAuthenticationMethodMock.InSequence(seq)
                                            .Setup(p => p.Authenticate(SessionMock.Object))
                                            .Returns(AuthenticationResult.PartialSuccess);
            PasswordAuthenticationMethodMock.InSequence(seq)
                                            .Setup(p => p.AllowedAuthentications)
                                            .Returns(new[] { "password" });

            /* Enumerate supported authentication methods */

            PasswordAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("password");
            PublicKeyAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("publickey");
            KeyboardInteractiveAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("keyboard-interactive");

            /* 4 */

            PasswordAuthenticationMethodMock.InSequence(seq)
                                            .Setup(p => p.Authenticate(SessionMock.Object))
                                            .Returns(AuthenticationResult.PartialSuccess);
            PasswordAuthenticationMethodMock.InSequence(seq)
                                            .Setup(p => p.AllowedAuthentications)
                                            .Returns(new[] { "password" });

            /* Enumerate supported authentication methods */

            PasswordAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("password");
            PublicKeyAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("publickey");
            KeyboardInteractiveAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("keyboard-interactive");

            /* 5: Record partial success limit reached exception, and skip password authentication method */

            PasswordAuthenticationMethodMock.InSequence(seq)
                                            .Setup(p => p.Name)
                                            .Returns("password-partial1");

            /* 6 */

            KeyboardInteractiveAuthenticationMethodMock.InSequence(seq)
                                                       .Setup(p => p.Authenticate(SessionMock.Object))
                                                       .Returns(AuthenticationResult.PartialSuccess);
            KeyboardInteractiveAuthenticationMethodMock.InSequence(seq)
                                                       .Setup(p => p.AllowedAuthentications)
                                                       .Returns(new[] {"password", "publickey"});

            /* Enumerate supported authentication methods */

            PasswordAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("password");
            PublicKeyAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("publickey");
            KeyboardInteractiveAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("keyboard-interactive");

            /* 7: Record partial success limit reached exception, and skip password authentication method */

            PasswordAuthenticationMethodMock.InSequence(seq)
                                            .Setup(p => p.Name)
                                            .Returns("password-partial2");

            /* 8 */

            PublicKeyAuthenticationMethodMock.InSequence(seq)
                                             .Setup(p => p.Authenticate(SessionMock.Object))
                                             .Returns(AuthenticationResult.Failure);
            PublicKeyAuthenticationMethodMock.InSequence(seq).Setup(p => p.Name).Returns("publickey");

            SessionMock.InSequence(seq).Setup(p => p.UnRegisterMessage("SSH_MSG_USERAUTH_FAILURE"));
            SessionMock.InSequence(seq).Setup(p => p.UnRegisterMessage("SSH_MSG_USERAUTH_SUCCESS"));
            SessionMock.InSequence(seq).Setup(p => p.UnRegisterMessage("SSH_MSG_USERAUTH_BANNER"));
        }

        protected override void Arrange()
        {
            base.Arrange();

            _clientAuthentication = new ClientAuthentication(_partialSuccessLimit);
        }

        protected override void Act()
        {
            try
            {
                _clientAuthentication.Authenticate(ConnectionInfoMock.Object, SessionMock.Object);
                Assert.Fail();
            }
            catch (SshAuthenticationException ex)
            {
                _actualException = ex;
            }
        }

        [TestMethod]
        public void AuthenticateOnKeyboardInteractiveAuthenticationMethodShouldHaveBeenInvokedOnce()
        {
            KeyboardInteractiveAuthenticationMethodMock.Verify(p => p.Authenticate(SessionMock.Object), Times.Once);
        }

        [TestMethod]
        public void AuthenticateOnPublicKeyAuthenticationMethodShouldHaveBeenInvokedTwice()
        {
            PublicKeyAuthenticationMethodMock.Verify(p => p.Authenticate(SessionMock.Object), Times.Exactly(2));
        }

        [TestMethod]
        public void AuthenticateShouldThrowSshAuthenticationException()
        {
            Assert.IsNotNull(_actualException);
            Assert.IsNull(_actualException.InnerException);
            Assert.AreEqual("Permission denied (publickey).", _actualException.Message);
        }
    }
}