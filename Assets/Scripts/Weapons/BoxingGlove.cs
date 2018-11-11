using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingGlove : Weapon {
    
    [Header("Boxing Glove Settings")]

    [SerializeField]
    private GameObject puncher;

    [SerializeField]
    private float punchDistance;

    [SerializeField]
    private float outSpeed;

    [SerializeField]
    private float inSpeed;

    [SerializeField]
    private float punchForce;

    private enum UseState {
        NotInUse, Extending, Retracting, CoolingDown
    }
    [SerializeField]
    private UseState state;

    public override void Use() {
        if (state != UseState.NotInUse) return;
        StartCoroutine(UseCR());
        base.Use();
    }

    private IEnumerator UseCR() {

        CircleCollider2D coll = puncher.GetComponent<CircleCollider2D>();
        state = UseState.Extending;

        float distance = 0;
        while (state == UseState.Extending) {
            distance += outSpeed * Time.deltaTime;
            if (distance > punchDistance) {
                state = UseState.Retracting;
                distance = punchDistance;
            }
            puncher.transform.localPosition = Vector2.up * distance;
            yield return null;
        }

        coll.enabled = false;
        while (state == UseState.Retracting) {
            distance -= inSpeed * Time.deltaTime;
            if (distance < 0) {
                state = UseState.CoolingDown;
                distance = 0;
            }
            puncher.transform.localPosition = Vector2.up * distance;
            yield return null;
        }
        coll.enabled = true;

        float cooldownTimer = cooldown;
        while (!ProcessCooldown(ref cooldownTimer)) {
            yield return null;
        }

        state = UseState.NotInUse;

    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(this.gameObject.name + " Glove");

        base.OnCollisionEnter2D(collision);

        if (state == UseState.Extending) {

            state = UseState.Retracting;

            Rigidbody2D collBody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (collBody != null) {

                ContactPoint2D[] contacts = new ContactPoint2D[100];
                int numContacts = collision.GetContacts(contacts);
                if (numContacts > 0) {
                    ContactPoint2D cp = contacts[0];
                    collBody.AddForceAtPosition(this.transform.up * punchForce, cp.point, ForceMode2D.Impulse);
                    CollisionBurstManager.Instance.CreateBurst(cp.point, this.transform.up * outSpeed);
                }

            }

        }

    }

}
