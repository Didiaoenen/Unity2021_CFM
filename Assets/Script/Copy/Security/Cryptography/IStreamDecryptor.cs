using System.IO;

namespace CFM.Framework.Security.Cryptography
{
    public interface IStreamDecryptor : IDecryptor
    {
        Stream Decrypt(Stream input);
    }
}