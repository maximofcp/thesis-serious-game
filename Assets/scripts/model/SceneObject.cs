using UnityEngine;
using System.Collections;

/*
 * Represents a static object to be inserted on the scene
 */
public class SceneObject
{
	public GameObject prefab;
	public float sizeX;
	public float sizeZ;
	public float level;
	public bool instantiated;
	
	public SceneObject(GameObject obj, float sizeX, float sizeZ, float level)
	{
		prefab = obj;
		this.sizeX = sizeX;
		this.sizeZ = sizeZ;
		this.level = level;
		instantiated = false;
	}
	
	public SceneObject(GameObject obj, float sizeX, float sizeZ)
	{
		prefab = obj;
		this.sizeX = sizeX;
		this.sizeZ = sizeZ;
		level = 1;
		instantiated = false;
	}
}