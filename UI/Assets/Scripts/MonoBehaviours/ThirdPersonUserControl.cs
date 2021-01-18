using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;
using System.Linq;
using System.Threading;

namespace Assets.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        public List<Transform> Children = new List<Transform>(); // Kids
        public List<Transform> Parents = new List<Transform>(); // Mother, father
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        public Transform target;
        private PhysiologicalModel m_Model;
        NavMeshAgent agent;
        private bool m_Jump;
        private Vector3 destination;
        private float timeSinceUpdate = 0;
        private const float mapHeigth = 1.25f;
        public bool isPositionAcquired = false;
        private List<Vector3> foodSpots = new List<Vector3>();
        private List<Vector3> waterSpots = new List<Vector3>();
        private List<Vector3> sleepSpots = new List<Vector3>();
        private List<Vector3> sexSpots = new List<Vector3>();
        private List<Vector3> toiletSpots = new List<Vector3>();

        private ThirdPersonCharacter m_Character2;
        private GameObject CurrentPartner;
        private int noAgents = 30;

        private void Start()
        {
            // get the third person character ( this should never be null due to require component )
            agent = GetComponent<NavMeshAgent>();
            m_Character = GetComponent<ThirdPersonCharacter>();
            m_Model = GetComponent<PhysiologicalModel>();
            timeSinceUpdate = Time.time;
            agent.isStopped = true;
            foodSpots = new List<Vector3>() {GameObject.Find("FoodSpot_0").transform.position,
                                             GameObject.Find("FoodSpot_1").transform.position };
            waterSpots = new List<Vector3>() {GameObject.Find("waterSpot_0").transform.position,
                                              GameObject.Find("waterSpot_1").transform.position };
            sleepSpots = new List<Vector3>() {GameObject.Find("sleepSpot_0").transform.position,
                                              GameObject.Find("sleepSpot_1").transform.position };
            sexSpots = new List<Vector3>() {GameObject.Find("sexSpot_0").transform.position,
                                            GameObject.Find("sexSpot_1").transform.position };
            toiletSpots = new List<Vector3>() {GameObject.Find("toiletSpot_0").transform.position,
                                               GameObject.Find("toiletSpot_1").transform.position };

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
            if (agent.name == "Character Number 1" && noAgents > 0)
            {
                    if (!IsInvoking("Reproduce"))
                    {
                    noAgents -= 1;
                    Invoke("Reproduce", 0.2f);
                    }
            }

            if (agent.remainingDistance < 2 && Time.time - timeSinceUpdate > 1)
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
                    destination = FindClosestSpot(sleepSpots);
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
                Debug.Log("Spot: " + spot.ToString());
                Debug.Log("Remaining distance: " + agent.remainingDistance.ToString());
                if (GetPath(path, agent.transform.position, spot, agent.areaMask) == true) {
                    Debug.Log("Path found------------------>");
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

        private void Reproduce()
        {
            GameObject kid = Instantiate(gameObject, transform.parent);
            CurrentPartner.GetComponent<ThirdPersonUserControl>().Children.Add(kid.transform);
            Children.Add(kid.transform);
            CancelInvoke("Reproduce");
        }
    }
}
