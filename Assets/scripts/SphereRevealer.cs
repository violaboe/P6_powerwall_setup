using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using static OVRInput;

public class SphereRevealer : MonoBehaviour
{
    public Transform conrtrollerPos;
    public GameObject colliderSphere;

    private bool FirstTime = true;

    private void Update()
    {
        if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger) && FirstTime)
        {
            StartReveal();
            FirstTime = false;
        }
    }

    private void StartReveal()
    {
        GameObject go = Instantiate(colliderSphere, conrtrollerPos.transform.position, Quaternion.identity);

        //now scale the sphere with  a lerp over time, if the flowers detect a contact with object they should reveal themselves
        StartCoroutine(ScaleOverTime(go.transform, new Vector3(20,20,20), 10f));
    }

    private IEnumerator ScaleOverTime(Transform obj, Vector3 endScale, float duration)
    {
        Vector3 startScale = obj.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.localScale = endScale;  // Ensure the object reaches the target scale
    }
}
