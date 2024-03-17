using UnityEngine;
public class SoundSettings : ScriptableObject
{
	private static SoundSettings _instance = null;
	public static SoundSettings Instance
	{
		get 
		{
			if (_instance == null)
			{
				_instance = (SoundSettings)Resources.Load("SoundSettings");
				if (_instance == null)
				{
					throw new UnityException ("Asset can't found");
				}
			}
			return _instance;
		}
	}
	// Data
 
 
 
	public AudioClip[] up;
	public AudioClip[] down;
 
	public AudioClip[] win;
	public AudioClip[] claps;

    public AudioClip buttonClick;
    public AudioClip destroyTwoCard;
    public AudioClip destroyKCard;
    public AudioClip missCard;
    public AudioClip clearRowCard;
    public AudioClip startNew;
    public AudioClip touchCard;
    public AudioClip undoCard;
    public AudioClip drawCard;
    public AudioClip hintCard;
}