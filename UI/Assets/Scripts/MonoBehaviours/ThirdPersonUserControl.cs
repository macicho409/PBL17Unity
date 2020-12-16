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
        private const float mapHeigth = 1.25f;
        private List<Vector3> foodSpots = new List<Vector3>() { new Vector3(139.92f, mapHeigth, -157.4f), 
                                                                new Vector3(92.05f, mapHeigth, -203.9f), 
                                                                new Vector3(112.5f, mapHeigth, -262.1f), 
                                                                new Vector3(221f, mapHeigth, -78f) };
        private List<Vector3> waterSpots = new List<Vector3>() { new Vector3(169.22f, mapHeigth, -126.51f), 
                                                                 new Vector3(47.8f, mapHeigth, -161f), 
                                                                 new Vector3(171.2f, mapHeigth, -273.6f), 
                                                                 new Vector3(247.1f, mapHeigth, -77.4f) };
        private List<Vector3> dreamSpots = new List<Vector3>() { new Vector3(47.26f, mapHeigth, -160.67f), 
                                                                 new Vector3(87.8f, mapHeigth, 119.2f), 
                                                                 new Vector3(62.5f, mapHeigth, -253.5f), 
                                                                 new Vector3(288.88f, mapHeigth, -23.7f) };
        private List<Vector3> sexSpots = new List<Vector3>() { new Vector3(120.24f, mapHeigth, -148.21f), 
                                                               new Vector3(132.7f, mapHeigth, -159.3f), 
                                                               new Vector3(6.25f, mapHeigth, -196.7f), 
                                                               new Vector3(362.5f, mapHeigth, 19f) };
        private List<Vector3> toiletSpots = new List<Vector3>() { new Vector3(88.4f, mapHeigth, -111.24f), 
                                                                  new Vector3(161.1f, mapHeigth, -171.7f), 
                                                                  new Vector3(155f, mapHeigth, -215.9f), 
                                                                  new Vector3(355.7f, mapHeigth, 68.2f) };

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

        private Vector3 FindClosestSpot()
        {

        }
    }
}
