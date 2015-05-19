namespace JIAO.JIAO
{
	using UnityEngine;

    public class Player : JIAOEntity
    {
		public Transform m_pos;
		public Camera m_maincam;
		public Vector3 m_screenPosition;
		public JIAOInputController m_inputController;
		public JIAOLevelController m_levelController;
		public float m_speed = 1.0f;
		public Rect m_rec;
		private Vector3 m_center;
		private Vector3 m_speed_direction = new Vector3(1.0f,0.0f,1.0f);
		public bool inputIn = true;
		public GameObject sonar;
		public GameObject m_sonarTarget;
		private Vector3 m_sonarTargetVector = new Vector3();
		private Vector3 m_startPoint = new Vector3(0f,0f,148f);
		private Vector3 m_positionFromCenter = new Vector3(0f,0f,148f);
		private float m_orthographicSize;
		private float m_aspect;

		void Start() {
			m_pos = this.transform;
			m_rec = new Rect();
			m_center = new Vector3 (0f, 0f, 150f);
			//SetSonarTarget();
			m_orthographicSize = Camera.main.orthographicSize;
			m_aspect = Camera.main.aspect;
		}

		void SetSonarTarget() {
			GameObject[] targetlist = GameObject.FindGameObjectsWithTag("Bonus");
			m_sonarTarget = targetlist[Random.Range(0,targetlist.Length -1)];
		}


		void FixedUpdate() {

			m_screenPosition = m_maincam.WorldToScreenPoint(m_pos.position);
			m_positionFromCenter.x = transform.position.x;
			m_positionFromCenter.y = 0f;
			m_positionFromCenter.z = transform.position.z-154f;
			/*m_sonarTargetVector.x = m_sonarTarget.transform.position.x;
			if(m_sonarTargetVector.x - transform.position.x > 2000f)
				m_sonarTargetVector.x = -m_sonarTargetVector.x;
			if(m_sonarTargetVector.z - transform.position.z > 2000f)
				m_sonarTargetVector.z = -m_sonarTargetVector.z;
			m_sonarTargetVector.y = 0f;
			m_sonarTargetVector.z = m_sonarTarget.transform.position.z;
			sonar.transform.LookAt(m_sonarTargetVector);*/

		
			if (inputIn ) {
				float distx =  Mathf.Abs (m_pos.position.x) /(m_orthographicSize);
				transform.forward = m_inputController.m_direction.normalized;
				if(Vector3.Dot(m_positionFromCenter,transform.forward) > 0 && distx > 0.6f)
					m_speed_direction.x = (m_inputController.m_direction.x ) * m_speed * (1f-distx);
				else
					m_speed_direction.x = (m_inputController.m_direction.x ) * m_speed;

				float disty = Mathf.Abs (m_positionFromCenter.z)/(m_orthographicSize/m_aspect);
				 if(Vector3.Dot(m_positionFromCenter,transform.forward) > 0 && disty > 1.0f)
					m_speed_direction.z = (m_inputController.m_direction.z ) * m_speed * (1.0f-disty);
				else
					m_speed_direction.z = (m_inputController.m_direction.z ) * m_speed;
				m_pos.position += m_speed_direction;


			}
			else {
				if((m_center - transform.position).magnitude >0.9f)
					m_pos.position += m_levelController.m_p_lerpTargetVector * m_levelController.lerp;
			}

		}
    }
}
