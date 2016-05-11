using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VehicleController : MonoBehaviour {

	private Utils.VehicleDirection direction;
	private Vector3 startPoint;
	private bool go;

	private float currentSpeed;
	private Utils.VehicleSpeed maxSpeed;
	private float brake;
	private ArcadeGameManager settings;
	private Vector3 target;
	private Text guiText;
	private TrafficLightState trafficLightState = TrafficLightState.None;

	void Awake () {
		settings = ArcadeGameManager.instance;
		brake = settings.vehicleSettings.defaultVehicleBrakeForce;
	}

	void Start()
	{
		int chosenSpeedIndex = Random.Range(0, 5);
		switch(chosenSpeedIndex)
		{
			case 0:
				maxSpeed = Utils.VehicleSpeed.Going20;
				break;

			case 1:
				maxSpeed = Utils.VehicleSpeed.Going40;
				break;

			case 2:
				maxSpeed = Utils.VehicleSpeed.Going50;
				break;

			case 3:
				maxSpeed = Utils.VehicleSpeed.Going70;
				break;

			case 4:
				maxSpeed = Utils.VehicleSpeed.Going90;
				break;
		}

		guiText = (Text)GetComponentInChildren<Text>();
		
	}

	void Update () {

		// outside bounds
		if((direction == Utils.VehicleDirection.Left && transform.position.x < settings.GameLeftBoundary) || (direction == Utils.VehicleDirection.Right && transform.position.x > settings.GameRightBoundary))
		{
			gameObject.SetActive(false);
		}
		

		// decides if vehicle should accelerate or stop
		if(go)
			currentSpeed = Mathf.Lerp(currentSpeed, (float)maxSpeed, brake);
		else 
			currentSpeed = Mathf.Lerp(currentSpeed, 0, brake);

		// to stop more quickly
		if(!go && currentSpeed < 1)
			currentSpeed = 0.0f;


		if(!go && trafficLightState == TrafficLightState.Green)
			go = true;
		

		// sets speed indicator above
		if(guiText != null)
			guiText.text = GetCurrentSpeedIndicator();


		// move
		transform.position = Vector3.MoveTowards (transform.position, target, Time.deltaTime * currentSpeed);

	}

	void OnTriggerStay(Collider col)
	{
		switch(col.gameObject.tag)
		{
			case Constants.Tag.TagVehicleStopZoneAvenueRight:
				TrafficLightsController trafficControllerR = col.transform.GetComponentInParent<TrafficLightsController>();
				trafficLightState = trafficControllerR.trafficRight;
				break;

			case Constants.Tag.TagVehicleStopZoneAvenueLeft:
				TrafficLightsController trafficControllerL = col.transform.GetComponentInParent<TrafficLightsController>();
				trafficLightState = trafficControllerL.trafficLeft;
				break;
		}
	}

	void OnTriggerEnter (Collider col)
	{
		switch(col.gameObject.tag)
		{
			// if encounters a crosswalk with the player about to cross, stop
			case Constants.Tag.TagVehicleStopZone:
				IntentionToCrossController intentionController = col.transform.GetComponentInParent<IntentionToCrossController>();
				
				if(intentionController != null && intentionController.HasIntention())
				{
					go = false;
					Invoke("Go", settings.vehicleSettings.vehicleCrosswalkWaitTime);
				}				
				break;

			case Constants.Tag.TagVehicleStopZoneAvenueRight:
				TrafficLightsController trafficControllerR = col.transform.GetComponentInParent<TrafficLightsController>();
				trafficLightState = trafficControllerR.trafficRight;
				if(trafficControllerR != null && (trafficControllerR.trafficRight == TrafficLightState.Red || trafficControllerR.trafficRight == TrafficLightState.Yellow))
					go = false;

				break;
			case Constants.Tag.TagVehicleStopZoneAvenueLeft:
				TrafficLightsController trafficControllerL = col.transform.GetComponentInParent<TrafficLightsController>();
				trafficLightState = trafficControllerL.trafficRight;
				if(trafficControllerL != null && (trafficControllerL.trafficRight == TrafficLightState.Red || trafficControllerL.trafficRight == TrafficLightState.Yellow))
					go = false;
			
				break;


			// if encounters a car, means that the car's going slower. Must slow down and match his pace.
			// if the car is stopping, must stop too.
			case Constants.Tag.TagVehicleWarningZone:
				VehicleController carAhead = col.GetComponentInParent<VehicleController>();

				// if this event was thrown by the car behind
				if((direction == Utils.VehicleDirection.Left && transform.position.x > carAhead.transform.position.x) || (direction == Utils.VehicleDirection.Right && transform.position.x < carAhead.transform.position.x))
				{
					brake += 0.03f;
					if(carAhead.go)
					{
						maxSpeed = carAhead.maxSpeed;

						if(carAhead.currentSpeed > currentSpeed)
							currentSpeed = carAhead.currentSpeed;
					}
					else
					{
						go = false;
					}

				}

				break;

		}
		
	}

	void OnTriggerExit(Collider col)
	{
		switch(col.gameObject.tag)
		{
			// if exited a car collided, means that this car can go
			case Constants.Tag.TagVehicleWarningZone:
				Invoke("Go", 1f);
				break;
		}

	}



	/*
	 * Orders the vehicle to go
	 */
	public void Go()
	{
		go = true;
	}


	/*
	 * Sets the direction of this vehicle
	 */
	public void SetDirection(Utils.VehicleDirection d)
	{
		direction = d;

		// define target point
		if(d == Utils.VehicleDirection.Left)
		{
			target = new Vector3(settings.GameLeftBoundary - 50, transform.position.y, transform.position.z);
		}
		else if(d == Utils.VehicleDirection.Right)
		{
			transform.rotation *= Quaternion.Euler(0,0,180f);		// must rotate 3d asset
			target = new Vector3(settings.GameRightBoundary + 50, transform.position.y, transform.position.z);
		}
	}

	/*
	 * Returns current car speed
	 */
	public float GetCurrentSpeed()
	{
		int oldMin = Constants.Vehicle.VehicleMinSpeedModel;
		int oldMax = Constants.Vehicle.VehicleMaxSpeedModel;
		int newMin = Constants.Vehicle.VehicleMinSpeedView;
		int newMax = Constants.Vehicle.VehicleMaxSpeedView;
		int oldValue = (int)currentSpeed;
		float newValue = (((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
		return newValue;
	}

	/*
	 * Sets current car speed
	 */
	public void SetCurrentSpeed(Utils.VehicleSpeed speed)
	{
		float newMin = Constants.Vehicle.VehicleMinSpeedModel;
		float newMax = Constants.Vehicle.VehicleMaxSpeedModel;
		float oldMin = Constants.Vehicle.VehicleMinSpeedView;
		float oldMax = Constants.Vehicle.VehicleMaxSpeedView;
		float oldValue = (float)speed;
		float newValue = (((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;


		maxSpeed = (Utils.VehicleSpeed)newValue;
	}


	/*
	 * Translates between game speed and visual speed: with a given speed
	 */
	public static string GetCurrentSpeedIndicator (int speed)
	{
		int oldMin = Constants.Vehicle.VehicleMinSpeedModel;
		int oldMax = Constants.Vehicle.VehicleMaxSpeedModel;
		int newMin = Constants.Vehicle.VehicleMinSpeedView;
		int newMax = Constants.Vehicle.VehicleMaxSpeedView;
		int oldValue = speed;
		int newValue = (((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
		return newValue.ToString();
	}

	/*
	 * Translates between game speed and visual speed
	 */
	string GetCurrentSpeedIndicator ()
	{
		int oldMin = Constants.Vehicle.VehicleMinSpeedModel;
		int oldMax = Constants.Vehicle.VehicleMaxSpeedModel;
		int newMin = Constants.Vehicle.VehicleMinSpeedView;
		int newMax = Constants.Vehicle.VehicleMaxSpeedView;
		int oldValue = (int)currentSpeed;
		float newValue = (((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
		return newValue.ToString();
	}

}
