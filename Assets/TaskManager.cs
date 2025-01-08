using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FU
{
    public class TaskManager : MonoBehaviour
    {
        [System.Serializable]
        public class Task
        {
            public string name;
            public string description;
            public bool isCompleted;
        }

        public List<Task> allTasks = new List<Task>
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
        new Task { name = "Azrail'in Tırpanı", description = "Azrail'in tırpanı, köyün karlı dağlara bakan tarafında bekliyor.", isCompleted = false }
    };

        public List<Task> activeTasks = new List<Task>();

        void Start()
        {
            DisplayRandomTasks(5);
        }

        void DisplayRandomTasks(int count)
        {
            // Shuffle the task list
            List<Task> shuffledTasks = new List<Task>(allTasks);
            for (int i = 0; i < shuffledTasks.Count; i++)
            {
                Task temp = shuffledTasks[i];
                int randomIndex = Random.Range(0, shuffledTasks.Count);
                shuffledTasks[i] = shuffledTasks[randomIndex];
                shuffledTasks[randomIndex] = temp;
            }

            // Select the first `count` tasks
            activeTasks = shuffledTasks.GetRange(0, Mathf.Min(count, shuffledTasks.Count));

            // Display the tasks in the console
            foreach (Task task in activeTasks)
            {
                Debug.Log($"Task: {task.name}, Description: {task.description}, Completed: {task.isCompleted}");
            }
        }
    }
}
