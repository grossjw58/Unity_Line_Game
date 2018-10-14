using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Controller2D))]
public class PlayerMotor : MonoBehaviour {
	//seralized global private variables to be edited in the inspector
	[Tooltip("The distance off the ground you want the player to be able to jump measured in world units")]
	[SerializeField]float jumpHeight=4f;
	[Tooltip("The value used to smooth the horizontal movement while in the air")]
	[SerializeField]float horizontalSmoothingInAir=.2f;
	[Tooltip("The value used to smooth the horizontal movement while on the ground")]
	[SerializeField]float horizontalSmoothingOnGround=.1f;
	[Tooltip("The horizontal speed at which you want the player to move measured in world units per second")]
	[SerializeField]float moveSpeed=6f;
	[Tooltip("The time it should take the player to reach maximum jump height measured in seconds")]
	[SerializeField]float timeToJumpHeight=.4f;
	//global private variables
	Controller2D controller;//a refernce to the Controller2D script attached to the game object
	Vector3 velocity;//a vector3 to hold how far the player will move this frame
	float gravity;//the gravity used for the player... THIS IS NOT UNIVERSAL
	float jumpVelocity;//the upward veloicty used when the player jumps
	float velocityXSmoothing;//a reference variable used to smoothout horizontal movement...DO NOT ASSIGN THIS A VALUE

	// Use this for initialization
	void Start(){
		controller = GetComponent<Controller2D>();//get the Controller2D from the gameobject
		//calculate the two jump variable given the player inputs
		gravity = -(2*jumpHeight)/Mathf.Pow(timeToJumpHeight,2);
		jumpVelocity=Mathf.Abs(gravity)*timeToJumpHeight;
	}

	// Update is called once a frame
	void Update(){
		//change vertical velocity to zero if the player has collided with something above or bellow itself
		if(controller.collisions.above||controller.collisions.below){
			velocity.y=0;
		}
		//get the input from the user and check for jumping 
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
		if(Input.GetKeyDown(KeyCode.Space)&&controller.collisions.below){
			velocity.y=jumpVelocity;
		}
		//smooth out the horizontal movement... smooth movement different if in air or on ground
		velocity.x=Mathf.SmoothDamp(velocity.x,input.x*moveSpeed,ref velocityXSmoothing,(controller.collisions.below)?horizontalSmoothingOnGround:horizontalSmoothingInAir);
		velocity.y+=(gravity*Time.deltaTime);
		//pass the velocity to the controller to process
		controller.Move(velocity*Time.deltaTime);
	}
}
