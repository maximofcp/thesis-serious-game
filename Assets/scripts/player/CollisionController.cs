using UnityEngine;
using System.Collections;

public class CollisionController : MonoBehaviour
{
	public GameObject cursor;
	private ArcadeGameManager gameManager;

	void Start()
	{
		gameManager = ArcadeGameManager.instance;
	}

	void OnTriggerEnter(Collider col)
	{
		switch (col.gameObject.tag) {
			case Constants.Tag.TagCrosswalk:
			case Constants.Tag.TagIntentionToCrossA:
			case Constants.Tag.TagIntentionToCrossB:
				IntentionToCrossController intentionController = col.transform.GetComponentInParent<IntentionToCrossController>();
				intentionController.RequestIntention(col.gameObject.tag);
				break;
			case Constants.Tag.TagVehicle:
				gameManager.DisplayGameOver();
				break;
			case Constants.Tag.TagObstacle:
				break;
		}
	}

	void OnTriggerStay(Collider col)
	{
		switch (col.gameObject.tag) {
			case Constants.Tag.TagBuilding:
				cursor.SetActive(false);
				break;
		}
	}

	void OnTriggerExit(Collider col)
	{
		switch (col.gameObject.tag) {
			case Constants.Tag.TagCrosswalk:
			case Constants.Tag.TagIntentionToCrossA:
			case Constants.Tag.TagIntentionToCrossB:
				//Debug.Log("Bazou palhaço");
				IntentionToCrossController intentionController = col.transform.GetComponentInParent<IntentionToCrossController>();
				intentionController.RemoveIntention(col.gameObject.tag);
				break;
		}
	}
	
}
