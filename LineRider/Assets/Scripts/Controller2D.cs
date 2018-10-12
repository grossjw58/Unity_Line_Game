using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	//seralized global private variables to be edited in the inspector
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
	BoxCollider2D collider;//a reference to the box collider attached to this gameobject
	float horizontalRaySpacing;//the spacing in world units between horizontal (front and back) raycasts
	RaycastOrigins raycastOrigins;//a refernce to a raycastOrigins struct which contains vector2s representing the corners of the collider
	float verticalRaySpacing;//the spacing in world units between vertical (top and bottom) raycasts

	// Use this for initialization
	void Start () {
		collider=GetComponent<BoxCollider2D>();//get the collider from the gameobject
		bounds = collider.bounds;//update the bounds of the object
	}
	// Update is called once a frame
	void Update(){
		UpdateBounds();
		UpdateRaycastOrigins();
		CalculateRaySpacing();
		//For testing purposes will remove later 
		for (int i=0; i<verticalRayCount;i++){
			Debug.DrawRay(raycastOrigins.bottomLeft + Vector2.right*verticalRaySpacing*i,Vector2.up*-2,Color.red);
		}
	}

	void UpdateBounds(){
		//updates the bounds based on the collider to make sure it is current with object movement and any time the collider changes size
		bounds = collider.bounds;
		bounds.Expand(skinWidth*-2);
	}

	void UpdateRaycastOrigins(){
		//update the current positions of the corners of the box collider based on the bounds
		raycastOrigins.bottomLeft=new Vector2(bounds.min.x,bounds.min.y);
		raycastOrigins.bottomRight=new Vector2(bounds.max.x,bounds.min.y);
		raycastOrigins.topLeft=new Vector2(bounds.min.x,bounds.max.y);
		raycastOrigins.topRight=new Vector2(bounds.max.x,bounds.max.y);
	}

	void CalculateRaySpacing(){
		//calcualte the horizontal and vertical spacing between raycasts 
		horizontalRaySpacing=bounds.size.y/(horizontalRayCount-1);
		verticalRaySpacing=bounds.size.x/(verticalRayCount-1);
	}

	struct RaycastOrigins{
		//a struct to hold the corners of the box collider
		public Vector2 topLeft,topRight,bottomLeft,bottomRight;
	}

}
