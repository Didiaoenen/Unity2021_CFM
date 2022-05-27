using System.Collections;
using XLua;

[LuaCallCSharp]
public class TestTemp<T>
{
    public T GetT()
    {
        return default(T);
    }
}

[LuaCallCSharp]
public class TestEnumerator : IEnumerator
{
    public object Current => throw new System.NotImplementedException();

    public bool MoveNext()
    {
        throw new System.NotImplementedException();
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }
}
