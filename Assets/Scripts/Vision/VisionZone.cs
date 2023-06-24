using UnityEngine;

public class VisionZone : MonoBehaviour
{
    public GameObject[] zones;

    private void Start()
    {
        SimpleBoxColliderTargetEvent e =GetComponent<SimpleBoxColliderTargetEvent>();
        e.target = GameManager.instance.player.transform;
        e.onTargetEnter.AddListener(EnableVision);
    }

    public void EnableVision()
    {
        if (VisionManager.instance.currentVision == this) return;
        VisionManager.instance.currentVision.DisableVision();
        foreach (GameObject zone in zones)
        {
            if (zone != null)
            {
                zone.SetActive(false);
            }
        }
        VisionManager.instance.currentVision = this;
    }

    public void DisableVision()
    {
        foreach (GameObject zone in zones)
        {
            if (zone != null)
            {
                zone.SetActive(true);
            }
        }
    }
}
