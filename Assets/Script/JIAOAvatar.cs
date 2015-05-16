namespace JIAO.JIAO
{
	using UnityEngine;
	
	public class JIAOAvatar : JIAOEntity
	{
		public Transform m_pos;
		public Camera m_maincam;
		public Vector3 m_screenPosition;
		public JIAOInputController m_inputController;
		public JIAOLevelController m_levelController;
		public float m_speed = 30f;
		public Rect m_rec;
		private Vector3 m_center;
		private Vector3 m_speed_direction = new Vector3 (1.0f, 0.0f, 1.0f);
		public bool inputIn = true;
		public GameObject sonar;
		public GameObject m_sonarTarget;
		private Vector3 m_sonarTargetVector = new Vector3 ();
		private Vector3 m_startPoint = new Vector3 (0f, 0f, 148f);
		private Vector3 m_positionFromCenter = new Vector3 (0f, 0f, 148f);
		private float m_orthographicSize;
		private float m_aspect;
		public GameObject m_mainCamera;
		public Vector3 m_mainCamPositionVector;
		public Vector3 m_direction = new Vector3 ();
		
		void Start ()
		{
			m_pos = this.transform;
			m_rec = new Rect ();
			m_center = new Vector3 (0f, 0f, 150f);
			//SetSonarTarget();
			m_orthographicSize = Camera.main.orthographicSize;
			m_aspect = Camera.main.aspect;
		}
		
		void SetSonarTarget ()
		{
			GameObject[] targetlist = GameObject.FindGameObjectsWithTag ("Bonus");
			m_sonarTarget = targetlist [Random.Range (0, targetlist.Length - 1)];
		}
		
		
		void FixedUpdate ()
		{
				

			m_screenPosition = m_maincam.WorldToScreenPoint (transform.position);
				
			m_direction.x = (Input.mousePosition.x - m_screenPosition.x) / Screen.width;
			m_direction.z = (Input.mousePosition.y - m_screenPosition.y) / Screen.height;

			float distx = Mathf.Abs (Input.mousePosition.x) / (m_orthographicSize);
				
			if (Vector3.Dot (m_positionFromCenter, transform.forward) > 0 && distx > 0.6f)
				m_speed_direction.x = (m_direction.x) * m_speed * Time.deltaTime;
			else
				m_speed_direction.x = (m_direction.x) * m_speed * Time.deltaTime * (1f - distx);
				
			float disty = Mathf.Abs (m_positionFromCenter.z) / (m_orthographicSize / m_aspect);
				
			if (Vector3.Dot (m_positionFromCenter, transform.forward) > 0 && disty > 1.0f)
				m_speed_direction.z = (m_direction.z) * m_speed * Time.deltaTime;
			else
				m_speed_direction.z = (m_direction.z) * m_speed * Time.deltaTime * (1.0f - disty);


			transform.position += m_speed_direction;

			m_mainCamPositionVector = m_maincam.WorldToScreenPoint(m_maincam.transform.position);
			m_mainCamPositionVector.y = 0;

			Vector3 diff = m_screenPosition - m_mainCamPositionVector;
			if((m_speed_direction.x < 0 && (m_screenPosition.x < (Screen.width * 0.3f))) || (m_speed_direction.x > 0 && (m_screenPosition.x > (Screen.width * 0.7f))))
				m_mainCamera.transform.position += m_speed_direction;
			if((m_speed_direction.z < 0 && (m_screenPosition.y < (Screen.height*0.2f))) || (m_speed_direction.z > 0 && (m_screenPosition.y > (Screen.height*0.8f))))
				m_mainCamera.transform.position += m_speed_direction;
		}
	}
}
