using UnityEngine;

namespace GGJ2021
{
  public class Destroyer : MonoBehaviour
  {
    void OnTriggerEnter2D(Collider2D other)
    {
      Destroyable destroyable;
      if (other.TryGetComponent<Destroyable>(out destroyable))
        Destroy(destroyable.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
      Destroyable destroyable;
      if (collision.collider.TryGetComponent<Destroyable>(out destroyable))
        Destroy(destroyable.gameObject);
    }
  }
}