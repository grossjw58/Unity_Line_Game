using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour {

	//gloabl private variables
	LineRenderer lineRenderer;//a reference to the line rendere component
	List<Vector3> verticies = new List<Vector3>();//a list of vector3 used to store the vertices being passed from LineDrawer.cs to the line renderer 

	// Use this for initialization
	void Awake () {
		lineRenderer=GetComponent<LineRenderer>();//access the line renderer
	}


	public void AddVertex(Vector3 _pointToAdd){
		//take a vector3 and add it to the vertex list then draw the line.. method is called in LineDrawer.cs
		verticies.Add(_pointToAdd);
		DrawLine();
	}

	public void FinishLine(){
		//wrapper method for DrawCollider so that other classes\scripts cannot access it directly 
		DrawCollider();
	}

	void DrawLine(){
		//set the vertex count for the line render and add the vertices to it
		lineRenderer.SetVertexCount(verticies.Count);
		lineRenderer.SetPositions(verticies.ToArray());
	}

	void DrawCollider(){
		//add a new edge collider convert the vector3 vertices to vector2 then set the points for the edge collider
		EdgeCollider2D edge = this.gameObject.AddComponent<EdgeCollider2D>();
		Vector2[] colVerts = new Vector2[verticies.Count];
		//convert Vec3 to Vec2
		for(int i=0; i<verticies.Count;i++){
			Vector2 temp = verticies[i];
			colVerts[i]=temp;
		}
		edge.points=colVerts;
	}

}
