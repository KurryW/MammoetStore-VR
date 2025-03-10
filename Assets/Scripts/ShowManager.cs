using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowManager : MonoBehaviour
{
	[SerializeField] private GameObject[] _viewObjects = null;
	[SerializeField] private bool _isPlaying = false;
	public bool IsPlaying { get { return _isPlaying; } set { _isPlaying = value; } }

	public int ShowFor { get; set; } = 15; // seconds

	public static ShowManager Instance;


	private float _elapsedTime = 0;
	private int _currentIndex = 0;

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(this.gameObject);
	}
	private void Start()
	{
		if (IsPlaying)
			UpdateModel();
	}
	private void Update()
	{
		if (IsPlaying)
			_elapsedTime += Time.deltaTime;
		if (_elapsedTime > ShowFor)
		{
			_elapsedTime = 0;
			UpdateModel();
		}

	}

	private void UpdateModel()
	{
		for (int i = 0; i < _viewObjects.Length; i++)
		{
			_viewObjects[i].SetActive(i == _currentIndex);
		}

		_currentIndex++;
		if (_currentIndex > _viewObjects.Length - 1) _currentIndex = 0;
	}

	public void PauseAutoplay()
	{
		IsPlaying = !IsPlaying;
	}
}
