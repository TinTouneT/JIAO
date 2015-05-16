namespace JIAO.JIAO
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
    public abstract class JIAOLevel
    {
		public GameObject chunk;
		public int width;
		public int height;
	    public GameObject m_maincontroller;
		private GameObject m_row;
		public static Dictionary<string,int> m_s_instances = new Dictionary<string, int>();
		public abstract string LevelName { get; }

		public List<JIAOChunk> listchunk = new List<JIAOChunk>();

		public JIAOLevel(GameObject o) {
			chunk = o;
			GameObject go = GameObject.Find (LevelName+"0");
			if (go != null)
				m_s_instances [LevelName]++;
			else
				m_s_instances [LevelName] = 0;
			m_maincontroller = new GameObject (LevelName+m_s_instances [LevelName]);
		}

		public void addLevel() {
			int h = 0;

			for (int i = 0; i < listchunk.Count; i++) {
				if(i%this.width == 0) {
					m_row = new GameObject();
					m_row.name = "Row"+h;
					m_row.transform.position = new Vector3(0f,0f,h*100f);
					h++;
				}
				listchunk[i].chunk.transform.position = new Vector3((i%width)*100f,0f,h*100f);
				listchunk[i].chunk.transform.parent = m_row.transform;
				//GameObject target = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				/*target.transform.parent = listchunk[i].chunk.transform;
				target.transform.localPosition = new Vector3(0f,0f,0f);
				target.tag = "Bonus";
				target.transform.parent = m_maincontroller.transform;
				target.transform.localScale = new Vector3(3f,3f,3f);*/
				m_row.transform.parent = m_maincontroller.transform;
			}

			m_maincontroller.transform.Translate(new Vector3((-100f*width)/2f+50f,0f,0f));
		}
		
    }
}
