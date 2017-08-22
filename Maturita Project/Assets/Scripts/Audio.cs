using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Audio : MonoBehaviour {

	#region Variables
	public static Audio instance;

	#endregion


	#region Unity Methonds
	private void Awake()
    {
        instance = this;
    }

	#endregion

	public void PlaySound(AudioClip clip, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(clip, pos);
    }

}
