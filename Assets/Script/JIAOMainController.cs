namespace JIAO.JIAO
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	public class JIAOMainController : MonoBehaviour {

	public JIAOAvatar m_player;
	public int m_phase;
	public Camera m_mainCamera;

	public static JIAOMainController m_maincontroller;

	private JIAOMainController() {
			m_maincontroller = this;
	}
		void Start () {
		}

		void FixedUpdate() {

		}

	}
}
