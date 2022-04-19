namespace CFM.Framework.ObjectPool
{
    public interface IObjectFactory<T> where T : class
    {
        T Create(IObjectPool<T> pool);

        void Destroy(T obj);

        void Reset(T obj);

        bool Validate(T obj);
    }
}
