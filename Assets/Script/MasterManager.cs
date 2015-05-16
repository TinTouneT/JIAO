using UnityEngine;
using System.Collections;

public class MasterManager : MonoBehaviour {

	public GameObject MasterSave;
	public bool saved =  false ;

	// Use this for initialization
	void Start () {
		//MasterSave = (GameObject) Instantiate (gameObject, gameObject.transform.position, gameObject.transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		if (!saved) {
			//MasterSave = (GameObject) Instantiate (GameObject.Find("Master"));
			saved = true;
		}
	}
}

