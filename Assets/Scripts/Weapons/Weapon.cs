using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Damageable {

    private Rigidbody2D rBody;
    public Rigidbody2D Rigidbody {
        get { return rBody; }
    }

    [Header("Weapon Settings")]
    public ShipController owner;
    public float cooldown = -1;

    [SerializeField]
    private float speedScale = 1f;
    [SerializeField]
    private float turnScale = 1f;
    [SerializeField]
    private float chargeSpeedScale = 1f;
    [SerializeField]
    private float chargeTurnScale = 1f;
    public float SpeedScale {
        get {
            return charging ? chargeSpeedScale * speedScale : speedScale;
        }
    }
    public float TurnScale {
        get {
            return charging ? chargeTurnScale * turnScale : turnScale;
        }
    }

    [Header("Charge Settings")]
    public bool chargeable;
    public float chargeTime;
    public float damageOnUse = 0f;
    public WeaponChargeSettings chargeSettings;
    protected bool charging = false;
    private float currentCharge;

    [Header("References")]
    private AudioSource audioSource;
    private AudioLibrary audioLibrary;
    private SpriteRenderer[] spriteRenderers;

    private GameObject chargingCircle;

    private float blinkTimer;

    protected override void Start() {

        base.Start();

        rBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioLibrary = AudioLibrary.Instance;

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        SaveRigidbody();

    }

    protected virtual void Update() {

        if (charging) {

            if (currentCharge < 1) {
                currentCharge += Time.deltaTime / chargeTime;
            } else {
                currentCharge += Time.deltaTime;
            }

            GenerateChargingParticles();
            UpdateChargeCircle();

        }

        if (durability < 20) {
            
            blinkTimer += Time.deltaTime * (broken ? chargeSettings.blinkRate * 4 : chargeSettings.blinkRate);
            float colorRatio = 1 - Mathf.Abs(Mathf.Cos(blinkTimer));
            foreach (SpriteRenderer sr in spriteRenderers) {
                sr.color = chargeSettings.damageGradient.Evaluate(colorRatio);
            }

        }

    }

    private Vector3 velocity;
    private float angularVelocity;
    private Vector3 lastPosition;
    private float lastRotation;
    private void FixedUpdate() {

        if (owner != null) {

            velocity = (this.transform.position - lastPosition) / Time.fixedDeltaTime;
            lastPosition = this.transform.position;

            angularVelocity = -100 * (owner.transform.rotation.z - lastRotation) / Time.fixedDeltaTime;
            lastRotation = owner.transform.rotation.z;

        }
        

    }

    public bool CanAttach() {
        return !broken;
    }

    public virtual void OnAttach() {
        Destroy(rBody);
    }

    public virtual void OnDetach() {
        StopCharging(false);
        RestoreReigidbody();
        rBody.velocity = velocity;
        rBody.angularVelocity = angularVelocity;
        this.transform.SetParent(null);
    }

    protected override void Break(GameObject _source) {
        base.Break(_source);
        if (owner != null) {
            owner.DetachWeapon();
        }
        StartCoroutine(Explode(2));
    }

    public virtual void Use() {
        this.Damage(damageOnUse, this.gameObject);
    }

    public virtual void StartCharging() {
        if (charging) return;
        currentCharge = 0;
        charging = true;
        if (chargingCircle == null) {
            chargingCircle = Instantiate(chargeSettings.chargingCirclePrefab);
            chargingCircle.transform.SetParent(owner.transform, false);
            chargingCircle.GetComponent<LineRenderer>().material.SetColor("_Color", new Color(1, 1, 1, 0));
        }
    }

    public virtual void StopCharging(bool _tryUse) {
        if (_tryUse && owner != null && currentCharge >= 0.95f) {
            Use();
        }
        charging = false;
        currentCharge = 0;
        if (chargingCircle != null) Destroy(chargingCircle);
    }

    protected bool ProcessCooldown(ref float _cooldownTime) {
        _cooldownTime -= Time.deltaTime;
        // Update visuals when I make them
        return _cooldownTime < 0;
    }

    public override void Damage(float _amount, GameObject _source) {
        // Only get damaged through contact with players
        if (_source.tag != "Player") return;
        base.Damage(_amount, _source);
    }

    protected override void OnCollisionEnter2D(Collision2D _collision) {
        base.OnCollisionEnter2D(_collision);
        if (audioSource != null) audioSource.PlayOneShot(audioLibrary.GetRandomContactSound());
        //CollisionBurstManager.Instance.RequestBurst(this.gameObject, _collision);
    }

    protected virtual void OnCollisionStay2D(Collision2D _collision) {
        CollisionBurstManager.Instance.RequestBurst(this.gameObject, _collision);
    }

    private IEnumerator Explode(float _delay) {
        yield return new WaitForSeconds(_delay);
        //Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private float particleRate = 0.01f;
    private float particleTimer = 0;
    private void GenerateChargingParticles() {

        particleTimer += Time.deltaTime;
        if (particleTimer >= particleRate) {
            particleTimer = 0;
            CreateChargingParticle();
        }

    }

    private void CreateChargingParticle() {

        if (currentCharge < 1) {

            Vector2 position = owner.transform.position;
            float distance = Random.Range(0.5f, 1f);
            float dir = Random.Range(0, 2f * Mathf.PI);
            Vector2 dirVec = new Vector2(Mathf.Cos(dir), Mathf.Sin(dir));

            BurstParticle bp = Instantiate(chargeSettings.chargingParticlePrefab).GetComponent<BurstParticle>();
            bp.transform.SetParent(owner.transform);
            bp.Setup(position - dirVec * distance, dirVec, 2f, distance * Random.Range(0.5f, 1f));

        } else {

            Vector2 position = owner.transform.position;
            float distance = Random.Range(0.5f, 1f);
            float dir = Random.Range(0, 2f * Mathf.PI);
            Vector2 dirVec = new Vector2(Mathf.Cos(dir), Mathf.Sin(dir));

            BurstParticle bp = Instantiate(chargeSettings.chargingParticlePrefab).GetComponent<BurstParticle>();
            bp.transform.SetParent(owner.transform);
            bp.Setup(position, dirVec, 2f, distance * Random.Range(0.5f, 1f));

        }
        

    }

    private void UpdateChargeCircle() {

        if (chargingCircle == null) return;

        if (currentCharge < 1) {

            Color c = Color.white;
            c.a = chargeSettings.chargeAlphaCurve.Evaluate(currentCharge);
            chargingCircle.GetComponent<LineRenderer>().material.SetColor("_Color", c);

            chargingCircle.transform.localScale = Vector3.one * chargeSettings.chargeScaleCurve.Evaluate(currentCharge);

        } else {

            Color c = Color.white;
            c.a = chargeSettings.chargeCompleteAlphaCurve.Evaluate(currentCharge - 1f);
            chargingCircle.GetComponent<LineRenderer>().material.SetColor("_Color", c);

            chargingCircle.transform.localScale = Vector3.one * chargeSettings.chargeCompleteScaleCurve.Evaluate(currentCharge - 1f);

        }

    }

    private RigidbodySettings rigidbodySettings;
    private void SaveRigidbody() {
        rigidbodySettings = new RigidbodySettings();
        rigidbodySettings.material = rBody.sharedMaterial;
        rigidbodySettings.mass = rBody.mass;
        rigidbodySettings.drag = rBody.drag;
        rigidbodySettings.angularDrag = rBody.angularDrag;
    }

    private void RestoreReigidbody() {
        if (rBody != null) {
            Destroy(rBody);
        }
        rBody = this.gameObject.AddComponent<Rigidbody2D>();
        rBody.sharedMaterial = rigidbodySettings.material;
        rBody.mass = rigidbodySettings.mass;
        rBody.drag = rigidbodySettings.drag;
        rBody.angularDrag = rigidbodySettings.angularDrag;
    }

    private class RigidbodySettings {
        public PhysicsMaterial2D material;
        public float mass;
        public float drag;
        public float angularDrag;
    }

}
