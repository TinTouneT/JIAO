using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlacementManager : MonoBehaviour {

	public int m_nbEnv;
	public int m_nbVeg;
	public int[] m_nbEnemy;
	public GameObject[] PrefabEnv;
	public GameObject[] PrefabEnemy;
	public GameObject[] PrefabVeg;
	public float m_minPos;
	public float m_maxPos;
	public GameObject m_avatar;
	public bool m_toActivate = false;
	public float m_positionXMin;
	public float m_positionXMax;
	public float m_positionYMin;
	public float m_positionYMax;

	private List<GameObject> m_gameObjectOnChunk = new List<GameObject>();

	// Use this for initialization
	void Start () {

		JIAO.JIAO.JIAOMainController.m_maincontroller.GenerateEnemies += this.OnGenerateEnemies;

		for (int i = 0; i < m_nbEnv; i++) {
			bool test = false;
			float x = 0f;
			float y = 0f;
			int indexEnv =  UnityEngine.Random.Range(0,PrefabEnv.Length);
			while(!test){
				test = true;
				x = UnityEngine.Random.Range(m_minPos, m_maxPos);
				y = UnityEngine.Random.Range(m_minPos, m_maxPos);
				Collider[] hits = Physics.OverlapSphere(new Vector3(x, 0f, y)+ transform.position, PrefabEnv[indexEnv].transform.localScale.x);
				foreach(Collider hit in hits){
					if(hit.transform.tag == "EnvObj" || hit.transform.tag == "Ennemy"|| hit.transform.tag == "Bonus"){
						test = false;
					}
				}
			}
			
			
			GameObject temp = (GameObject) Instantiate(PrefabEnv[indexEnv],new Vector3(x,0.1f,y)+ transform.position,PrefabEnv[indexEnv].transform.rotation);
			temp.transform.parent = transform;
			m_gameObjectOnChunk.Add(temp);
			//temp.GetComponent<EnvLifeManager>().enabled = false;
		} 

		for (int i = 0; i < m_nbVeg; i++) {
			bool test = false;
			float x = 0f;
			float y = 0f;
			int indexVeg =  UnityEngine.Random.Range(0,PrefabVeg.Length);
			while(!test){
				test = true;
				x = UnityEngine.Random.Range(m_minPos, m_maxPos);
				y = UnityEngine.Random.Range(m_minPos, m_maxPos);
				Collider[] hits = Physics.OverlapSphere(new Vector3(x, 0f, y)+ transform.position, PrefabVeg[indexVeg].transform.localScale.x*1.2f);
				foreach(Collider hit in hits){
					if(hit.transform.tag == "EnvObj" || hit.transform.tag == "Ennemy"|| hit.transform.tag == "Bonus"|| hit.transform.tag == "Veg"){
						test = false;
					}
				}
			}
			
			
			GameObject temp = (GameObject) Instantiate(PrefabVeg[indexVeg],new Vector3(x,0.1f,y)+ transform.position,PrefabVeg[indexVeg].transform.rotation);
			temp.transform.parent = transform;
			m_gameObjectOnChunk.Add(temp);
			//temp.GetComponent<EnvLifeManager>().enabled = false;
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


	public void OnGenerateEnemies(object source, EventArgs e){
		GenerateEnemies(JIAO.JIAO.JIAOMainController.m_maincontroller.m_state);
	}

	void GenerateEnemies(int stateNumber){
		for (int i = 0; i < m_nbEnemy[stateNumber-1]; i++) {
			bool test = false;
			float x = 0f;
			float y = 0f;
			int indexEnemy =  UnityEngine.Random.Range(0,PrefabEnemy.Length);
			while(!test){
				test = true;
				x = UnityEngine.Random.Range(m_minPos, m_maxPos);
				y = UnityEngine.Random.Range(m_minPos, m_maxPos);

				foreach(GameObject obj in m_gameObjectOnChunk){
					if(obj.transform.tag == "EnvObj"|| obj.transform.tag == "Bonus"){
						if((obj.transform.position - (new Vector3(x,0.1f,y)+ transform.position)).magnitude <
						   obj.transform.localScale.x * obj.GetComponent<SphereCollider>().radius)
						{
							test = false;
						}
					}
				}
				if(x + transform.position.x > m_positionXMax || x+transform.position.x < m_positionXMin 
				   || y +transform.position.z > m_positionYMax || y+transform.position.z < m_positionYMin)
				{
					test = false;
				}
			}

			GameObject temp = (GameObject) Instantiate(PrefabEnemy[indexEnemy],new Vector3(x,0.1f,y)+ transform.position,PrefabEnemy[indexEnemy].transform.rotation);
			temp.transform.parent = transform;
			temp.GetComponent<JIAO.JIAO.JIAOEnemy2>().m_avatar = m_avatar;
			if(!m_toActivate){
				temp.SetActive(false);
			}
			m_gameObjectOnChunk.Add(temp);
			//temp.GetComponent<JIAO.JIAO.JIAOEnemy2>().enabled =false;
		} 
	}

	void OnCollisionEnter(Collision col){
		m_toActivate = true;
		for (int i = 0; i < this.transform.childCount; i++) {
			this.transform.GetChild(i).gameObject.SetActive(true);
		}
		//this.enabled = true;*/
	}

	void OnCollisionExit(Collision col){
		m_toActivate = false;
		for (int i = 0; i < this.transform.childCount; i++) {
			this.transform.GetChild(i).gameObject.SetActive(false);
		}
		//this.enabled =  false;*/
	}

	public void RemoveObjectFromList(GameObject obj){
		m_gameObjectOnChunk.Remove(obj);
	}
}
