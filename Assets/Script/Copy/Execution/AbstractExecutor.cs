namespace CFM.Framework.Execution
{
    public abstract class AbstractExecutor
    {
        static AbstractExecutor()
        {
            Executors.Create();
        }
    }
}

