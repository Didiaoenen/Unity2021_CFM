using System;
using System.Text;
using System.Security.Cryptography;

namespace CFM.Framework.Prefs
{
    public class DefaultEncryptor : IEncryptor
    {
        private const int IV_SIZE = 16;

        private static readonly byte[] DEFAULT_IV;

        private static readonly byte[] DEFAULT_KEY;

        private RijndaelManaged cipher;

        private byte[] iv = null;

        private byte[] key = null;

        static DefaultEncryptor()
        {
            DEFAULT_IV = Encoding.ASCII.GetBytes("5CyM5tcL3yDFiWlN");
            DEFAULT_KEY = Encoding.ASCII.GetBytes("W8fnmqMynlTJXPM1");
        }

        public DefaultEncryptor() : this(null, null)
        {
        }

        public DefaultEncryptor(byte[] key, byte[] iv)
        {
            if (iv == null)
                this.iv = DEFAULT_IV;

            if (key == null)
                this.key = DEFAULT_KEY;

            CheckIV(this.iv);
            CheckKey(this.key);

            cipher = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,//use CBC
                Padding = PaddingMode.PKCS7,//default PKCS7
                KeySize = 128,//default 256
                BlockSize = 128,//default 128
                FeedbackSize = 128      //default 128
            };
        }

        protected bool CheckKey(byte[] bytes)
        {
            if (bytes == null || (bytes.Length != 16 && bytes.Length != 24 && bytes.Length != 32))
                throw new ArgumentException("The 'Key' must be 16byte 24byte or 32byte!");
            return true;
        }

        protected bool CheckIV(byte[] bytes)
        {
            if (bytes == null || bytes.Length != IV_SIZE)
                throw new ArgumentException("The 'IV' must be 16byte!");
            return true;
        }

        public byte[] Encode(byte[] plainData)
        {
            ICryptoTransform encryptor = cipher.CreateEncryptor(key, iv);
            return encryptor.TransformFinalBlock(plainData, 0, plainData.Length);
        }

        public byte[] Decode(byte[] cipherData)
        {
            ICryptoTransform decryptor = cipher.CreateDecryptor(key, iv);
            return decryptor.TransformFinalBlock(cipherData, 0, cipherData.Length);
        }
    }
}

