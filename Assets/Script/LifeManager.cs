using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifeManager : MonoBehaviour {

	public List<GameObject> m_enemies = new List<GameObject>();
	public List<GameObject> m_env = new List<GameObject> ();
	public float m_maxLife;
	public float m_life;
	public float m_lowAlpha;

	// Use this for initialization
	void Start () {
		m_life = m_maxLife;
	}

	// Update is called once per frame
	void Update () {

		if (m_enemies.Count != 0) {
			int i = m_enemies.Count - 1;
			while (i>=0) {
				GameObject obj = m_enemies [i];
				if (obj != null) {
					m_life -= Time.deltaTime * obj.GetComponent<JIAO.JIAO.JIAOEnemy2> ().m_damage;
				} else {
					m_env.Remove (obj);
				}
				i--;
			}
		}

		if (m_env.Count != 0) {
			int  i = m_env.Count-1;
			while(i>=0){
				GameObject obj = m_env[i];
				if(obj!= null){
					if(m_life < m_maxLife){
						m_life += obj.GetComponent<EnvLifeManager>().LifeUpdate(Time.deltaTime);
						if(m_life > m_maxLife){
							m_life  = m_maxLife;
						}
					}
				}
				else{
					m_env.Remove(obj);
				}
				i--;
			}
		}

		Color temp = gameObject.GetComponent<MeshRenderer> ().material.color;
		temp.a = m_lowAlpha + (1 - m_lowAlpha) * (m_life / m_maxLife);
		gameObject.GetComponent<MeshRenderer> ().material.color = temp;


		if (m_life <= 0) {
			KillAvatar();
		}

	}

	void OnCollisionEnter(Collision col){
		if (col.transform.tag == "Ennemy") {
			m_enemies.Add( col.gameObject);
		}
		if (col.transform.tag == "Veg") {
			m_env.Add( col.gameObject);
		}
	}

	void OnCollisionExit(Collision col){
		if (col.transform.tag == "Ennemy") {
			m_enemies.Remove(col.gameObject);
		}
		if (col.transform.tag == "Veg") {
			m_env.Remove( col.gameObject);
		}
	}
	public void KillAvatar(){
		Debug.Log ("KillAvatar");
	}

}
