using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Charge Settings")]
public class WeaponChargeSettings : ScriptableObject {

    public AnimationCurve chargeScaleCurve;
    public AnimationCurve chargeAlphaCurve;
    public AnimationCurve chargeCompleteScaleCurve;
    public AnimationCurve chargeCompleteAlphaCurve;

    public GameObject chargingParticlePrefab;
    public GameObject chargingCirclePrefab;

    public Gradient damageGradient;
    public float blinkRate = 1f;

}
