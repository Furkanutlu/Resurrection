using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FU
{
    public class GameManager : MonoBehaviour
    {
        [Header("Timer")]
        [SerializeField] private float gameTime = 240;
        [SerializeField] private TextMeshProUGUI timerText;
        private bool isGameOver = false;

        [Header("End Game")]
        [SerializeField] private Canvas winCanvas;
        [SerializeField] private Canvas loseCanvas;

        private void Start()
        {
            FindAndAssignCanvases();
        }

        private void Update()
        {
            HandleTimer();
        }

        private void FindAndAssignCanvases()
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>(true);

            foreach (Canvas canvas in canvases)
            {
                if (canvas.CompareTag("WinCanvasTag"))
                {
                    winCanvas = canvas;
                }
                else if (canvas.CompareTag("LoseCanvasTag"))
                {
                    loseCanvas = canvas;
                }
            }

            if (winCanvas == null)
            {
                Debug.LogError("Win Canvas bulunamadı!");
            }
            if (loseCanvas == null)
            {
                Debug.LogError("Lose Canvas bulunamadı!");
            }
        }

        #region Timer Management
        private void HandleTimer()
        {
            if (!isGameOver)
            {
                if (gameTime > 0)
                {
                    gameTime -= Time.deltaTime;
                    UpdateTimerUI();
                }
                else
                {
                    isGameOver = true;
                    EndGame(false);
                }
            }
        }

        private void UpdateTimerUI()
        {
            int minutes = Mathf.FloorToInt(gameTime / 60);
            int seconds = Mathf.FloorToInt(gameTime % 60);

            if (timerText != null)
            {
                timerText.text = $"{minutes:00}:{seconds:00}";
            }
        }
        #endregion

        #region End Game

        private void EndGame(bool isWin)
        {
            isGameOver = true;

            if (isWin)
            {
                winCanvas.gameObject.SetActive(true);
            }
            else
            {
                loseCanvas.gameObject.SetActive(true);
            }

            StartCoroutine(RestartGameAfterDelay(5f));
        }

        private IEnumerator RestartGameAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0); // 0. sahneye geçiş yap
        }

        #endregion

        // Kazanma Durumunu Kontrol Etmek İçin WorldSaveGameManager'dan Çağrılacak Fonksiyon
        public void CheckWinCondition()
        {
            if (WorldSaveGameManager.instance.remainingTasks == 0) // Tüm görevler tamamlandıysa
            {
                EndGame(true);
            }
        }
    }
}
