using System;

public class DebugCommand : DebugCommandBase
{
    private Action _command;

    public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
    {
        _command = command;
    }

    public void Invoke()
    {
        _command.Invoke();
    }
}

public class DebugCommand<T> : DebugCommandBase
{
    private Action<T> _command;

    public DebugCommand(string id, string description, string format, Action<T> command) : base(id, description, format)
    {
        _command = command;
    }

    public void Invoke(T param)
    {
        _command.Invoke(param);
    }
}

public class DebugCommand<T1, T2> : DebugCommandBase
{
    private Action<T1, T2> _command;

    public DebugCommand(string id, string description, string format, Action<T1, T2> command) : base(id, description, format)
    {
        _command = command;
    }

    public void Invoke(T1 param1, T2 param2)
    {
        _command.Invoke(param1, param2);
    }
}

public class DebugCommand<T1, T2, T3> : DebugCommandBase
{
    private Action<T1, T2, T3> _command;

    public DebugCommand(string id, string description, string format, Action<T1, T2, T3> command) : base(id, description, format)
    {
        _command = command;
    }

    public void Invoke(T1 param1, T2 param2, T3 param3)
    {
        _command.Invoke(param1, param2, param3);
    }
}

public class DebugCommand<T1, T2, T3, T4> : DebugCommandBase
{
    private Action<T1, T2, T3, T4> _command;

    public DebugCommand(string id, string description, string format, Action<T1, T2, T3, T4> command) : base(id, description, format)
    {
        _command = command;
    }

    public void Invoke(T1 param1, T2 param2, T3 param3, T4 param4)
    {
        _command.Invoke(param1, param2, param3, param4);
    }
}

public class DebugCommand<T1, T2, T3, T4, T5> : DebugCommandBase
{
    private Action<T1, T2, T3, T4, T5> _command;

    public DebugCommand(string id, string description, string format, Action<T1, T2, T3, T4, T5> command) : base(id, description, format)
    {
        _command = command;
    }

    public void Invoke(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
    {
        _command.Invoke(param1, param2, param3, param4, param5);
    }
}
