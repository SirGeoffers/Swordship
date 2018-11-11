using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipController))]
public class PlayerController : MonoBehaviour {
    
    [SerializeField]
    private string controlSchemeString;

    private ShipController shipController;
    
    private ControlScheme controlScheme;

	// Use this for initialization
	void Start () {
        shipController = GetComponent<ShipController>();
        if (controlScheme == null) {
            SetControls(controlSchemeString);
        }
	}
	
	// Update is called once per frame
	void Update () {

        float hIn = Input.GetAxis(controlScheme.HORIZONTAL);
        if (hIn != 0) {
            shipController.TurnDime(hIn);
        }

        float vIn = Input.GetAxis(controlScheme.VERTICAL);
        if (vIn > 0) {
            shipController.Thrust();
        } else if (vIn < 0) {
            shipController.Reverse();
        }

        if (Input.GetButtonDown(controlScheme.ACTION1)) {
            shipController.CycleWeapon();
        }

        if (Input.GetButtonDown(controlScheme.ACTION2)) {
            shipController.UseWeapon();
        } else if (Input.GetButtonUp(controlScheme.ACTION2)) {
            shipController.UseWeaponRelease();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            GameManager.Instance.pauseMenu.Toggle();
        }

	}

    public void SetControls(string _inputBase) {
        controlSchemeString = _inputBase;
        controlScheme = new ControlScheme(_inputBase);
    }

    private class ControlScheme {
        public readonly string HORIZONTAL;
        public readonly string VERTICAL;
        public readonly string ACTION1;
        public readonly string ACTION2;
        public ControlScheme(string _name) {
            HORIZONTAL = _name + "_Horizontal";
            VERTICAL = _name + "_Vertical";
            ACTION1 = _name + "_Action1";
            ACTION2 = _name + "_Action2";
        }
    }

}
