using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship Color")]
public class ShipColor : ScriptableObject {

    public enum Type {
        Main,
        Highlight,
        Score
    }

    public Color main;
    public Color highlight;
    public Color score;

    public Color Get(Type _type) {
        switch (_type) {
            case Type.Main:
                return main;
            case Type.Highlight:
                return highlight;
            case Type.Score:
                return score;
            default:
                return main;
        }
    }

}
