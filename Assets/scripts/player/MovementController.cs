using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{
	private Animator anim;
	private Vector3 target;
	private GameManager settings;
	private float previousZ;
	public GameObject cursor;

	void Start ()
	{
		target = transform.position;
		previousZ = transform.position.z;
		settings = GameManager.instance;
		anim = GetComponent<Animator>();
		anim.speed = 0.5f;
	}
	
	void Update ()
	{
		// avoid the player to go back
		if(transform.position.z > previousZ)
		{
			previousZ = transform.position.z;
			if(transform.position.z - settings.playerWindowSizeZ > settings.playerStartPosition.z)
				settings.playerBoundaryZ = transform.position.z - settings.playerWindowSizeZ;
		}

		// handle user inputs and movement
		if (Input.GetMouseButtonDown (0)) 
		{
			cursor.SetActive(true);
			Plane playerPlane = new Plane (Vector3.up, transform.position);
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			float hitdist = 0;
			
			if (playerPlane.Raycast (ray, out hitdist)) {
				var targetPoint = ray.GetPoint (hitdist);
				target = ray.GetPoint (hitdist);
				var targetRotation = Quaternion.LookRotation (targetPoint - transform.position);
				transform.rotation = targetRotation;
			}
		}

		// Move arrow
		cursor.transform.position = target;
		if(transform.position == cursor.transform.position)
			cursor.SetActive(false);

		// point which player is trying to move
		Vector3 moveTo = Vector3.MoveTowards (transform.position, target, Time.deltaTime * settings.playerSpeed);

		// gets game bound box
		float leftBound = settings.playerStartPosition.x - settings.playerWindowSizeX;
		float rightBound = settings.playerStartPosition.x + settings.playerWindowSizeX;

		// checks if player can move to point
		if (moveTo.x < rightBound && moveTo.x > leftBound && moveTo.z > settings.playerBoundaryZ && cursor.activeInHierarchy)
		{
			anim.Play(Constants.AnimPlayerWalking);
			transform.position = moveTo;
		}
		else
		{
			anim.Play(Constants.AnimPlayerIdle);
		}

	}

}
