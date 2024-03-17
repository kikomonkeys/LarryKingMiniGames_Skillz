public class MoveCardCommand : ICommand
{
	private bool executed = false;
	private IViewBaseCommands viewer;
	private int id;
	private int dest_id;
	private int parent_id;
	private bool animation;

	public MoveCardCommand (IViewBaseCommands viewContext, int id, int destination_id, int parent_id, bool animation = false)
	{
		this.viewer = viewContext;
		this.id = id;
		this.dest_id = destination_id;
		this.parent_id = parent_id;
		this.animation = animation;
	}

	#region ICommand implementation
	public void execute ()
	{
		if (executed) throw new UnityEngine.UnityException ("Cant execute command already executed");
        if(ContinueModeGame.instance.LoadSuccess)
		viewer.MoveCard (id, dest_id, animation, false);
		executed = true;
	}
	public void unexecute ()
	{
		if (!executed) throw new UnityEngine.UnityException ("Cant undo command not executed yet");
		viewer.MoveCard (id, parent_id, animation, false);
		executed = false;
	}
	#endregion
}