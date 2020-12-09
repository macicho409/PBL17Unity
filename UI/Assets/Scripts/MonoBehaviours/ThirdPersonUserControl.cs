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
        public Transform target;
        private PhysiologicalModel m_Model;
        NavMeshAgent agent;
        private bool m_Jump;
        private Vector3 destination;
        private float timeSinceUpdate = 0;

        private void Start()
        {
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
            if (agent.remainingDistance < 0.001f && Time.time - timeSinceUpdate > 1)
            {
                Debug.Log("Stoping");
                timeSinceUpdate = Time.time;
                m_Character.ChangeAnimatorState(0.0f);
                destination = FindDestination();
                agent.SetDestination(destination);
                agent.isStopped = true;
            }
            if (Time.time - timeSinceUpdate > 5 && agent.isStopped == true)
            {
                Debug.Log("Moving");
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
