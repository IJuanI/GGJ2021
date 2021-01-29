using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;


namespace GGJ2021
{
  public class LightHolder : MonoBehaviour
  {

    private Lantern lantern;
    private bool lightEnabled = false;
    private float rawSwing;

    void OnEnable()
    {
      lantern = GetComponentInChildren<Lantern>(true);
    }

    void FixedUpdate()
    {
      lantern.Swing(rawSwing * Time.fixedDeltaTime * 360);
    }

    public void DoToggleLight(CallbackContext ctx)
    {
      switch (ctx.phase)
      {
        case InputActionPhase.Performed:
          lightEnabled = !lightEnabled;
          lantern.gameObject.SetActive(lightEnabled);
          break;
      }
    }

    public void DoSwing(CallbackContext ctx)
    {
      switch (ctx.phase)
      {
        case InputActionPhase.Performed:
        case InputActionPhase.Canceled:
          rawSwing = -ctx.ReadValue<float>();
          break;
      }
    }
  }
}