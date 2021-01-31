using UnityEngine;

namespace GGJ2021
{

  public class Damager : MonoBehaviour
  {

    private float hitThreshold = 3f;
    private Rigidbody2D rb;

    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
      PlayerEnt player;
      if (other.TryGetComponent<PlayerEnt>(out player))
        player.Kill();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
      PlayerEnt player;
      if (rb != null && collision.collider.TryGetComponent<PlayerEnt>(out player))
      {
        float baseForce = collision.relativeVelocity.magnitude * rb.mass / player.rb.mass;
        float force = 0f;
        foreach (ContactPoint2D contact in collision.contacts)
          force += baseForce * (Vector3.Dot(contact.normal, player.transform.up) + 1) / 2;

        if (force > hitThreshold * collision.contactCount)
          player.Kill();
      }
    }

  }
}