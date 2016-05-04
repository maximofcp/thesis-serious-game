using UnityEngine;
using System.Collections;


public class IntentionToCrossController : MonoBehaviour {

	public bool intention;

	public bool intentionEnterA, intentionEnterB, crossing;
	public CrossState state;

	public enum CrossState
	{
		NoIntention = 0,
		IntentionOnA = 1,
		IntentionOnB = 2,
		Crossing = 3
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.CompareTag(Constants.TagCrosswalk)){
			crossing = true;
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		if(col.CompareTag(Constants.TagCrosswalk)){
			crossing = false;
		}
	}
	
}
