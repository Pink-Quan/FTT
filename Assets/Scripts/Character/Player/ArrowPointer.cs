using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public Transform target;

    public float damping;

    private void Update()
    {
        if (target == null) return;
        
        var rotation = new NativeArray<Quaternion>(1,Allocator.TempJob);
        rotation[0] = transform.rotation;
        new RotateToTargetJob
        {
            target = target.position,
            damping = damping,
            deltaTime = Time.deltaTime,
            position = transform.position,
            rotation = rotation,
        }.Schedule().Complete();

        transform.rotation = rotation[0];
        rotation.Dispose();
    }

    private struct RotateToTargetJob : IJob
    {
        public Vector3 target;
        public float damping;
        public float deltaTime;
        public Vector3 position;
        public NativeArray<Quaternion> rotation;

        public void Execute()
        {
            var dir = (Vector2)(target - position);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var rot = Quaternion.Euler(0, 0, angle);
            rotation[0] = Quaternion.Slerp(rotation[0], rot, damping * deltaTime);
        }
    }
}
