using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;
using System.Linq;
using System.Threading;
using Assets.Scripts.Models.Enums;
using Assets.Scripts.Services;
using System.Threading.Tasks;

namespace Assets.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private readonly int satisfyNeedsRange = 5;

        public List<Transform> Children = new List<Transform>(); // Kids
        public List<Transform> Parents = new List<Transform>(); // Mother, father
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        public Transform target;
        private PhysiologicalModel m_Model;
        NavMeshAgent agent;
        private bool m_Jump;
        private Vector3? destination;
        private float timeSinceUpdate = 0;
        private float timeSincePopulating = 0;
        private float stopTime = 0;
        private const float mapHeigth = 1.25f;
        public bool isPositionAcquired = false;
        public bool isNeedSatisfied = false;
        private List<Vector3> foodSpots = new List<Vector3>();
        private List<Vector3> waterSpots = new List<Vector3>();
        private List<Vector3> sleepSpots = new List<Vector3>();
        private List<Vector3> sexSpots = new List<Vector3>();
        private List<Vector3> toiletSpots = new List<Vector3>();
        private List<Vector3> highSpots = new List<Vector3>();

        private ThirdPersonCharacter m_Character2;
        private GameObject CurrentPartner;
        public int noAgents = 30;

        public List<ListOfNeeds> currentPossitionNeeds;

        private void Start()
        {
            noAgents = StaticContainerService.NoAgents - 1;
            // get the third person character ( this should never be null due to require component )
            agent = GetComponent<NavMeshAgent>();
            m_Character = GetComponent<ThirdPersonCharacter>();
            m_Model = GetComponent<PhysiologicalModel>();
            timeSinceUpdate = Time.time;
            timeSincePopulating = Time.time;
            agent.isStopped = true;
            foodSpots = new List<Vector3>() {GameObject.Find("FoodSpot_0").transform.position,
                                             GameObject.Find("FoodSpot_1").transform.position };
            waterSpots = new List<Vector3>() {GameObject.Find("waterSpot_0").transform.position,
                                              GameObject.Find("waterSpot_1").transform.position };
            sleepSpots = new List<Vector3>() {
                GameObject.Find("sleepSpot_0").transform.position,
                GameObject.Find("sleepSpot_1").transform.position,
                GameObject.Find("sleepSpot_2").transform.position,
                GameObject.Find("sleepSpot_3").transform.position,
                GameObject.Find("sleepSpot_4").transform.position,
                GameObject.Find("sleepSpot_5").transform.position,
                GameObject.Find("sleepSpot_6").transform.position,
                GameObject.Find("sleepSpot_7").transform.position,
                GameObject.Find("sleepSpot_8").transform.position,
                GameObject.Find("sleepSpot_9").transform.position };

            sexSpots = new List<Vector3>() {GameObject.Find("sexSpot_0").transform.position,
                                            GameObject.Find("sexSpot_1").transform.position };
            toiletSpots = new List<Vector3>() {GameObject.Find("toiletSpot_0").transform.position,
                                               GameObject.Find("toiletSpot_1").transform.position };
            highSpots = new List<Vector3>() {GameObject.Find("highSpot_0").transform.position,
                                               GameObject.Find("highSpot_1").transform.position };

            agent.isStopped = false;
            //agent.SetDestination(foodSpots[0]);
            isPositionAcquired = false;
            //m_Character.ChangeAnimatorState(0.4f);
            //Debug.Log("Moving");
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
            if (agent.name == "Bob" && noAgents > 0)
            {
                for (int i = 0; i < noAgents; i++)
                {
                    if (!IsInvoking("Reproduce") && Time.time - timeSincePopulating > 0.02)
                    {
                        noAgents -= 1;
                        Invoke(nameof(Reproduce), 0.02f);
                        timeSincePopulating = Time.time;
                    }
                }
            }

            if (agent.remainingDistance < 2 && Time.time - timeSinceUpdate > 1)
            {
                Debug.Log("Stoping");
                timeSinceUpdate = Time.time;
                m_Character.ChangeAnimatorState(0.0f);
                destination = FindDestination();
                if(destination != null)
                    agent.SetDestination((Vector3)destination);
                agent.isStopped = true;
                isPositionAcquired = true;
            }

            Debug.Log("Need satisfied: _______________" + isNeedSatisfied.ToString());

            if (Time.time - timeSinceUpdate > 15 && agent.isStopped == true)
            {
                Debug.Log("Moving");
                timeSinceUpdate = Time.time;
                agent.isStopped = false;
                m_Character.ChangeAnimatorState(0.4f);
                isNeedSatisfied = false;
                isPositionAcquired = false;
            }

            if(agent.isStopped && timeSinceUpdate > 10)
            {
                isNeedSatisfied = true;
            }

            m_Jump = false;

            BroadcastSatysfyingNeedsSpots(satisfyNeedsRange);
        }

        private Vector3? FindDestination()
        {
            Vector3? destination;

            switch (m_Model.PurposeOfLife)
            {
                case (ListOfNeeds.Food):
                    Debug.Log("Looking for Food");
                    //destination = new Vector3(139.92f, 1.25f, -157.4f);
                    destination = FindClosestSpot(foodSpots);
                    break;
                case (ListOfNeeds.Water):
                    Debug.Log("Looking for Water");
                    //destination = new Vector3(169.22f, 1.25f, -126.51f);
                    destination = FindClosestSpot(waterSpots);
                    break;
                case (ListOfNeeds.Dream):
                    Debug.Log("Looking for Dream");
                    //destination = new Vector3(47.26f, 1.25f, -160.67f);
                    destination = FindClosestSpot(sleepSpots);
                    break;
                case (ListOfNeeds.Sex):
                    Debug.Log("Looking for Sex");
                    //destination = new Vector3(120.24f, 1.25f, -148.21f);
                    destination = FindClosestSpot(sexSpots);
                    break;
                case (ListOfNeeds.Toilet):
                    Debug.Log("Looking for Toilet");
                    //destination = new Vector3(88.4f, 1.25f, -111.24f);
                    destination = FindClosestSpot(toiletSpots);
                    break;
                case (ListOfNeeds.HigherOrderNeeds):
                    Debug.Log("Looking for Higher order needs");
                    //destination = new Vector3(88.4f, 1.25f, -111.24f);
                    destination = FindClosestSpot(highSpots);
                    break;
                default:
                    destination = null;
                    break;
            }

            return destination;
        }

        private void BroadcastSatysfyingNeedsSpots(int needSatisfyRange)
        {
            currentPossitionNeeds = new List<ListOfNeeds>();

            foreach(var spot in foodSpots)
                if (Vector3.Distance(spot, agent.transform.position) <= needSatisfyRange) currentPossitionNeeds.Add(ListOfNeeds.Food);
            foreach (var spot in waterSpots)
                if (Vector3.Distance(spot, agent.transform.position) <= needSatisfyRange) currentPossitionNeeds.Add(ListOfNeeds.Water);
            foreach (var spot in sleepSpots)
                if (Vector3.Distance(spot, agent.transform.position) <= needSatisfyRange) currentPossitionNeeds.Add(ListOfNeeds.Dream);
            foreach (var spot in sexSpots)
                if (Vector3.Distance(spot, agent.transform.position) <= needSatisfyRange) currentPossitionNeeds.Add(ListOfNeeds.Sex);
            foreach (var spot in toiletSpots)
                if (Vector3.Distance(spot, agent.transform.position) <= needSatisfyRange) currentPossitionNeeds.Add(ListOfNeeds.Toilet);
            foreach (var spot in highSpots)
                if (Vector3.Distance(spot, agent.transform.position) <= needSatisfyRange) currentPossitionNeeds.Add(ListOfNeeds.HigherOrderNeeds);
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
                //Debug.Log("Spot: " + spot.ToString());
                //Debug.Log("Remaining distance: " + agent.remainingDistance.ToString());
                if (GetPath(path, agent.transform.position, spot, agent.areaMask) == true) {
                    //Debug.Log("Path found------------------>");
                    distances.Add(GetPathLength(path));
                }
                else
                {
                    distances.Add(340282300000000); //magical number?
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
            //GameObject kid = Instantiate(gameObject, transform.parent); 
            var pos_x = UnityEngine.Random.Range(65.7f, 112.5f);
            var pos_y = UnityEngine.Random.Range(-150f, -133.2f);

            var position = new Vector3(pos_x, 1, pos_y);

            //GameObject kid = Instantiate(gameObject, transform.parent);
            GameObject kid = Instantiate(gameObject, position, new Quaternion());
            int char_num = 30 - noAgents;
            kid.name = "agent_" + char_num.ToString();
            //kid.transform.tag = "agent_"+ char_num.ToString();
            //kid.tag = "Character_Number_"+ char_num.ToString();
            if(CurrentPartner != null)
                CurrentPartner.GetComponent<ThirdPersonUserControl>().Children.Add(kid.transform);
            CancelInvoke("Reproduce");
    }
}
}
