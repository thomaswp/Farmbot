using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BlocklyGenerator
{

}

public class BlocklyMethod
{
    protected List<Func<bool>> todo = new List<Func<bool>>();

    public BlocklyMethod UpdateUntil(Func<bool> until)
    {
        todo.Add(until);
        return this;
    }

    public BlocklyMethod Do(Action action)
    {
        todo.Add(() =>
        {
            action();
            return true;
        });
        return this;
    }

    public bool Update()
    {
        while (todo.Count > 0 && todo[0]()) todo.RemoveAt(0);
        return todo.Count == 0;
    }
}

public class BlocklyFunction<T> : BlocklyMethod
{
    private T value;

    public T GetReturnValue()
    {
        return value;
    }

    protected void SetReturnValue(T value)
    {
        this.value = value;
    }

    public BlocklyFunction<T> Return(Func<T> returner)
    {
        todo.Add(() =>
        {
            value = returner();
            return true;
        });
        return this;
    }
}
