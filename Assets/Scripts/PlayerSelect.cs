using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    [Header("СТАТИЧНЫЕ персонажи для меню")]
    public GameObject[] menuCharacters; // Только модели без скриптов

    [Header("Текст имени персонажа")]
    public Text nameText;

    [Header("Кнопки")]
    public Button nextBtn;
    public Button prevBtn;
    public Button selectBtn;
    public Button startBtn; // Кнопка "Начать игру"

    [Header("Имена персонажей")]
    public string[] names = { "Воин", "Маг", "Лучник" };

    [Header("Настройки сцен")]
    public string gameSceneName = "MainLevel";

    private int currentIndex = 0;

    void Start()
    {
        // Загружаем сохраненный выбор
        if (PlayerPrefs.HasKey("SelectedCharacter"))
        {
            currentIndex = PlayerPrefs.GetInt("SelectedCharacter");
        }

        InitMenu();
        SetupButtons();
    }

    void InitMenu()
    {
        if (menuCharacters == null || menuCharacters.Length == 0)
        {
            Debug.LogWarning("Нет персонажей для меню!");
            return;
        }

        // Скрываем всех и показываем текущего
        foreach (GameObject character in menuCharacters)
        {
            if (character != null)
                character.SetActive(false);
        }

        ShowCurrentCharacter();
    }

    void SetupButtons()
    {
        if (nextBtn != null)
            nextBtn.onClick.AddListener(NextCharacter);

        if (prevBtn != null)
            prevBtn.onClick.AddListener(PreviousCharacter);

        if (selectBtn != null)
            selectBtn.onClick.AddListener(SelectCharacter);

        if (startBtn != null)
            startBtn.onClick.AddListener(StartGame);
    }

    void ShowCurrentCharacter()
    {
        // Скрываем всех персонажей
        for (int i = 0; i < menuCharacters.Length; i++)
        {
            if (menuCharacters[i] != null)
                menuCharacters[i].SetActive(false);
        }

        // Показываем текущего
        if (menuCharacters[currentIndex] != null)
        {
            menuCharacters[currentIndex].SetActive(true);
        }

        // Обновляем имя
        UpdateNameText();
    }

    void UpdateNameText()
    {
        if (nameText != null)
        {
            if (names != null && currentIndex < names.Length)
            {
                nameText.text = names[currentIndex];
            }
            else
            {
                nameText.text = "Персонаж " + (currentIndex + 1);
            }
        }
    }

    public void NextCharacter()
    {
        currentIndex++;
        if (currentIndex >= menuCharacters.Length)
            currentIndex = 0;
        ShowCurrentCharacter();
    }

    public void PreviousCharacter()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = menuCharacters.Length - 1;
        ShowCurrentCharacter();
    }

    public void SelectCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacter", currentIndex);
        PlayerPrefs.Save();

        ShowConfirmation("Выбрано!");
        Debug.Log("Сохранен персонаж: " + currentIndex);
    }

    public void StartGame()
    {
        // Убеждаемся что выбор сохранен
        if (!PlayerPrefs.HasKey("SelectedCharacter"))
        {
            PlayerPrefs.SetInt("SelectedCharacter", currentIndex);
            PlayerPrefs.Save();
        }

        Debug.Log("Запуск игры с персонажем: " + PlayerPrefs.GetInt("SelectedCharacter"));

        // Загружаем игровую сцену
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("Название сцены не указано!");
        }
    }

    void ShowConfirmation(string message)
    {
        if (nameText != null)
        {
            string originalText = nameText.text;
            nameText.text = message;
            Invoke("RestoreName", 1f);
        }
    }

    void RestoreName()
    {
        UpdateNameText();
    }
}