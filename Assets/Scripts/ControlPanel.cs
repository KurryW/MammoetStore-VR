using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
	#region "Variables"

	[SerializeField] private TMPro.TMP_Text _fpsTextField = null;
	[SerializeField] private TMPro.TMP_InputField _showForInputField = null;
	[SerializeField] private TMPro.TMP_Text _pauseButtonText = null;
	public float Fps { get; private set; }

	private float _deltaTime = 0.0f;
	private GUIStyle _style;
	private float _msec;
	#endregion

	#region "Unity built-in Methods"
	private void Start()
	{
		_style = new GUIStyle();
		_style.alignment = TextAnchor.UpperRight;
		_style.fontSize = 14;
		_style.normal.textColor = Color.white;
	}
	private void Update()
	{
		_deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
		_msec = _deltaTime * 1000.0f;
		Fps = 1.0f / _deltaTime;
	}
	private void OnGUI()
	{
		DoShowFPS();
	}
	#endregion


	public void DoShowFPS()
	{
		//int screenWidth = Screen.width, screenHeight = Screen.height;
		//Rect rect = new Rect(0, 0, screenWidth - 160, screenHeight * 2 / 100);
		float level = QualitySettings.GetQualityLevel();
		string text = string.Format("{0:0.0} ms ({1:0.} fps) Q:{2:0.}", _msec, Fps, level);
		//GUI.Label(rect, text, _style);
		_fpsTextField.text = text;
	}

	public void OnPauseButtonClicked()
	{
		ShowManager.Instance.PauseAutoplay();

		if (ShowManager.Instance.IsPlaying) _pauseButtonText.text = "Pause";
		else _pauseButtonText.text = "Play";
	}

	public void OnShowForInputFieldChanged()
	{
		if (_showForInputField.text != "" && int.TryParse(_showForInputField.text, out int result))
		{
			ShowManager.Instance.ShowFor = result;
		}
	}
}
