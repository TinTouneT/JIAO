
namespace JIAO.JIAO
{
	using UnityEngine;
	using System;
    class JIAOAndroidController : JIAOInputController
    {
		private bool m_fingerIn = false;
		private Vector3 m_lastPosition = new Vector3 ();
		private Touch m_touch;
		
		override public void resolveInputs() {

			if (Input.touches.Length > 0) {
			    m_touch = Input.GetTouch (0);
				m_intputInAction.InputInPosition = m_touch.position;
				inputInAction (m_intputInAction);

				m_lastPosition= m_touch.position;

				m_fingerIn = true;
			} else {
				m_fingerIn = false;
				m_direction.x = 0f;
				m_direction.y = 0f;
				m_direction.z = 0f;
				m_intputOutAction.InputOutPosition =m_lastPosition;
				inputOutAction(m_intputOutAction);
			}

			if (m_fingerIn) {

				m_direction.x = (m_touch.position.x - m_player.m_screenPosition.x) / Screen.width;
				m_direction.z = (m_touch.position.y - m_player.m_screenPosition.y) / Screen.height;
			}
		}
    }
}
