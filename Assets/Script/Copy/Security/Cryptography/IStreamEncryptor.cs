using System.IO;

namespace CFM.Framework.Security.Cryptography
{
    internal interface IStreamEncryptor : IEncryptor
    {
        Stream Encrypt(Stream input);
    }
}