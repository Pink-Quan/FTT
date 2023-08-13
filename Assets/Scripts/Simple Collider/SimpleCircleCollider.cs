using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCircleCollider : SimpleCollider
{
    [SerializeField] private Vector2 center;
    [SerializeField, Min(0)] private float radius;

    public Vector2 GetCenter() => center + (Vector2)transform.position;
    public Vector2 Center => center + (Vector2)_transform.position;
    public float GetRadius() => radius;

    protected override void Awake()
    {
        base.Awake();
        SetType(ColliderType.Circle);
    }

    public override bool CheckCollision(SimpleCollider target)
    {
        if (target.colliderType == ColliderType.Circle)
        {
            SimpleCircleCollider targetCircle = (SimpleCircleCollider)target;
            return (GetRadius() + targetCircle.GetRadius()) * (GetRadius() + targetCircle.GetRadius()) - Vector2.SqrMagnitude(GetCenter() - targetCircle.GetCenter()) > 0f;
        }
        else if (target.colliderType == ColliderType.Box)
        {
            SimpleBoxCollider box = (SimpleBoxCollider)target;
            float Xn = Mathf.Max(box.MinPoint.x, Mathf.Min(Center.x, box.MaxPoint.x));
            float Yn = Mathf.Max(box.MinPoint.y, Mathf.Min(Center.y, box.MaxPoint.y));
            float Dx = Xn - Center.x;
            float Dy = Yn - Center.y;
            return (Dx * Dx + Dy * Dy) <= GetRadius() * GetRadius();
        }
        else
        {
            return CheckPoint(target.transform.position);
        }

    }

    public bool CheckPoint(Vector2 target)
    {
        return Mathf.Pow(Vector2.SqrMagnitude(Center - target), 2) - Mathf.Pow(GetRadius(), 2) < 0;
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawIcon(GetCenter(), "Center", true);
        Gizmos.DrawWireSphere(GetCenter(), GetRadius());
    }

    public void SetRadius(float newValue)
    {
        if (newValue <= 0f) return;

        radius = newValue;
    }

    public float GetArea() => radius * radius * Mathf.PI;

    public void SetCenter(Vector2 newValue)
    {
        center = newValue;
    }
}
