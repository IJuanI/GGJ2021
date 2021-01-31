using UnityEngine;
using UnityEngine.VFX;

namespace GGJ2021
{
  public class PlayerSpawn : MonoBehaviour
  {

    public Transform spawnPosition;

    public LightColor lightColor;

    private VisualEffect vfx;

    void OnEnable() { vfx = GetComponent<VisualEffect>(); }

    void OnTriggerEnter2D(Collider2D other)
    {
      PlayerEnt player;
      if (other.TryGetComponent<PlayerEnt>(out player))
        if (lightColor == player.lightColor)
          vfx.SendEvent("PlayerIn");
    }

    void OnTriggerExit2D(Collider2D other)
    {
      PlayerEnt player;
      if (other.TryGetComponent<PlayerEnt>(out player))
        if (lightColor == player.lightColor)
          vfx.SendEvent("PlayerOut");
    }

    public void Spawn(PlayerEnt player)
    {
      player.transform.position = spawnPosition.position;
      player.rb.velocity = Vector2.zero;

      VFXEventAttribute attribute = vfx.CreateVFXEventAttribute();
      attribute.SetVector3("position", transform.InverseTransformPoint(spawnPosition.position));
      vfx.SendEvent("PlayerSpawn", attribute);
    }
  }
}