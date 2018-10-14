using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour {
	//seralized global private variables to be edited in the inspector
	[SerializeField]float angleThreshold=5;
	[SerializeField]float lineWidth=1;

	[SerializeField]GameObject tempPointPrefab;

	//gloabl private variables
	LineRenderer lineRenderer;//a reference to the line rendere component
	List<Vector3> verticies = new List<Vector3>();//a list of vector3 used to store the vertices being passed from LineDrawer.cs to the line renderer 

	// Use this for initialization
	void Awake () {
		lineRenderer=GetComponent<LineRenderer>();//access the line renderer
		lineRenderer.startWidth=lineWidth;
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
		lineRenderer.SetVertexCount(verticies.Count);
		lineRenderer.SetPositions(verticies.ToArray());
	}

	void SegmentLine(){
		//add a new edge collider convert the vector3 vertices to vector2 then set the points for the edge collider
		//check to make sure there are at least two points in the vertex array
		if(verticies.Count>1){
			for(int i=0;i<verticies.Count-2;i++){
				DrawCollider(verticies[i],verticies[i+1]);
			}
		}
	}
	void DrawCollider(Vector3 _startPoint, Vector3 _endPoint){
		float angle=Mathf.Atan2(_endPoint.y-_startPoint.y,_endPoint.x-_startPoint.x)*Mathf.Rad2Deg;
		float midX=(_startPoint.x+_endPoint.x)/2;
		float midy=(_startPoint.y+_endPoint.y)/2;
		Vector3 position= new Vector3(midX,midy,0);
		float lineLength=Vector3.Distance(_startPoint,_endPoint);
		GameObject boxcol=new GameObject();
		boxcol.transform.position=position;
		boxcol.transform.rotation=Quaternion.Euler(new Vector3(0,0,angle));
		boxcol.transform.SetParent(this.gameObject.transform);
		boxcol.AddComponent<BoxCollider2D>();
		BoxCollider2D bc= boxcol.GetComponent<BoxCollider2D>();
		bc.size=new Vector2(lineLength,lineWidth);
	}

	public struct FindEndOut{
		public float averageAngle;
		public int currentIndex;
	}

}

