using UnityEngine;

namespace GGJ2021
{

  public class PlayerEnt : MonoBehaviour
  {

    public LightColor lightColor;

    public Checkpoint checkpoint;

    [HideInInspector]
    public Rigidbody2D rb;

    void Start()
    {
      rb = GetComponent<Rigidbody2D>();

      Spawn();
    }

    public void SetCheckpoint(Checkpoint checkpoint) { this.checkpoint = checkpoint; }


    public void Spawn()
    {
      checkpoint.Spawn(this);
    }

    public void Kill()
    {
      Spawn();
    }


  }
}