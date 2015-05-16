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


		}
    }
}
