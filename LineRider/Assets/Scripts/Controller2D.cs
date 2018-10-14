using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	//global public variables
	public CollisionInfo collisions;//consider changing to private if nothing outside this program can see it
	//seralized global private variables to be edited in the inspector
	[Tooltip("layers to include for collision detection of 2Dcontroller")]
	[SerializeField] LayerMask collisionMask;
	[Tooltip("The number of rays to send out in the horizontal direction (front and back)")]
	[Range(2,300)]
	[SerializeField] int horizontalRayCount=4;
	[Tooltip("The number of rays to send out in the vertical direction (top and bottom)")]
	[Range(2,300)]
	[SerializeField] int verticalRayCount=4;

	//global private constant variables
	const float skinWidth = .015f;//the width inside the collider from which to iniate any raycasts

	//global private variables
	Bounds bounds;//a reference to the bounds of the box collider
	new BoxCollider2D collider;//a reference to the box collider attached to this gameobject
	float horizontalRaySpacing;//the spacing in world units between horizontal (front and back) raycasts
	RaycastOrigins raycastOrigins;//a refernce to a raycastOrigins struct which contains vector2s representing the corners of the collider
	float verticalRaySpacing;//the spacing in world units between vertical (top and bottom) raycasts

	// Use this for initialization
	void Start () {
		collider=GetComponent<BoxCollider2D>();//get the collider from the gameobject
		CalculateRaySpacing();
	}


	public void Move(Vector3 _velocity){
		UpdateRaycastOrigins();
		collisions.Reset();
		if(_velocity.x!=0){
			HorizontalCollisions(ref _velocity);
		}
		if(_velocity.y!=0){
			VerticalCollisions(ref _velocity);
		}
		transform.Translate(_velocity);
	}

	void UpdateRaycastOrigins(){
		//update bounds then update the current positions of the corners of the box collider based on those bounds
		//update the bounds of the object
		bounds = collider.bounds;
		bounds.Expand(skinWidth*-2);
		//update the current positions of the corners of the box collider based on the bounds
		raycastOrigins.bottomLeft=new Vector2(bounds.min.x,bounds.min.y);
		raycastOrigins.bottomRight=new Vector2(bounds.max.x,bounds.min.y);
		raycastOrigins.topLeft=new Vector2(bounds.min.x,bounds.max.y);
		raycastOrigins.topRight=new Vector2(bounds.max.x,bounds.max.y);
	}

	void CalculateRaySpacing(){
		//calcualte the horizontal and vertical spacing between raycasts 
		bounds = collider.bounds;
		bounds.Expand(skinWidth*-2);
		horizontalRaySpacing=bounds.size.y/(horizontalRayCount-1);
		verticalRaySpacing=bounds.size.x/(verticalRayCount-1);
	}

	void VerticalCollisions(ref Vector3 _velocity){
		//detect verticle collisions (top and bottom) and adjust player position accordingly 
		float directionY=Mathf.Sign(_velocity.y);//get the direction the player is moving vertically
		float rayLength=Mathf.Abs(_velocity.y)+skinWidth;//set the length of the collision dectection ray equal to the distance the player wants to move this frame
		//loop through each of the raycast origins and cast a ray to check for colliions 
		for (int i=0; i<verticalRayCount;i++){
			Vector2 rayOrigin = (directionY==-1)?raycastOrigins.bottomLeft+Vector2.right*(verticalRaySpacing*i+_velocity.x):raycastOrigins.topLeft+Vector2.right*(verticalRaySpacing*i+_velocity.x);//determine which end (top vs bottom) to detect collisions from and calculate the length
			RaycastHit2D hit=Physics2D.Raycast(rayOrigin,Vector2.up*directionY,rayLength,collisionMask);//send out the raycast
			Debug.DrawRay(rayOrigin,Vector2.up*directionY*rayLength,Color.red);//debug statement may remove
			//if a raycast hits anything change the movement distance accordinly, shorten the raylength to match that distance, toggle the appropriate collision information
			if(hit){
				_velocity.y=(hit.distance-skinWidth)*directionY;
				rayLength=hit.distance;
				//update the the collision information 
				collisions.below= directionY ==-1;
				collisions.above= directionY ==1;
			}
		}
	}

	void HorizontalCollisions(ref Vector3 _velocity){
		//detect horizontal collisions (left and right) and adjust player move distance accordingly for detailed comments see VerticalCollisions method
		float directionX=Mathf.Sign(_velocity.x);
		float rayLength=Mathf.Abs(_velocity.x)+skinWidth;
		for (int i=0; i<horizontalRayCount;i++){
			Vector2 rayOrigin = (directionX==-1)?raycastOrigins.bottomLeft+Vector2.up*(horizontalRaySpacing*i):raycastOrigins.bottomRight+Vector2.up*(horizontalRaySpacing*i);
			RaycastHit2D hit=Physics2D.Raycast(rayOrigin,Vector2.right*directionX,rayLength,collisionMask);
			Debug.DrawRay(rayOrigin,Vector2.right*directionX*rayLength,Color.red);
			if(hit){
				_velocity.x=(hit.distance-skinWidth)*directionX;
				rayLength=hit.distance;

				collisions.left= directionX ==-1;
				collisions.right= directionX ==1;

			}
		}
	}

	struct RaycastOrigins{
		//a struct to hold the corners of the box collider
		public Vector2 topLeft,topRight,bottomLeft,bottomRight;
	}

	public struct CollisionInfo{
		//a struct to hold the information about which corners of the game object are currently colliding and a method to reset them
		public bool above,below,left,right;

		public void Reset(){
			//reset all the collision tags 
			above=below=left=right=false;
		}
	}

}
