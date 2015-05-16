using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JIAOCirclingRecognizer : MonoBehaviour {

	//public GameObject m_prefabEnv;
	//public LineRenderer m_line;
	//public Camera m_MainCamera;
	private List<Vector3> m_previousPoints = new List<Vector3>();
	private List<float> m_timePreviousPoints = new List<float> ();
	public float m_timeMax;
	public float m_epsilonTime;
	private float m_currentTime;
	public float m_epsilonDist;
	public float m_minRadius;
	public float m_maxRadius;


	// Use this for initialization
	void Start () {
		m_previousPoints.Add(transform.position);
		m_timePreviousPoints.Add(0f);
	
	}
	
	// Update is called once per frame
	void Update () {

		//////////Direction Avatar: useles to the proto just for testing
		Ray ray = Camera.main.ScreenPointToRay( new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		RaycastHit[] hits = Physics.RaycastAll (ray);
		for (int i=0; i < hits.Length; i++) {
			if(hits[i].collider.gameObject.tag ==  "Terrain"){
				transform.position = hits[i].point + Vector3.up * 0.5f;
			}
		}
		//////////


		m_currentTime += Time.deltaTime;
		UpdateTimeQueue();

		if (m_previousPoints.Count != 0) {
			if(m_currentTime >= m_epsilonTime){
				UpdateAddPoint(transform.position);
				m_currentTime = 0f;
			}
		} 
		else {
			UpdateAddPoint(transform.position);
		}

		/*if (m_timePreviousPoints.ToArray()(m_timePreviousPoints.Count - 1)> m_timeMax ) {
			m_previousPoints.Dequeue();
			m_timePreviousPoints.Dequeue();
		}*/

		Circling ();

	}

	void UpdateAddPoint(Vector3 point){
		if (m_previousPoints.Count > 3) {
			Vector3 t1 = (m_previousPoints [m_previousPoints.Count - 3] - m_previousPoints [m_previousPoints.Count - 2]).normalized;
			Vector3 t2 = (m_previousPoints [m_previousPoints.Count - 2] - m_previousPoints [m_previousPoints.Count - 1]).normalized;
			Vector3 t3 = (m_previousPoints [m_previousPoints.Count - 1] - point).normalized; 
			float angle1 = Mathf.Acos(t1.x * t2.x + t1.z * t2.z);
			float temp = t1.x * t2.z - t1.z * t2.x;
			if (temp < 0) {
				angle1 *= -1;
			}
			float angle2 = Mathf.Acos (t2.x * t3.x + t2.z * t3.z);
			temp = t2.x * t3.z - t2.z * t3.x;
			if (temp < 0) {
				angle2 *= -1;
			}

			if( angle1*angle2 < 0 || Mathf.Abs(angle2) > Mathf.PI/2  || Mathf.Abs(angle1) > Mathf.PI/2 ){
				m_previousPoints.Clear();
				m_timePreviousPoints.Clear();
			}
		}
		m_previousPoints.Add(transform.position);
		m_timePreviousPoints.Add(0f);
	}

	void UpdateTimeQueue(){
		for (int i = 0; i < m_timePreviousPoints.Count; i++) {
			m_timePreviousPoints[i] += Time.deltaTime;
		}
		if (m_timePreviousPoints.Count > 0) {
			while (m_timePreviousPoints[0] > m_timeMax) {
				m_previousPoints.RemoveAt (0);
				m_timePreviousPoints.RemoveAt (0);
			}
		}
		//Debug.Log (m_timePreviousPoints.Count);
	}

	bool Circling(){
		
		if (m_previousPoints.Count >= 4) {
			Vector3 Center = Vector3.zero;				 
			for (int i = 0; i < m_previousPoints.Count; i++) {
				Center += m_previousPoints [i];
			}
		
			Center /= m_previousPoints.Count;
			float totalAngle = 0f;
			for (int i = 0; i < m_previousPoints.Count; i++) {
				if ((m_previousPoints [i] - Center).magnitude < m_minRadius
					|| (m_previousPoints [i] - Center).magnitude > m_maxRadius) {
					return false;
				}
				if(i > 0){
					Vector3 t1 = (m_previousPoints[i-1] - Center).normalized;
					Vector3 t2 = (m_previousPoints[i] -  Center).normalized;
					totalAngle += Mathf.Acos(t1.x * t2.x + t1.z * t2.z);
				}
				//Debug.Log( totalAngle);
				if(totalAngle >=  Mathf.PI *2){
					Debug.Log("Circle");
					m_previousPoints.Clear();
					m_timePreviousPoints.Clear();
					return true;
				}
			}	

		}
		return false;

		/*if (m_previousPoints.Count > 4) {
			float[] angles = new float[m_previousPoints.Count - 2];
			float moyAngle = 0f;
			for (int i = m_previousPoints.Count - 3; i >= 0; i--) {
				Vector3 t1 = (m_previousPoints [i + 2] - m_previousPoints [i + 1]).normalized;
				Vector3 t2 = (m_previousPoints [i + 1] - m_previousPoints [i]).normalized;
				angles [i] = t1.x * t2.x + t1.z * t2.z;
				float temp = t1.x * t2.z - t1.z * t2.x;
				if (temp < 0) {
					angles [i] *= -1;
				}
				moyAngle += angles [i];
			}
			int counter = angles.Length - 1;
			counter --;
			while (counter > 0  && angles[counter] * angles[counter+ 1] > 0 
			       && Mathf.Abs(angles[counter]) < (Mathf.PI/4)) {
				counter --;
			}

			//Debug.Log (counter);

			if (counter < (m_previousPoints.Count - 4)) { 

				//Debug.Log(m_previousPoints.Count + " " + counter);
				while ((m_previousPoints[m_previousPoints.Count-1] - m_previousPoints[counter]).magnitude 
				       < m_epsilonDist 
				       && counter < (m_previousPoints.Count-3)) {
					counter ++;
				}
			}



			if ((m_previousPoints.Count-counter) > 5 
			    && (m_previousPoints[m_previousPoints.Count-1] - m_previousPoints[counter]).magnitude < m_epsilonDist ) {

				Debug.Log (m_previousPoints.Count - counter);
				Vector3 tempPoint = new Vector3 ();
				for (int i = counter; i < m_previousPoints.Count; i++) {
					tempPoint += m_previousPoints [i];
				}
				tempPoint /= (m_previousPoints.Count - counter);

				//float radius = 0f;
				float diameter = 0f;
				bool temp = true;
				for (int i = counter; i < m_previousPoints.Count - 1; i++) {
					//radius += (m_previousPoints [i] - tempPoint).magnitude;
					diameter +=  (m_previousPoints [i] - m_previousPoints[i+1]).magnitude ;
					//Debug.Log ("Circle");
				}

				for (int i = counter; i < m_previousPoints.Count - 1; i++) {
					if(((m_previousPoints[counter]-tempPoint).magnitude < diameter * 0.6f)){
						temp = false;
					}
				}

				diameter /= (m_previousPoints.Count - counter - 1);
				//radius /= (m_previousPoints.Count - counter);
				//Debug.Log(diameter);
				/*if (radius >= m_minRadius && radius <= m_maxRadius) {
					//Debug.Log ("Circle");
				}*/
			/*
				if (diameter >= m_minRadius * Mathf.PI*2  && diameter <= m_maxRadius* Mathf.PI*2 && temp) {

					Debug.Log (diameter + " " +"Circle");
					m_previousPoints.Clear();
					m_timePreviousPoints.Clear();
				}
			}

		}*/

		//Debug.Log (moyAngle / angles.Length);
	}

	Vector3 DefineCenter(){
		Vector3 center = new Vector3 ();
		Vector3[] points = m_previousPoints.ToArray ();
		for (int i = 0 ; i < points.Length ; i++) {
			center += points[i];
		}
		//Debug.Log (center);

		return center / points.Length;
	}
}
