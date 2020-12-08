namespace Renci.SshNetCore.Security.Org.BouncyCastle.Math.Field
{
    internal interface IPolynomialExtensionField
        : IExtensionField
    {
        IPolynomial MinimalPolynomial { get; }
    }
}