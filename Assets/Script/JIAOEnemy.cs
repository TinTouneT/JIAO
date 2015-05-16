namespace JIAO.JIAO
{
	using UnityEngine;
	
	public enum EnnemiesStates : int
	{
		idle=1,
		hunt =2,
		swimAround = 3}
	;

	public class JIAOEnemy : JIAOEntity
	{
		public float m_zoneMinLength;
		public float m_zoneMaxLength;

		[Range(0,2)]
		public float m_epsilonDistToPoint;
		private EnnemiesStates m_state;
		private Vector3 m_next_pos;
		public float m_maxTimeTurn;
		private float m_elapsedTime;
		private float m_timeToTurn;
		public float m_speed;
		private GameObject m_avatar;


		//change direction parameter

		public float m_minAngle;
		public float m_maxAngle;

		//private bool m_turning = false;

		public void Start ()
		{
			m_state = EnnemiesStates.swimAround;
			m_next_pos = this.NewPoint (transform.position);
		}

		public void ennemyUpdate ()
		{
			switch (m_state) {
			case (EnnemiesStates.idle):
				idleState ();
				break;
			case (EnnemiesStates.hunt):
				huntState ();
				break;
			case (EnnemiesStates.swimAround):
				swimAroundState ();
				break;
			}
		}

		public EnnemiesStates GetState(){
			return m_state;
		}
		
		private void idleState ()
		{
			
		}
		
		private void huntState ()
		{
			float dist = (m_next_pos - gameObject.transform.position).magnitude;
			if (dist >= m_epsilonDistToPoint) {
				m_elapsedTime += Time.deltaTime;
				Quaternion targetRotation = Quaternion.LookRotation (m_next_pos - transform.position, Vector3.up);
				if(m_timeToTurn!=0 && (Mathf.Exp(m_elapsedTime/m_timeToTurn)-1f)/(Mathf.Exp(1)-1f) <= 1f){
					transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, (Mathf.Exp(m_elapsedTime/m_timeToTurn)-1f)/(Mathf.Exp(1)-1f));
				}
				else{
					//m_turning = false;
				}
				
			} else {
				m_next_pos = NewPointHunt ();
			}		
			transform.position = transform.position + transform.forward * m_speed;
			
		}
		
		private void swimAroundState ()
		{
			float dist = (m_next_pos - gameObject.transform.position).magnitude;
			if (dist >= m_epsilonDistToPoint) {
				m_elapsedTime += Time.deltaTime;
				Quaternion targetRotation = Quaternion.LookRotation (m_next_pos - transform.position, Vector3.up);
				if(m_timeToTurn!=0 && (Mathf.Exp(m_elapsedTime/m_timeToTurn)-1f)/(Mathf.Exp(1)-1f) <= 1f){
					transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, (Mathf.Exp(m_elapsedTime/m_timeToTurn)-1f)/(Mathf.Exp(1)-1f));
				}
				else{
					//m_turning = false;
				}
				
			} else {
				m_next_pos = this.NewPoint (m_next_pos);
			}		
			transform.position = transform.position + transform.forward * m_speed;
		}

		public void ChangeToHuntState(GameObject avatar){
			m_state = EnnemiesStates.hunt;
			m_avatar = avatar;
			NewPointHunt ();
			Debug.Log ("HUNT");
		}

		public void ChangeToSwimState(GameObject avatar){
			m_state = EnnemiesStates.swimAround;
			m_avatar = null;
			Debug.Log ("SWIM");
		}


		public Vector3 NewPoint (Vector3 previousPoint)
		{
			Vector3 temp =  previousPoint;
			bool goodDir;
			do {
				goodDir = true;
				float d = Random.Range (m_zoneMinLength, m_zoneMaxLength);
				float t = 2 * Mathf.PI * Random.Range (0, 1f);
				float x = Mathf.Cos (t) * d;
				float y = Mathf.Sin (t) * d;
				temp = (new Vector3 (x, 0.0f, y)) + transform.position;
				Ray ray = new Ray (transform.position, temp);
				RaycastHit[] hits;
				hits = Physics.RaycastAll (ray, (temp - transform.position).magnitude);
				for (int i =0; i < hits.Length; i++) {
					if (hits [i].transform.tag == "EnvObj") {
						goodDir = false;
					}
				}

			} while(!goodDir);
			Vector3 t1 = transform.forward;
			Vector3 t2 = (temp - transform.position).normalized;
			m_timeToTurn = (Mathf.Acos (t1.x * t2.x + t1.z * t2.z) / Mathf.PI * m_maxTimeTurn);
			m_elapsedTime = 0;
			return temp;
		}

		public Vector3 NewPointHunt(){
			Vector3 temp =  m_avatar.transform.position;
			RaycastHit tempObj;
			Ray ray = new Ray (transform.position, temp);
			bool test = Physics.Raycast (ray,out tempObj, (temp - transform.position).magnitude, LayerMask.NameToLayer("EnvLayer"));
			//m_turning = true;			

			if(test){
				Vector3 t1 = (tempObj.point - transform.position).normalized;
				
				float dir = tempObj.normal.x * t1.z - tempObj.normal.z * t1.x;
				float dist = 0.3f;
				
				if (dir >= 0) {
					temp  = tempObj.point + (new Vector3(-tempObj.normal.z, 0 , tempObj.normal.x)) * dist; 
					
				} else {
					temp  = tempObj.point  + (new Vector3(tempObj.normal.z, 0 , -tempObj.normal.x)) * dist; 
				}
				
				Vector3 t3 = transform.forward;
				Vector3 t4 = (temp - transform.position).normalized;
				m_timeToTurn = (Mathf.Acos (t3.x * t4.x + t3.z * t4.z) / Mathf.PI * m_maxTimeTurn);
				m_elapsedTime = 0;
			}
			return temp;
		}



		public void FixedUpdate ()
		{
			ennemyUpdate ();
		}

		public void changeDirection(GameObject obj, Vector3 colNormal, Vector3 colPoint){
			//m_turning = true;			

			float dist = Random.Range (m_zoneMinLength, m_zoneMaxLength);
			
			Vector3 temp = new Vector3 ();
			Vector3 t1 = (colPoint - transform.position).normalized;

			float dir = colNormal.x * t1.z - colNormal.z * t1.x;
			
			if (dir >= 0) {
				temp  = colPoint + (new Vector3(-colNormal.z, 0 , colNormal.x)) * dist; 
				
			} else {
				temp  = colPoint + (new Vector3(colNormal.z, 0 , -colNormal.x)) * dist; 
			}
			
			Vector3 t3 = transform.forward;
			Vector3 t4 = (temp - transform.position).normalized;
			m_timeToTurn = (Mathf.Acos (t3.x * t4.x + t3.z * t4.z) / Mathf.PI * m_maxTimeTurn);
			m_elapsedTime = 0;
			m_next_pos = temp;
			//Debug.Log (temp);
		}




	}
}


/*
public void changeDirectionBis(GameObject obj, Vector3 colPoint){
	if (m_turning == false) {
		m_turning = true;
		Vector3 t1 = (colPoint - transform.position).normalized;
		Vector3 t2 = (obj.transform.position - transform.position).normalized;
		
		float dir = (t2.x * t1.z) - (t2.z * t1.x);
		float angle = Random.Range (m_minAngle, m_maxAngle);
		float dist = Random.Range (m_zoneMinLength, m_zoneMaxLength);
		
		Vector3 temp = new Vector3 ();
		
		if (dir >= 0) {
			float x = t1.x * Mathf.Cos (angle) - t1.z * Mathf.Sin (angle);
			float z = t1.x * Mathf.Sin (angle) - t1.z * Mathf.Cos (angle);
			temp = transform.position + (new Vector3 (x, 0, z)) * dist;
			
		} else {
			float x = t1.x * Mathf.Cos (-angle) - t1.z * Mathf.Sin (-angle);
			float z = t1.x * Mathf.Sin (-angle) - t1.z * Mathf.Cos (-angle);
			temp = transform.position + (new Vector3 (x, 0, z)) * dist;
		}
		
		Vector3 t3 = transform.forward;
		Vector3 t4 = (temp - transform.position).normalized;
		m_timeToTurn = (Mathf.Acos (t3.x * t4.x + t3.z * t4.z) / Mathf.PI * m_maxTimeTurn);
		m_elapsedTime = 0;
		m_next_pos = temp;
		Debug.Log (temp);
	}
}
*/