using System.Collections.Generic;

public class CommandExecutor {

	List<ICommand> executedCommands = new List<ICommand>();


	public void Execute (ICommand command, bool canUndo = true) {
		command.execute ();
     
		if (canUndo) {
			executedCommands.Add (command);
		}
	}
	public void Unexecute () {
		if (!HasCommands ()) {
			throw new System.Exception ("no commands found");
		}
		executedCommands [executedCommands.Count - 1].unexecute ();
		executedCommands.RemoveAt (executedCommands.Count - 1);
        ContinueModeGame.instance.UndoCardInMatch();
    }


	public void Reset(){
		executedCommands.Clear ();
	}

//	public void Execute (ICommand command, bool addToHistory) {
//		command.execute ();
//		if (addToHistory) {
//			executedCommands.Add (command);
//		}
//	}

//	public void ExecuteNext(){
//		executedCommands [executedCommands.Count - 1].execute ();
//		executedCommands.RemoveAt [executedCommands.Count - 1];
//	}

	public bool HasCommands(){
		return executedCommands.Count > 0;
	}

//	public int GetCommandsCount(){
//		return executedCommands.Count;
//	}
}
