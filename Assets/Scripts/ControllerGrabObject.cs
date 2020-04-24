using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerGrabObject : MonoBehaviour {

	public TextMesh mission2Val;
	public TextMesh mission3Val;

	public TextMesh missionValue;

	public static int numberOfMove;

	private SteamVR_TrackedObject trackedObj;

	// 1
	private GameObject collidingObject; 
	// 2
	private GameObject objectInHand; 

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void Start()
	{
		if (SceneManager.GetActiveScene ().name == "Mission2") {
			numberOfMove = 35;
		}
	}


	private void SetCollidingObject(Collider col)
	{
		// 1
		if (collidingObject || !col.GetComponent<Rigidbody>())
		{
			return;
		}
		// 2
		collidingObject = col.gameObject;
	}

	// 1
	public void OnTriggerEnter(Collider other)
	{
		SetCollidingObject(other);
	}

	// 2
	public void OnTriggerStay(Collider other)
	{
		SetCollidingObject(other);
	}

	// 3
	public void OnTriggerExit(Collider other)
	{
		if (!collidingObject)
		{
			return;
		}

		collidingObject = null;
	}

	private void GrabObject()
	{
		// 1
		objectInHand = collidingObject;
		collidingObject = null;
		// 2
		var joint = AddFixedJoint();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
	}

	// 3
	private FixedJoint AddFixedJoint()
	{
		FixedJoint fx = gameObject.AddComponent<FixedJoint>();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}
		
	private void ReleaseObject()
	{
		// 1
		if (GetComponent<FixedJoint>())
		{
			// 2
			GetComponent<FixedJoint>().connectedBody = null;
			Destroy(GetComponent<FixedJoint>());
			// 3
			objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
			objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;

		}
		// 4

		if(objectInHand.tag=="Finish")
		{
			SceneManager.LoadScene("Menu", LoadSceneMode.Single);
		}

		if(objectInHand.tag=="mission1")
		{
			SceneManager.LoadScene("Mission1", LoadSceneMode.Single);
		}

		if(objectInHand.tag=="mission2")
		{
			SceneManager.LoadScene("Mission2", LoadSceneMode.Single);
		}

		if(objectInHand.tag=="mission3")
		{
			SceneManager.LoadScene("Mission3", LoadSceneMode.Single);
		}

		if(objectInHand.tag=="exit")
		{
			Application.Quit ();
		}

		objectInHand = null;
	}
		

	// Update is called once per frame
	void Update () {

		// 1
		if (Controller.GetHairTriggerDown())
		{
			if (collidingObject)
			{
				GrabObject();
			}
		}

		// 2
		if (Controller.GetHairTriggerUp())
		{
			if (objectInHand) {		
				
				if (objectInHand.tag == "Finish") {
					numberOfMove += 1;
				}
					
					ReleaseObject ();

					if (SceneManager.GetActiveScene ().name == "Mission2") {
						numberOfMove -= 1;
						missionValue.text = "Hamle: " + numberOfMove;

						if (numberOfMove < 1) {
							SceneManager.LoadScene ("Menu", LoadSceneMode.Additive);
						}
					}
			}
			mission2Val.text = "2. görevden kalan hamle sayısı "+ numberOfMove;
			mission3Val.text = "3. görevden kalan " + string.Format("{0:0}", MissionController.timeLeft.ToString())+ " sn";
		}
	}
}
