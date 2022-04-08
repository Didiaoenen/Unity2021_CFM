namespace CFM.Framework.Prefs
{
    public abstract class AbstractFactory: IFactory
    {
        private IEncryptor encryptor;

        private ISerializer serializer;

        public AbstractFactory(): this(null, null)
        {

        }

        public AbstractFactory(ISerializer serializer): this(serializer, null)
        {

        }

        public AbstractFactory(ISerializer serializer, IEncryptor encryptor)
        {

        }

        public IEncryptor Encryptor
        {
            get { return encryptor; }
            protected set { encryptor = value; }
        }

        public ISerializer Serializer
        {
            get { return serializer; }
            protected set { serializer = value; }
        }

        public abstract Preferences Create(string name);
    }
}

