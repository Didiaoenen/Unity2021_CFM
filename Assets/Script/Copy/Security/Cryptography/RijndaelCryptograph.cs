using CFM.Log;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CFM.Framework.Security.Cryptography
{
    public class RijndaelCryptograph : IStreamDecryptor, IStreamEncryptor
    {
    private static readonly ILog log = LogManager.GetLogger(typeof(RijndaelCryptograph));

        private const int IV_SIZE = 16;

        private readonly static byte[] DEFAULT_IV = new byte[] { 45, 23, 12, 33, 44, 98, 67, 69, 22, 56, 22, 98, 99, 68, 75, 74 };
        
        private readonly static byte[] DEFAULT_KEY = new byte[] { 67, 69, 44, 98, 22, 12, 33, 12, 33, 44, 98, 67, 99, 68, 75, 74, 69, 22, 56, 22, 98, 98, 99, 68, 75, 74, 45, 23, 22, 56, 45, 23 };

        private readonly static char[] arr = new char[]{
            'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
            '0','1','2','3','4','5','6','7','8','9',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
        };

        public static string GenerateIV()
        {
            StringBuilder buf = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < IV_SIZE; i++)
                buf.Append(arr[rnd.Next(0, arr.Length)]);
            return buf.ToString();
        }

        public static string GenerateKey(int size)
        {
            if (size != 16 && size != 24 && size != 32)
                throw new ArgumentNullException("The 'size' must be 16 24 or 32.");

            StringBuilder buf = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < size; i++)
                buf.Append(arr[rnd.Next(0, arr.Length)]);
            return buf.ToString();
        }

        private string algorithmName;
        private RijndaelManaged rijndael;
        private byte[] key;
        private byte[] iv;

        public RijndaelCryptograph() : this(256, DEFAULT_KEY, DEFAULT_IV)
        {
        }

        public RijndaelCryptograph(byte[] key, byte[] iv) : this(256, key, iv)
        {
        }

        public RijndaelCryptograph(int keySize, byte[] key, byte[] iv)
        {
            CheckKeySize(keySize);
            CheckIV(iv);
            CheckKey(keySize, key);


            if (key == DEFAULT_KEY || iv == DEFAULT_IV)
            {
                if (log.IsWarnEnabled)
                    log.Warn("Note:Do not use the default Key and IV in the production environment.");
            }

            this.key = key;
            this.iv = iv;

            algorithmName = string.Format("AES{0}_{1}_{2}", keySize, "CBC", "PKCS7");
            rijndael = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,//use CBC
                Padding = PaddingMode.PKCS7,//default PKCS7
                KeySize = keySize,//default 256
                BlockSize = 128,//default 128
                FeedbackSize = 128  //default 128
            };

            algorithmName = string.Format("AES{0}_{1}_{2}", rijndael.KeySize, rijndael.Mode, rijndael.Padding);
        }

        protected virtual void CheckKeySize(int keySize)
        {
            if (keySize != 128 && keySize != 192 && keySize != 256)
                throw new ArgumentException("The key size must be 128, 192, or 256.");
        }

        protected virtual void CheckKey(int keySize, byte[] bytes)
        {
            if (bytes == null || bytes.Length * 8 != keySize)
                throw new ArgumentException(string.Format("The 'Key' must be {0} byte!", keySize / 8));
        }

        protected virtual void CheckIV(byte[] bytes)
        {
            if (bytes == null || bytes.Length != IV_SIZE)
                throw new ArgumentException("The 'IV' must be 16 byte!");
        }

        public virtual string AlgorithmName { get { return algorithmName; } }

        public virtual byte[] Decrypt(byte[] buffer)
        {
            using (ICryptoTransform decryptor = rijndael.CreateDecryptor(key, iv))
            {
                return decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
            }
        }

        public virtual Stream Decrypt(Stream input)
        {
            return new CryptoStream(input, rijndael.CreateDecryptor(key, iv), CryptoStreamMode.Read);
        }

        public virtual byte[] Encrypt(byte[] buffer)
        {
            using (ICryptoTransform encryptor = rijndael.CreateEncryptor(key, iv))
            {
                return encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
            }
        }

        public virtual Stream Encrypt(Stream input)
        {
            return new CryptoStream(input, rijndael.CreateEncryptor(key, iv), CryptoStreamMode.Read);
        }
    }
}
