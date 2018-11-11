using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Weapon {

    [Header("Spear Settings")]
    public float throwImpulse;
    public SpriteRenderer aimGuide;

    public override void Use() {
        ThrowSpear();
        base.Use();
    }

    public override void OnAttach() {
        base.OnAttach();
        Color aimGuideColor = owner.GetColor(ShipColor.Type.Main);
        aimGuideColor.a = 0.5f;
        aimGuide.color = aimGuideColor;
    }

    public override void StartCharging() {
        base.StartCharging();
        aimGuide.gameObject.SetActive(true);
    }

    public override void StopCharging(bool _tryUse) {
        base.StopCharging(_tryUse);
        aimGuide.gameObject.SetActive(false);
    }

    private void ThrowSpear() {
        owner.DetachWeapon();
        this.GetComponent<Rigidbody2D>().AddForce(this.transform.up * throwImpulse, ForceMode2D.Impulse);
    }

}
