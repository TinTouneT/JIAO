namespace JIAO.JIAO
{
	using UnityEngine;
	public class FirstLevel : JIAOLevel
	{
		public override string LevelName {
			get {
				return "FirstLevel";
				
			}
		}

		public FirstLevel (GameObject o) : base(o)
		{

			width = 20;
			height = 20;
			for (int i = 0; i < width*height; i++) {
				listchunk.Add (new JIAOChunk (GameObject.Instantiate<GameObject> (this.chunk)));
				listchunk[i].chunk.SetActive(false);
				listchunk[i].chunk.GetComponent<PlacementManager>().m_avatar = GameObject.Find("MainCharacter");
			}
				
		}
	}
}