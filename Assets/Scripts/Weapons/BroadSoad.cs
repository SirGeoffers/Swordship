using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadSoad : Weapon {

    [Header("Broad Soad Settings")]
    
    public float spinAmount;
    public float spinSpeed;
    public float useImpulse;

    private enum UseState {
        NotInUse, Spinning, CoolingDown
    }
    [SerializeField]
    private UseState state;

    private bool breakWhenDone = false;
    private GameObject breakSource = null;

    public override void Use() {
        if (state == UseState.NotInUse) StartCoroutine(Spin());
        base.Use();
    }

    protected override void Break(GameObject _source) {
        if (UseState.Spinning == state) {
            if (breakSource == null) {
                breakWhenDone = true;
                breakSource = _source;
            }
        } else {
            base.Break(_source);
        }
    }

    private IEnumerator Spin() {

        state = UseState.Spinning;

        Rigidbody2D rBody = owner.Rigidbody;
        owner.controllable = false;

        rBody.AddForce(owner.transform.up * useImpulse, ForceMode2D.Impulse);

        float totalRotation = 0;
        float startRotation = rBody.rotation;
        while (Mathf.Abs(totalRotation) < spinAmount) {

            totalRotation += spinSpeed * Time.deltaTime;
            if (Mathf.Abs(totalRotation) > spinAmount) totalRotation = Mathf.Sign(totalRotation) * spinAmount;

            rBody.MoveRotation(startRotation + totalRotation);

            yield return null;

        }
        owner.controllable = true;

        state = UseState.CoolingDown;
        if (breakWhenDone) {

            Break(breakSource);

        } else {

            float cooldownTimer = cooldown;
            while (!ProcessCooldown(ref cooldownTimer)) {
                yield return null;
            }

            state = UseState.NotInUse;

        }

        

    }

}
