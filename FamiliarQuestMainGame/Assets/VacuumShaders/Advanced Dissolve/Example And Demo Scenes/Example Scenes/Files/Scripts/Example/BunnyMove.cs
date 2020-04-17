using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AdvancedDissolve_Example
{
    public class BunnyMove : MonoBehaviour
    {
        public Transform target;
        NavMeshAgent agent;
        Animator animator;


        float speedChangeDelta;
        // Use this for initialization
        void Start()
        {
            transform.localScale = Vector3.one * Random.Range(1f, 2f);

            agent = GetComponent<NavMeshAgent>();

            animator = GetComponent<Animator>();
            animator.Play("Move", -1, Random.Range(0f, 1f));

            ChangeSpeed();
        }

        // Update is called once per frame
        void Update()
        {
            //Make sure transform is always on the ground
            Vector3 pos = transform.position;
            pos.y = 0;
            transform.position = pos;


            //Look toward target
            Vector3 lookAt = target.position - transform.position;
            lookAt.y = 0;
            lookAt = lookAt.normalized;

            if (lookAt.magnitude > 0)
                transform.rotation = Quaternion.LookRotation(lookAt);


            //Change speed
            speedChangeDelta -= Time.deltaTime;
            if (speedChangeDelta < 0)
                ChangeSpeed();
        }

         void FixedUpdate()
        {
            Vector3 destination = target.position;
            destination.y = 0;

            agent.destination = destination;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Rigidbody body = collision.rigidbody;
          
            if (body == null || body.gameObject.transform != target)
                return;
            
            body.velocity = (new Vector3(Random.Range(-1.0f, 1f), Random.Range(0.0f, 1f), Random.Range(-1f, 1f))).normalized * Random.Range(15f, 25f);

           
        }

        void ChangeSpeed()
        {
            speedChangeDelta = Random.Range(4f, 12f);

            agent.speed = Random.Range(2f, 7f);
            animator.speed = Mathf.Lerp(1f, 2f, agent.speed / 7f);
        }
    }
}