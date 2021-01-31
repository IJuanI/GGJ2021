using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GGJ2021
{
  [RequireComponent(typeof(SpriteRenderer))]
  public class ToggleablePlatform : MonoBehaviour
  {

    public bool toggled = true;

    public Material matOn, matOff;
    private MeshRenderer rend;
    private Collider2D coll;
    private Settings settings;

    private MaterialPropertyBlock rendProps;

    void OnEnable()
    {
      Addressables
          .LoadAssetAsync<Settings>(Settings.PATH)
          .Completed += handle =>
          {
            settings = handle.Result;
            Toggle(toggled);
          };

      rend = GetComponent<MeshRenderer>();
      coll = GetComponent<Collider2D>();
      rendProps = new MaterialPropertyBlock();
    }

    void Start() { Toggle(toggled); }

    public virtual void Toggle(bool enable)
    {
      if (settings == null) return;

      if (enable) rend.material = matOn;
      else rend.material = matOff;

      if (coll) coll.enabled = enable;

      toggled = enable;
    }
  }
}
