public class MoveHintCommand : ICommand
{
	private IViewBaseCommands viewer;
	private int id;
	private int dest_id;

	public MoveHintCommand (IViewBaseCommands viewContext, int id, int destination_id)
	{
		this.viewer = viewContext;
		this.id = id;
		this.dest_id = destination_id;
	}
	#region ICommand implementation
	public void execute ()
	{
		viewer.MoveCard (id, dest_id, true, true);
	}
	public void unexecute ()
    {
#if UNITY_EDITOR
        throw new UnityEngine.UnityException ("Hint can't be unexecuted!");
#endif
    }
	#endregion
}