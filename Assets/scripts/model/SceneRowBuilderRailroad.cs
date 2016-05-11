using UnityEngine;
using System.Collections.Generic;

/*
 * Builds a railroad row
 */
public class SceneRowBuilderRailroad : ISceneBlockBuilder
{
	private GameObject pfRailroad;
	private GameObject pfRailroadPassage;
	
	public SceneRowBuilderRailroad(GameObject prefab, GameObject prefabPassage)
	{
		pfRailroad = prefab;
		pfRailroadPassage = prefabPassage;
	}
	
	public List<SceneObject> GenerateSceneRow(float startX, float endX, float leftBound, float rightBound)
	{
		List<SceneObject> sceneObjects = new List<SceneObject>();
		GameObject prefabToUse = pfRailroad;
		float sizeXToUse = Constants.Dimension.RailroadSizeX;
		
		int index = 0;
		bool hasCrossing = false;								// tells whether a crossing has been placed or not
		int crossingIndex = Random.Range(0, (int) ( (Mathf.Abs (rightBound) + Mathf.Abs (leftBound)) / sizeXToUse) );	// divides bounds and calculates how many road blocks can be created and then generates an integer pointing for the crossing position
		
		while(startX < endX)
		{
			if(startX < leftBound || startX >= rightBound)
			{
				prefabToUse = pfRailroad;		// to be changed for offset
			}
			else
			{
				if(!hasCrossing && index == crossingIndex)
				{
					hasCrossing = true;
					prefabToUse = pfRailroadPassage;
				}else
				{
					prefabToUse = pfRailroad;
				}
				
				index++;
			}
			
			
			sceneObjects.Add(new SceneObject(prefabToUse, sizeXToUse, Constants.Dimension.RailroadSizeZ));
			startX += sizeXToUse;
		}
		
		return sceneObjects;
	}
}