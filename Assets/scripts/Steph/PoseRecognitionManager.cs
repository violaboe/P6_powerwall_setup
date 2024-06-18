using System.Collections.Generic;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Assertions;

namespace Oculus.Interaction.Samples
{
    public class PoseRecognitionManager : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IHmd))]
        private UnityEngine.Object _hmd;
        private IHmd Hmd { get; set; }

        [SerializeField]
        private ActiveStateSelector[] _poses;

        [SerializeField]
        private GameObject visualFeedbackPrefab;
        
        [SerializeField]
        private GameObject butterflyPrefab;

        private GameObject[] activePosesStored;

        private bool[] noActivePoses;
        private int paperRightIndex = -1;
        private int stopPoseRightIndex = -1;
        private int thumbsUpRightIndex = -1;

        protected virtual void Awake()
        {
            Hmd = _hmd as IHmd;
        }

        protected virtual void Start()
        {
            this.AssertField(Hmd, nameof(Hmd));
            this.AssertField(butterflyPrefab, nameof(butterflyPrefab));

            activePosesStored = new GameObject[_poses.Length];
            noActivePoses = new bool[_poses.Length];


            // this is where you add the poses to be used
            for (int i = 0; i < _poses.Length; i++)
            {
                if (_poses[i].name == "PaperRight")
                {
                    paperRightIndex = i;
                }
                else if (_poses[i].name == "StopPoseRight")
                {
                    stopPoseRightIndex = i;
                    //Debug.Log("STOPPOSERIGHT FOUND!!");
                }
                else if (_poses[i].name == "ThumbsUpRight")
                {
                    thumbsUpRightIndex = i;
                    //Debug.Log("THUMBSUPRIGHT FOUND!!");
                }

                // instantiate butterfly on hand condition
                if (_poses[i].name == "PaperRight")
                {
                    SetupPoseIndicator(paperRightIndex);
                }
                else
                {
                    SetupPoseEvents(i);
                }

            }
        }

        // responsible for setting up the indicator for PaperRight visuals
        private void SetupPoseIndicator(int poseIndex)
        {
            if (poseIndex != -1)
            {
                activePosesStored[poseIndex] = Instantiate(butterflyPrefab);
                //activePosesStored[poseIndex] = Instantiate(visualFeedbackPrefab);
                activePosesStored[poseIndex].SetActive(false);
                noActivePoses[poseIndex] = false;

                int poseNumber = poseIndex;
                _poses[poseIndex].WhenSelected += () => ShowVisuals(poseNumber);
                _poses[poseIndex].WhenUnselected += () => HideVisuals(poseNumber);
            }
        }

        // responsible for setting up the indicator for other poses (no visuals)
        private void SetupPoseEvents(int poseIndex)
        {
            if (poseIndex != -1)
            {
                activePosesStored[poseIndex] = Instantiate(visualFeedbackPrefab);

                int poseNumber = poseIndex;
                _poses[poseIndex].WhenSelected += () => ShowVisuals(poseNumber);
                _poses[poseIndex].WhenUnselected += () => HideVisuals(poseNumber);
            }
        }

        private void Update()
        {
            /*if (paperRightIndex != -1 && noActivePoses[paperRightIndex])
            {
                UpdateVisualPosition(paperRightIndex);
            }*/

            if (paperRightIndex != -1 && noActivePoses[paperRightIndex])
            {
                UpdateVisualPosition(paperRightIndex);
            }
            if (stopPoseRightIndex != -1 && noActivePoses[stopPoseRightIndex])
            {
                UpdateVisualPosition(stopPoseRightIndex);
            }
            if (thumbsUpRightIndex != -1 && noActivePoses[thumbsUpRightIndex])
            {
                UpdateVisualPosition(thumbsUpRightIndex);
            }

        }

        private void ShowVisuals(int poseNumber)
        {
            if (poseNumber < 0 || poseNumber >= activePosesStored.Length)
            {
                Debug.LogError($"Invalid poseNumber: {poseNumber}");
                return;
            }

            noActivePoses[poseNumber] = true;
            UpdateVisualPosition(poseNumber);
            activePosesStored[poseNumber].SetActive(true);
        }

        private void HideVisuals(int poseNumber)
        {
            if (poseNumber < 0 || poseNumber >= activePosesStored.Length)
            {
                Debug.LogError($"Invalid poseNumber: {poseNumber}");
                return;
            }

            noActivePoses[poseNumber] = false;
            activePosesStored[poseNumber].gameObject.SetActive(false);
        }

        private void UpdateVisualPosition(int poseNumber)
        {
            if (!Hmd.TryGetRootPose(out Pose hmdPose))
            {
                return;
            }

            var hands = _poses[poseNumber].GetComponents<HandRef>();
            Vector3 visualsPos = Vector3.zero;
            foreach (var hand in hands)
            {
                hand.GetRootPose(out Pose wristPose);
                Vector3 forward = hand.Handedness == Handedness.Left ? wristPose.right : -wristPose.right;
                visualsPos += wristPose.position + forward * .15f + Vector3.up * .02f;
            }
            activePosesStored[poseNumber].transform.position = visualsPos / hands.Length;
            activePosesStored[poseNumber].gameObject.SetActive(true);
        }

        public bool IsPoseActive(string poseName)
        {
            for(int i = 0; i < _poses.Length; i++)
            {
                //Debug.Log($"Checking pose: {_poses[i].name} - Active: {noActivePoses[i]}");
                if (_poses[i].name == poseName && noActivePoses[i])
                {
                    //Debug.Log($"Pose {poseName} is active!");
                    return true;
                }
            }
            return false;
        }
    }
}