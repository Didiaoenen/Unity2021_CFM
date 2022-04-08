namespace CFM.Framework.Prefs
{
    public interface IEncryptor
    {
        byte[] EnCode(byte[] plainData);

        byte[] DeCode(byte[] cipherData);
    }
}

