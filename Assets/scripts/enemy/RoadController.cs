using UnityEngine;
using System.Collections.Generic;

public enum Lane
{
	NormalRightDirection = 0,
	NormalLeftDirection = 1,
	AvenueRightDirectionR = 2,
	AvenueRightDirectionL = 3,
	AvenueLeftDirectionR = 4,
	AvenueLeftDirectionL = 5,

}

public class LaneDirection
{
	public Vector3 startPosition;
	public VehicleDirection direction;

	public LaneDirection(Vector3 pos, VehicleDirection dir)
	{
		direction = dir;
		startPosition = pos;
	}
}

public class RoadController : MonoBehaviour {

	public bool avenue;

	private GameManager settings;
	public List<GameObject> normalVehicles;
	public List<GameObject> emergencyVehicles;
	private List<GameObject> vehiclesOnLanes;


	// Use this for initialization
	void Start () {
		settings = GameManager.instance;
		vehiclesOnLanes = new List<GameObject>();
		InvokeRepeating("SpawnEnemy", 0, settings.vehicleSpawnFrequency);
	}



	/*
	 * When this road line is disabled by the game builder, disables current on going vehicles
	 */ 
	void OnDisable() {
		foreach(GameObject vehicle in vehiclesOnLanes)
			Destroy(vehicle);

		Destroy(this);
	}



	/*
	 * Spawns a random enemy
	 */
	void SpawnEnemy()
	{

		if(normalVehicles.Count > 0 && emergencyVehicles.Count > 0)
		{
			GameObject vehicle = normalVehicles[0];
			LaneDirection laneDirection = null;

			// diferentiate between avenue or normal road
			if(avenue)
			{
				Lane lane = (Lane)Random.Range(2, 6);
				laneDirection = GetVehicleStartPointOnLane(lane);
			}
			else
			{
				Lane lane = (Lane)Random.Range(0, 2);
				laneDirection = GetVehicleStartPointOnLane(lane);
			}
			
			// generates emergency vehicle
			if(Random.value < Constants.ProbabilityLow)
			{
				vehicle = emergencyVehicles[Random.Range(0, emergencyVehicles.Count)];
			}
			else // generates normal vehicle
			{
				vehicle = normalVehicles[Random.Range(0, normalVehicles.Count)];
			}
			
			
			// instantiates enemy at position
			GameObject enemy = InstantiateSceneObject(vehicle, laneDirection.startPosition);
			
			// saves new enemy on the list
			vehiclesOnLanes.Add(enemy);
			
			// sets the direction and orders to go
			VehicleAI model = enemy.GetComponent<VehicleAI>();
			model.SetDirection(laneDirection.direction);
			model.Go();

		}
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



	/*
	 * Given a lane, returns the start position of the vehicle on the chosen lane
	 */
	LaneDirection GetVehicleStartPointOnLane(Lane lane)
	{
		Vector3 lanePosition = new Vector3(0, settings.vehicleDefaultY, 0);
		VehicleDirection direction = VehicleDirection.Left;

		switch(lane)
		{
			case Lane.NormalLeftDirection:
				lanePosition.x = settings.GameRightBoundary;
				lanePosition.z = transform.position.z + Constants.LaneNormalLeftDirectionZ;
				direction = VehicleDirection.Left;
				break;

			case Lane.NormalRightDirection:
				lanePosition.x = settings.GameLeftBoundary;
				lanePosition.z = transform.position.z + Constants.LaneNormalRightDirectionZ;
				direction = VehicleDirection.Right;
				break;

			case Lane.AvenueLeftDirectionL:	
				lanePosition.x = settings.GameRightBoundary;
				lanePosition.z = transform.position.z + Constants.LaneAvenueLeftDirectionLZ;
				direction = VehicleDirection.Left;
				break;

			case Lane.AvenueLeftDirectionR:
				lanePosition.x = settings.GameRightBoundary;
				lanePosition.z = transform.position.z + Constants.LaneAvenueLeftDirectionRZ;
				direction = VehicleDirection.Left;
				break;

			case Lane.AvenueRightDirectionL:
				lanePosition.x = settings.GameLeftBoundary;
				lanePosition.z = transform.position.z + Constants.LaneAvenueRightDirectionLZ;
				direction = VehicleDirection.Right;
				break;

			case Lane.AvenueRightDirectionR:
				lanePosition.x = settings.GameLeftBoundary;
				lanePosition.z = transform.position.z + Constants.LaneAvenueRightDirectionRZ;
				direction = VehicleDirection.Right;
				break;
		}

		return new LaneDirection(lanePosition, direction);
	}
	
}
