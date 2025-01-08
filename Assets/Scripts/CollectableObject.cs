using FU;
using UnityEngine;
using UnityEngine.Events;

public class CollectableObject : MonoBehaviour
{
    public string itemName;
    public UnityAction OnObjectCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(itemName + " toplandı!");
            OnObjectCollected?.Invoke();
            WorldSaveGameManager.instance.CompleteTaskByName(itemName);
            WorldSaveGameManager.instance.HandleObjectCollected();
            Destroy(gameObject);
        }
    }
}