namespace JIAO.JIAO
{
	using UnityEngine;
	using System;
    public class JIAOMouseController : JIAOInputController
	{
		private bool m_mouseIn = false;

		override public void resolveInputs() {
			if (!m_mouseIn && Input.mousePosition.x < Screen.width &&
				Input.mousePosition.x > 0 &&
				Input.mousePosition.y < Screen.height &&
				Input.mousePosition.y > 0) {
				m_intputInAction.InputInPosition = Input.mousePosition;
				inputInAction(m_intputInAction);
				m_mouseIn = true;
			}
			else if(m_mouseIn && ( Input.mousePosition.x > Screen.width ||
			        Input.mousePosition.x < 0
			        || Input.mousePosition.y > Screen.height ||
			        Input.mousePosition.y < 0)) {
				m_mouseIn = false;
				m_direction.x = 0f;
				m_direction.y = 0f;
				m_direction.z = 0f;
				m_intputOutAction.InputOutPosition = Input.mousePosition;
				inputOutAction(m_intputOutAction);
			}

			if (m_mouseIn) {
				m_direction.x = (Input.mousePosition.x - m_player.m_screenPosition.x) / Screen.width;
				m_direction.z = (Input.mousePosition.y - m_player.m_screenPosition.y) / Screen.height;
			}

		}
	}
}
