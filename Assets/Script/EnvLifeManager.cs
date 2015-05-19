using UnityEngine;
using System.Collections;

public class EnvLifeManager : MonoBehaviour {

	public float m_maxLife;
	public float m_life;
	public float m_lifeRegenPerSecond;
	public float m_lowAlpha;


	// Use this for initialization
	void Start () {
		m_life = m_maxLife;
	}
	
	// Update is called once per frame
	void Update () {
		Color temp = gameObject.GetComponent<MeshRenderer> ().material.color;
		temp.a = m_lowAlpha + (1 - m_lowAlpha) * (m_life / m_maxLife);
		gameObject.GetComponent<MeshRenderer> ().material.color = temp;
		if (m_life <= 0f) {
			KillEnv();
		}
	}

	public float LifeUpdate(float time){
		m_life -= time * m_lifeRegenPerSecond;
		return time * m_lifeRegenPerSecond;
	}

	public void KillEnv(){
		transform.parent.GetComponent<PlacementManager>().RemoveObjectFromList(gameObject);
		Destroy (gameObject);
	}
}
