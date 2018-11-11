using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningObject : MonoBehaviour {

    public float speed;
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = new Vector3(0, 0, speed);
        this.transform.Rotate(rot);
	}

}
