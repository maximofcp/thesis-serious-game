using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ArcadeGameManager : MonoBehaviour {

	public static ArcadeGameManager instance = null;		// singleton
	public SpeedLimitController speedLimitController;
	public int activeRoadIndex;

	public VehicleSettings vehicleSettings;
	public SceneSettings sceneSettings;
	public ScoreController scoreController;
	public GameObject gameOverUI; 


	// properties
	public float GameLeftPlayableBoundary
	{
		get
		{
			return sceneSettings.playerStartPosition.x - sceneSettings.playerWindowSizeX;
		}
	}

	public float GameRightPlayableBoundary
	{
		get
		{
			return sceneSettings.playerStartPosition.x + sceneSettings.playerWindowSizeX;
		}
	}

	public float GameLeftBoundary
	{
		get
		{
			return sceneSettings.playerStartPosition.x - sceneSettings.playerWindowSizeX - sceneSettings.generationOffsetX;
		}
	}

	public float GameRightBoundary
	{
		get
		{
			return sceneSettings.playerStartPosition.x + sceneSettings.playerWindowSizeX + sceneSettings.generationOffsetX;
		}
	}

	void Start()
	{
		speedLimitController = new SpeedLimitController();
		scoreController = new ScoreController();
		gameOverUI = GameObject.Find("GameOverPanel");
		gameOverUI.SetActive(false);
		activeRoadIndex = -1;
	}

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);    

		DontDestroyOnLoad(gameObject);
	}

	public void DisplayGameOver()
	{
		if(gameOverUI != null)
			gameOverUI.SetActive(true);
	}

}
