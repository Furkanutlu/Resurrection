using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FU
{
    public class CollectableObject : MonoBehaviour
    {
        public string itemName;

        public delegate void ObjectCollectedAction();
        public event ObjectCollectedAction OnObjectCollected;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Öncelikle nesnenin görev listesinde olup olmadığını kontrol et
                if (WorldSaveGameManager.instance.IsTaskActive(itemName))
                {
                    Debug.Log($"Görev nesnesi toplandı: {itemName}");

                    WorldSaveGameManager.instance.CompleteTaskByName(itemName);

                    OnObjectCollected?.Invoke();

                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log($"Bu nesne görev listesinde yok: {itemName}");
                }
            }
        }
    }
}