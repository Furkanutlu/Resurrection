using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FU
{
    public class DoorTeleport : MonoBehaviour
    {
        public Transform frontPosition;
        public Transform backPosition;

        private bool teleportToFront = true;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // PlayerLocomotionManager'ı al
                PlayerLocomotionManager locomotionManager = other.GetComponent<PlayerLocomotionManager>();
                if (locomotionManager != null)
                {
                    // Hedef pozisyonu belirle
                    Vector3 targetPosition = teleportToFront ? frontPosition.position : backPosition.position;

                    // TeleportPlayer metodunu çağır
                    locomotionManager.TeleportPlayer(targetPosition);

                    // Sıradaki hedef pozisyonu değiştir
                    teleportToFront = !teleportToFront;
                }
            }
        }
    }
}


