using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCannon : Weapon {

    [SerializeField]
    private GameObject gravityOrbPrefab;

    [SerializeField]
    private float spawnDistance;

    private enum State {
        NotInUse, CoolingDown
    }
    private State state;

    public override void Use() {
        if (state == State.NotInUse) StartCoroutine(FireCR());
        base.Use();
    }

    private IEnumerator FireCR() {

        state = State.CoolingDown;

        Vector3 spawnPosition = this.transform.position + this.transform.up * spawnDistance;
        GravityOrb gravityOrb = Instantiate(gravityOrbPrefab, spawnPosition, this.transform.rotation).GetComponent<GravityOrb>();
        gravityOrb.Initialize(this);

        yield return null;

    }

    public void NotifyOrbCollision() {
        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown() {
        yield return new WaitForSeconds(cooldown);
        state = State.NotInUse;
    }

}
