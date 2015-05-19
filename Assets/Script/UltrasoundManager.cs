using UnityEngine;
using System.Collections;

public class UltrasoundManager : MonoBehaviour {

	public GameObject Bonus1;
	public GameObject Bonus2;
	public JIAO.JIAO.JIAOAvatar m_avatar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	//	Debug.Log(PositionBonus(Bonus1));
	}

	void UltraSoundFeedback(){
		if(Bonus1 != null){
		//	PositionBonus(Bonus1);


		}
	}

	Vector2 PositionBonus(GameObject Bonus){
		Vector3 PositionBonus = Bonus.transform.position;
		if(Mathf.Abs(PositionBonus.x- m_avatar.gameObject.transform.position.x) > m_avatar.m_posXMax)
		{
			if((PositionBonus.x-m_avatar.gameObject.transform.position.x) > 0){
				PositionBonus.x -= 2f*m_avatar.m_posXMax;
			}
			else{
				PositionBonus.x += 2f*m_avatar.m_posXMax;
			}
		}
		if(Mathf.Abs(PositionBonus.z- m_avatar.gameObject.transform.position.z) > m_avatar.m_posYMax)
		{
			if((PositionBonus.z-m_avatar.gameObject.transform.position.z) > 0){
				PositionBonus.z -= 2f*m_avatar.m_posYMax;
			}
			else{
				PositionBonus.z+= 2f*m_avatar.m_posYMax;
			}
		}
		
		Vector3 direction = m_avatar.transform.position - PositionBonus;
		direction.y = 0;
		direction = direction.normalized;
		Vector2 direction2 = new Vector2(direction.x, direction.z);
		direction2 = direction2.normalized;
		Vector2 feedbackPosition =Vector2.zero;
		
		float screenAngle = Mathf.Atan(Screen.height/Screen.width);
		float bonusangle = Vector2.Angle(Vector2.right, direction2);
		if( bonusangle < screenAngle){
			
			feedbackPosition.x = Screen.width/2f;
			feedbackPosition.y = Mathf.Tan(bonusangle) * feedbackPosition.x;
			if(direction2.y < 0){
				feedbackPosition.y *= -1;
			}
		}
		else if(bonusangle > 180f - screenAngle){
			feedbackPosition.x = - Screen.width/2;
			feedbackPosition.y = Mathf.Tan(bonusangle) * feedbackPosition.x ;
			if(direction2.y < 0){
				feedbackPosition.y *= -1;
			}
		}
		else{
			feedbackPosition.y = Screen.height/2f;
			if(direction2.y < 0){
				feedbackPosition.y *= -1;
			}
			
			if(bonusangle > 90f){
				bonusangle = 180f - bonusangle;
			}
			
			feedbackPosition.x = Mathf.Tan(90f - bonusangle)* Screen.height / 2f; 
			
			if(direction2.x < 0){
				feedbackPosition.x *= -1;
			}
		}
		
		return feedbackPosition;
	}
}
