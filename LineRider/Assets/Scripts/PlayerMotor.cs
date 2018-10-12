using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Controller2D))]
public class PlayerMotor : MonoBehaviour {
	//seralized global private variables to be edited in the inspector
	[Tooltip("The gravity to be used on the player meaured in world units per second")]
	[SerializeField]float gravity=-20f;
	[Tooltip("The horizontal speed at which you want the player to move measured in world units per second")]
	[SerializeField]float moveSpeed=6f;
	//global private variables
	Controller2D controller;//a refernce to the Controller2D script attached to the game object
	Vector3 velocity;//a vector3 to hold how far the player will move this frame


	// Use this for initialization
	void Start(){
		controller = GetComponent<Controller2D>();//get the Controller2D from the gameobject
	}

	// Update is called once a frame
	void Update(){

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));

		velocity.x=input.x*moveSpeed;
		velocity.y+=gravity*Time.deltaTime;
		controller.Move(velocity*Time.deltaTime);
	}
}
