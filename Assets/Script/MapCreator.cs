using UnityEngine;
using System.Collections;

public class MapCreator : MonoBehaviour {

	public int m_size;
	public float m_height;
	public float m_width;
	public GameObject m_chunk;
	public GameObject m_avatar;
	public float m_distStayActive;


	// Use this for initialization
	void Start () {
		for (int i = 0; i < m_size; i++){
			for(int j = 0; j< m_size; j++){
				Vector3 pos = new Vector3(-m_size*0.5f*m_width + 0.5f * m_width + i *m_width, 0f , -m_size*0.5f* m_height + 0.5f * m_height + j*m_height);
				GameObject temp = (GameObject) Instantiate(m_chunk, pos, Quaternion.identity);
				temp.GetComponent<PlacementManager>().m_avatar = m_avatar;
				if(pos.magnitude < m_distStayActive){
					temp.GetComponent<PlacementManager>().m_toActivate = true;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
