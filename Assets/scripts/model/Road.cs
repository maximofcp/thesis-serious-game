using UnityEngine;
using System.Collections;

public class Road
{
	public float top;
	public float bot;
	public Utils.VehicleSpeed speedlimit;
	
	public Road(float topLimit, float bottomLimit, Utils.VehicleSpeed speed)
	{
		top = topLimit;
		bot = bottomLimit;
		speedlimit = speed;
	}
}