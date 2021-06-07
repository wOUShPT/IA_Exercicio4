using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Events;

public class MyAgent : Agent
{
    private BufferSensorComponent _bufferSensor;
    private Transform projectileTransform;
    private GameObject[] projectiles;
    public Spawner spawner;
    private UnityEvent resetEvent;
    private void Awake()
    {
        resetEvent = new UnityEvent();
        _bufferSensor = GetComponent<BufferSensorComponent>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        AddReward(+0.05f);
    }

    public override void OnEpisodeBegin()
    {
        SetReward(0.2f);
        transform.localPosition = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        foreach (var projectile in spawner.projectilePool)
        {
            if (projectile.activeInHierarchy)
            {
                float[] tracker = {projectile.transform.position.x, projectile.transform.position.y};
            
                _bufferSensor.AppendObservation(tracker);   
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float MoveX = actions.ContinuousActions[0];

        float moveSpeed = 4f;

        transform.position += new Vector3(MoveX, 0, 0) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Wall"))
        {
            SetReward(-0.5f);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            SetReward(-0.5f);
            EndEpisode();
        }
    }
    
}
