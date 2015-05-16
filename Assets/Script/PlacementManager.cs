using UnityEngine;
using System.Collections;

public class PlacementManager : MonoBehaviour {

	public int m_nbEnv;
	public int m_nbVeg;
	public int m_nbEnemy;
	public GameObject[] PrefabEnv;
	public GameObject[] PrefabEnemy;
	public GameObject[] PrefabVeg;
	public float m_minPos;
	public float m_maxPos;
	public GameObject m_avatar;
	public GameObject m_master;
	public bool m_toActivate = false;

	// Use this for initialization
	void Start () {
		m_master = gameObject;
		for (int i = 0; i < m_nbEnv; i++) {
			bool test = false;
			float x = 0f;
			float y = 0f;
			int indexEnv =  Random.Range(0,PrefabEnv.Length);
			while(!test){
				test = true;
				x = Random.Range(m_minPos, m_maxPos);
				y = Random.Range(m_minPos, m_maxPos);
				Collider[] hits = Physics.OverlapSphere(new Vector3(x, 0f, y)+ transform.position, PrefabEnv[indexEnv].transform.localScale.x);
				foreach(Collider hit in hits){
					if(hit.transform.tag == "EnvObj" || hit.transform.tag == "Ennemy"|| hit.transform.tag == "Bonus"){
						test = false;
					}
				}
			}
			
			
			GameObject temp = (GameObject) Instantiate(PrefabEnv[indexEnv],new Vector3(x,0.1f,y)+ transform.position,PrefabEnv[indexEnv].transform.rotation);
			temp.transform.parent = m_master.transform;
			//temp.GetComponent<EnvLifeManager>().enabled = false;
		} 

		for (int i = 0; i < m_nbVeg; i++) {
			bool test = false;
			float x = 0f;
			float y = 0f;
			int indexVeg =  Random.Range(0,PrefabVeg.Length);
			while(!test){
				test = true;
				x = Random.Range(m_minPos, m_maxPos);
				y = Random.Range(m_minPos, m_maxPos);
				Collider[] hits = Physics.OverlapSphere(new Vector3(x, 0f, y)+ transform.position, PrefabVeg[indexVeg].transform.localScale.x*1.5f);
				foreach(Collider hit in hits){
					if(hit.transform.tag == "EnvObj" || hit.transform.tag == "Ennemy"|| hit.transform.tag == "Bonus"|| hit.transform.tag == "Veg"){
						test = false;
					}
				}
			}
			
			
			GameObject temp = (GameObject) Instantiate(PrefabVeg[indexVeg],new Vector3(x,0.1f,y)+ transform.position,PrefabVeg[indexVeg].transform.rotation);
			temp.transform.parent = m_master.transform;
			//temp.GetComponent<EnvLifeManager>().enabled = false;
		} 
		for (int i = 0; i < m_nbEnemy; i++) {
			bool test = false;
			float x = 0f;
			float y = 0f;
			int indexEnemy =  Random.Range(0,PrefabEnemy.Length);
			while(!test){
				test = true;
				x = Random.Range(m_minPos, m_maxPos);
				y = Random.Range(m_minPos, m_maxPos);
				Collider[] hits = Physics.OverlapSphere(new Vector3(x, 0f, y)+transform.position , PrefabEnemy[indexEnemy].transform.localScale.x*1.5f);
				foreach(Collider hit in hits){
					if(hit.transform.tag == "EnvObj"|| hit.transform.tag == "Bonus"){
						test = false;
					}
				}
			}
			
			
			GameObject temp = (GameObject) Instantiate(PrefabEnemy[indexEnemy],new Vector3(x,0.1f,y)+ transform.position,PrefabEnemy[indexEnemy].transform.rotation);
			temp.transform.parent = m_master.transform;
			temp.GetComponent<JIAO.JIAO.JIAOEnemy2>().m_avatar = m_avatar;
			//temp.GetComponent<JIAO.JIAO.JIAOEnemy2>().enabled =false;
			
		} 


		if(!m_toActivate){
			for (int i = 0; i < this.transform.childCount; i++) {
				this.transform.GetChild(i).gameObject.SetActive(false);
			}
		}

		//this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col){
		for (int i = 0; i < this.transform.childCount; i++) {
			this.transform.GetChild(i).gameObject.SetActive(true);
		}
		//this.enabled = true;*/
	}

	void OnCollisionExit(Collision col){
		for (int i = 0; i < this.transform.childCount; i++) {
			this.transform.GetChild(i).gameObject.SetActive(false);
		}
		//this.enabled =  false;*/
	}
}
