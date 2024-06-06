using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Script scales a object in all 3 Dimensions based on the live audio

namespace ULAR {
    public class Scaler : MonoBehaviour
    {

        public MP3HighLevelAccess Script;
        public List<GameObject> object2scale = new List<GameObject>();

        public float MinScale = 16f;
        public float MaxScale = 20f;
        public int Band = 1;

        void Update()
        {
            float value = Script.GetBandValue(Band, MinScale, MaxScale, true);
            

            foreach (GameObject obj in object2scale)
            {
                obj.transform.localScale = new Vector3(value, value, value);
            }
        }
    }
}