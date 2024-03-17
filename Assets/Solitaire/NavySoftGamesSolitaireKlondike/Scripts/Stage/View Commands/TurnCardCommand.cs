public class TurnCardCommand : ICommand
{
    private bool executed = false;
    private IViewBaseCommands viewer;
    private int id;
    private bool open;
    private bool lockOpen = false;
    public TurnCardCommand(IViewBaseCommands viewContext, int id, bool open)
    {
        this.viewer = viewContext;
        this.id = id;
        this.open = open;
        this.lockOpen = false;
    }

    public void DisableOpen()
    {
        lockOpen = true;
    }
    #region ICommand implementation
    public void execute()
    {
#if UNITY_EDITOR
        if (executed) throw new UnityEngine.UnityException("Cant execute command already executed");

#endif

        viewer.TurnCard(id, true);


        executed = true;
    }
    public void unexecute()
    {
#if UNITY_EDITOR
        if (!executed) throw new UnityEngine.UnityException("Cant undo command not executed yet");

#endif

        viewer.TurnCard(id, open);
        executed = false;
    }
    #endregion
}
