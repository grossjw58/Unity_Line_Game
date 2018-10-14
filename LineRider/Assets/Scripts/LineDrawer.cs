using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour {
	//seralized global private variables to be edited in the inspector
	[Tooltip("The linePrefab gameobject which houses the lineRenderer component")]
	[SerializeField]GameObject linePrefab;
	[Tooltip("The minimum amount of distance away from the previous vertex the mouse must be before passing new vertex to the linePrefab")]
	[SerializeField]float minDrawDistance;
	[Tooltip("The minimum amount of time to wait between passing a new vertex to the linePrefab")]
	[SerializeField]float timeToDraw;
	//gloabl private variables
	float currentDrawTime=0;//the amount of time in ms since the last vertex was added
	Line currentLine;//a referece to the current linePrefab game object being edited
	Vector3 prevPoint;//a reference to the position of the last added vertex


	// Update is called once per frame
	void Update () {
		CheckLines();
		currentDrawTime+=Time.deltaTime;//increment currentDrawTime by time.deltatime
	}

	void CheckLines(){
		/*This method checks the current position of the mouse in world coordinates and checks user input to see if it should create a new linePrefab, 
		add a vertex to an exisitng linePrefab, or finish the current linePrefab*/

		//get the mouse position in world corridinates and set the z value to 0;
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
		mousePos.z=0;
		//first time mouse is down...create a new linePrefab game object, pass it the first vertex, reset drawTime and prevPoint
		if(Input.GetMouseButtonDown(1)){
			GameObject line=GameObject.Instantiate(linePrefab);
			currentLine=line.GetComponent<Line>();
			currentLine.AddVertex(mousePos);
			currentDrawTime=0;
			prevPoint=mousePos;
		}
		/*mouse is being held down and enough time has passed to draw another point and the mouse if far enough from the previous point...
		pass another vertex to the intanced linePrefab game object, reset drawTime and prevPoint*/
		else if(Input.GetMouseButton(1) && currentDrawTime>=timeToDraw && (Mathf.Abs(Vector3.Distance(prevPoint,mousePos))>minDrawDistance)){
			currentLine.AddVertex(mousePos);
			currentDrawTime=0;
			prevPoint=mousePos;
		}
		//mouse has been released...tell the instanced linePrefab game object it is done, set currentDrawTime to the drawWaitTime
		else if(Input.GetMouseButtonUp(1)){
			currentLine.AddVertex(mousePos);
			currentLine.FinishLine();
			currentDrawTime=timeToDraw;
		}
	}

}
