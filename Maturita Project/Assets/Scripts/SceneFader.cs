using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour {

	#region Variables

	public Image img;
	public AnimationCurve curve;

	#endregion

	#region Unity Methods

	private void Start()
	{
		print(gameObject.activeSelf);
		StartCoroutine(FadeIn());
	}

	#endregion

	public void FadeTo(string scene)
	{
		gameObject.SetActive(true);
		StartCoroutine(FadeOut(scene));
	}

	IEnumerator FadeIn()
	{
		float t = 1f;

		while (t > 0f)
		{
			t -= Time.deltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color(0f, 0f, 0f, a);
			yield return null;
		}
	}

	IEnumerator FadeOut(string scene)
	{
		float t = 0f;

		while (t < 1f)
		{
			t += Time.deltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color(0f, 0f, 0f, a);
			yield return null;
		}

		SceneManager.LoadScene(scene);
	}



}
