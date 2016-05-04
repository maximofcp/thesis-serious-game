using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VehicleAI : MonoBehaviour {


	public VehicleDirection direction;
	public Vector3 startPoint;
	public bool go;

	private float currentSpeed;
	private VehicleSpeed maxSpeed;
	private float brake;
	private GameManager settings;
	private Vector3 target;
	private Text guiText;
	private TrafficLightState trafficLightState = TrafficLightState.None;

	void Awake () {
		settings = GameManager.instance;
		brake = settings.defaultVehicleBrakeForce;
	}

	void Start()
	{

		int chosenSpeedIndex = Random.Range(0, 5);
		switch(chosenSpeedIndex)
		{
			case 0:
				maxSpeed = VehicleSpeed.Going20;
				break;

			case 1:
				maxSpeed = VehicleSpeed.Going40;
				break;

			case 2:
				maxSpeed = VehicleSpeed.Going50;
				break;

			case 3:
				maxSpeed = VehicleSpeed.Going70;
				break;

			case 4:
				maxSpeed = VehicleSpeed.Going90;
				break;
		}

		guiText = (Text)GetComponentInChildren<Text>();
		
	}

	void Update () {

		// outside bounds
		if((direction == VehicleDirection.Left && transform.position.x < settings.GameLeftBoundary) || (direction == VehicleDirection.Right && transform.position.x > settings.GameRightBoundary))
		{
			gameObject.SetActive(false);
		}
		

		// decides if vehicle should accelerate or stop
		if(go)
			currentSpeed = Mathf.Lerp(currentSpeed, (float)maxSpeed, brake);
		else 
			currentSpeed = Mathf.Lerp(currentSpeed, 0, brake);
		

		if(!go && trafficLightState == TrafficLightState.Green)
			go = true;

		// to stop more quickly
		if(!go && currentSpeed < 1)
			currentSpeed = 0.0f;


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
			case Constants.TagVehicleStopZoneAvenueRight:
				TrafficLightsController trafficControllerR = col.transform.GetComponentInParent<TrafficLightsController>();
				trafficLightState = trafficControllerR.trafficRight;
				break;

			case Constants.TagVehicleStopZoneAvenueLeft:
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
			case Constants.TagVehicleStopZone:
				IntentionToCrossController intentionController = col.transform.GetComponentInParent<IntentionToCrossController>();
				
				if(intentionController != null && intentionController.intention)
				{
					go = false;
					Invoke("Go", settings.vehicleCrosswalkWaitTime);
				}				
				break;

			case Constants.TagVehicleStopZoneAvenueRight:
				TrafficLightsController trafficControllerR = col.transform.GetComponentInParent<TrafficLightsController>();
				trafficLightState = trafficControllerR.trafficRight;
				if(trafficControllerR != null && (trafficControllerR.trafficRight == TrafficLightState.Red || trafficControllerR.trafficRight == TrafficLightState.Yellow))
					go = false;

				break;
			case Constants.TagVehicleStopZoneAvenueLeft:
				TrafficLightsController trafficControllerL = col.transform.GetComponentInParent<TrafficLightsController>();
				trafficLightState = trafficControllerL.trafficRight;
				if(trafficControllerL != null && (trafficControllerL.trafficRight == TrafficLightState.Red || trafficControllerL.trafficRight == TrafficLightState.Yellow))
					go = false;
			
				break;


			// if encounters a car, means that the car's going slower. Must slow down and match his pace.
			// if the car is stopping, must stop too.
			case Constants.TagVehicleWarningZone:
				VehicleAI carAhead = col.GetComponentInParent<VehicleAI>();

				// if this event was thrown by the car behind
				if((direction == VehicleDirection.Left && transform.position.x > carAhead.transform.position.x) || (direction == VehicleDirection.Right && transform.position.x < carAhead.transform.position.x))
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
			case Constants.TagVehicleWarningZone:
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
	public void SetDirection(VehicleDirection d)
	{
		direction = d;

		// define target point
		if(d == VehicleDirection.Left)
		{
			target = new Vector3(settings.GameLeftBoundary - 50, transform.position.y, transform.position.z);
		}
		else if(d == VehicleDirection.Right)
		{
			transform.rotation *= Quaternion.Euler(0,0,180f);
			target = new Vector3(settings.GameRightBoundary + 50, transform.position.y, transform.position.z);
		}
	}

	/*
	 * Returns current car speed
	 */
	public float GetCurrentSpeed()
	{
		int oldMin = Constants.VehicleMinSpeedModel;
		int oldMax = Constants.VehicleMaxSpeedModel;
		int newMin = Constants.VehicleMinSpeedView;
		int newMax = Constants.VehicleMaxSpeedView;
		int oldValue = (int)currentSpeed;
		float newValue = (((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
		return newValue;
	}

	/*
	 * Sets current car speed
	 */
	public void SetCurrentSpeed(VehicleSpeed speed)
	{
		float newMin = Constants.VehicleMinSpeedModel;
		float newMax = Constants.VehicleMaxSpeedModel;
		float oldMin = Constants.VehicleMinSpeedView;
		float oldMax = Constants.VehicleMaxSpeedView;
		float oldValue = (float)speed;
		float newValue = (((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;


		maxSpeed = (VehicleSpeed)newValue;
	}


	/*
	 * Translates between game speed and visual speed: with a given speed
	 */
	public static string GetCurrentSpeedIndicator (int speed)
	{
		int oldMin = Constants.VehicleMinSpeedModel;
		int oldMax = Constants.VehicleMaxSpeedModel;
		int newMin = Constants.VehicleMinSpeedView;
		int newMax = Constants.VehicleMaxSpeedView;
		int oldValue = speed;
		int newValue = (((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
		return newValue.ToString();
	}

	/*
	 * Translates between game speed and visual speed
	 */
	string GetCurrentSpeedIndicator ()
	{
		int oldMin = Constants.VehicleMinSpeedModel;
		int oldMax = Constants.VehicleMaxSpeedModel;
		int newMin = Constants.VehicleMinSpeedView;
		int newMax = Constants.VehicleMaxSpeedView;
		int oldValue = (int)currentSpeed;
		float newValue = (((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
		return newValue.ToString();
	}

}
