using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        public Transform target;
        private PhysiologicalModel m_Model;
        NavMeshAgent agent;
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;
        private Vector3 destination;
        private float timeSinceUpdate = 0;
        private bool simStart = true;

        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            agent = GetComponent<NavMeshAgent>();
            m_Character = GetComponent<ThirdPersonCharacter>();
            m_Model = GetComponent<PhysiologicalModel>();
            timeSinceUpdate = Time.time;
            agent.isStopped = true;

        }

        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                //m_Move = v*m_CamForward + h*m_Cam.right;
            }

            if (agent.remainingDistance < 0.001f && Time.time - timeSinceUpdate > 1)
            {
                Debug.Log("First if");
                timeSinceUpdate = Time.time;
                m_Character.ChangeAnimatorState(0.0f);
                //position.RemoveAt(0);
                agent.SetDestination(FindDestination());
                agent.isStopped = true;
            }
            if (Time.time - timeSinceUpdate > 5 && agent.isStopped == true)
            {
                Debug.Log("Second if");
                agent.isStopped = false;
                m_Character.ChangeAnimatorState(0.4f);
            }
            m_Jump = false;
        }
        private Vector3 FindDestination()
        {
            Vector3 destination = new Vector3(0f, 0f, 0f);
            switch (m_Model.PurposeOfLife)
            {
                case (PhysiologicalModel.ListOfNeeds.Food):
                    destination = new Vector3(139.92f, 1.25f, -157.4f);
                    break;
                case (PhysiologicalModel.ListOfNeeds.Water):
                    destination = new Vector3(169.22f, 1.25f, -126.51f);
                    break;
                case (PhysiologicalModel.ListOfNeeds.Dream):
                    destination = new Vector3(47.26f, 1.25f, -160.67f);
                    break;
                case (PhysiologicalModel.ListOfNeeds.Sex):
                    destination = new Vector3(120.24f, 1.25f, -148.21f);
                    break;
                case (PhysiologicalModel.ListOfNeeds.Toilet):
                    destination = new Vector3(88.4f, 1.25f, -111.24f);
                    break;
            }
            return destination;
        }
    }
}
