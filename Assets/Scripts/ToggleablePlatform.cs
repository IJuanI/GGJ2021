using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GGJ2021
{
  [RequireComponent(typeof(SpriteRenderer))]
  public class ToggleablePlatform : MonoBehaviour
  {

    public bool toggled = true;

    private SpriteRenderer rend;
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

      rend = GetComponent<SpriteRenderer>();
      coll = GetComponent<Collider2D>();
      rendProps = new MaterialPropertyBlock();
    }

    void Start() { Toggle(toggled); }

    public virtual void Toggle(bool enable)
    {
      if (settings == null) return;

      rend.GetPropertyBlock(rendProps);
      Color color = rend.material.GetColor("_BaseColor");
      if (enable)
        color.a = settings.enabledPlatformAlpha;
      else
        color.a = settings.disabledPlatformAlpha;

      rendProps.SetColor("_BaseColor", color);
      rend.SetPropertyBlock(rendProps);
      if (coll) coll.enabled = enable;

      toggled = enable;
    }
  }
}
