using UnityEngine;
using System.Collections;

[System.Serializable]
public class SceneSettings
{
	public float playerWindowSizeX;
	public float playerWindowSizeZ;
	public Vector3 playerStartPosition;
	public float playerBoundaryZ;
	public float playerSpeed;
	public float cameraSpeed;
	public float generationOffsetX;
	public float generationOffsetZ;
	public float defaultGenerationPositionZ;
	public float defaultGenerationPositionY;
	public string sceneObjectsNodeName;
	public int crosswalkDensity;
	public float generateFrequency;
	
	public float trafficLightsInterval;
	public float trafficLightsWaitTime;
}
