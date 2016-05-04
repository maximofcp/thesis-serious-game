using UnityEngine;
using System.Collections.Generic;


public class StaticSceneGenerator : MonoBehaviour {

	// Prefabs
	public GameObject pfSidewalk;
	public GameObject pfGrass;
	public GameObject pfRoad;
	public GameObject pfCrosswalk;
	public GameObject pfVehicle;

	public Material mOffGameAreaGrass;
	public Material mOffGameAreaSidewalk;
	public Material mOffGameAreaRoad;

	public GameObject player;

	private SceneStaticObjectType previous;
	private Vector3 pointer;
	private GameManager settings;
	private Transform levelParent;

	private enum SceneStaticObjectType
	{
		Sidewalk, Grass, Road, Crosswalk
	}

	private enum SceneDynamicObjectType
	{
		NormalVehicle
	}
	
	#region Unity Methods
	void Start () {

		// Gets singleton
		settings = GameManager.instance;

		levelParent = GameObject.Find (settings.sceneObjectsNodeName).transform;

		// Sets pointer to initial state
		float pointerx = settings.GameLeftBoundary;
		float pointery = settings.defaultGenerationPositionY;
		float pointerz = settings.defaultGenerationPositionZ;
		pointer = new Vector3(pointerx, pointery, pointerz);

		/*
		GenerateStaticSceneObjectLine(SceneStaticObjectType.Sidewalk);
		GenerateStaticSceneObjectLine(SceneStaticObjectType.Road);
		GenerateStaticSceneObjectLine(SceneStaticObjectType.Sidewalk);
		*/
	}


	void Update()
	{
		GenerateStaticSceneObjects();
		DestroyStaticSceneObjects();
	}


	#endregion
	
	#region Methods

	/*
	 * Destroys all objects that are outside of the Z offset
	 */
	void DestroyStaticSceneObjects()
	{
		foreach(Transform t in levelParent)
		{
			if((int)t.gameObject.transform.position.z < (int)player.transform.position.z - (int)settings.generationOffsetZ)
				Destroy(t.gameObject);
		}
	}

	/*
	 * Generates a scene objects
	 */
	void GenerateStaticSceneObjects()
	{
		if(pointer.z < player.transform.position.z + settings.generationOffsetZ)
		{	
			int generate = Random.Range(0,3);

			switch(generate)
			{
				case 0:	// sidewalk
					GenerateStaticSceneObjectLine(SceneStaticObjectType.Sidewalk);
					break;

				case 1:	// road
					GenerateStaticSceneObjectLine(SceneStaticObjectType.Sidewalk);
					GenerateStaticSceneObjectLine(SceneStaticObjectType.Road);
					GenerateStaticSceneObjectLine(SceneStaticObjectType.Sidewalk);
					break;

				case 2:	// grass
					GenerateStaticSceneObjectLine(SceneStaticObjectType.Grass);
					break;
			}
		}
	}



	/*
	 * Generates a static scene object line (a sidewalk, a road, a grass block...)
	 */
	void GenerateStaticSceneObjectLine(SceneStaticObjectType type)
	{
		float capacity = settings.GameRightBoundary - Constants.PathSizeX;
		int crosswalkGenerationCapacity = settings.crosswalkDensity;

		while(pointer.x <= capacity)
		{
			// Spawn a crosswalk if the scene object type is road
			if(type == SceneStaticObjectType.Road || type == SceneStaticObjectType.Crosswalk)
			{
				if(crosswalkGenerationCapacity > 0 && Random.value < Constants.ProbabilityMedium && IsInPlayableArea(pointer))
				{
					type = SceneStaticObjectType.Crosswalk;
					crosswalkGenerationCapacity -= 1;
				}
				else
					type = SceneStaticObjectType.Road;
			}

			GenerateStaticSceneObject(type);									// Creates new scene object
		}

		// Resets pointer
		pointer.x = settings.GameLeftBoundary;
		pointer.z += GetSceneObjectZ(type);
	}



	/*
	 * Generates a static scene object (a sidewalk, a road, a grass block...)
	 */
	GameObject GenerateStaticSceneObject(SceneStaticObjectType type)
	{
		GameObject prefab = GetGameObjectByType(type);
		GameObject dummy = Instantiate (prefab) as GameObject;
		dummy.name = prefab.name;
		dummy.transform.position = pointer;
		dummy.transform.parent = GameObject.Find (settings.sceneObjectsNodeName).transform;		// Change new object parent

		// changes object material to differentiate game area from off zone
		if(!IsInPlayableArea(pointer))
			dummy.GetComponentInChildren<Renderer>().material = GetOffGameAreaMaterial(type);

		pointer.x += GetSceneObjectX(type);														// Jumps pointer to next element
		return dummy;
	}



	/*
	 * Generates a dynamic scene object (a sidewalk, a road, a grass block...)
	 */
	GameObject GenerateDynamicSceneObject(SceneStaticObjectType type)
	{
		GameObject prefab = GetGameObjectByType(type);
		GameObject dummy = Instantiate (prefab) as GameObject;
		dummy.name = prefab.name;
		dummy.transform.position = pointer;
		dummy.transform.parent = GameObject.Find (settings.sceneObjectsNodeName).transform;		// Change new object parent
		
		// changes object material to differentiate game area from off zone
		if(!IsInPlayableArea(pointer))
			dummy.GetComponentInChildren<Renderer>().material = GetOffGameAreaMaterial(type);
		
		pointer.x += GetSceneObjectX(type);														// Jumps pointer to next element
		return dummy;
	}
	#endregion

	#region Access methods

	Material GetOffGameAreaMaterial(SceneStaticObjectType type)
	{
		switch(type)
		{
			case SceneStaticObjectType.Sidewalk:
			return mOffGameAreaSidewalk;
				
			case SceneStaticObjectType.Grass:
				return mOffGameAreaGrass;
			default:
				return mOffGameAreaSidewalk;
		}
	}

	bool IsInPlayableArea(Vector3 position)
	{
		return position.x > settings.GameLeftPlayableBoundary - Constants.PathSizeX && position.x < settings.GameRightPlayableBoundary;
	}

	float GetSceneObjectZ(SceneStaticObjectType type)
	{
		switch(type)
		{
			case SceneStaticObjectType.Crosswalk:
				return Constants.CrosswalkSizeZ;
				
			case SceneStaticObjectType.Road:
				return Constants.RoadSizeZ;
				
			case SceneStaticObjectType.Sidewalk:
				return Constants.PathSizeZ;
				
			case SceneStaticObjectType.Grass:
				return Constants.PathSizeZ;
			default:
				return 0;
		}
	}

	float GetSceneObjectX(SceneStaticObjectType type)
	{
		switch(type)
		{
			case SceneStaticObjectType.Crosswalk:
				return Constants.CrosswalkSizeX;
				
			case SceneStaticObjectType.Road:
				return Constants.RoadSizeX;
				
			case SceneStaticObjectType.Sidewalk:
				return Constants.PathSizeX;
				
			case SceneStaticObjectType.Grass:
				return Constants.PathSizeX;
			default:
				return 0;
		}
	}

	/*
	 * Translates a scene object type into a game object prefab
	 */
	GameObject GetGameObjectByType(SceneStaticObjectType type)
	{
		switch(type)
		{
			case SceneStaticObjectType.Crosswalk:
				return pfCrosswalk;
				
			case SceneStaticObjectType.Road:
				return pfRoad;
				
			case SceneStaticObjectType.Sidewalk:
				return pfSidewalk;
				
			case SceneStaticObjectType.Grass:
				return pfGrass;
			default:
				return null;
		}
	}
	#endregion

}
