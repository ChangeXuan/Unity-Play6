using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour {

	private Vector3 nowPos;
	private Vector3 oldPos;

	public float lenX = 5f;
	
	// Update is called once per frame
	void Update () {
		nowPos = Input.mousePosition;
		if (Input.GetMouseButton(0)) {
			Vector3 offset = nowPos - oldPos;
			if (Mathf.Abs (offset.x) > Mathf.Abs (offset.y) && Mathf.Abs (offset.x) > lenX) {
				transform.Rotate (Vector3.up, -offset.x);
			}
		}
		oldPos = Input.mousePosition;
	}
}
