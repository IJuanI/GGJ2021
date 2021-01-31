using UnityEngine;

namespace GGJ2021
{
  public class Spawnable : MonoBehaviour
  {

    [HideInInspector]
    public ObjectSpawner spawner;

    private void OnDestroy()
    {
      spawner.OnObjectDespawn();
    }
  }
}