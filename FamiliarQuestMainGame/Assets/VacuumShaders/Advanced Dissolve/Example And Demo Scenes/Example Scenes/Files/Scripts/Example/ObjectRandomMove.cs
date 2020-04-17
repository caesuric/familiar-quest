using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedDissolve_Example
{
    public class ObjectRandomMove : MonoBehaviour
    {
        Vector3 targetPosition;
        float time;
        float speed;

        Vector3 randomRotate;
        float rotateSpeed;

        void Start()
        {
            Init();
        }

        void Update()
        {
            time += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(transform.position, targetPosition, time);

            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
                Init();


            transform.Rotate(randomRotate, rotateSpeed * Time.deltaTime, Space.Self);
        }

        void Init()
        {
            time = 0;
            speed = Random.Range(0.005f, 0.015f);
            targetPosition = new Vector3(Random.Range(-8f, 8f), 0, Random.Range(-8f, 8f));

            randomRotate = Random.onUnitSphere;
            rotateSpeed = Random.Range(3f, 10f);
        }
    }
}