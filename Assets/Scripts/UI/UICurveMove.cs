using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICurveMove : MonoBehaviour {

    public float moveTime;
    public AnimationCurve moveCurveX;
    public AnimationCurve moveCurveY;
    public bool destroyAtEnd;

    private RectTransform rectTransform;
    private float time;

	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
        time = 0;
        MoveToTime(time);
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime / moveTime;
        MoveToTime(time);
	}

    private void MoveTo(float _x, float _y) {
        this.rectTransform.anchoredPosition = new Vector2(_x, _y);
    }

    private void MoveToTime(float _time) {
        float xPos = moveCurveX.Evaluate(time);
        float yPos = moveCurveY.Evaluate(time);
        MoveTo(xPos, yPos);
    }

}
