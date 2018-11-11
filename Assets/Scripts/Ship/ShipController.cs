using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour {
    
    public Vector2 Forward2D {
        get {
            return this.transform.up;
        }
    }

    public Rigidbody2D Rigidbody {
        get {
            return rBody;
        }
    }

    public bool IsBroken {
        get {
            return core.IsBroken;
        }
    }

    [Header("Player Settings")]
    public int playerId;

    [Header("Movement Settings")]

    [SerializeField]
    private float forwardThrust;

    [SerializeField]
    private float maxForwardSpeed;

    [SerializeField]
    private float reverseThrust;

    [SerializeField]
    private float maxReverseSpeed;

    [SerializeField]
    private float turnThrust;

    [SerializeField]
    private float maxTurnSpeed;

    public bool controllable = true;

    [Header("References")]
    public ShipCore core;
    public SpriteRenderer baseplate;
    public Armor[] armorPieces;

    private GameManager gameManager;
    private ShipColor shipColor;

    [Header("Prefabs")]

    [SerializeField]
    private GameObject explosionPrefab;

    private bool initialized = false;

    private Rigidbody2D rBody;
    private ShipThrusters thrusters;
    private Weapon attachedWeapon;

    private const string WEAPON_TAG = "Weapon";
    List<Weapon> nearbyWeapons = new List<Weapon>();
    private Weapon NearestWeapon {
        get {
            if (nearbyWeapons.Count == 0) return null;
            return nearbyWeapons[0];
        }
    }
    //private Vector2 weaponAnchorPosition = new Vector2(0, -0.85f);
    private float weaponThrowForce = 3f;

    private enum State {
        None, Respawning
    }
    private State currentState;

    // Use this for initialization
    private void Start () {
        if (!initialized) Init(playerId, null);
	}

    public void Init(int _id, Transform _thrusterParticleHolder) {

        rBody = GetComponent<Rigidbody2D>();
        thrusters = GetComponent<ShipThrusters>();

        playerId = _id;
        currentState = State.None;

        rBody.centerOfMass = Vector2.zero;

        gameManager = GameManager.Instance;
        gameManager.AddCameraTarget(this.name, this.transform);

        thrusters.SetHolder(_thrusterParticleHolder);

        initialized = true;

    } 

    // Smoothly turn in a direction
    public void Turn(float _direction) {
        if (!controllable) return;
        _direction = Mathf.Sign(_direction);
        if (Mathf.Abs(rBody.angularVelocity) >= maxTurnSpeed) {
            rBody.angularVelocity = Mathf.Sign(rBody.angularVelocity) * maxTurnSpeed;
        } else {
            _direction = Mathf.Sign(_direction);
            rBody.AddTorque(_direction * turnThrust * Time.deltaTime);
        }
    }

    // Rigidly turn in a direction
    public void TurnDime(float _direction) {
        if (!controllable) return;
        _direction = Mathf.Sign(_direction);
        float rotationAmount = _direction * maxTurnSpeed;
        if (attachedWeapon != null) {
            rotationAmount *= attachedWeapon.TurnScale;
        }
        rBody.MoveRotation(rBody.rotation + rotationAmount);
        //rBody.angularVelocity = rotationAmount;
    }

    // Thrust in forwards
    public void Thrust() {
        if (!controllable) return;
        thrusters.GenerateThrusterParticles();
        float mfs = maxForwardSpeed;
        if (attachedWeapon != null) {
            mfs *= attachedWeapon.SpeedScale;
        }
        if (rBody.velocity.magnitude >= mfs) {
            //rBody.velocity = rBody.velocity.normalized * maxForwardSpeed;
        } else {
            rBody.AddForce(Forward2D * forwardThrust);
        }
    }

    // Thrust backwards
    public void Reverse() {
        if (!controllable) return;
        if (rBody.velocity.magnitude >= maxReverseSpeed) {
            //rBody.velocity = rBody.velocity.normalized * maxForwardSpeed;
        } else {
            rBody.AddForce(-Forward2D * reverseThrust);
        }
    }

    // Attach/Detach weapon
    public void CycleWeapon() {
        if (!controllable) return;
        if (attachedWeapon != null) {
            DetachWeapon();
        } else {
            AttachWeapon(NearestWeapon);
        }
    }

    // Attach nearest weapon to player
    public void AttachWeapon(Weapon _weapon) {
        
        if (_weapon == null || _weapon.owner != null || !_weapon.CanAttach()) return;

        GameObject g = _weapon.gameObject;

        _weapon.transform.rotation = this.transform.rotation;
        _weapon.transform.position = this.transform.position + this.transform.up * 1.1f;
        _weapon.transform.SetParent(this.transform);

        //joint.connectedBody = _weapon.Rigidbody;
        //joint.connectedAnchor = weaponAnchorPosition;
        //joint.enabled = true;

        //_weapon.GetComponent<Rigidbody2D>().mass = 0.1f;

        _weapon.owner = this;
        attachedWeapon = _weapon;

        attachedWeapon.OnAttach();

    }

    // Detach and throw held weapon
    public void DetachWeapon() {
        
        if (attachedWeapon == null) return;

        attachedWeapon.OnDetach();
        Rigidbody2D weaponRigidbody = attachedWeapon.GetComponent<Rigidbody2D>();
        weaponRigidbody.AddForce(Forward2D * weaponThrowForce, ForceMode2D.Impulse);

        attachedWeapon.owner = null;

        attachedWeapon = null;

    }

    public void UseWeapon() {
        if (!controllable) return;
        if (attachedWeapon != null) {
            if (attachedWeapon.chargeable) {
                attachedWeapon.StartCharging();
            } else {
                attachedWeapon.Use();
            }
        }
    }

    public void UseWeaponRelease() {
        if (attachedWeapon != null) {
            attachedWeapon.StopCharging(true);
        }
    }

    public void SetColor(ShipColor _color) {
        shipColor = _color;
        core.GetComponent<SpriteRenderer>().color = _color.main;
        baseplate.color = _color.main;
        foreach (Armor a in armorPieces) {
            a.SetColor(_color.highlight);
        }
    }

    public Color GetColor(ShipColor.Type _type) {
        return shipColor.Get(_type);
    }

    public void Damage(float _amount, GameObject _source) {
        
        int sourceId = -1;
        ShipController sourceController = GetOwner(_source);
        if (sourceController != null) sourceId = sourceController.playerId;

        GameEventPlayerDamaged e = new GameEventPlayerDamaged(sourceId, this.playerId);
        gameManager.NotifyEvent(e);

    }

    public void Break(GameObject _source) {

        int sourceId = -1;
        ShipController sourceController = GetOwner(_source);
        if (sourceController != null) sourceId = sourceController.playerId;

        GameEventPlayerDeath e = new GameEventPlayerDeath(sourceId, this.playerId);
        gameManager.NotifyEvent(e);

        //Respawn();
    }

    private ShipController GetOwner(GameObject _o) {

        ShipController sc = _o.GetComponent<ShipController>();
        if (sc != null) {
            return sc;
        }

        Weapon w = _o.GetComponent<Weapon>();
        if (w != null && w.owner != null) {
            return w.owner;
        }

        return null;

    }

    private void Explode() {
        Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
    }

    // Initiate respawn sequence for player
    [ContextMenu("Respawn")]
    public void Respawn() {

        if (currentState == State.Respawning) return;
        currentState = State.Respawning;

        DetachWeapon();
        Explode();

        GameObject deathPoint = new GameObject();
        deathPoint.name = "Death Point " + Time.time;
        deathPoint.transform.position = this.transform.position;
        gameManager.AddCameraTarget(deathPoint.name, deathPoint.transform);

        controllable = false;
        rBody.velocity = Vector2.zero;
        gameManager.RemoveCameraTarget(this.name);
        this.transform.position = Vector2.one * -2000;

        StartCoroutine(RespawnCR(deathPoint));

    }

    public void Spawn(Transform _xform) {

        controllable = true;
        rBody.velocity = Vector2.zero;
        this.transform.position = _xform.position;
        this.transform.rotation = _xform.rotation;
        //ResetDurability();
        gameManager.AddCameraTarget(this.name, this.transform);

    }

    private IEnumerator RespawnCR(GameObject _deathPoint) {

        yield return new WaitForSeconds(2);

        gameManager.RemoveCameraTarget(_deathPoint.name);
        Destroy(_deathPoint);

        currentState = State.None;

        //Spawn(startPos);
        gameManager.SpawnPlayer(this.playerId);

    }

    private void OnTriggerEnter2D(Collider2D collision) {

        GameObject g = collision.gameObject;

        // Manage nearby weapons
        if (g.tag == WEAPON_TAG) {
            Weapon weapon = g.GetComponent<Weapon>();
            if (!nearbyWeapons.Contains(weapon)) nearbyWeapons.Add(weapon);
        }

    }

    private void OnTriggerExit2D(Collider2D collision) {

        GameObject g = collision.gameObject;

        // Manage nearby weapons
        if (g.tag == WEAPON_TAG) {
            Weapon weapon = g.GetComponent<Weapon>();
            nearbyWeapons.Remove(weapon);
        }

    }

}