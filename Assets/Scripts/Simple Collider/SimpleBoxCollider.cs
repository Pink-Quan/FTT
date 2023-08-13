using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class SimpleBoxCollider : SimpleCollider
{
    [SerializeField, Tooltip("All value of maxPoint must greater than min point")] private Vector2 maxPoint = new Vector3(1, 1);
    [SerializeField, Tooltip("All value of maxPoint must greater than min point")] private Vector2 minPoint = new Vector3(-1, -1);

    public Vector2 GetMaxPoint() => maxPoint + (Vector2)transform.position;
    public Vector2 GetMinPoint() => minPoint + (Vector2)transform.position;

    public Vector2 MaxPoint => maxPoint+(Vector2)_transform.position;
    public Vector2 MinPoint => minPoint+(Vector2)_transform.position;


    protected override void Awake()
    {
        base.Awake();
        SetType(ColliderType.Box);

        if (maxPoint.x < minPoint.x || minPoint.y > maxPoint.y)
            Debug.LogError("All value of maxPoint must greater than min point");
    }

    public override bool CheckCollision(SimpleCollider target)
    {
        if (target.colliderType == ColliderType.Box)
        {
            SimpleBoxCollider targetBox = (SimpleBoxCollider)target;

            float d1x = targetBox.MinPoint.x - this.MaxPoint.x;
            float d1y = targetBox.MinPoint.y - this.MaxPoint.y;
            float d2x = this.MinPoint.x - targetBox.MaxPoint.x;
            float d2y = this.MinPoint.y - targetBox.MaxPoint.y;

            if (d1x > 0.0f || d1y > 0.0f)
                return false;

            if (d2x > 0.0f || d2y > 0.0f)
                return false;

            return true;
        }
        else if (target.colliderType == ColliderType.Circle)
        {
            SimpleCircleCollider cirle = (SimpleCircleCollider)target; ;
            float Xn = Mathf.Max(MinPoint.x, Mathf.Min(cirle.Center.x, MaxPoint.x));
            float Yn = Mathf.Max(MinPoint.y, Mathf.Min(cirle.Center.y, MaxPoint.y));
            float Dx = Xn - cirle.Center.x;
            float Dy = Yn - cirle.Center.y;
            return (Dx * Dx + Dy * Dy) <= cirle.GetRadius() * cirle.GetRadius();
        }
        else
        {
            return CheckPoint(target.transform.position);
        }

    }

    public bool CheckPoint(Vector3 target)
    {
        return target.x < MaxPoint.x && target.y < MaxPoint.y && target.x > MinPoint.x && target.y > MinPoint.y;
    }

    public bool CheckPointJob(Vector3 target)
    {
        NativeArray<bool> isIn = new NativeArray<bool>(1, Allocator.TempJob);
        bool isin;
        new CheckPointUseJob
        {
            isIn = isIn,
            target = target,
            maxPoint = MaxPoint,
            minPoint = MinPoint,
        }.Schedule().Complete();
        isin = isIn[0];
        isIn.Dispose();
        return isin;
    }

    public struct CheckPointUseJob : IJob
    {
        public Vector3 target;
        public Vector3 maxPoint;
        public Vector3 minPoint;
        public NativeArray<bool> isIn;
        public void Execute()
        {
            isIn[0]= target.x < maxPoint.x && target.y < maxPoint.y && target.x > minPoint.x && target.y > minPoint.y;
        }
    }

    public override void OnDrawGizmosSelected()
    {
        if (maxPoint.x < minPoint.x || minPoint.y > maxPoint.y)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;

        Gizmos.DrawLine((Vector3)maxPoint + transform.position, new Vector3(maxPoint.x, minPoint.y) + transform.position);
        Gizmos.DrawLine((Vector3)minPoint + transform.position, new Vector3(maxPoint.x, minPoint.y) + transform.position);
        Gizmos.DrawLine((Vector3)minPoint + transform.position, new Vector3(minPoint.x, maxPoint.y) + transform.position);
        Gizmos.DrawLine((Vector3)maxPoint + transform.position, new Vector3(minPoint.x, maxPoint.y) + transform.position);

        Gizmos.DrawIcon((Vector3)maxPoint + transform.position, "Max Point", true);
        Gizmos.DrawIcon((Vector3)minPoint + transform.position, "Min Point", true);
    }

    public void SetMaxPoint(Vector2 value)
    {
        maxPoint = value;
    }

    public void SetMinPoint(Vector2 value)
    {
        minPoint = value;
    }
    public void SetBottonRight(Vector2 value)
    {
        maxPoint.x = value.x;
        minPoint.y = value.y;
    }

    public void SetTopLeft(Vector2 value)
    {
        maxPoint.y = value.y;
        minPoint.x = value.x;
    }

    public float GetArea() => (maxPoint.x - minPoint.x) * (maxPoint.y - minPoint.y);

}
