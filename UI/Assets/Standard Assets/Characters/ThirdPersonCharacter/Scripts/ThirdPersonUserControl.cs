using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        public Transform target;
        NavMeshAgent agent;
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        private float h = 0;
        private float v = 0;
        private int counter = 0;
        private List<Vector3> position = new List<Vector3>();
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
            //Debug.Log("Traverse link:" + agent.autoTraverseOffMeshLink);
            //TODO: change position to aim positions on new map
            position.Add(new Vector3(139.92f, 1.25f, -157.4f));
            position.Add(new Vector3(47.26f, 1.25f, -160.67f));
            position.Add(new Vector3(169.22f, 1.25f, -126.51f));
            timeSinceUpdate = Time.time;


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
            if (simStart)
            {
                agent.SetDestination(position[0]);
                m_Character.ChangeAnimatorState(0.4f);
                //Debug.Log(agent.remainingDistance.ToString());
                simStart = false;
            }
            // read inputs
            if (counter % 100 == 0)
            {
                h = UnityEngine.Random.Range(-1.0f, 1.0f);
                v = UnityEngine.Random.Range(-1.0f, 1.0f);
            }
            counter++;
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                //m_Move = v*m_CamForward + h*m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                //m_Move = v*Vector3.forward + h*Vector3.right;
                
            }
            if (agent.remainingDistance < 0.001f && Time.time - timeSinceUpdate > 1)
            {
                Debug.Log("First if");
                timeSinceUpdate = Time.time;
                m_Character.ChangeAnimatorState(0.0f);
                position.RemoveAt(0);
                agent.SetDestination(position[0]);
                agent.isStopped = true;
            }
            if (Time.time - timeSinceUpdate > 5 && agent.isStopped == true)
            {
                Debug.Log("Second if");
                agent.isStopped = false;
                m_Character.ChangeAnimatorState(0.4f);
            }
            //m_Character.Move(new Vector3(0,0,1), crouch, m_Jump);
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            //m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
