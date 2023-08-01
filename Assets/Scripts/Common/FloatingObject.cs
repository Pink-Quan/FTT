using System.Collections;
using System.Collections.Generic;
//using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float frequency=1;
    public float amplitude=0.3f;

    private Vector3 basePos;
    private NativeArray<Vector3> position;

    private void Start()
    {
        basePos=transform.position;
        position = new NativeArray<Vector3>(1, Allocator.Persistent);
    }

    private void Update()
    {
        new FloatingYJob
        {
            fixedTime = Time.fixedTime,
            amplitude = amplitude,
            frequency = frequency,
            basePos = basePos,
            position = position
        }.Schedule().Complete();
        transform.position = position[0];
    }

    private void OnDestroy()
    {
        position.Dispose();
    }
    //[BurstCompile]
    public struct FloatingYJob : IJob
    {
        public float frequency;
        public float amplitude;
        public float fixedTime;
        public Vector3 basePos;
        public NativeArray<Vector3> position;

        public void Execute()
        {
            basePos.y += Mathf.Sin(fixedTime * frequency * Mathf.PI) * amplitude;
            position[0] = basePos;
        }
    }
}
