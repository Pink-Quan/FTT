using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBoxCollider : SimpleCollider
{
    [SerializeField, Tooltip("All value of maxPoint must greater than min point")] private Vector2 maxPoint = new Vector3(1, 1);
    [SerializeField, Tooltip("All value of maxPoint must greater than min point")] private Vector2 minPoint = new Vector3(-1, -1);

    public Vector2 GetMaxPoint() => maxPoint + (Vector2)transform.position;
    public Vector2 GetMinPoint() => minPoint + (Vector2)transform.position;


    private void Awake()
    {
        SetType(ColliderType.Box);

        if (maxPoint.x < minPoint.x || minPoint.y > maxPoint.y)
            Debug.LogError("All value of maxPoint must greater than min point");
    }

    public override bool CheckCollision(SimpleCollider target)
    {
        if (target.colliderType == ColliderType.Box)
        {
            SimpleBoxCollider targetBox = (SimpleBoxCollider)target;

            float d1x = targetBox.GetMinPoint().x - this.GetMaxPoint().x;
            float d1y = targetBox.GetMinPoint().y - this.GetMaxPoint().y;
            float d2x = this.GetMinPoint().x - targetBox.GetMaxPoint().x;
            float d2y = this.GetMinPoint().y - targetBox.GetMaxPoint().y;

            if (d1x > 0.0f || d1y > 0.0f)
                return false;

            if (d2x > 0.0f || d2y > 0.0f)
                return false;

            return true;
        }
        else if (target.colliderType == ColliderType.Circle)
        {
            SimpleCircleCollider cirle = (SimpleCircleCollider)target; ;
            float Xn = Mathf.Max(GetMinPoint().x, Mathf.Min(cirle.GetCenter().x, GetMaxPoint().x));
            float Yn = Mathf.Max(GetMinPoint().y, Mathf.Min(cirle.GetCenter().y, GetMaxPoint().y));
            float Dx = Xn - cirle.GetCenter().x;
            float Dy = Yn - cirle.GetCenter().y;
            return (Dx * Dx + Dy * Dy) <= cirle.GetRadius() * cirle.GetRadius();
        }
        else
        {
            return CheckPoint(target.transform.position);
        }

    }

    public bool CheckPoint(Vector3 target)
    {
        return target.x < GetMaxPoint().x && target.y < GetMaxPoint().y && target.x > GetMinPoint().x && target.y > GetMinPoint().y;
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

    public float GetArea() => (maxPoint.x - minPoint.x) * (maxPoint.y - minPoint.y);

}
