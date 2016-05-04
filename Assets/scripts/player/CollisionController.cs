using UnityEngine;
using System.Collections;

public class CollisionController : MonoBehaviour
{
	public GameObject cursor;
	private GameManager gameManager;

	void Start()
	{
		gameManager = GameManager.instance;
	}

	void OnTriggerEnter(Collider col)
	{
		switch (col.gameObject.tag) {
			case Constants.TagIntentionToCrossA:
			case Constants.TagIntentionToCrossB:
				//Debug.Log("QUer atravessar");
				IntentionToCrossController intentionController = col.transform.GetComponentInParent<IntentionToCrossController>();
				intentionController.intention = true;
				break;
			case Constants.TagVehicle:
				gameManager.DisplayGameOver();
				break;
			case Constants.TagObstacle:
				break;
		}
	}

	void OnTriggerStay(Collider col)
	{
		switch (col.gameObject.tag) {
			case Constants.TagBuilding:
			cursor.SetActive(false);
			break;
		}
	}

	void OnTriggerExit(Collider col)
	{
		switch (col.gameObject.tag) {
			case Constants.TagIntentionToCrossA:
			case Constants.TagIntentionToCrossB:
				//Debug.Log("Bazou palhaço");
				IntentionToCrossController intentionController = col.transform.GetComponentInParent<IntentionToCrossController>();
				intentionController.intention = false;
				break;
		}
	}
	
}
