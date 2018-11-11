using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor Color Settings")]
public class ArmorColorSettings : ScriptableObject {

    public Gradient colorGradient;
    public Color damagedColor;
    public AnimationCurve damageColorCurve;
    public float blinkPercentage;
    public float blinkRate;
    public float fadeTime;
    public float fadeRotationSpeed;

}
