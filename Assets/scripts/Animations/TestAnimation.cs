using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject cubeStartTransform;
    public GameObject cubeEndTransform;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ani();
        }
    }

    void Ani()
    {
        this.Animate(this.transform, Easing.AnimationType.Position, Easing.Ease.EaseInOutQuad, cubeStartTransform.transform.position, cubeEndTransform.transform.position, 2f, AniBack);
    }

    void AniBack()
    {
        this.Animate(this.transform, Easing.AnimationType.Position, Easing.Ease.EaseInOutQuad, cubeEndTransform.transform.position, cubeStartTransform.transform.position, 2f, Ani);

    }
}
