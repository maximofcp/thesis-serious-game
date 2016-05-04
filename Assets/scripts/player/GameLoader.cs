using UnityEngine;
using System.Collections;

/*
 * This class loads all singletons
 */
public class GameLoader : MonoBehaviour {

	public GameObject settings;
	//public GameObject gameManager;

	void Awake()
	{
		if(GameManager.instance == null)
			Instantiate(settings);

		//if(GameManager.instance == null)
		//	Instantiate(gameManager);
	}
}
