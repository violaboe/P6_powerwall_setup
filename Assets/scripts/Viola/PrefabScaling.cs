using System.Collections;
using UnityEngine;

public class PrefabScaling : MonoBehaviour
{
    public Transform targetTransform; // The transform of the prefab to be scaled
    public Vector3 startScale = Vector3.zero; // Starting scale
    public Vector3 endScale = Vector3.one; // Target scale
    public bool keepScale;
    private float duration; // Duration of the scaling animation

    void Start()
    {
        duration = Random.Range(1f, 2f);

        // Ensure the targetTransform is assigned, if not, assign the transform of the GameObject this script is attached to
        if (targetTransform == null)
        {
            targetTransform = transform;
        }

        if (keepScale)
        {
            endScale = transform.localScale;
        }

        // Start the scaling animation
        this.Animate(targetTransform, Easing.AnimationType.Scale, Easing.Ease.EaseOutExpo, startScale, endScale, duration);
    }

}
