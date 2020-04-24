using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionController : MonoBehaviour {

	public TextMesh missionValue;
	public static float timeLeft;

	// Use this for initialization
	void Start () {
		timeLeft = 70;
	}
	
	// Update is called once per frame
	void Update () {

		Timer ();
	}

	public void Timer()
	{
		
		if (timeLeft <= 0)
		{
			SceneManager.LoadScene("Menu");
			timeLeft = 0;
		}

		else if (SceneManager.GetActiveScene ().name == "Mission3") {
			timeLeft -= Time.deltaTime;
		}

		missionValue.text = "Zaman: " + string.Format("{0:0}", timeLeft.ToString());
	}
}
