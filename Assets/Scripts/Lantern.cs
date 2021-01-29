using UnityEngine;

namespace GGJ2021
{
  [RequireComponent(typeof(PolygonCollider2D))]
  public class Lantern : MonoBehaviour
  {

    public LightColor lightColor;

    [Range(0f, 90f)]
    public float fov = 30f;
    [Range(1f, 40f)]
    public float distance = 8f;
    [Range(0f, 20f)]
    public float blur = 10f;

    private Light lanternLight;
    private PolygonCollider2D coll;
    private float baseRotation;

    private bool locked;

    void OnValidate()
    {
      if (lanternLight == null || coll == null) Awake();
      lanternLight.spotAngle = (fov + blur) * 2;
      lanternLight.innerSpotAngle = fov * 2;
      lanternLight.range = distance;

      float angle = (90f - fov) * Mathf.Deg2Rad;
      Vector2 endRight = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

      coll.points = new Vector2[]{
          Vector2.zero,
          endRight,
          new Vector2(-endRight.x, endRight.y)
      };
    }

    void Awake()
    {
      lanternLight = GetComponentInChildren<Light>();
      coll = GetComponent<PolygonCollider2D>();

      baseRotation = transform.rotation.eulerAngles.z;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
      LitObject litObject;
      if (other.TryGetComponent<LitObject>(out litObject))
        litObject.OnLit(this);
    }

    void OnTriggerExit2D(Collider2D other)
    {
      LitObject litObject;
      if (other.TryGetComponent<LitObject>(out litObject))
        litObject.OnUnlit(this);
    }

    public void SetLock(bool locked)
    {
      if (!this.locked && locked) ResetSwing();
      this.locked = locked;
    }

    public void Swing(float amount)
    {
      if (!locked)
      {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z += amount;
        transform.rotation = Quaternion.Euler(rot);
      }
    }

    public void ResetSwing()
    {
      Vector3 rot = transform.rotation.eulerAngles;
      rot.z = baseRotation;
      transform.rotation = Quaternion.Euler(rot);
    }
  }
}
