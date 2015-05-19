namespace JIAO.JIAO
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	
	public class JIAOEnemy2 : JIAOEntity
	{
		
		private EnnemiesStates m_state;
		public List<GameObject> m_env = new List<GameObject> ();
		public float m_epsGoalPoint;
		public float m_speed;
		public Vector3 m_goalPoint;
		public float m_repulsStrength;
		public Vector3 m_startPoint;
		public float m_zoneRadius;
		public float m_timeToTurn;
		public float m_maxTimeToTurn;
		public float m_maxTimeToTurnHunt;
		private float m_elapsedTime;
		public float m_minimumDist;
		public GameObject m_avatar;
		public float m_distAvatar;
		private bool m_contactAvatar;
		
		//damage management
		public float m_damage;
		
		
		public void Start ()
		{
			m_state = EnnemiesStates.swimAround;
			m_startPoint = transform.position;
			m_goalPoint = transform.position;
			m_contactAvatar = false;
			//NewPoint();
		}
		
		public void FixedUpdate ()
		{
			if ((transform.position - m_avatar.transform.position).magnitude <= m_distAvatar && (m_startPoint-m_avatar.transform.position).magnitude < m_zoneRadius) {
				ChangeState (EnnemiesStates.hunt);
				NewPointHunt ();
			} else {
				ChangeState (EnnemiesStates.swimAround);
			}
			ennemyUpdate ();
		}
		
		public void ennemyUpdate ()
		{
			//Debug.Log(m_state);
			switch (m_state) {
			case (EnnemiesStates.hunt):
				huntState ();
				break;
			case (EnnemiesStates.swimAround):
				swimAroundState ();
				break;
			}
		}
		
		public void ChangeState (EnnemiesStates state)
		{
			m_state = state; 
		}
		
		public EnnemiesStates GetState ()
		{
			return m_state;
		}
		
		private void huntState ()
		{
			if (m_goalPoint != m_avatar.transform.position) {
				NewPointHunt ();
			}
			
			if (!m_contactAvatar) {
				
				m_elapsedTime += Time.deltaTime;
				Quaternion targetRotation = Quaternion.LookRotation (m_goalPoint - transform.position, Vector3.up);
				if (m_timeToTurn != 0) {
					transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, m_elapsedTime / m_timeToTurn);
				} else {
					float tempAngle2 = Vector3.Angle (transform.forward, (m_goalPoint - transform.position));
					m_timeToTurn = (tempAngle2 / 180f) * m_maxTimeToTurn;
					m_elapsedTime = 0;
				}
				Vector3 temp = transform.forward;
				if (m_env.Count != 0) {
					foreach( GameObject obj in m_env){
						if (obj != null) {
							Vector3 t1 = (m_goalPoint - transform.position);
							Vector3 t2 = (obj.transform.position - transform.position);
							float angle = Vector3.Angle (t1, t2);
							float ratioDist = 1 - (((obj.transform.position - transform.position).magnitude - obj.transform.localScale.x) /
							                       ((obj.transform.localScale.x * obj.GetComponent<SphereCollider> ().radius) - obj.transform.localScale.x));
							//Debug.Log (ratioDist);
							//Debug.Log(ratioDist);
							if (angle < 90f && ratioDist >= 0) { 
								/*((transform.position - obj.transform.position).magnitude - obj.transform.localScale.x)
								/ ((obj.transform.localScale.x * obj.GetComponent<SphereCollider>().radius)*2  -obj.transform.localScale.x) */
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
		
		
		
		private void swimAroundState ()
		{
			
			if ((transform.position - m_goalPoint).magnitude > m_epsGoalPoint) {
				
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
								/*((transform.position - obj.transform.position).magnitude - obj.transform.localScale.x)
								/ ((obj.transform.localScale.x * obj.GetComponent<SphereCollider>().radius)*2  -obj.transform.localScale.x) */
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
				
			} else {
				NewPoint ();
			}
		}
		
		public void NewPointHunt ()
		{
			m_goalPoint = new Vector3 (m_avatar.transform.position.x, m_startPoint.y, m_avatar.transform.position.z);
			float tempAngle = Vector3.Angle (transform.forward, (m_goalPoint - transform.position).normalized);
			m_timeToTurn = (tempAngle / 180f) * m_maxTimeToTurnHunt;
			m_elapsedTime = 0;
		}
		
		public void NewPoint ()
		{
			bool test = false;
			while (!test) {
				Vector3 temp = new Vector3 ();
				float dist = UnityEngine.Random.Range (0f, m_zoneRadius);
				//Debug.Log(dist);
				float angle = UnityEngine.Random.Range (-Mathf.PI, Mathf.PI);
				float x = m_startPoint.x + Mathf.Cos (angle) * dist;
				float y = m_startPoint.z + Mathf.Sin (angle) * dist;
				
				temp = new Vector3 (x, m_startPoint.y, y);
				test = true;
				while ((transform.position - temp).magnitude < m_minimumDist) {
					dist = UnityEngine.Random.Range (0f, m_zoneRadius);
					//Debug.Log(dist);
					angle = UnityEngine.Random.Range (-Mathf.PI, Mathf.PI);
					x = m_startPoint.x + Mathf.Cos (angle) * dist;
					y = m_startPoint.z + Mathf.Sin (angle) * dist;
					
					temp = new Vector3 (x, m_startPoint.y, y);
				}
				//Debug.Log(temp);
				
				Collider[] hits = Physics.OverlapSphere (temp, 1);
				foreach (Collider hit in hits) {
					if (hit.transform.tag == "EnvObj") {
						test = false;
					}
				}
				
				if (test) {
					m_goalPoint = temp;
					float tempAngle = Vector3.Angle (transform.forward, (m_goalPoint - transform.position));
					m_timeToTurn = (tempAngle / 180f) * m_maxTimeToTurn;
					m_elapsedTime = 0;
				}
			}
			
			
		}

		public void Kill(){
			transform.parent.GetComponent<PlacementManager>().RemoveObjectFromList(gameObject);
			//Put AnimManager here
			//
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
		}
		
	}
}

