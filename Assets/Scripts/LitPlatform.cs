using UnityEngine;

namespace GGJ2021
{
  public class LitPlatform : ToggleablePlatform, LitObject
  {

    public LightColor lightColor;

    private int lightCount = 0;

    public void OnLit(Lantern lantern)
    {
      if (lightColor == LightColor.NEUTRAL || lightColor == lantern.lightColor)
      {
        ++lightCount;
        if (lightCount == 1)
          Toggle(true);
      }
    }

    public void OnUnlit(Lantern lantern)
    {
      if (lightColor == LightColor.NEUTRAL || lightColor == lantern.lightColor)
      {
        --lightCount;
        if (lightCount == 0)
          Toggle(false);
      }
    }

    public override void Toggle(bool enable)
    {
      if (lightColor != LightColor.NEUTRAL)
      {
        if (enable)
          gameObject.layer = LayerMask.NameToLayer("Solid");
        else if (lightColor == LightColor.BLUE)
          gameObject.layer = LayerMask.NameToLayer("P1 Cam");
        else gameObject.layer = LayerMask.NameToLayer("P2 Cam");
      }
      base.Toggle(enable);
    }

  }
}
