using UnityEngine;
using System.Collections.Generic;

public class SpeedLimitController {
	
	private List<Road> current;
	public Utils.VehicleSpeed currentRoadSpeed;
	public int activeIndex;
	
	public SpeedLimitController () {
		current = new List<Road>();
		activeIndex = -1;
		currentRoadSpeed = 0;
	}
	
	public void AddRoad(Road road)
	{
		if(road != null)
		{
			current.Add(road);
		}
	}
	
	public Road IsInsideRoadBoundaries(float posZ)
	{
		foreach(Road road in current)
		{
			if(posZ >= road.bot && posZ < road.top)
			{
				activeIndex = current.IndexOf(road);
				return road;
			}
		}
		
		activeIndex = -1;
		return null;
	}
	
	public bool IsVehicleOnCurrentRoad(float vehicleZ)
	{
		if(activeIndex != -1 && current.Count > 0 && activeIndex < current.Count)
		{
			Road currentRoad = current[activeIndex];
			return vehicleZ < currentRoad.top && vehicleZ > currentRoad.bot;
		}
		return false;
	}
}
