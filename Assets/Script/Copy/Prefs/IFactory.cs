namespace CFM.Framework.Prefs
{
    public interface IFactory
    {
        Preferences Create(string name);
    }
}

