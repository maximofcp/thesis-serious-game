using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController{

	public Text scoreUI;
	private int currentScore;


	// Use this for initialization
	public ScoreController() {
		scoreUI = GameObject.Find("Score").GetComponent<Text>();
		currentScore = 0;
		UpdateScore();
	}

	public void AddScoreVehicleHit()
	{
		currentScore += Constants.ScoreVehicleHit;
		UpdateScore();
	}

	public void SubScoreVehicleHit()
	{
		currentScore -= Constants.ScoreVehicleHit;
		UpdateScore();
	}

	public void AddScoreCrosswalkCrossed()
	{
		currentScore += Constants.ScoreCrosswalkCrossed;
		UpdateScore();
	}

	public void SubScoreCrosswalkCrossed()
	{
		currentScore -= Constants.ScoreCrosswalkCrossed;
		UpdateScore();
	}

	void UpdateScore()
	{
		scoreUI.text = currentScore.ToString();
	}

}
