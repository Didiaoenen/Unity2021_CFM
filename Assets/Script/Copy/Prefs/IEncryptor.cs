namespace CFM.Framework.Prefs
{
    public interface IEncryptor
    {
        byte[] Encode(byte[] plainData);

        byte[] Decode(byte[] cipherData);
    }
}

