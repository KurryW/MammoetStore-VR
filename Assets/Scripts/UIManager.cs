using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	#region "Variables"
	[SerializeField] private RectTransform _buttonsPanel = null;
	[SerializeField] private Button _driveButton = null;

	private TMPro.TMP_Text DriveButtonText { get { return _driveButton.GetComponentInChildren<TMPro.TMP_Text>(); } }

	private InputControls _actions;
	private bool _showMenu = false;
	#endregion

	#region "Unity built-in methods"
	private void Start()
	{
		_actions = new InputControls();
		_actions.UI.Enable();
		_actions.UI.ShowButtons.performed += ShowButtons_performed;
	}
	private void Update()
	{
		if (_showMenu)
		{
			// position just in front of the user, move and rotate with the user
			_buttonsPanel.parent.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
			_buttonsPanel.parent.position = Camera.main.transform.position + Camera.main.transform.forward * 2;
		}
	}
	private void OnDestroy()
	{
		if (_actions != null)
		{
			_actions.UI.Disable();
			_actions.UI.ShowButtons.performed += ShowButtons_performed;
		}
	}
	#endregion

	private void ShowButtons_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		_showMenu = !_showMenu;
		_buttonsPanel.gameObject.SetActive(_showMenu);
	}

	public void OnResetButtonPressed()
	{
		SceneManager.LoadScene(0);
	}


	public void OnDriveButtonPressed()
	{
		if (DriveButtonText.text == "Drive")
		{
			DriveManager.Static.SPMT.StartDriving();
			DriveButtonText.text = "Stop";
		}
		else
		{
			DriveManager.Static.SPMT.StopDriving();
			DriveButtonText.text = "Drive";
		}
	}
	public void OnSetWheelsStraightPressed()
	{
		DriveManager.Static.SPMT.RotateBogiesAngle(Quaternion.identity * Quaternion.Euler(-90, 0, 0));
	}

	public void OnSetWheelsTransversePressed()
	{
		DriveManager.Static.SPMT.RotateBogiesAngle(Quaternion.identity * Quaternion.Euler(-90, 90, 0));
	}

	public void OnSetWheelsDiagonalPressed()
	{
		DriveManager.Static.SPMT.RotateBogiesAngle(Quaternion.identity * Quaternion.Euler(-90, 45, 0));
	}

	public void OnSetWheelsRotateButtonPressed()
	{
		DriveManager.Static.SPMT.RotateBogiesLookAt();
	}

}
