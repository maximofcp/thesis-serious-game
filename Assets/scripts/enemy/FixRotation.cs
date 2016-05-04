using UnityEngine;
using System.Collections;

public class FixRotation : MonoBehaviour {

	Quaternion rotation;

	void Awake()
	{
		rotation = GetComponent<RectTransform>().rotation;
	}
	void LateUpdate()
	{
		transform.rotation = rotation;
	}
}
