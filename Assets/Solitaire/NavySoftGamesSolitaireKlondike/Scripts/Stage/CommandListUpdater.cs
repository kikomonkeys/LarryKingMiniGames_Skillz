using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class CommandListUpdater : MonoBehaviour {

	private CommandListExecutor executor = new CommandListExecutor();

	private UnityAction onCommandExecuted = null;
	private UnityAction onCommandsListExecuted = null;

	public static CommandListUpdater instanse;
	void Awake(){
		instanse = this;
	}


	 

	public void ExecuteList (List<ICommand> listToExecute, UnityAction onCommandExecuted = null, UnityAction onListExecuted = null)
	{
		this.onCommandExecuted = onCommandExecuted;
		this.onCommandsListExecuted = onListExecuted;
 

		foreach (var c in listToExecute) {
			executor.Add (c);
		}
		pause = false;
	}

    // Update is called once per frame



    void Update()
    {
        if (Time.frameCount % 10 == 0)
            UpdateCommandExecute();
    }

	bool pause = false;

	bool is_hints_anim = false;
	private void UpdateCommandExecute()
	{
 
		if(pause)
			return;

		if (executor.HasNext ()) {
            showNext();
			is_hints_anim = true;
		}
		else {
			if (is_hints_anim) {
				stop ();
			}
		}
	}
	void  showNext(){
     
            executor.ExecuteNext();

		if (onCommandExecuted != null)
			onCommandExecuted ();
 

	}

	public void Pause ()
	{
		pause = true;
	}

	public void Continue ()
	{
		pause = false;
	}

	void stop(){
		is_hints_anim = false;

		if (onCommandsListExecuted != null)
			onCommandsListExecuted ();
 
	}





	public void Reset(){
		executor.Reset ();
		onCommandExecuted = null;
    	onCommandsListExecuted = null;

		// ??"??
	}
}
