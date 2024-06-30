using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentControllerCave : MonoBehaviour
{

    [Header("PlaneOrientation Vectors")]
    public Vector3 VectorToScreen;
    public Vector3 planeNormal;
    private Vector3 CaveScreenNormal;

    [Header("Inputs")]
    [SerializeField] private GameObject CaveScreenCenterGameobject;
    [SerializeField] private GameObject CaveCameraGameObject;

    //What should be done on server 

    // -> Call SetValues Function
    // -> Call the reposition Function


    //On Start get Normal of the plane representing the screen
    private void Start()
    {
        CaveScreenNormal = -CaveScreenCenterGameobject.transform.forward; //- is there for alighnment reasons how it is now if Rotation is inverted it might be this 
    }

    //public void SetQuestValuesInCave(Vector3 vectorToScreen, Vector3 CaveScreenNormal)
    //{
    //    VectorToScreen = vectorToScreen;
    //    planeNormal = CaveScreenNormal;
    //}

    public void RepositionCaveCamera(Vector3 vectorToScreen, Vector3 QuestScreenNormal)
    {
        if (CaveCameraGameObject != null)
        {
            CaveCameraGameObject.transform.position = TransformPositionAroundCoordinateSystem(vectorToScreen, QuestScreenNormal, CaveScreenNormal);
        }
    }

    private Vector3 TransformPositionAroundCoordinateSystem(Vector3 VectorToImmitate, Vector3 normal1, Vector3 normal2)
    {
        //Calculate angle between the two normals
        float angle = Vector3.Angle(normal1, normal2);

        //Find Rot Axis
        Vector3 rotationAxis = Vector3.Cross(normal1, normal2).normalized;

        //Combine rotation with axis in a quaternion
        Quaternion rotation = Quaternion.AngleAxis(angle, rotationAxis);

        Vector3 RotatedVector = rotation * VectorToImmitate;

        //Adds the pos of the Screen Ontop as origin of the offset
        return (RotatedVector + CaveScreenCenterGameobject.transform.position);

    }

    private void ResizeCaveScreen(float width, float height)
    {
        //Try get the Child with the projection plane
        //give that Child the two values 
    }
}    