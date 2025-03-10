using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnityExtededShortKeys : ScriptableObject
{
	private static Scene currentScene;
	[MenuItem("Tools/Save and Run  _F4")]
	private static void SaveAndRun()
	{
		if (!Application.isPlaying)// game is not in play mode
		{
			currentScene = SceneManager.GetActiveScene();
			EditorSceneManager.SaveOpenScenes();
			//EditorSceneManager.SaveScene(currentScene, "", false); // save scene
			//EditorSceneManager.OpenScene("Assets/Scenes/Start.unity");
			EditorApplication.ExecuteMenuItem("Edit/Play"); // play/ stop playing
		}
		else
		{

			EditorApplication.ExecuteMenuItem("Edit/Play"); // play/ stop playing
															//EditorSceneManager.OpenScene("Assets/Scenes/" + currentScene.name + "Start.unity");
		}
	}

	[MenuItem("Tools/HideUI _F5")]
	private static void HideUI()
	{
		Canvas[] canvasses = FindObjectsOfType<Canvas>();
		foreach (Canvas canvas in canvasses)
		{
			SceneVisibilityManager.instance.Hide(canvas.gameObject, true);

			for (int i = 0; i < canvas.transform.childCount; i++)
			{
				SceneVisibilityManager.instance.Hide(canvas.transform.GetChild(i).gameObject, true);
			}
		}

		SceneVisibilityManager.instance.Hide(SceneManager.GetSceneByName("Start"));
		SceneVisibilityManager.instance.Hide(SceneManager.GetSceneByName("DontDestroyOnLoad"));
	}

	[MenuItem("Tools/ShowUI _F6")]
	private static void ShowUI()
	{
		Canvas[] canvasses = FindObjectsOfType<Canvas>();
		SceneVisibilityManager.instance.Show(canvasses.Select(x => x.gameObject).ToArray(), true);
	}

	[MenuItem("Window/LayoutShortcuts/1 _F12", false, 999)]
	private static void Layout1()
	{
		EditorApplication.ExecuteMenuItem("Window/Layouts/iMove");
	}

	//[MenuItem("HotKey/Name Goes Here _F6")]
	//static void CanBeDifferent()
	//{
	//	// action goes here
	//}
}