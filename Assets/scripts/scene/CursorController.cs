using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

	private GameManager gameManager;

	void Start()
	{
		gameManager = GameManager.instance;
	}

	void OnTriggerEnter (Collider col)
	{
		// simulates click on a car
		if(col.gameObject.CompareTag(Constants.TagVehicle))
		{
			gameObject.SetActive(false);

			VehicleAI vehicle = col.gameObject.GetComponent<VehicleAI>();

			bool isVehicleOnCurrentRoad = gameManager.speedLimitController.IsVehicleOnCurrentRoad(col.gameObject.transform.position.z);
			if(isVehicleOnCurrentRoad)
			{
				if(vehicle.GetCurrentSpeed() > (float)gameManager.speedLimitController.currentRoadSpeed)
				{
					vehicle.SetCurrentSpeed(gameManager.speedLimitController.currentRoadSpeed);
					gameManager.scoreController.AddScoreVehicleHit();
					//Debug.Log("WIN POINTS because vehicle speed = " + vehicle.GetCurrentSpeed() + " and road is " + 0 + gameManager.speedLimitController.currentRoadSpeed);
				}
				else{
					gameManager.scoreController.SubScoreVehicleHit();
					//Debug.Log("LOSE POINTS because vehicle speed = " + vehicle.GetCurrentSpeed() + " and road is " + 0 + gameManager.speedLimitController.currentRoadSpeed);
				}
			}

		}
	}

}
