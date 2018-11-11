using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityOrb : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;

    private GravityCannon owner;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += this.transform.up * moveSpeed * Time.deltaTime;
	}

    public void Initialize(GravityCannon _owner) {
        owner = _owner;
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {

        if (owner != null) {
            owner.NotifyOrbCollision();
        }
        Destroy(this.gameObject);

    }

}
