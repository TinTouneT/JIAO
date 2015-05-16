namespace JIAO.JIAO
{
	using System;
	using UnityEngine;
	public class JIAOInputInAction : JIAOAction
	{
		private Vector3 m_inputInPosition;
		public Vector3 InputInPosition {
			get {
				return m_inputInPosition;
			}
			set {
				m_inputInPosition = value;
			}
		}

	}
}

