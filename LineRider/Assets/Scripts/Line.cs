using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour {
	//seralized global private variables to be edited in the inspector

	[SerializeField]float lineWidth=1;


	//gloabl private variables
	LineRenderer lineRenderer;//a reference to the line rendere component
	List<Vector3> verticies = new List<Vector3>();//a list of vector3 used to store the vertices being passed from LineDrawer.cs to the line renderer 

	// Use this for initialization
	void Awake () {
		lineRenderer=GetComponent<LineRenderer>();//access the line renderer
		lineRenderer.startWidth=lineWidth;//set the line width
		lineRenderer.endWidth=lineWidth;
	}


	public void AddVertex(Vector3 _pointToAdd){
		//take a vector3 and add it to the vertex list then draw the line.. method is called in LineDrawer.cs
		verticies.Add(_pointToAdd);
		DrawLine();
	}

	public void FinishLine(){
		//wrapper method for SegmentLine so that other classes\scripts cannot access it directly 
		SegmentLine();
	}

	void DrawLine(){
		//set the vertex count for the line render and add the vertices to it
		lineRenderer.positionCount=verticies.Count;
		lineRenderer.SetPositions(verticies.ToArray());
	}

	void SegmentLine(){
		//check to make sure there are at least two points in the vertex array and then loop through the pairs of vertices and send them to DrawCollider
		//check to make sure there are at least two points in the vertex array
		if(verticies.Count>1){
			//loop through the pairs of vertices and draw a box collider around them
			for(int i=0;i<verticies.Count-1;i++){
				DrawCollider(verticies[i],verticies[i+1]);
			}
		}
	}
	void DrawCollider(Vector3 _startPoint, Vector3 _endPoint){
		//draw a box collider around two points in space
		//calcualte the angle between the lines in degrees
		float angle=Mathf.Atan2(_endPoint.y-_startPoint.y,_endPoint.x-_startPoint.x)*Mathf.Rad2Deg;
		//get the mid point between the lines and set the position 
		float midX=(_startPoint.x+_endPoint.x)/2;
		float midy=(_startPoint.y+_endPoint.y)/2;
		Vector3 position= new Vector3(midX,midy,0);
		//get how long you want the line to be 
		float lineLength=Vector3.Distance(_startPoint,_endPoint);
		//create a new game object to hold the box collider, add the box collider to it
		GameObject go=new GameObject();
		go.AddComponent<BoxCollider2D>();
		BoxCollider2D bc= go.GetComponent<BoxCollider2D>();
		//set all its properties using the previously defined variables
		go.transform.position=position;
		go.transform.rotation=Quaternion.Euler(new Vector3(0,0,angle));
		go.transform.SetParent(this.gameObject.transform);
		bc.size=new Vector2(lineLength,lineWidth);
		go.layer=8;//layer 8 is the obstacles layer used to define ground
	}

	public struct FindEndOut{
		public float averageAngle;
		public int currentIndex;
	}

}

