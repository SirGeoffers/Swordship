using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buzzsaw : DamageScalar {

    [Header("Buzzsaw Settings")]

    [SerializeField]
    private float spinSpeed;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        Vector3 spinDelta = new Vector3(0, 0, spinSpeed) * Time.deltaTime;
        this.transform.Rotate(spinDelta);

	}

    private void OnCollisionEnter2D(Collision2D collision) {

        //Damageable d = collision.gameObject.GetComponent<Damageable>();
        //if (d != null) d.Damage(1000);

    }

}
