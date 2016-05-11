using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameBuilder : MonoBehaviour 
{
	
	// Prefabs
	public List<GameObject> pfBuildings;
	public GameObject pfGrassOffset;
	public GameObject pfRoad;
	public GameObject pfRoadOffset;
	public GameObject pfCrosswalk;
	public GameObject pfRoadAvenue;
	public GameObject pfRoadAvenueOffset;
	public GameObject pfRoadAvenueCrossroad;
	public GameObject pfRailroad;
	public GameObject pfRailroadPassage;

	public Text speedLimitUI;
	public Image imageUI;

	public GameObject player;

	private Vector3 pointer;
	private ArcadeGameManager gameManager;
	private Transform scene;

	private List<ISceneBlockBuilder> builderPool;
	
	private enum SceneRowType
	{
		SidewalkSimple = 0, 
		GrassSimple = 1, 
		RoadWithCrosswalk = 2
	}

	void Start()
	{
		// initialize
		gameManager = ArcadeGameManager.instance;

		builderPool = new List<ISceneBlockBuilder>();
		scene = GameObject.Find (gameManager.sceneSettings.sceneObjectsNodeName).transform;

		// sets pointer to initial state
		float pointerx = gameManager.GameLeftBoundary;
		float pointery = gameManager.sceneSettings.defaultGenerationPositionY;
		float pointerz = gameManager.sceneSettings.defaultGenerationPositionZ;
		pointer = new Vector3(pointerx, pointery, pointerz);

		// adds premade builder strategies to the pool
		ISceneBlockBuilder road = new SceneRowBuilderRoad(pfRoad, pfRoadOffset, pfCrosswalk);
		ISceneBlockBuilder avenue = new SceneRowBuilderRoadAvenue(pfRoadAvenue, pfRoadAvenueOffset, pfRoadAvenueCrossroad);
		ISceneBlockBuilder grass = new SceneRowBuilderRandomBuilding(pfBuildings, pfGrassOffset);
		ISceneBlockBuilder railroad = new SceneRowBuilderRailroad(pfRailroad, pfRailroadPassage);

		builderPool.Add(road);
		builderPool.Add(avenue);
		builderPool.Add(grass);
		builderPool.Add(railroad);

		BuildScene();

		InvokeRepeating("RecycleSceneObjects", 0, 5);
	}

	void Update()
	{
		BuildScene();
		ControlSpeedLimit();
	}

	void ControlSpeedLimit()
	{
		Road current = gameManager.speedLimitController.IsInsideRoadBoundaries(player.transform.position.z);
		if(current != null && gameManager.activeRoadIndex != gameManager.speedLimitController.activeIndex)
		{
			// show UI (inside road bounds)
			ToggleUI(true);

			// store current road index on list
			gameManager.activeRoadIndex = gameManager.speedLimitController.activeIndex;

			// store road speed
			//TODO: maybe change this index and speed to a Road object. Makes more sense
			gameManager.speedLimitController.currentRoadSpeed = current.speedlimit;
			
			// change new road speed
			speedLimitUI.text = VehicleController.GetCurrentSpeedIndicator((int)current.speedlimit);

		}
		else if(current == null)
		{
			// Hide UI (outisde a road)
			ToggleUI(false);
		}
	}

	void BuildScene()
	{
		// if pointer is inside game window
		if(pointer.z < player.transform.position.z + gameManager.sceneSettings.generationOffsetZ && builderPool.Count > 0)
		{	
			List<GameObject> instantiatedObjects = new List<GameObject>();
			float totalSizeZ = 0.0f;
			int currentLevel = 1;
			int chosen = Random.Range(0, builderPool.Count);

			// pick a random scene builder from the bool
			ISceneBlockBuilder builder = builderPool[chosen];

			// calls the scene builder to generate itself
			List<SceneObject> objects = builder.GenerateSceneRow(gameManager.GameLeftBoundary, gameManager.GameRightBoundary, gameManager.GameLeftPlayableBoundary, gameManager.GameRightPlayableBoundary);

			// register road
			if(builder.GetType() == typeof(SceneRowBuilderRoad))
			{
				Road road = new Road(pointer.z + Constants.Dimension.CrosswalkWithSidewalkSizeZ, pointer.z, GenerateRoadSpeed() );
				gameManager.speedLimitController.AddRoad(road);
			}

			// register avenue
			if(builder.GetType() == typeof(SceneRowBuilderRoadAvenue))
			{
				Road road = new Road(pointer.z + Constants.Dimension.RoadWithSidewalkAvenueSizeZ, pointer.z, GenerateRoadSpeed() );
				gameManager.speedLimitController.AddRoad(road);
			}
				
			
			// builds the scene based on the objects from scene builder
			foreach(SceneObject obj in objects)
			{
				// set first row z value
				if(totalSizeZ == 0.0f)
					totalSizeZ = obj.sizeZ;

				// if level increases, means that other row has been added
				if(obj.level > currentLevel)
				{
					// Resets pointer
					pointer.x = gameManager.GameLeftBoundary;
					pointer.z += obj.sizeZ;
					currentLevel++;
				}

				// instantiates new game object
				GameObject instantiatedObject = InstantiateSceneObject(obj.prefab);


				// flips buildings with low chance
				if(builder.GetType() == typeof(SceneRowBuilderRandomBuilding) && Random.value < Constants.Probability.ProbabilityLow)
				{
					// flips building 180 degrees
					instantiatedObject.transform.rotation *= Quaternion.Euler(0,0,180f);
					instantiatedObject.transform.position = new Vector3(instantiatedObject.transform.position.x + obj.sizeX, instantiatedObject.transform.position.y, instantiatedObject.transform.position.z + obj.sizeZ);
				}

				instantiatedObjects.Add(instantiatedObject);

				// jumps pointer to next x value
				pointer.x += obj.sizeX;
			}

			// Resets pointer
			pointer.x = gameManager.GameLeftBoundary;
			pointer.z += totalSizeZ;

		}
	}

	/*
	 * Instantates a scene object (a sidewalk, a road, a grass block...)
	 */
	GameObject InstantiateSceneObject(GameObject prefab)
	{
		GameObject dummy = Instantiate (prefab) as GameObject;
		dummy.name = prefab.name;
		dummy.transform.position = pointer;
		dummy.transform.parent = GameObject.Find (gameManager.sceneSettings.sceneObjectsNodeName).transform;		// Change new object parent
		return dummy;
	}

	/*
	 * Scan for every object in the scene. Those which are outside player bounds should become inactive
	 */
	void RecycleSceneObjects()
	{
		foreach(Transform obj in scene)
		{
			if((int)obj.gameObject.transform.position.z < (int)player.transform.position.z - (int)gameManager.sceneSettings.generationOffsetZ)
			{
				obj.gameObject.SetActive(false);
			}
				
		}
	}

	/*
	 * Generates a road speed
	 */
	Utils.VehicleSpeed GenerateRoadSpeed()
	{
		int random = Random.Range(0, 2);
		switch(random)
		{
			case 0:
				return Utils.VehicleSpeed.Going40;
			case 1:
				return Utils.VehicleSpeed.Going50;
			default:
				return Utils.VehicleSpeed.Going20;
		}
	}

	/*
	 * toggles the ui road speed
	 */
	void ToggleUI(bool state)
	{
		imageUI.enabled = state;
		speedLimitUI.enabled = state;
	}
	

}
