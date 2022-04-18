namespace CFM.Framework.Security.Cryptography
{
    public interface IDecryptor
    {
        string AlgorithmName { get; }

        byte[] Decrypt(byte[] buffer);
    }
}