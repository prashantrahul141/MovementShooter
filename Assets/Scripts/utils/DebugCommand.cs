using System;

public class DebugCommandBase
{
    private string m_commandID;
    private string m_commandDesc;
    private string m_commandFormat;

    public string commandID
    {
        get { return m_commandID; }
    }

    public string commandDesc
    {
        get { return m_commandDesc; }
    }

    public string commandFormat
    {
        get { return m_commandFormat; }
    }

    public DebugCommandBase(string id, string desc, string format)
    {
        m_commandID = id;
        m_commandDesc = desc;
        m_commandFormat = format;
    }
}

public class DebugCommand : DebugCommandBase
{
    private Action command;

    public DebugCommand(string id, string desc, string format, Action command)
        : base(id, desc, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}
