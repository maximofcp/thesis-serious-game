using UnityEngine;
using System.Collections;

public class Constants
{
	public class Probability
	{
		public const float ProbabilityLow = 0.2f;
		public const float ProbabilityMedium = 0.5f;
		public const float ProbabilityHigh = 0.8f;
	}
	
	public class Dimension
	{
		public const float PathSizeX = 16;		//*
		public const float PathSizeZ = 8;		//*
		public const float CrosswalkSizeX = 8;	//*
		public const float CrosswalkSizeZ = 17;	//*
		public const float RoadSizeX = 16;		//*
		public const float RoadSizeZ = 17;		//*
		
		public const float CrosswalkWithSidewalkSizeX = 16;		//*
		public const float CrosswalkWithSidewalkSizeZ = 33;		//*
		public const float CrosswalkWithSidewalkSizeOffset = 48;	//*
		public const float RoadWithSidewalkAvenueSizeX = 16;		//*
		public const float RoadWithSidewalkAvenueSizeZ = 54;		//*
		public const float RoadWithSidewalkAvenueSizeOffset = 48;		//*
		public const float BuildingSizeX = 16;		//*
		public const float BuildingSizeZ = 16;		//*
		public const float RailroadSizeX = 16;		//*
		public const float RailroadSizeZ = 24;		//*
		public const float GrassSizeOffset = 48;		//*
		public const float SpeedIndicatorOffsetY = 6;
		
		public const float LaneNormalRightDirectionZ = 12;		//*
		public const float LaneNormalLeftDirectionZ = 21;		//*
		public const float LaneAvenueRightDirectionRZ = 21;		//*
		public const float LaneAvenueRightDirectionLZ = 12;		//*
		public const float LaneAvenueLeftDirectionRZ = 42;		//*
		public const float LaneAvenueLeftDirectionLZ = 33;		//*
	}
	
	public class Tag
	{
		public const string TagVehicle = "Vehicle";
		public const string TagBicycle = "Bicycle";
		public const string TagPath = "Path";
		public const string TagCrosswalk = "Crosswalk";
		public const string TagRoad = "Road";
		public const string TagVehicleStopZone = "VehicleStopZone";
		public const string TagVehicleStopZoneAvenueRight = "VehicleStopZoneAvenueR";
		public const string TagVehicleStopZoneAvenueLeft = "VehicleStopZoneAvenueL";
		public const string TagIntentionToCrossA = "IntentionToCrossA";
		public const string TagIntentionToCrossB = "IntentionToCrossB";
		public const string TagVehicleWarningZone = "VehicleWarningZone";
		public const string TagObstacle = "Obstacle";
		public const string TagBuilding = "Building";
	}
	
	public class Vehicle
	{
		public const int VehicleMinSpeed = 8;
		public const int VehicleMinSpeedModel = 0;
		public const int VehicleMaxSpeedModel = 20;
		public const int VehicleMinSpeedView = 0;
		public const int VehicleMaxSpeedView = 100;
	}
	
	public class Animation
	{
		public const string AnimPlayerIdle = "Idle";
		public const string AnimPlayerWalking = "Walking";
	}
	
	public class Score
	{
		public const int ScoreVehicleHit = 100;
		public const int ScoreCrosswalkCrossed = 200;
	}
	
}
