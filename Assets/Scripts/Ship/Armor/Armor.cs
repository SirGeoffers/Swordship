using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Armor : Damageable {

    [Header("Armor Settings")]
    public ShipController owner;
    public ArmorColorSettings armorColorSettings;

    private SpriteRenderer baseRenderer;
    private Color startBaseColor = Color.white;

    private float currentRatio;
    private float lastRatio;
    private float evalRatio;

    private float blinkTimer;

    protected override void Start() {

        base.Start();

        baseRenderer = GetComponent<SpriteRenderer>();

        currentRatio = GetTargetRatio();
        lastRatio = 2;

    }

    private void Update() {

        float targetRatio = GetTargetRatio();

        if (targetRatio > armorColorSettings.blinkPercentage) {

            blinkTimer = 0;

            currentRatio = Mathf.Max(targetRatio, currentRatio - Time.deltaTime / 1f * (lastRatio - targetRatio));

            evalRatio = (currentRatio - targetRatio) / (lastRatio - targetRatio);
            evalRatio = 1 - evalRatio;
            if (float.IsNaN(evalRatio)) {
                evalRatio = 1;
            }

            float eval = armorColorSettings.damageColorCurve.Evaluate(evalRatio);

            float colorRatio = Mathf.LerpUnclamped(lastRatio, targetRatio, eval);

            SetColor(colorRatio);

        } else {

            blinkTimer += Time.deltaTime * armorColorSettings.blinkRate;
            float colorRatio = 1 - Mathf.Abs(Mathf.Cos(blinkTimer));
            SetColor(colorRatio);

        }

    }

    public void SetColor(Color _color) {
        startBaseColor = _color;
    }

    public override void Damage(float _amount, GameObject _source) {
        if (evalRatio > 0.2f) lastRatio = GetTargetRatio();
        owner.Damage(_amount, _source);
        base.Damage(_amount, _source);
    }

    private float GetTargetRatio() {
        return Mathf.Max(0, durability) / startDurability;
    }

    private void SetColor(float _ratio) {
        _ratio = 1 - _ratio;
        Color c = Color.Lerp(startBaseColor, armorColorSettings.damagedColor, _ratio);
        baseRenderer.color = c;
    }

    [ContextMenu("Break")]
    protected override void Break(GameObject _source) {

        base.Break(_source);
        
        SetColor(0);

        //Rigidbody2D r = this.gameObject.AddComponent<Rigidbody2D>();
        //r.mass = 0.2f;
        //r.drag = 2f;
        //r.angularDrag = 1f;
        //r.gravityScale = 0;
        //r.collisionDetectionMode = CollisionDetectionMode2D.Continuous;


        StartCoroutine(FadeAway());

    }

    private IEnumerator FadeAway() {

        this.transform.SetParent(null);
        Vector3 initialScale = this.transform.localScale;

        Collider2D coll = GetComponent<PolygonCollider2D>();
        if (coll != null) {
            coll.enabled = false;
        }

        float fadeTime = armorColorSettings.fadeTime;
        float rotationSpeed = armorColorSettings.fadeRotationSpeed;
        float fade = fadeTime;
        Vector3 rotVec = new Vector3(0, 0, 1);
        while (fade > 0) {

            fade -= Time.deltaTime;
            this.transform.Rotate(rotVec * rotationSpeed * Time.deltaTime);
            this.transform.localScale = initialScale * (fade / fadeTime);

            yield return null;

        }

        Destroy(this.gameObject);

        yield return null;

    }

}
