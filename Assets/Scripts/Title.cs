using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
	public void ReturnToTitleScreen()
	{
		SceneManager.LoadScene(0);
	}

	public void PlayGame()
	{
		SceneManager.LoadScene(1);
	}

	public void ShowHelp()
	{
		SceneManager.LoadScene(2);
	}

	public void QuitGame()
	{
		Application.Quit();

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}