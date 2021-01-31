using UnityEngine;

namespace GGJ2021
{
  [RequireComponent(typeof(SpriteRenderer))]
  public class ToggleablePlatform : MonoBehaviour
  {

    public bool toggled = true;

    public Material matOn, matOff;
    private MeshRenderer rend;
    private Collider2D coll;

    private MaterialPropertyBlock rendProps;

    void OnEnable()
    {
      rend = GetComponent<MeshRenderer>();
      coll = GetComponent<Collider2D>();
      rendProps = new MaterialPropertyBlock();
    }

    void Start() { Toggle(toggled); }

    public virtual void Toggle(bool enable)
    {
      if (enable) rend.material = matOn;
      else rend.material = matOff;

      if (coll) coll.enabled = enable;

      toggled = enable;
    }
  }
}
