using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownNumber : MonoBehaviour {

    public Image numberImage;
    public Image shadowImage;

    [SerializeField]
    private NumberData[] numberData;

    public void SetNumber(int _n) {

        NumberData nd = numberData[_n];
        SetSprites(nd.number, nd.shadow);

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(nd.sound);

    }

    private void SetSprites(Sprite _number, Sprite _shadow) {
        numberImage.overrideSprite = _number;
        shadowImage.overrideSprite = _shadow;
    }

    [Serializable]
    private class NumberData {
        public Sprite number = null;
        public Sprite shadow = null;
        public AudioClip sound = null;
    }

}
