using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public float m_speed;
	public float m_posXMax;
	public float m_posXMin;
	public float m_posYMax;
	public float m_posYMin;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 PosFinger = Input.mousePosition;
		PosFinger.x /=Screen.width;
		PosFinger.y /= Screen.height;
		Vector2 posToCenter = new Vector2(-0.5f + PosFinger.x, -0.5f + PosFinger.y)*2;
		transform.position += new Vector3(posToCenter.x, 0f,posToCenter.y)*m_speed*Time.deltaTime;

		if(transform.position.x > m_posXMax){
			transform.position += new Vector3(- 2 * m_posXMax, 0f,0f);
		}
		if(transform.position.x < m_posXMin){
			transform.position += new Vector3(- 2 * m_posXMin, 0f,0f);
		}

		if(transform.position.z > m_posYMax){
			transform.position += new Vector3(0f , 0f,- 2 * m_posYMax);
		}

		if(transform.position.z < m_posYMin){
			transform.position += new Vector3(0f , 0f,- 2 * m_posYMin);
		}


	}
}
