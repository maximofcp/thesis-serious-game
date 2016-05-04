using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Constants
{
	public const float ProbabilityLow = 0.2f;
	public const float ProbabilityMedium = 0.5f;
	public const float ProbabilityHigh = 0.8f;

	public const float PathSizeX = 16;
	public const float PathSizeZ = 8;
	public const float CrosswalkSizeX = 8;
	public const float CrosswalkSizeZ = 17;
	public const float RoadSizeX = 16;
	public const float RoadSizeZ = 17;

	public const float CrosswalkWithSidewalkSizeX = 16;
	public const float CrosswalkWithSidewalkSizeZ = 33;
	public const float CrosswalkWithSidewalkSizeOffset = 48;
	public const float RoadWithSidewalkSizeX = 16;
	public const float RoadWithSidewalkSizeZ = 33;
	public const float RoadWithSidewalkSizeOffset = 48;
	public const float RoadWithSidewalkAvenueSizeX = 16;
	public const float RoadWithSidewalkAvenueSizeZ = 54;
	public const float RoadWithSidewalkAvenueSizeOffset = 48;
	public const float BuildingSizeX = 16;
	public const float BuildingSizeZ = 16;
	public const float RailroadSizeX = 16;
	public const float RailroadSizeZ = 24;
	public const float GrassSizeOffset = 48;
	public const float NumberOfCrosswalks = 1;

	public const float SpeedIndicatorOffsetY = 6;

	public const int VehicleMinSpeed = 8;
	public const int VehicleMinSpeedModel = 0;
	public const int VehicleMaxSpeedModel = 20;
	public const int VehicleMinSpeedView = 0;
	public const int VehicleMaxSpeedView = 100;

	public const float LaneNormalRightDirectionZ = 12;
	public const float LaneNormalLeftDirectionZ = 21;
	public const float LaneAvenueRightDirectionRZ = 21;
	public const float LaneAvenueRightDirectionLZ = 12;
	public const float LaneAvenueLeftDirectionRZ = 42;
	public const float LaneAvenueLeftDirectionLZ = 33;

	public const string TagVehicle = "Vehicle";
	public const string TagBicycle = "Bicycle";
	public const string TagPath = "Path";
	public const string TagCrosswalk = "Crosswalk";
	public const string TagRoad = "Road";
	public const string TagVehicleStopZone = "VehicleStopZone";
	public const string TagVehicleStopZoneAvenueRight = "VehicleStopZoneAvenueR";
	public const string TagVehicleStopZoneAvenueLeft = "VehicleStopZoneAvenueL";
	public const string TagIntentionToCrossA = "IntentionToCrossA";
	public const string TagIntentionToCrossB = "IntentionToCrossB";
	public const string TagVehicleWarningZone = "VehicleWarningZone";
	public const string TagObstacle = "Obstacle";
	public const string TagBuilding = "Building";

	public const string AnimPlayerIdle = "Idle";
	public const string AnimPlayerWalking = "Walking";

	public const int ScoreVehicleHit = 100;
	public const int ScoreCrosswalkCrossed = 200;
}

[System.Serializable]
public enum VehicleDirection
{
	Left = 0, Right = 1
}

public enum VehicleSpeed
{
	Going20 = 8,
	Going40 = 10,
	Going50 = 13,
	Going70 = 15,
	Going90 = 20
}

public class Road
{
	public float top;
	public float bot;
	public VehicleSpeed speedlimit;
	
	public Road(float topLimit, float bottomLimit, VehicleSpeed speed)
	{
		top = topLimit;
		bot = bottomLimit;
		speedlimit = speed;
	}
}

public class SpeedLimitController {
	
	private List<Road> current;
	public VehicleSpeed currentRoadSpeed;
	public int activeIndex;
	
	public SpeedLimitController () {
		current = new List<Road>();
		activeIndex = -1;
		currentRoadSpeed = 0;
	}
	
	public void AddRoad(Road road)
	{
		if(road != null)
		{
			current.Add(road);
		}
	}
	
	public Road IsInsideRoadBoundaries(float posZ)
	{
		foreach(Road road in current)
		{
			if(posZ >= road.bot && posZ < road.top)
			{
				activeIndex = current.IndexOf(road);
				return road;
			}
		}
		
		activeIndex = -1;
		return null;
	}

	public bool IsVehicleOnCurrentRoad(float vehicleZ)
	{
		if(activeIndex != -1 && current.Count > 0 && activeIndex < current.Count)
		{
			Road currentRoad = current[activeIndex];
			return vehicleZ < currentRoad.top && vehicleZ > currentRoad.bot;
		}
		return false;
	}
}

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;		// singleton
	public SpeedLimitController speedLimitController;
	public int activeRoadIndex;

	// variables
	public float playerWindowSizeX;
	public float playerWindowSizeZ;
	public Vector3 playerStartPosition;
	public float playerBoundaryZ;
	public float playerSpeed;
	public float cameraSpeed;
	public float generationOffsetX;
	public float generationOffsetZ;
	public float defaultGenerationPositionZ;
	public float defaultGenerationPositionY;
	public string sceneObjectsNodeName;
	public int crosswalkDensity;
	public float generateFrequency;
	public float vehicleCrosswalkWaitTime;
	public float defaultVehicleBrakeForce;
	public float vehicleDefaultSpeed;
	public float vehicleDefaultY;
	public float vehicleSpawnFrequency;
	public float trainDefaultSpeed;
	public float trainSpawnTime;

	public float trafficLightsInterval;
	public float trafficLightsWaitTime;

	public ScoreController scoreController;
	public GameObject gameOverUI; 


	// properties
	public float GameLeftPlayableBoundary
	{
		get
		{
			return playerStartPosition.x - playerWindowSizeX;
		}
	}

	public float GameRightPlayableBoundary
	{
		get
		{
			return playerStartPosition.x + playerWindowSizeX;
		}
	}

	public float GameLeftBoundary
	{
		get
		{
			return playerStartPosition.x - playerWindowSizeX - generationOffsetX;
		}
	}

	public float GameRightBoundary
	{
		get
		{
			return playerStartPosition.x + playerWindowSizeX + generationOffsetX;
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
