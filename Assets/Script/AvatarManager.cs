using UnityEngine;
using System.Collections;

public class AvatarManager : MonoBehaviour {
	/*
	public float m_maxDistTocenter;
	public CameraMovement m_cameraMovement;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float Actual
		if ((transform.position - m_goalPoint).magnitude > 0) {
			
			m_elapsedTime += Time.deltaTime;
			Quaternion targetRotation = Quaternion.LookRotation (m_goalPoint - transform.position, Vector3.up);
			if (m_timeToTurn != 0 && (Mathf.Exp (m_elapsedTime / m_timeToTurn) - 1f) / (Mathf.Exp (1) - 1f) <= 1f) {
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, (Mathf.Exp (m_elapsedTime / m_timeToTurn) - 1f) / (Mathf.Exp (1) - 1f));
			} else {
				float tempAngle2 = Vector3.Angle (transform.forward, (m_goalPoint - transform.position));
				m_timeToTurn = (tempAngle2 / 180f) * m_maxTimeToTurn;
				m_elapsedTime = 0;
			}
			Vector3 temp = transform.forward;
			if (m_env.Count != 0) {
				foreach(GameObject obj in m_env){
					if (obj != null) {
						Vector3 t1 = (m_goalPoint - transform.position);
						Vector3 t2 = (obj.transform.position - transform.position);
						float angle = Vector3.Angle (t1, t2);
						float ratioDist = 1 - (((obj.transform.position - transform.position).magnitude - obj.transform.localScale.x) /
						                       ((obj.transform.localScale.x * obj.GetComponent<SphereCollider> ().radius) - obj.transform.localScale.x));
						//Debug.Log(ratioDist);
						if (angle < 90f && ratioDist >= 0) {

							Vector3 tempAngle;
							if ((t1.x * t2.z - t1.z * t2.x) < 0f) {
								tempAngle = Vector3.Cross (Vector3.up, (transform.position - obj.transform.position).normalized).normalized;
							} else {
								tempAngle = Vector3.Cross ((transform.position - obj.transform.position).normalized, Vector3.up).normalized;
							}
							temp += m_repulsStrength * tempAngle * Mathf.Pow (ratioDist, 2);
						}
					}
					else{
						m_env.Remove(obj);
					}
				}
			}
			gameObject.transform.LookAt (transform.position + temp.normalized * m_speed * Time.deltaTime);
			transform.position += temp.normalized * m_speed * Time.deltaTime;
		} 
	}


	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "EnvObj") {
			m_env.Add (col.gameObject);
		}
		if (col.gameObject.tag == "Avatar") {
			m_contactAvatar = true;
		}
	}
	
	void OnCollisionExit (Collision col)
	{
		if (col.gameObject.tag == "EnvObj") {
			int ind = m_env.IndexOf (col.gameObject);
			if (ind != -1) {
				m_env.RemoveAt (ind);
			}
		}
		if (col.gameObject.tag == "Avatar") {
			m_contactAvatar = false;
		}
	}*/

}
