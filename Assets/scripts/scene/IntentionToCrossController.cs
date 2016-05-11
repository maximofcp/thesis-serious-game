using UnityEngine;
using System.Collections;


public class IntentionToCrossController : MonoBehaviour {

	private bool intention;
	private bool intentionEnterA, intentionEnterB, crossing;
	private CrossState state;

	public enum CrossState
	{
		NoIntention,
		IntentionOnA,
		IntentionOnB,
		Crossing
	}

	void Start()
	{
		state = CrossState.NoIntention;
	}

	public bool HasIntention()
	{
		return intention;
	}

	public void RequestIntention(string tag)
	{
		switch (tag)
		{
			case Constants.Tag.TagCrosswalk:
				intention = true;
				state = CrossState.Crossing;
				break;
			case Constants.Tag.TagIntentionToCrossA:
				intention = true;
				state = CrossState.IntentionOnA;
				break;
			case Constants.Tag.TagIntentionToCrossB:
				intention = true;
				state = CrossState.IntentionOnB;
				break;
		}
	}

	public void RemoveIntention(string tag)
	{
		switch (tag)
		{
			case Constants.Tag.TagCrosswalk:
				intention = false;
				state = CrossState.Crossing;
				break;
			case Constants.Tag.TagIntentionToCrossA:
				intention = false;
				state = CrossState.IntentionOnA;
				break;
			case Constants.Tag.TagIntentionToCrossB:
				intention = false;
				state = CrossState.IntentionOnB;
				break;
		}
	}

	/*
	void OnTriggerEnter(Collider col)
	{
		if(col.CompareTag(Constants.Tag.TagCrosswalk)){
			crossing = true;
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		if(col.CompareTag(Constants.Tag.TagCrosswalk)){
			crossing = false;
		}
	}
	*/
}
