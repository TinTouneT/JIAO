using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BonusInteractionManager: MonoBehaviour {



	private int m_phase;
	private Vector3 initialVector;
	private Vector3 currentVector;
	private Vector3 maxVector;
	private int direction = 0;
	public GameObject m_nextBonus1;
	public GameObject m_nextBonus2;
	public GameObject m_otherBonus;

	[Range(0.0f,Mathf.PI/4)]
	public float m_epsilon ;

	// Use this for initialization
	void Start () {
		m_phase = 0;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void UpdatePhase(int newPhase){
		m_phase = newPhase;
	}

	void FixedUpdate(){
		float theta = Mathf.Acos(currentVector.x * initialVector.x + currentVector.z * initialVector.z);
		if(initialVector == maxVector && currentVector != maxVector && direction == 0){
			maxVector = currentVector;
			float temp = initialVector.x * currentVector.z - initialVector.z * currentVector.x;
			if(temp > 0){
				direction = 1;
			}
			else{
				direction = -1;
			}
		}
		else{
			float thetaMax =  Mathf.Acos(maxVector.x * initialVector.x + maxVector.z * initialVector.z);
			float temp = initialVector.x * currentVector.z - initialVector.z * currentVector.x;
			float tempMax = initialVector.x * maxVector.z - initialVector.z * maxVector.x;
			
			if(temp < 0){
				theta = 2*Mathf.PI - theta ;
			}
			if (tempMax < 0){
				thetaMax = 2*Mathf.PI -thetaMax;
			}
			else if (tempMax == 0 && direction == -1){
				thetaMax = 2*Mathf.PI;
			}


			if(Mathf.Abs(theta-thetaMax) > Mathf.PI){
				if(thetaMax > Mathf.PI) theta += 2*Mathf.PI;
				else if(thetaMax < Mathf.PI) theta -= 2*Mathf.PI;
			}
			
			//Debug.Log(currentVector);
			//Debug.Log(initialVector);
			//Debug.Log(temp);
			
			if(direction == 1){
			
				if (theta > thetaMax && (theta - thetaMax) <Mathf.PI){
					maxVector = currentVector;
					Debug.Log((theta*360/(2*Mathf.PI)).ToString() + "  " +  (thetaMax*360/(2*Mathf.PI)).ToString() + " " + direction.ToString());
				}
				else if(theta < thetaMax && (thetaMax - theta) > m_epsilon && (thetaMax - theta) < Mathf.PI ){
					Debug.Log("ChangeDirection");
					direction = 0;
					initialVector = maxVector;
					maxVector = currentVector;
					temp = initialVector.x * currentVector.z - initialVector.z * currentVector.x;
					if(temp > 0){
						direction = 1;
					}
					else{
						direction = -1;
					}
				}
				if (theta > 2*Mathf.PI){
					Debug.Log("Circle complete");
					ActionAfterCircled();
				}
			}
			else if(direction == -1){
				

				if (theta < thetaMax && (thetaMax - theta) < Mathf.PI){
					maxVector = currentVector;
					Debug.Log((theta*360/(2*Mathf.PI)).ToString() + "  " +  (thetaMax*360/(2*Mathf.PI)).ToString()+ " " + direction.ToString());
				}
				else if(theta > thetaMax && (theta - thetaMax) > m_epsilon &&  (theta - thetaMax) < Mathf.PI){
					Debug.Log("ChangeDirection");
					direction = 0;
					initialVector = maxVector;
					maxVector = currentVector;
					temp = initialVector.x * currentVector.z - initialVector.z * currentVector.x;
					if(temp > 0){
						direction = 1;
					}
					else{
						direction = -1;
					}
				}
				if (theta < 0){
					Debug.Log("Circle complete");
					ActionAfterCircled();
				}
			}

		}
		

			

	}
	void ActionAfterCircled(){
		if (m_otherBonus != null) {
			m_nextBonus1.SetActive (true);
			if (m_nextBonus2 != null) {
				m_nextBonus2.SetActive (true);
			}
			Destroy (m_otherBonus);
			Destroy (gameObject);
		} 
		else {
			// Anim de fin
		}
	}

	void OnCollisionEnter(Collision col){

		if (col.gameObject.tag == "Avatar") {
			Debug.Log("In");
			gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(0,255,0,0.15f));
			initialVector = new Vector3(col.transform.position.x -transform.position.x, 0 , col.transform.position.z - transform.position.z);
			initialVector.Normalize();
			currentVector = initialVector;
			maxVector = initialVector;
		}

	}

	void OnCollisionStay(Collision col){
		if (col.gameObject.tag == "Avatar") {
			currentVector = new Vector3(col.transform.position.x - transform.position.x,0.0f, col.transform.position.z - transform.position.z);
			currentVector.Normalize();
		}
	}

	void OnCollisionExit(Collision col){

		if (col.gameObject.tag == "Avatar") {
			Debug.Log("Out");
			gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(255,0,0,0.15f));
			initialVector =Vector3.zero;
			currentVector = Vector3.zero;
			maxVector = Vector3.zero	;
		}
		
	}

}
