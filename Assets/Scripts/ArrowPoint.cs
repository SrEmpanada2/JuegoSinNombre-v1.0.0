using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPoint : MonoBehaviour {

    private Transform target;
    [SerializeField] private float hideDistance;

    private void Update() {
        if (target != null) {
            var dir = target.position - transform.position;

            if (dir.magnitude < hideDistance) {
                SetChildrenActive(false);
            } else {
                SetChildrenActive(true);

                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        } else {
            Destroy(gameObject);
        }
    }

    private void SetChildrenActive(bool value) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(value);
        }
    }

    public void SetTarget(Transform target) {
        this.target = target;
    }

    public Transform GetTarget() {
        return target;
    }
}
