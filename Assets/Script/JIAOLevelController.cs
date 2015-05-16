namespace JIAO.JIAO
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	
	public class JIAOLevelController
	{
		public List<JIAOLevel> m_levelsArray = new List<JIAOLevel> ();
		public GameObject m_prefab;
		public GameObject m_container;
		public Camera m_camera;
		public int m_currentLevel = 0;
		public JIAOEntity m_Target;
		public Vector3 m_p_lerpTargetVector;
		public float lerp;
		public int m_positionX;
		public int m_positionY;
		static public int m_s_GameWidth = 3;
		static public int m_s_GameHeight = 3;
		static public int m_s_StandardChunkUnit = 100;
		static public int[] m_s_LevelsMap = {1,2,3,4,5,6,7,8,9};
		static public Vector3 m_s_newRightVector = new Vector3 ();
		static public Vector3 m_s_newLeftVector = new Vector3 ();
		static public Vector3 m_s_newUpVector = new Vector3 ();
		static public Vector3 m_s_newDownVector = new Vector3 ();
		
		public JIAOLevelController (GameObject p)
		{
			m_prefab = p;
			m_p_lerpTargetVector = new Vector3 ();
			m_levelsArray.Add (new FirstLevel (m_prefab));
			m_levelsArray.Add (new FirstLevel (m_prefab));
			m_levelsArray.Add (new FirstLevel (m_prefab));
			m_levelsArray.Add (new FirstLevel (m_prefab));
			m_levelsArray.Add (new FirstLevel (m_prefab));
			m_levelsArray.Add (new FirstLevel (m_prefab));
			m_levelsArray.Add (new FirstLevel (m_prefab));
			m_levelsArray.Add (new FirstLevel (m_prefab));
			m_levelsArray.Add (new FirstLevel (m_prefab));

			m_levelsArray [0].addLevel ();
			m_levelsArray [0].m_maincontroller.transform.position = new Vector3 (-2950f, 0f, -2000f);
			m_levelsArray [1].addLevel ();
			m_levelsArray [1].m_maincontroller.transform.position = new Vector3 (-950f, 0f, -2000f);
			m_levelsArray [2].addLevel ();
			m_levelsArray [2].m_maincontroller.transform.position = new Vector3 (1050f, 0f, -2000f);
			m_levelsArray [3].addLevel ();
			m_levelsArray [3].m_maincontroller.transform.position = new Vector3 (-2950f, 0f, 0f);
			m_levelsArray [4].addLevel ();
			m_currentLevel = 4;
			m_levelsArray [4].m_maincontroller.transform.position = new Vector3 (-950f, 0f, 0f);
			m_container = m_levelsArray [4].m_maincontroller;
			m_levelsArray [5].addLevel ();
			m_levelsArray [5].m_maincontroller.transform.position = new Vector3 (1050f, 0f, 0f);
			m_levelsArray [6].addLevel ();
			m_levelsArray [6].m_maincontroller.transform.position = new Vector3 (-2950f, 0f, 2000f);
			m_levelsArray [7].addLevel ();
			m_levelsArray [7].m_maincontroller.transform.position = new Vector3 (-950f, 0f, 2000f);
			m_levelsArray [8].addLevel ();
			m_levelsArray [8].m_maincontroller.transform.position = new Vector3 (1050f, 0f, 2000f);
		}


		public void swapTransform (JIAOLevel o, JIAOLevel o2)
		{
			Vector3 temp = o.chunk.transform.position;
			o.chunk.transform.position = o2.chunk.transform.position;
			o2.chunk.transform.position = temp;
		}

		public JIAOLevel CurrentLevel {
			
			get {
			
				int direction = 0;
				if (m_container.transform.position.x > 50f)
					direction = 1;
				else if (m_container.transform.position.x < (-(m_levelsArray [m_currentLevel].width * m_s_StandardChunkUnit) + 50f)) {
					direction = 2;
				} else if (m_container.transform.position.z < -(m_levelsArray [m_currentLevel].height * m_s_StandardChunkUnit - 100f))
					direction = 3;
				else if (m_container.transform.position.z > 100f)
					direction = 4;

				if (direction != 0) {
					switch (direction) {
					case(1):
						for (int i = 0; i < 3; i++) {
							JIAOLevel level0 = m_levelsArray [3 * i];
							swapTransform (m_levelsArray [3 * i], m_levelsArray [3 * i + 2]);
							m_levelsArray [3 * i] = m_levelsArray [3 * i + 2];
							JIAOLevel level1 = m_levelsArray [3 * i + 1];
							swapTransform (m_levelsArray [3 * i + 1], level0);
							m_levelsArray [3 * i + 1] = level0;
							swapTransform (m_levelsArray [3 * i + 2], level1);
							m_levelsArray [3 * i + 2] = level1;
						} 
						break;
					case(2):
						for (int i = 0; i < 3; i++) {
							JIAOLevel level0 = m_levelsArray [3 * i];
							swapTransform (m_levelsArray [3 * i], m_levelsArray [3 * i + 1]);
							m_levelsArray [3 * i] = m_levelsArray [3 * i + 1];
							swapTransform (m_levelsArray [3 * i + 1], m_levelsArray [3 * i + 2]);
							m_levelsArray [3 * i + 1] = m_levelsArray [3 * i + 2];
							swapTransform (m_levelsArray [3 * i + 2], level0);
							m_levelsArray [3 * i + 2] = level0;
						}
						break;
					case(3):
						for (int i = 0; i < 3; i++) {
							JIAOLevel level0 = m_levelsArray [i];
							swapTransform (m_levelsArray [i], m_levelsArray [i + 3]);
							m_levelsArray [i] = m_levelsArray [i + 3];
							JIAOLevel level1 = m_levelsArray [i + 6];
							swapTransform (m_levelsArray [i + 3], m_levelsArray [i + 6]);
							m_levelsArray [i + 3] = m_levelsArray [i + 6];
							swapTransform (m_levelsArray [i + 6], level0);
							m_levelsArray [i + 6] = level0;
						}
						break;
					case(4):

						for (int i = 0; i < 3; i++) {
							JIAOLevel level0 = m_levelsArray [i];
							swapTransform (m_levelsArray [i], m_levelsArray [i + 6]);
							m_levelsArray [i] = m_levelsArray [i + 6];
							JIAOLevel level1 = m_levelsArray [i + 3];
							swapTransform (m_levelsArray [i + 3], level0);
							m_levelsArray [i + 3] = level0;
							swapTransform (m_levelsArray [i + 6], level1);
							m_levelsArray [i + 6] = level1;
						}
						break;
					}
				}

				m_container = m_levelsArray [4].m_maincontroller;
				return m_levelsArray [m_currentLevel]; 
			}
		}
		
		public void updateLevelPosition ()
		{
			m_p_lerpTargetVector.x = - m_Target.transform.position.x;
			m_p_lerpTargetVector.z = - (m_Target.transform.position.z - 148f);
		    lerp = m_p_lerpTargetVector.magnitude / 40f;
			m_p_lerpTargetVector.Normalize ();
			
			for (int i = 0; i < m_levelsArray.Count; i++) {
				m_levelsArray [i].m_maincontroller.transform.position = m_levelsArray [i].m_maincontroller.transform.position + (m_p_lerpTargetVector * lerp);
				if (m_levelsArray [i].m_maincontroller.transform.position.x < -3950f) {
					m_s_newLeftVector.x = 2050f;
					m_s_newLeftVector.y = m_levelsArray [i].m_maincontroller.transform.position.y;
					m_s_newLeftVector.z = m_levelsArray [i].m_maincontroller.transform.position.z;
					m_levelsArray [i].m_maincontroller.transform.position = m_s_newLeftVector;
				} else if (m_levelsArray [i].m_maincontroller.transform.position.x > 2050f) {
					m_s_newRightVector.x = -3950f;
					m_s_newRightVector.y = m_levelsArray [i].m_maincontroller.transform.position.y;
					m_s_newRightVector.z = m_levelsArray [i].m_maincontroller.transform.position.z;
					m_levelsArray [i].m_maincontroller.transform.position = m_s_newRightVector;
				}
				if (m_levelsArray [i].m_maincontroller.transform.position.z < -3000f) {
					m_s_newUpVector.x = m_levelsArray [i].m_maincontroller.transform.position.x;
					m_s_newUpVector.y = m_levelsArray [i].m_maincontroller.transform.position.y;
					m_s_newUpVector.z = 3000f;
					m_levelsArray [i].m_maincontroller.transform.position = m_s_newUpVector;
				} else if (m_levelsArray [i].m_maincontroller.transform.position.z > 3000f) {
					m_s_newDownVector.x = m_levelsArray [i].m_maincontroller.transform.position.x;
					m_s_newDownVector.y = m_levelsArray [i].m_maincontroller.transform.position.y;
					m_s_newDownVector.z = -3000f;
					m_levelsArray [i].m_maincontroller.transform.position = m_s_newDownVector;
				}
				m_levelsArray [i].m_maincontroller.transform.position = m_levelsArray [i].m_maincontroller.transform.position + (m_p_lerpTargetVector * lerp);
			}
		}
		
		public void updateCurrentChunk ()
		{

			JIAOLevel currentLevel = CurrentLevel;

			m_positionY = (int)Mathf.Abs ((currentLevel.height - 1 - (int)((m_container.transform.position.z + currentLevel.height * m_s_StandardChunkUnit + (m_s_StandardChunkUnit / 2f) - m_Target.transform.position.z) / m_s_StandardChunkUnit)) % CurrentLevel.height);

			m_positionX = (int)Mathf.Abs ((currentLevel.width - (m_container.transform.position.x + currentLevel.width * m_s_StandardChunkUnit - (m_s_StandardChunkUnit / 2f) - m_Target.transform.position.x) / m_s_StandardChunkUnit) % CurrentLevel.width);

			for (int i = 0; i < currentLevel.listchunk.Count; i++) {
				currentLevel.listchunk [i].chunk.SetActive (false);
			}

			currentLevel.listchunk [m_positionY * currentLevel.width + m_positionX].chunk.SetActive (true);

			if (m_positionX > 0 && !currentLevel.listchunk [m_positionY * currentLevel.width + m_positionX - 1].chunk.activeSelf) {
				currentLevel.listchunk [m_positionY * currentLevel.width + m_positionX - 1].chunk.SetActive (true);
			}

			if (m_positionX < currentLevel.width - 1 && !currentLevel.listchunk [m_positionY * currentLevel.width + m_positionX + 1].chunk.activeSelf) {
				currentLevel.listchunk [m_positionY * currentLevel.width + m_positionX + 1].chunk.SetActive (true);
			}

			if (m_positionY > 0 && !currentLevel.listchunk [(m_positionY - 1) * currentLevel.width + m_positionX].chunk.activeSelf) {
				currentLevel.listchunk [(m_positionY - 1) * currentLevel.width + m_positionX].chunk.SetActive (true);
			}

			if (m_positionY < currentLevel.height - 1 && !currentLevel.listchunk [(m_positionY + 1) * currentLevel.width + m_positionX].chunk.activeSelf) {
				currentLevel.listchunk [(m_positionY + 1) * currentLevel.width + m_positionX].chunk.SetActive (true);
			}

			if (m_positionY < currentLevel.height - 1 && m_positionX < currentLevel.width - 1 && !currentLevel.listchunk [(m_positionY + 1) * currentLevel.width + (m_positionX + 1)].chunk.activeSelf) {
				currentLevel.listchunk [(m_positionY + 1) * currentLevel.width + (m_positionX + 1)].chunk.SetActive (true);
			}

			if (m_positionY > 0 && m_positionX < currentLevel.width - 1 && !currentLevel.listchunk [(m_positionY - 1) * currentLevel.width + (m_positionX + 1)].chunk.activeSelf) {
				currentLevel.listchunk [(m_positionY - 1) * currentLevel.width + (m_positionX + 1)].chunk.SetActive (true);
			}

			if (m_positionY < currentLevel.height - 1 && m_positionX > 0 && !currentLevel.listchunk [(m_positionY + 1) * currentLevel.width + (m_positionX - 1)].chunk.activeSelf) {
				currentLevel.listchunk [(m_positionY + 1) * currentLevel.width + (m_positionX - 1)].chunk.SetActive (true);
			}
			
			if (m_positionY > 0 && m_positionX > 0 && !currentLevel.listchunk [(m_positionY - 1) * currentLevel.width + (m_positionX - 1)].chunk.activeSelf) {
				currentLevel.listchunk [(m_positionY - 1) * currentLevel.width + (m_positionX - 1)].chunk.SetActive (true);
			}
		
			if (m_positionY == 0 && m_positionX > 0 && m_positionX < 19) {
				m_levelsArray [1].listchunk [(19) * currentLevel.width + (m_positionX - 1)].chunk.SetActive (true);
				m_levelsArray [1].listchunk [(19) * currentLevel.width + (m_positionX)].chunk.SetActive (true);
				m_levelsArray [1].listchunk [(19) * currentLevel.width + (m_positionX + 1)].chunk.SetActive (true);
			} else if (m_positionY > 0 && m_positionX == 0 && m_positionY < 19) {
				m_levelsArray [3].listchunk [m_positionY * currentLevel.width + 19].chunk.SetActive (true);
				m_levelsArray [3].listchunk [(m_positionY - 1) * currentLevel.width + 19].chunk.SetActive (true);
				m_levelsArray [3].listchunk [(m_positionY + 1) * currentLevel.width + 19].chunk.SetActive (true);
			} else if (m_positionY == 19 && m_positionX > 0 && m_positionX < 19) {
				m_levelsArray [7].listchunk [(0) * currentLevel.width + (m_positionX - 1)].chunk.SetActive (true);
				m_levelsArray [7].listchunk [(0) * currentLevel.width + (m_positionX)].chunk.SetActive (true);
				m_levelsArray [7].listchunk [(0) * currentLevel.width + (m_positionX + 1)].chunk.SetActive (true);
			} else if (m_positionY <19 &&  m_positionX == 19 & m_positionY > 0) {
				m_levelsArray [5].listchunk [(m_positionY + 1) * currentLevel.width + (0)].chunk.SetActive (true);
				m_levelsArray [5].listchunk [(m_positionY) * currentLevel.width + (0)].chunk.SetActive (true);
				m_levelsArray [5].listchunk [(m_positionY - 1) * currentLevel.width + (0)].chunk.SetActive (true);
			}  else if (m_positionY == 0 && m_positionX == 0) {
				m_levelsArray [3].listchunk [(0) * currentLevel.width + 19].chunk.SetActive (true);
				m_levelsArray [3].listchunk [(1) * currentLevel.width + (19)].chunk.SetActive (true);
				m_levelsArray [0].listchunk [(19) * currentLevel.width + (19)].chunk.SetActive (true);
				m_levelsArray [1].listchunk [(19) * currentLevel.width + (0)].chunk.SetActive (true);
				m_levelsArray [1].listchunk [(19) * currentLevel.width + (1)].chunk.SetActive (true);
			}  else if (m_positionY == 19 && m_positionX == 0) {
				m_levelsArray [6].listchunk [(0) * currentLevel.width + (19)].chunk.SetActive (true);
				m_levelsArray [7].listchunk [(0) * currentLevel.width + (m_positionX)].chunk.SetActive (true);
				m_levelsArray [7].listchunk [(0) * currentLevel.width + (m_positionX + 1)].chunk.SetActive (true);
				m_levelsArray [3].listchunk [(19) * currentLevel.width + (19)].chunk.SetActive (true);
				m_levelsArray [3].listchunk [(18) * currentLevel.width + (19)].chunk.SetActive (true);
			} else if (m_positionY == 19 && m_positionX == 19) {
				m_levelsArray [7].listchunk [(0) * currentLevel.width + (19)].chunk.SetActive (true);
				m_levelsArray [7].listchunk [(0) * currentLevel.width + (18)].chunk.SetActive (true);
				m_levelsArray [8].listchunk [(0) * currentLevel.width + (0)].chunk.SetActive (true);
				m_levelsArray [5].listchunk [(19) * currentLevel.width + (0)].chunk.SetActive (true);
				m_levelsArray [5].listchunk [(18) * currentLevel.width + (0)].chunk.SetActive (true);
			} else if (m_positionY == 0 && m_positionX == 19) {
				m_levelsArray [5].listchunk [m_positionY * currentLevel.width + 19].chunk.SetActive (true);
				m_levelsArray [5].listchunk [(m_positionY + 1) * currentLevel.width + 0].chunk.SetActive (true);
				m_levelsArray [5].listchunk [(m_positionY) * currentLevel.width + 0].chunk.SetActive (true);
				m_levelsArray [1].listchunk [(19) * currentLevel.width + 19].chunk.SetActive (true);
				m_levelsArray [1].listchunk [(19) * currentLevel.width + 18].chunk.SetActive (true);
				m_levelsArray [2].listchunk [(19) * currentLevel.width + 0].chunk.SetActive (true);
			} 

		}
	}
}