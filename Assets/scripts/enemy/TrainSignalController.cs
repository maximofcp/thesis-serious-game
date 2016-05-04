using UnityEngine;
using System.Collections;

public class TrainSignalController : MonoBehaviour {

	private bool canCross;
	public GameObject redLight;
	public GameObject greenLight;
	public GameObject pfTrain;

	private GameObject train;
	private Vector3 trainDestiny;
	private GameManager settings;

	private int blinkCounter;

	void Start()
	{
		blinkCounter = 0;
		settings = GameManager.instance;
		train = (GameObject)Instantiate(pfTrain);
		train.SetActive(false);
		redLight.SetActive(false);
		greenLight.SetActive(true);

		InvokeRepeating("SpawnTrain", 0, settings.trainSpawnTime);
	}

	void Update()
	{
		if(train.transform.position == trainDestiny)
		{
			//BlinkGreenSign();
			train.SetActive(false);
			SetCanCross(true);
		}
		else
		{
			// make the train move
			train.transform.position = Vector3.MoveTowards (train.transform.position, trainDestiny, Time.deltaTime * settings.trainDefaultSpeed);
		}
	}


	/*
	 * When the railroad passage is disabled by the game builder, disables current on going train and this script also
	 */ 
	void OnDisable() {
		Destroy (train);
		Destroy(this);
	}

	/*
	 * Resets train to start position
	 */
	void ResetTrain()
	{
		train.SetActive(true);
		float startX = settings.playerStartPosition.x + settings.playerWindowSizeX + settings.generationOffsetX + 20;
		float endX = settings.playerStartPosition.x - settings.playerWindowSizeX - settings.generationOffsetX - 20;
		train.transform.position = new Vector3(startX, transform.position.y + 1, transform.position.z + 12);
		trainDestiny = new Vector3(endX, transform.position.y + 1, transform.position.z + 12);
	}


	/*
	 * Spawns the train
	 */
	void SpawnTrain()
	{
		if(!train.activeInHierarchy && Random.value < Constants.ProbabilityLow)
		{
			ResetTrain();
			SetCanCross(false);
		}
	}


	/*
	 * Sets the cross lighting green (can cross) or red (can't cross)
	 */
	public void SetCanCross(bool cross)
	{
		canCross = cross;
		redLight.SetActive(!canCross);
		greenLight.SetActive(canCross);
	}

	private void BlinkGreenSignOn()
	{

	}

	private void BlinkGreenSignOff()
	{
		if(blinkCounter++ < 3)
		{
			greenLight.SetActive(!greenLight.activeInHierarchy);
			Invoke("BlinkGreenSignOn", 1f);
		}
		else{
			blinkCounter = 0;
			train.SetActive(false);
			SetCanCross(true);
		}

	}
}
