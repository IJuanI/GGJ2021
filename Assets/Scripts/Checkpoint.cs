using System;
using UnityEngine;

namespace GGJ2021
{
  [Serializable]
  public struct SpawnPoint
  {
    public PlayerSpawn spawn;
    public LightColor color;
  }

  public class Checkpoint : MonoBehaviour
  {

    public float priority = 0;
    public SpawnPoint[] spawns;


    void OnTriggerEnter2D(Collider2D other)
    {
      PlayerEnt player;
      if (other.TryGetComponent<PlayerEnt>(out player))
        if (player.checkpoint.priority <= priority)
          player.SetCheckpoint(this);
    }

    public void Spawn(PlayerEnt player)
    {
      foreach (SpawnPoint spawnPoint in spawns)
        if (player.lightColor == spawnPoint.color)
        {
          spawnPoint.spawn.Spawn(player);
          break;
        }
    }
  }
}