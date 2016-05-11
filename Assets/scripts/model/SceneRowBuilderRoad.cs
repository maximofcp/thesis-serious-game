using UnityEngine;
using System.Collections.Generic;


/*
 * Builds a road row
 */
public class SceneRowBuilderRoad : ISceneBlockBuilder
{
	private GameObject pfRoad;
	private GameObject pfCrosswalk;
	private GameObject pfRoadOffset;
	
	public SceneRowBuilderRoad(GameObject prefab, GameObject offset, GameObject crosswalkPrefab)
	{
		pfRoad = prefab;
		pfRoadOffset = offset;
		pfCrosswalk = crosswalkPrefab;
	}
	
	public List<SceneObject> GenerateSceneRow(float startX, float endX, float leftBound, float rightBound)
	{
		List<SceneObject> sceneObjects = new List<SceneObject>();
		GameObject prefabToUse = pfRoad;
		float sizeXToUse = Constants.Dimension.CrosswalkWithSidewalkSizeX;
		
		int index = 0;
		bool hasCrosswalk = false;								// tells whether a crosswalk has been placed or not
		int crosswalkIndex = Random.Range(0, (int) ( (Mathf.Abs (rightBound) + Mathf.Abs (leftBound)) / sizeXToUse) );	// divides bounds and calculates how many road blocks can be created and then generates an integer pointing for the crosswalk position
		
		
		while(startX < endX)
		{
			if(startX < leftBound || startX >= rightBound)
			{
				prefabToUse = pfRoadOffset;
				sizeXToUse = Constants.Dimension.CrosswalkWithSidewalkSizeOffset;
			}
			else 
			{
				if(!hasCrosswalk && index == crosswalkIndex)
				{
					prefabToUse = pfCrosswalk;
					sizeXToUse = Constants.Dimension.CrosswalkWithSidewalkSizeX;
					hasCrosswalk = true;
				}
				else
				{
					prefabToUse = pfRoad;
					sizeXToUse = Constants.Dimension.CrosswalkWithSidewalkSizeX;
				}
				index++;
			}
			
			sceneObjects.Add(new SceneObject(prefabToUse, sizeXToUse, Constants.Dimension.CrosswalkWithSidewalkSizeZ));
			startX += sizeXToUse;
		}
		
		return sceneObjects;
	}
}
