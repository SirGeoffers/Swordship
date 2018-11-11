using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCore : Damageable {

    [Header("Core Settings")]
    public ShipController shipController;
    public Gradient damageGradient;
    public SpriteRenderer coreSprite;
    public float blinkDurability;
    public float blinkRate;

    private float blinkTimer;

    private void Update() {

        if (durability > blinkDurability) {
            CalculateColor();
        } else {
            BlinkColor();
        }

    }

    public override void Damage(float _amount, GameObject _source) {
        shipController.Damage(_amount, _source);
        base.Damage(_amount, _source);
    }

    protected override void Break(GameObject _source) {
        base.Break(_source);
        shipController.Break(_source);
    }

    private void CalculateColor() {
        float ratio = durability / startDurability;
        SetColor(damageGradient.Evaluate(ratio));
    }

    private void BlinkColor() {
        blinkTimer += Time.deltaTime * blinkRate;
        float ratio = 1 - Mathf.Abs(Mathf.Cos(blinkTimer));
        SetColor(damageGradient.Evaluate(ratio));
    }

    private void SetColor(Color _color) {
        coreSprite.color = _color;
    }

}
