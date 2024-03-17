using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class CommandListExecutor {


	int iterator = 0;

	List<ICommand> listCommands = new List<ICommand>();


	public void Add(ICommand command){
		listCommands.Add (command);
	}

	public void ExecuteNext () {
		if (!HasNext ()) {
			throw new System.Exception ("no next command");
		}
     
		listCommands[iterator].execute ();
		iterator++;
	}

//	public void UnexecutePrevious () {
//	}


	public void Reset(){
		listCommands.Clear ();
		iterator = 0;
	}

	public bool HasNext(){
		return listCommands.Count > iterator;
	}

 
}
