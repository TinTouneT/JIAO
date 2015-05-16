namespace JIAO.JIAO
{
	using UnityEngine;

		public class JIAOEntity : MonoBehaviour
	    {
	        private Color color;
		    public Vector3 m_velocity = new Vector3(0f,0f,0f);
			Material mat;
			public Transform m_previousPosition;

			public void Start() {
				mat = GetComponent<Renderer>().material;
			}

	    }
}
