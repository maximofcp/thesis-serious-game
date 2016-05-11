using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

	private ArcadeGameManager gameManager;

	void Start()
	{
		gameManager = ArcadeGameManager.instance;
	}

	void OnTriggerEnter (Collider col)
	{
		// simulates click on a car
		if(col.gameObject.CompareTag(Constants.Tag.TagVehicle))
		{
			gameObject.SetActive(false);

			VehicleController vehicle = col.gameObject.GetComponent<VehicleController>();

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
