using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


/*
 * Interface for a generic scene row
 */
public interface ISceneBlockBuilder
{
	List<SceneObject> GenerateSceneRow(float startX, float endX, float leftBound, float rightBound);
}

/*
 * Represents a static object to be inserted on the scene
 */
public class SceneObject
{
	public GameObject prefab;
	public float sizeX;
	public float sizeZ;
	public float level;
	public bool instantiated;

	public SceneObject(GameObject obj, float sizeX, float sizeZ, float level)
	{
		prefab = obj;
		this.sizeX = sizeX;
		this.sizeZ = sizeZ;
		this.level = level;
		instantiated = false;
	}

	public SceneObject(GameObject obj, float sizeX, float sizeZ)
	{
		prefab = obj;
		this.sizeX = sizeX;
		this.sizeZ = sizeZ;
		level = 1;
		instantiated = false;
	}
}


/*
 * Builds a railroad row
 */
public class SceneRowBuilderRailroad : ISceneBlockBuilder
{
	private GameObject pfRailroad;
	private GameObject pfRailroadPassage;
	
	public SceneRowBuilderRailroad(GameObject prefab, GameObject prefabPassage)
	{
		pfRailroad = prefab;
		pfRailroadPassage = prefabPassage;
	}
	
	public List<SceneObject> GenerateSceneRow(float startX, float endX, float leftBound, float rightBound)
	{
		List<SceneObject> sceneObjects = new List<SceneObject>();
		GameObject prefabToUse = pfRailroad;
		float sizeXToUse = Constants.RailroadSizeX;
		int numberOfPassages = 0;
		int numberOfPassagesLimit = 1;
		
		while(startX < endX)
		{
			bool isPassage = Random.Range(0,2) == 0;

			if(isPassage && numberOfPassages < numberOfPassagesLimit && (startX >= leftBound && startX < rightBound))
			{
				numberOfPassages++;
				prefabToUse = pfRailroadPassage;
			}else
			{
				prefabToUse = pfRailroad;
			}
			
			sceneObjects.Add(new SceneObject(prefabToUse, sizeXToUse, Constants.RailroadSizeZ));
			startX += sizeXToUse;
		}
		
		return sceneObjects;
	}
}


/*
 * Builds a road row
 */
public class SceneRowBuilderRoad : ISceneBlockBuilder
{
	private GameObject pfRoad;
	private GameObject pfCrosswalk;
	private GameObject pfRoadOffset;
	
	public SceneRowBuilderRoad(GameObject prefab, GameObject offset, GameObject crosswalkPrefab)
	{
		pfRoad = prefab;
		pfRoadOffset = offset;
		pfCrosswalk = crosswalkPrefab;
	}
	
	public List<SceneObject> GenerateSceneRow(float startX, float endX, float leftBound, float rightBound)
	{
		List<SceneObject> sceneObjects = new List<SceneObject>();
		GameObject prefabToUse = pfRoad;
		float sizeXToUse = Constants.CrosswalkWithSidewalkSizeX;
		int numberOfCrosswalks = 0;

		while(startX < endX)
		{
			if(startX < leftBound || startX >= rightBound)
			{
				prefabToUse = pfRoadOffset;
				sizeXToUse = Constants.CrosswalkWithSidewalkSizeOffset;
			}
			else if(Random.value < Constants.ProbabilityMedium && numberOfCrosswalks < Constants.NumberOfCrosswalks)
			{
				prefabToUse = pfCrosswalk;
				sizeXToUse = Constants.CrosswalkWithSidewalkSizeX;
				numberOfCrosswalks++;
			}
			else
			{
				prefabToUse = pfRoad;
				sizeXToUse = Constants.CrosswalkWithSidewalkSizeX;
			}

			sceneObjects.Add(new SceneObject(prefabToUse, sizeXToUse, Constants.CrosswalkWithSidewalkSizeZ));
			startX += sizeXToUse;
		}

		return sceneObjects;
	}
}


/*
 * Builds a road avenue row
 */
public class SceneRowBuilderRoadAvenue : ISceneBlockBuilder
{
	private GameObject pfRoad;
	private GameObject pfCrosswalk;
	private GameObject pfRoadOffset;
	
	public SceneRowBuilderRoadAvenue(GameObject prefab, GameObject offset, GameObject crosswalkPrefab)
	{
		pfRoad = prefab;
		pfRoadOffset = offset;
		pfCrosswalk = crosswalkPrefab;
	}
	
	public List<SceneObject> GenerateSceneRow(float startX, float endX, float leftBound, float rightBound)
	{
		List<SceneObject> sceneObjects = new List<SceneObject>();
		GameObject prefabToUse = pfRoad;
		float sizeXToUse = Constants.RoadWithSidewalkAvenueSizeX;
		int numberOfCrosswalks = 0;
		
		while(startX < endX)
		{
			if(startX < leftBound || startX >= rightBound)
			{
				prefabToUse = pfRoadOffset;
				sizeXToUse = Constants.RoadWithSidewalkAvenueSizeOffset;
			}
			else if(Random.value < Constants.ProbabilityMedium && numberOfCrosswalks < Constants.NumberOfCrosswalks)
			{
				prefabToUse = pfCrosswalk;
				sizeXToUse = Constants.RoadWithSidewalkAvenueSizeX;
				numberOfCrosswalks++;
			}
			else
			{
				prefabToUse = pfRoad;
				sizeXToUse = Constants.RoadWithSidewalkAvenueSizeX;
			}
			
			sceneObjects.Add(new SceneObject(prefabToUse, sizeXToUse, Constants.RoadWithSidewalkAvenueSizeZ));
			startX += sizeXToUse;
		}
		
		return sceneObjects;
	}
}


/*
 * Builds a random building row
 */
public class SceneRowBuilderRandomBuilding : ISceneBlockBuilder
{
	private List<GameObject> pfBuildings;
	private GameObject pfOffset;
	
	public SceneRowBuilderRandomBuilding(List<GameObject> buildings, GameObject offset)
	{
		pfBuildings = buildings;
		pfOffset = offset;
	}
	
	public List<SceneObject> GenerateSceneRow(float startX, float endX, float leftBound, float rightBound)
	{
		List<SceneObject> sceneObjects = new List<SceneObject>();

		if(pfBuildings.Count > 0)
		{

			GameObject prefabToUse = pfBuildings[0];
			float sizeXToUse = Constants.BuildingSizeX;

			while(startX < endX)
			{
				if(startX < leftBound || startX >= rightBound)
				{
					prefabToUse = pfOffset;
					sizeXToUse = Constants.GrassSizeOffset;
				}
				else
				{
					int toUse = Random.Range(0, pfBuildings.Count);
					prefabToUse = pfBuildings[toUse];
					sizeXToUse = Constants.BuildingSizeX;
				}
				
				sceneObjects.Add(new SceneObject(prefabToUse, sizeXToUse, Constants.BuildingSizeZ));
				startX += sizeXToUse;
			}
		}
		
		return sceneObjects;
	}
}

// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 

/*
 * Game Builder class
 */
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
	private GameManager gameManager;
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
		gameManager = GameManager.instance;

		builderPool = new List<ISceneBlockBuilder>();
		scene = GameObject.Find (gameManager.sceneObjectsNodeName).transform;

		// sets pointer to initial state
		float pointerx = gameManager.GameLeftBoundary;
		float pointery = gameManager.defaultGenerationPositionY;
		float pointerz = gameManager.defaultGenerationPositionZ;
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

		ApplyPoolSceneRows();

		InvokeRepeating("RecycleSceneObjects", 0, 5);
	}

	void Update()
	{
		ApplyPoolSceneRows();

		CheckRoadSpeed();
	}

	void CheckRoadSpeed()
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
			speedLimitUI.text = VehicleAI.GetCurrentSpeedIndicator((int)current.speedlimit);

		}
		else if(current == null)
		{
			// Hide UI (outisde a road)
			ToggleUI(false);
		}
	}

	void ApplyPoolSceneRows()
	{
		// if pointer is inside game window
		if(pointer.z < player.transform.position.z + gameManager.generationOffsetZ && builderPool.Count > 0)
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
				Road road = new Road(pointer.z + Constants.CrosswalkWithSidewalkSizeZ, pointer.z, GenerateRoadSpeed() );
				gameManager.speedLimitController.AddRoad(road);
			}

			// register avenue
			if(builder.GetType() == typeof(SceneRowBuilderRoadAvenue))
			{
				Road road = new Road(pointer.z + Constants.RoadWithSidewalkAvenueSizeZ, pointer.z, GenerateRoadSpeed() );
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
				if(builder.GetType() == typeof(SceneRowBuilderRandomBuilding) && Random.value < Constants.ProbabilityLow)
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
		dummy.transform.parent = GameObject.Find (gameManager.sceneObjectsNodeName).transform;		// Change new object parent
		return dummy;
	}

	/*
	 * Scan for every object in the scene. Those which are outside player bounds should become inactive
	 */
	void RecycleSceneObjects()
	{
		foreach(Transform obj in scene)
		{
			if((int)obj.gameObject.transform.position.z < (int)player.transform.position.z - (int)gameManager.generationOffsetZ)
			{
				obj.gameObject.SetActive(false);
			}
				
		}
	}

	/*
	 * Generates a road speed
	 */
	VehicleSpeed GenerateRoadSpeed()
	{
		int random = Random.Range(0, 2);
		switch(random)
		{
			case 0:
				return VehicleSpeed.Going40;
			case 1:
				return VehicleSpeed.Going50;
			default:
				return VehicleSpeed.Going20;
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
