namespace JIAO.JIAO
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	public class JIAOMainController : MonoBehaviour {
		public static JIAOMainController m_maincontroller;
		public JIAOAvatar m_player;
		public int m_state = 1;
		public Camera m_mainCamera;

		//Event Generation enemies

		public delegate void GenerateEnemiesEventHandler( object source, EventArgs args);

		public event GenerateEnemiesEventHandler GenerateEnemies;


		private JIAOMainController(){
			m_maincontroller = this;
		}

		public virtual void OnGenerateEnemies(){
			if (GenerateEnemies!= null){
				GenerateEnemies(this, EventArgs.Empty);
			}
		}

		void Start () {

		}

		void FixedUpdate() {

		}


	}
}
