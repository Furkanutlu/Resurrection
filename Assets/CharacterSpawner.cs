using UnityEngine;

namespace FU
{
    public class CharacterSpawner : MonoBehaviour
    {
        public GameObject characterPrefab; // Karakter prefab'i
        public Transform spawnPoint; // SpawnPoint nesnesi

        void Start()
        {
            if (characterPrefab != null && spawnPoint != null)
            {
                Instantiate(characterPrefab, spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                Debug.LogError("CharacterPrefab veya SpawnPoint atanmadı!");
            }
        }
    }

}
