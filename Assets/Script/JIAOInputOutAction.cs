namespace JIAO.JIAO
{
	using System;
	using UnityEngine;
	public class JIAOInputOutAction : JIAOAction
	{
		private Vector3 m_inputOutPosition;
		public Vector3 InputOutPosition {
			get {
				return m_inputOutPosition;
			}
			set {
				m_inputOutPosition = value;
			}
		}
		
	}
}

