using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

    [SerializeField]
    private float minZoom;

    [SerializeField]
    private float zoomBuffer;
    
    private Dictionary<string, Transform> targets = new Dictionary<string, Transform>();

    private Camera cam;
    private Vector3 moveVelocity;
    private float zoomVelocity;

    private float xyRatio;

    private void Start() {
        cam = GetComponent<Camera>();
        xyRatio = 16f / 9f;
    }

    // RigidBodies updated in fixed update
    private void FixedUpdate () {

        // Move camera towards center position
        Vector3 centerPos = GetCenterPosition(targets);
        centerPos.z = -10;
        this.transform.position = Vector3.SmoothDamp(this.transform.position, centerPos, ref moveVelocity, 0.5f);

        // Adjust zoom based on distance of players
        Vector2 boundsSize = GetBoundsSize(targets);
        float targetWidth = boundsSize.x + zoomBuffer * 2f;
        float targetHeight = boundsSize.y + zoomBuffer * 2f;

        float targetSize = targetHeight;
        float zoomScale = 1;
        if (targetWidth / xyRatio > targetHeight) {
            targetSize = targetWidth;
            zoomScale = xyRatio;
        }
        
        float targetZoom = targetSize / 2f / zoomScale;
        targetZoom = Mathf.Max(targetZoom, minZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, 0.1f);

	}

    private Vector3 GetCenterPosition(Dictionary<string, Transform> _transforms) {
        Vector3 avg = Vector3.zero;
        foreach(Transform t in _transforms.Values) {
            avg += t.position;
        }
        avg /= (float)_transforms.Count;
        return avg;
    }

    private Vector2 GetBoundsSize(Dictionary<string, Transform> _transforms) {

        Vector2 min = Vector2.one * float.MaxValue;
        Vector2 max = Vector2.one * float.MinValue;
        foreach(Transform t in _transforms.Values) {
            Vector3 p = t.position;
            if (p.x < min.x) min.x = p.x;
            if (p.x > max.x) max.x = p.x;
            if (p.y < min.y) min.y = p.y;
            if (p.y > max.y) max.y = p.y;
        }

        Vector2 size = new Vector2(max.x - min.x, max.y - min.y);
        return size;

    }

    public void AddTarget(string _name, Transform _target) {
        if (!targets.ContainsKey(_name)) {
            targets.Add(_name, _target);
        } else {
            targets[_name] = _target;
        }
    }

    public void RemoveTarget(string _name) {
        if (targets.ContainsKey(_name)) {
            targets.Remove(_name);
        }
    }

}
