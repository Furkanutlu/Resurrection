using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.VisualScripting;

namespace FU
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance { get; private set; }

        [Header("Scene Management")]
        [SerializeField] private int worldSceneIndex = 1;

        private List<Task> activeTasks;

        [System.Serializable]
        public class Task
        {
            public string name;
            public string description;
            public bool isCompleted;
        }

        [SerializeField]
        private List<Task> tasks = new List<Task>
        {
            new Task { name = "Gök Tanrı'nın Kılıcı", description = "Gök Tanrı'nın kutsal kılıcı, Tengri Dağı'nın zirvesinde saklanmış.", isCompleted = false },
            new Task { name = "Umay Ana'nın Su Tulumu", description = "Umay Ana'nın şifalı suyu, köyün merkezindeki kutsal kuyunun yanında bulunuyor.", isCompleted = false },
            new Task { name = "Kutsal Mor İksir", description = "Bilgelik getiren iksir, köy meydanındaki tezgâhların birinde saklı.", isCompleted = false },
            new Task { name = "Doğa'nın Yeşil İksiri", description = "Doğa Ana'nın ruhunu temsil eden iksir, köy meydanındaki tezgâhların birinde.", isCompleted = false },
            new Task { name = "Bilgelik Kitabı", description = "Bilgelik Kitabı, Bilge Kağan'ın evinin önündeki ağacın altında saklı.", isCompleted = false },
            new Task { name = "Uçan Halı Parşömeni", description = "Efsanevi uçan halının sırrını taşıyan parşömen, nehrin bir ucunda bulunuyor.", isCompleted = false },
            new Task { name = "Demirci'nin Dirgeni", description = "Efsanevi demirci Korkut'un dirgeni, merkezin biraz dışında yerde duruyor.", isCompleted = false },
            new Task { name = "Kutsal Savaş Kalkanı", description = "Alp Er Tunga'nın kalkanı, kutsal dağın zirvesinden düşmüş.", isCompleted = false },
            new Task { name = "Ergenekon Baltası", description = "Demir Dağı delen efsanevi balta, karların yakınındaki bir yerde saklanmış.", isCompleted = false },
            new Task { name = "Azrail'in Tırpanı", description = "Azrail'in tırpanı, köyün karlı dağlara bakan tarafında bekliyor.", isCompleted = false },
            new Task { name = "Güneş Sembolü", description = "Güneş'in gücünü temsil eden sembol, evlerin çatılarında parlıyor.", isCompleted = false },
            new Task { name = "Altın Elma", description = "Kutsal Altın Elma, nehrin ucunda bir yerlerde gizlenmiş.", isCompleted = false },
            new Task { name = "Ay Sembolü", description = "Ay Ana'nın gücünü temsil eden sembol, Yıldız sembolünün yanında duruyor.", isCompleted = false },
            new Task { name = "Yıldız Sembolü", description = "Gökyüzündeki yıldızları temsil eden sembol, Ay sembolünün yanında.", isCompleted = false },
            new Task { name = "Şaman'ın Asası", description = "Kadim şamanın asası, karlarla kaplı bir kulübenin içinde saklı.", isCompleted = false }
        };

        [Header("Task Management")]
        [SerializeField] private GameObject taskPrefab;
        [SerializeField] private Transform taskListParent;

        private Dictionary<string, string> taskToTagMap = new Dictionary<string, string>
        {
            { "Gök Tanrı'nın Kılıcı", "Görev_Kılıcı" },
            { "Umay Ana'nın Su Tulumu", "Görev_SuTulumu" },
            { "Kutsal Mor İksir", "Görev_Morİksir" },
            { "Doğa'nın Yeşil İksiri", "Görev_Yeşilİksir" },
            { "Bilgelik Kitabı", "Görev_Kitap" },
            { "Uçan Halı Parşömeni", "Görev_Parşömen" },
            { "Demirci'nin Dirgeni", "Görev_Dirgen" },
            { "Kutsal Savaş Kalkanı", "Görev_Kalkan" },
            { "Ergenekon Baltası", "Görev_Balta" },
            { "Azrail'in Tırpanı", "Görev_Tırpan" },
            { "Güneş Sembolü", "Görev_Güneş" },
            { "Altın Elma", "Görev_AltınElma" },
            { "Ay Sembolü", "Görev_Ay" },
            { "Yıldız Sembolü", "Görev_Yıldız" },
            { "Şaman'ın Asası", "Görev_Asa" }
        };

        public int remainingTasks; // Kazanma durumunu kontrol etmek için public yapıldı

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            activeTasks = GetRandomTasks(5);
            PopulateTasks();
            UpdateMissionObjectVisibility();
            remainingTasks = 5;
            Debug.Log("Oyun Başladı! Kalan Görev Sayısı: " + remainingTasks);
        }

        #region Task Management

        private List<Task> GetRandomTasks(int count)
        {
            List<Task> randomTasks = new List<Task>();
            List<Task> taskPool = new List<Task>(tasks);

            System.Random random = new System.Random();

            while (randomTasks.Count < count && taskPool.Count > 0)
            {
                int randomIndex = random.Next(taskPool.Count);
                randomTasks.Add(taskPool[randomIndex]);
                taskPool.RemoveAt(randomIndex);
            }

            return randomTasks;
        }

        private void PopulateTasks()
        {
            foreach (Transform child in taskListParent)
            {
                Destroy(child.gameObject);
            }

            foreach (var task in activeTasks)
            {
                GameObject taskObject = Instantiate(taskPrefab, taskListParent);

                TextMeshProUGUI taskText = taskObject.transform.Find("TaskText").GetComponent<TextMeshProUGUI>();
                if (taskText != null)
                {
                    taskText.text = task.name;
                }

                TextMeshProUGUI descriptionText = taskObject.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
                if (descriptionText != null)
                {
                    descriptionText.text = task.description;
                }
            }
        }

        public void UpdateMissionObjectVisibility()
        {
            Transform missionObjectsParent = GameObject.Find("MissionObjects").transform;


            // Aktif görevlere ait nesneleri listele
            List<GameObject> activeObjects = new List<GameObject>();
            foreach (var task in activeTasks)
            {
                if (taskToTagMap.TryGetValue(task.name, out string tag))
                {
                    GameObject missionObject = GameObject.FindGameObjectWithTag(tag);
                    if (missionObject != null)
                    {
                        activeObjects.Add(missionObject);
                    }
                    else
                    {
                        Debug.LogWarning($"Tag'i '{tag}' olan görev nesnesi bulunamadı!");
                    }
                }
            }

            // Tüm görev nesnelerini dolaş
            foreach (Transform missionObject in missionObjectsParent)
            {
                // Eğer nesne aktif görevlerde yoksa, oyundan kaldır
                if (!activeObjects.Contains(missionObject.gameObject))
                {
                    Destroy(missionObject.gameObject);
                }
                else
                {
                    // Aktif görev nesnelerini ayarla
                    missionObject.gameObject.SetActive(true);
                    CollectableObject collectableObject = missionObject.AddComponent<CollectableObject>();
                    collectableObject.itemName = taskToTagMap.FirstOrDefault(x => x.Value == missionObject.tag).Key;
                    collectableObject.OnObjectCollected += HandleObjectCollected;
                    Debug.Log($"Aktif görev nesnesi: {missionObject.name}");
                }
            }
        }

        public void CompleteTaskByName(string taskName)
        {
            Task task = activeTasks.Find(t => t.name == taskName);
            if (task != null)
            {
                task.isCompleted = true;
                Debug.Log($"Görev tamamlandı: {task.name}");

                // Görev UI'ını güncelle
                UpdateTaskUI(taskName);
            }
        }

        private void UpdateTaskUI(string taskName)
        {
            // taskName'e göre UI elementini bulun ve güncelleyin
            foreach (Transform child in taskListParent)
            {
                // "TaskText" adlı TextMeshProUGUI bileşenini bul
                TextMeshProUGUI taskText = child.GetComponentInChildren<TextMeshProUGUI>();
                if (taskText != null && taskText.text == taskName)
                {
                    taskText.text = $"<s>{taskName}</s>"; // Görevin üstünü çiz
                    break;
                }
            }
        }

        public bool IsTaskActive(string taskName)
        {
            // activeTasks listesini kullanarak kontrol et
            return activeTasks.Any(t => t.name == taskName);
        }

        public void HandleObjectCollected()
        {
            remainingTasks--;
            Debug.Log($"Nesne Toplandı! Kalan görev sayısı: {remainingTasks}");

            // Kazanma Durumunu GameManager'a Bildir
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.CheckWinCondition();
            }
            else
            {
                Debug.LogError("GameManager bulunamadı! Kazanma durumu kontrol edilemiyor.");
            }
        }

        #endregion

        #region Scene Management
        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }

        #endregion
    }
}