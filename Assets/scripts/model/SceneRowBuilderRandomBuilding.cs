using UnityEngine;
using System.Collections.Generic;


/*
 * Builds a random building row
 */
public class SceneRowBuilderRandomBuilding : ISceneBlockBuilder
{
	private List<GameObject> pfBuildings;
	private GameObject pfOffset;
	
	public SceneRowBuilderRandomBuilding(List<GameObject> buildings, GameObject offset)
	{
		pfBuildings = buildings;
		pfOffset = offset;
	}
	
	public List<SceneObject> GenerateSceneRow(float startX, float endX, float leftBound, float rightBound)
	{
		List<SceneObject> sceneObjects = new List<SceneObject>();
		
		if(pfBuildings.Count > 0)
		{
			
			GameObject prefabToUse = pfBuildings[0];
			float sizeXToUse = Constants.Dimension.BuildingSizeX;
			
			while(startX < endX)
			{
				if(startX < leftBound || startX >= rightBound)
				{
					prefabToUse = pfOffset;
					sizeXToUse = Constants.Dimension.GrassSizeOffset;
				}
				else
				{
					int toUse = Random.Range(0, pfBuildings.Count);
					prefabToUse = pfBuildings[toUse];
					sizeXToUse = Constants.Dimension.BuildingSizeX;
				}
				
				sceneObjects.Add(new SceneObject(prefabToUse, sizeXToUse, Constants.Dimension.BuildingSizeZ));
				startX += sizeXToUse;
			}
		}
		
		return sceneObjects;
	}
}