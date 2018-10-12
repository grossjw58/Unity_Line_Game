using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Controller2D))]
public class PlayerMotor : MonoBehaviour {
	//global private variables
	Controller2D controller;//a refernce to the Controller2D script attached to the game object

	// Use this for initialization
	void Start(){
		controller = GetComponent<Controller2D>();//get the Controller2D from the gameobject
	}
}
