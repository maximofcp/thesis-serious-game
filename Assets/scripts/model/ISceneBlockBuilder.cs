using UnityEngine;
using System.Collections.Generic;

/*
 * Interface for a generic scene row
 */
public interface ISceneBlockBuilder
{
	List<SceneObject> GenerateSceneRow(float startX, float endX, float leftBound, float rightBound);
}
