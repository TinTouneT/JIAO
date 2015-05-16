
namespace JIAO.JIAO
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

    public abstract class JIAOInputController
    {
		public delegate void m_InputInHandler(object sender,JIAOInputInAction action);
		public delegate void m_InputOutHandler(object sender,JIAOInputOutAction action);
		public event m_InputInHandler m_eInputIn;
		public event m_InputOutHandler m_eInputOut;
		public Vector3 m_direction = new Vector3(0f,0f,0f);
		protected JIAOInputInAction m_intputInAction;
		protected JIAOInputOutAction m_intputOutAction;
		public Player m_player;
		public abstract void resolveInputs();

		public JIAOInputController() {
			m_intputInAction = new JIAOInputInAction ();
			m_intputOutAction = new JIAOInputOutAction ();
		}

		protected void inputInAction(JIAOInputInAction action) {
			m_eInputIn (this, action);
		}

		protected void inputOutAction(JIAOInputOutAction action) {
			m_eInputOut (this,action);
		}
    }
}
