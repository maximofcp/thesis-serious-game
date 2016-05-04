using UnityEngine;
using System.Collections.Generic;

public class EnemyBuilder : MonoBehaviour {

	// prefabs
	public List<GameObject> pfVehicles;
	public GameObject pfPoliceCar;
	public GameObject pfAmbulance;


	private GameManager settings;
	private List<float> roads;
	
	void Start () {
		settings = GameManager.instance;
		roads = new List<float>();
		InvokeRepeating("Process", 0, 5);
	}

	/*
	 * Processes all the registered roads and generates enemys on them
	 */
	void Process()
	{
		foreach(float roadZ in roads)
		{
			SpawnEnemy(roadZ);
		}
	}

	/*
	 * Spawns a random enemy
	 */
	void SpawnEnemy(float roadZ)
	{
		//TODO: Generate random enemy

		// generates random side position
		float enemyX = 0;
		float enemyZ = 0;


		// chooses what enemy to spawn
		int enemyType = Random.Range(0, 3);
		GameObject vehicle = pfVehicles[0];
		if(enemyType == 0)
		{
			vehicle = pfPoliceCar;
		}
		if(enemyType == 1)
		{
			vehicle = pfAmbulance;	
		}
		if(enemyType == 2)
		{
			vehicle = pfVehicles[Random.Range(0,pfVehicles.Count)];	
		}

		// defines a direction (left or right) and sets the lane position	
		VehicleDirection direction = (VehicleDirection)Random.Range(0,2);
		switch(direction)
		{
			case VehicleDirection.Left:
				enemyX = settings.GameRightBoundary;
				enemyZ = roadZ + (2*Constants.PathSizeZ + 1) + Constants.PathSizeZ/2;
				break;
			case VehicleDirection.Right:
				enemyX = settings.GameLeftBoundary;
				enemyZ = roadZ + Constants.PathSizeZ + Constants.PathSizeZ/2;
				break;
		}
		Vector3 enemyPosition = new Vector3(enemyX, 2, enemyZ);

		// instantiates enemy at position
		GameObject enemy = InstantiateSceneObject(vehicle, enemyPosition);
		VehicleAI model = enemy.GetComponent<VehicleAI>();
		model.SetDirection(direction);
		model.Go();
	}

	/*
	 * Registers a new road on the system
	 */
	public void RegisterRoad(float roadZ)
	{
		if(roads != null && !roads.Contains(roadZ))
			roads.Add(roadZ);
	}

	/*
	 * Instantates a scene object
	 */
	GameObject InstantiateSceneObject(GameObject prefab, Vector3 position)
	{
		GameObject dummy = Instantiate (prefab) as GameObject;
		dummy.name = prefab.name;
		dummy.transform.position = position;
		dummy.transform.parent = GameObject.Find (settings.sceneObjectsNodeName).transform;		// Change new object parent
		return dummy;
	}

}
