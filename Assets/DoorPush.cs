using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FU
{
    public class DoorPush : MonoBehaviour
    {
        public float pushDistance = 5f; // İleri itilecek mesafe

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Sadece "Player" tagli nesnelere işlem uygula
            {
                // Oyuncunun Rigidbody'sini veya Transform'unu al
                Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();

                if (playerRigidbody != null)
                {
                    // Kapının yönüne göre oyuncuyu ileri iter
                    Vector3 pushDirection = transform.forward; // Kapının ön yüzü yönünde
                    playerRigidbody.MovePosition(playerRigidbody.position + pushDirection * pushDistance);
                }
                else
                {
                    // Eğer Rigidbody yoksa Transform kullanarak pozisyonu değiştir
                    other.transform.position += transform.forward * pushDistance;
                }

                Debug.Log("Kapıya çarpıldı, oyuncu ileri itildi!");
            }
        }
    }
}

