using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;
using System.Linq;

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
        public bool isPositionAcquired = false;
        private List<Vector3> foodSpots = new List<Vector3>() { new Vector3(116.43f, mapHeigth, -136.6f) };
                                                                //new Vector3(139.92f, mapHeigth, -157.4f), 
                                                                //new Vector3(92.05f, mapHeigth, -203.9f), 
                                                                //new Vector3(112.5f, mapHeigth, -262.1f), 
                                                                //new Vector3(221f, mapHeigth, -78f) };
        private List<Vector3> waterSpots = new List<Vector3>() { new Vector3(109.96f, mapHeigth, -189.14f) };
                                                                //new Vector3(169.22f, mapHeigth, -126.51f), 
                                                                 //new Vector3(47.8f, mapHeigth, -161f), 
                                                                 //new Vector3(171.2f, mapHeigth, -273.6f), 
                                                                 //new Vector3(247.1f, mapHeigth, -77.4f) };
        private List<Vector3> dreamSpots = new List<Vector3>() { new Vector3(77.88f, mapHeigth, -176.18f) };
                                                                 //new Vector3(47.26f, mapHeigth, -160.67f), 
                                                                 //new Vector3(87.8f, mapHeigth, 119.2f), 
                                                                 //new Vector3(62.5f, mapHeigth, -253.5f), 
                                                                 //new Vector3(288.88f, mapHeigth, -23.7f) };
        private List<Vector3> sexSpots = new List<Vector3>() { new Vector3(48.91f, mapHeigth, -153.49f) };
                                                                //new Vector3(120.24f, mapHeigth, -148.21f), 
                                                               //new Vector3(132.7f, mapHeigth, -159.3f), 
                                                               //new Vector3(6.25f, mapHeigth, -196.7f), 
                                                               //new Vector3(362.5f, mapHeigth, 19f) };
        private List<Vector3> toiletSpots = new List<Vector3>() { new Vector3(90.1f, mapHeigth, -118.2f) };
                                                                  //new Vector3(88.4f, mapHeigth, -111.24f), 
                                                                  //new Vector3(161.1f, mapHeigth, -171.7f), 
                                                                  //new Vector3(155f, mapHeigth, -215.9f), 
                                                                  //new Vector3(355.7f, mapHeigth, 68.2f) };

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
                isPositionAcquired = true;
            }
            if (Time.time - timeSinceUpdate > 5 && agent.isStopped == true)
            {
                Debug.Log("Moving");
                agent.isStopped = false;
                m_Character.ChangeAnimatorState(0.4f);
                isPositionAcquired = false;
            }
            m_Jump = false;
        }
        private Vector3 FindDestination()
        {
            Vector3 destination = new Vector3(0f, 0f, 0f);
            switch (m_Model.PurposeOfLife)
            {
                case (PhysiologicalModel.ListOfNeeds.Food):
                    Debug.Log("Looking for Food");
                    //destination = new Vector3(139.92f, 1.25f, -157.4f);
                    destination = FindClosestSpot(foodSpots);
                    break;
                case (PhysiologicalModel.ListOfNeeds.Water):
                    Debug.Log("Looking for Water");
                    //destination = new Vector3(169.22f, 1.25f, -126.51f);
                    destination = FindClosestSpot(waterSpots);
                    break;
                case (PhysiologicalModel.ListOfNeeds.Dream):
                    Debug.Log("Looking for Dream");
                    //destination = new Vector3(47.26f, 1.25f, -160.67f);
                    destination = FindClosestSpot(dreamSpots);
                    break;
                case (PhysiologicalModel.ListOfNeeds.Sex):
                    Debug.Log("Looking for Sex");
                    //destination = new Vector3(120.24f, 1.25f, -148.21f);
                    destination = FindClosestSpot(sexSpots);
                    break;
                case (PhysiologicalModel.ListOfNeeds.Toilet):
                    Debug.Log("Looking for Toilet");
                    //destination = new Vector3(88.4f, 1.25f, -111.24f);
                    destination = FindClosestSpot(toiletSpots);
                    break;
            }
            return destination;
        }

        private Vector3 FindClosestSpot(List<Vector3> listOfSpots)
        {
            List<float> distances = new List<float>();
            int minimumValueIndex=0;
            foreach (Vector3 spot in listOfSpots)
            {
                //agent.SetDestination(spot);
                NavMeshPath path = new NavMeshPath();
                //agent.CalculatePath(spot, path);
                //NavMesh.CalculatePath(agent.transform.position, spot, agent.areaMask, path);
                Debug.Log("XXXXXXXXSpot: " + spot.ToString());
                Debug.Log("XXXXXXXXRemaining distance: " + agent.remainingDistance.ToString());
                if (GetPath(path, agent.transform.position, spot, agent.areaMask) == true) {
                    distances.Add(GetPathLength(path));
                }
                else
                {
                    distances.Add(340282300000000);
                }
            }
            minimumValueIndex = distances.IndexOf(distances.Min());
            Debug.Log("Going to point: " + listOfSpots[minimumValueIndex].ToString() + "Index: " + minimumValueIndex.ToString());
            return listOfSpots[minimumValueIndex];
        }

        private bool GetPath(NavMeshPath path, Vector3 fromPos, Vector3 toPos, int passableMask)
        {
            path.ClearCorners();

            if (NavMesh.CalculatePath(fromPos, toPos, passableMask, path) == false)
                return false;

            return true;
        }
        private float GetPathLength(NavMeshPath path)
        {
            float lng = 0.0f;

            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                for (int i = 1; i < path.corners.Length; ++i)
                {
                    lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
            }

            return lng;
        }
    }
}
