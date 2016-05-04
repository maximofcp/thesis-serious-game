using UnityEngine;
using System.Collections;

public class CameraTracking : MonoBehaviour
{
	public GameObject objectToTrack;
	private Vector3 cameraPosition;
	private GameManager settings;

	void Start ()
	{
		cameraPosition = transform.position;
		settings = GameManager.instance;
	}

	void Update ()
	{
		transform.position = Vector3.MoveTowards (transform.position, new Vector3 (objectToTrack.transform.position.x, cameraPosition.y + 15, objectToTrack.transform.position.z), settings.cameraSpeed * Time.deltaTime);
	}
}
