namespace Renci.SshNetCore
{
    internal interface IClientAuthentication
    {
        void Authenticate(IConnectionInfoInternal connectionInfo, ISession session);
    }
}
