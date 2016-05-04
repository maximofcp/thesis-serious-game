using UnityEngine;
using System.Collections;

public enum TrafficLightState
{
	None, Red, Yellow, Green
}

public class TrafficLightsController : MonoBehaviour {

	public GameObject pedestrianGreenLightL;
	public GameObject pedestrianGreenLightR;
	public GameObject pedestrianRedLightL;
	public GameObject pedestrianRedLightR;
	public GameObject vehicleRedLightL;
	public GameObject vehicleRedLightR;
	public GameObject vehicleYellowLightL;
	public GameObject vehicleYellowLightR;
	public GameObject vehicleGreenLightL;
	public GameObject vehicleGreenLightR;

	public TrafficLightState trafficRight;
	public TrafficLightState trafficLeft;
	private GameManager settings;
	
	void Start () {
		settings = GameManager.instance;
		ResetLights();
		InvokeRepeating("ChangeTrafficRight", Random.Range(0, settings.trafficLightsWaitTime), settings.trafficLightsWaitTime / 2);
		InvokeRepeating("ChangeTrafficLeft", Random.Range(0, settings.trafficLightsWaitTime), settings.trafficLightsWaitTime / 2);
	}

	void ChangeTrafficRight()
	{
		if(trafficRight == TrafficLightState.Green && Random.value < Constants.ProbabilityMedium)
			DisableTrafficR();
	}

	void ChangeTrafficLeft()
	{
		if(trafficLeft == TrafficLightState.Green && Random.value < Constants.ProbabilityMedium)
			DisableTrafficL();
	}

	/*
	 * Enable traffic on right lane
	 */
	void EnableTrafficR()
	{
		if(isActiveAndEnabled)
		{
			StartCoroutine(TogglePedestrianLightsR(TrafficLightState.Red, settings.trafficLightsInterval));
			StartCoroutine(ToggleTrafficLightsR(TrafficLightState.Green, 2 * settings.trafficLightsInterval));
		}
	}

	/*
	 * Disable traffic on right lane
	 */	
	void DisableTrafficR()
	{
		if(isActiveAndEnabled)
		{
			StartCoroutine(ToggleTrafficLightsR(TrafficLightState.Yellow, settings.trafficLightsInterval));
			StartCoroutine(ToggleTrafficLightsR(TrafficLightState.Red, 2 * settings.trafficLightsInterval));
			StartCoroutine(TogglePedestrianLightsR(TrafficLightState.Green, 3 * settings.trafficLightsInterval));
			Invoke("EnableTrafficR", 5 * settings.trafficLightsInterval + settings.trafficLightsWaitTime);
		}
	}

	/*
	 * Enable traffic on left lane
	 */	
	void EnableTrafficL()
	{
		if(isActiveAndEnabled)
		{
			StartCoroutine(TogglePedestrianLightsL(TrafficLightState.Red, settings.trafficLightsInterval));
			StartCoroutine(ToggleTrafficLightsL(TrafficLightState.Green, 2 * settings.trafficLightsInterval));
		}
	}

	/*
	 * Disable traffic on left lane
	 */	
	void DisableTrafficL()
	{
		if(isActiveAndEnabled)
		{
			StartCoroutine(ToggleTrafficLightsL(TrafficLightState.Yellow, settings.trafficLightsInterval));
			StartCoroutine(ToggleTrafficLightsL(TrafficLightState.Red, 2 * settings.trafficLightsInterval));
			StartCoroutine(TogglePedestrianLightsL(TrafficLightState.Green, 3 * settings.trafficLightsInterval));
			Invoke("EnableTrafficL", 5 * settings.trafficLightsInterval + settings.trafficLightsWaitTime);
		}
	}

	/*
	 * Resets all traffic lights to default
	 */	
	void ResetLights()
	{
		vehicleRedLightR.SetActive(false);
		vehicleYellowLightR.SetActive(false);
		vehicleGreenLightR.SetActive(true);
		pedestrianGreenLightR.SetActive(false);
		pedestrianRedLightR.SetActive(true);

		vehicleRedLightL.SetActive(false);
		vehicleYellowLightL.SetActive(false);
		vehicleGreenLightL.SetActive(true);
		pedestrianGreenLightL.SetActive(false);
		pedestrianRedLightL.SetActive(true);

		trafficRight = TrafficLightState.Green;
		trafficLeft = TrafficLightState.Green;
	}
	
	/*
	 * Toggles traffic light right side
	 */
	IEnumerator ToggleTrafficLightsR(TrafficLightState state, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);

		trafficRight = state;

		switch(state)
		{
			case TrafficLightState.Red:
				vehicleRedLightR.SetActive(true);
				vehicleYellowLightR.SetActive(false);
				vehicleGreenLightR.SetActive(false);
				break;
				
			case TrafficLightState.Yellow:
				vehicleRedLightR.SetActive(false);
				vehicleYellowLightR.SetActive(true);
				vehicleGreenLightR.SetActive(false);
				break;
				
			case TrafficLightState.Green:
				vehicleRedLightR.SetActive(false);
				vehicleYellowLightR.SetActive(false);
				vehicleGreenLightR.SetActive(true);
				break;
				
		}
		
	}
	
	/*
	 * Toggles pedestrian light right side
	 */
	IEnumerator TogglePedestrianLightsR(TrafficLightState state, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		
		switch(state)
		{
			case TrafficLightState.Red:
				pedestrianGreenLightR.SetActive(false);
				pedestrianRedLightR.SetActive(true);
				break;
				
			case TrafficLightState.Green:
				pedestrianGreenLightR.SetActive(true);
				pedestrianRedLightR.SetActive(false);
				break;
				
		}
		
	}

	
	/*
	 * Toggles traffic light left side
	 */
	IEnumerator ToggleTrafficLightsL(TrafficLightState state, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);

		trafficLeft = state;

		switch(state)
		{
			case TrafficLightState.Red:
				vehicleRedLightL.SetActive(true);
				vehicleYellowLightL.SetActive(false);
				vehicleGreenLightL.SetActive(false);
				break;
				
			case TrafficLightState.Yellow:
				vehicleRedLightL.SetActive(false);
				vehicleYellowLightL.SetActive(true);
				vehicleGreenLightL.SetActive(false);
				break;
				
			case TrafficLightState.Green:
				vehicleRedLightL.SetActive(false);
				vehicleYellowLightL.SetActive(false);
				vehicleGreenLightL.SetActive(true);
				break;
		}
		
	}
	
	/*
	 * Toggles pedestrian light right side
	 */
	IEnumerator TogglePedestrianLightsL(TrafficLightState state, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		
		switch(state)
		{
			case TrafficLightState.Red:
				pedestrianGreenLightL.SetActive(false);
				pedestrianRedLightL.SetActive(true);
				break;
				
			case TrafficLightState.Green:
				pedestrianGreenLightL.SetActive(true);
				pedestrianRedLightL.SetActive(false);
				break;
			
		}
		
	}

}
