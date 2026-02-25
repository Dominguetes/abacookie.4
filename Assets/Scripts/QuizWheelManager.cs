using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import para TextMeshPro 

public class QuizWheelManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestionData
    {
        public string question; public string[] options; public int correctAnswer;

        public QuestionData(string q, string[] opts, int correct)
        {
            question = q;
            options = opts;
            correctAnswer = correct;
        }
    }

    [Header("UI References - arraste no Inspector")]
    public Transform wheelTransform;
    public Button spinButton;
    public GameObject questionPanel;
    public TMP_Text questionText;
    public Button[] optionButtons;
    public TMP_Text[] optionTexts;
    public Button backToWheelButton;
    public TMP_Text wheelStatusText;

    [Header("Wheel Settings")]
    public float spinDuration = 3f;

    [Header("Perguntas")]
    public QuestionData[] questions;

    private List<int> availableQuestions = new List<int>();
    private List<int> usedQuestions = new List<int>();
    private bool isSpinning = false;
    private int currentQuestionIndex = -1;


    private List<int> questionNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 10 };
    private List<int> availableQuestionNumbers = new List<int>();

    // Guardar a cor original do painel 
    private Color originalPanelColor;

    void Start()
    {
        InitializeGame();

        if (spinButton != null)
            spinButton.onClick.AddListener(SpinWheel);

        if (backToWheelButton != null)
            backToWheelButton.onClick.AddListener(BackToWheel);

        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => SelectOption(index));
        }

        if (questionPanel != null)
            questionPanel.SetActive(false);

        // Salva a cor original do painel, se tiver componente Image 
        if (questionPanel.TryGetComponent<Image>(out Image panelImage))
        {
            originalPanelColor = panelImage.color;
        }
    }

    void InitializeGame()
    {
        availableQuestionNumbers.Clear();
        availableQuestionNumbers.AddRange(questionNumbers);

        availableQuestions.Clear();
        for (int i = 0; i < questions.Length; i++)
            availableQuestions.Add(i);

        UpdateWheelStatus();
    }

    public void SpinWheel()
    {
        if (isSpinning) return;

        if (availableQuestions.Count == 0)
        {
            wheelStatusText.text = "?? Todas as perguntas foram respondidas! ??";
            spinButton.interactable = false;
            return;
        }

        StartCoroutine(SpinWheelCoroutine());
    }

    IEnumerator SpinWheelCoroutine()
    {
        isSpinning = true;
        spinButton.interactable = false;

        int result = Random.Range(1, 11);

        float startRotation = wheelTransform.eulerAngles.z;
        float targetRotation = startRotation + (360f * 3) + (result * (360f / 10f));

        float elapsed = 0f;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / spinDuration;
            float easeOutProgress = 1f - Mathf.Pow(1f - progress, 3f);
            float currentRotation = Mathf.Lerp(startRotation, targetRotation, easeOutProgress);
            wheelTransform.eulerAngles = new Vector3(0, 0, currentRotation);
            yield return null;
        }

        wheelTransform.eulerAngles = new Vector3(0, 0, targetRotation);
        ProcessWheelResult(result);
        isSpinning = false;
    }

    void ProcessWheelResult(int result)
    {
        if (availableQuestions.Count > 0)
        {
            int questionIndex = Random.Range(0, availableQuestions.Count);
            int actualQuestionIndex = availableQuestions[questionIndex];
            ShowQuestion(actualQuestionIndex);
            availableQuestions.RemoveAt(questionIndex);
            usedQuestions.Add(actualQuestionIndex);
        }
        else
        {
            wheelStatusText.text = "Não há mais perguntas.";
            spinButton.interactable = true;
        }

        UpdateWheelStatus();
    }

    IEnumerator TwoQuestionsMode()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 2 && availableQuestions.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableQuestions.Count);
            int questionIndex = availableQuestions[randomIndex];
            ShowQuestion(questionIndex);
            availableQuestions.RemoveAt(randomIndex);
            usedQuestions.Add(questionIndex);

            yield return new WaitUntil(() => !questionPanel.activeInHierarchy);

            if (i < 1 && availableQuestions.Count > 0)
                yield return new WaitForSeconds(1f);
        }
    }

    void ShowQuestionSelection()
    {
        if (availableQuestions.Count > 0)
        {
            int randomIndex = Random.Range(0, availableQuestions.Count);
            int questionIndex = availableQuestions[randomIndex];
            ShowQuestion(questionIndex);
            availableQuestions.RemoveAt(randomIndex);
            usedQuestions.Add(questionIndex);
        }
        else
        {
            wheelStatusText.text += "\nMas não há mais perguntas!";
            spinButton.interactable = true;
        }
    }

    void ShowQuestion(int questionIndex)
    {
        currentQuestionIndex = questionIndex;
        QuestionData question = questions[questionIndex];

        questionPanel.SetActive(true);
        questionText.text = question.question;

        for (int i = 0; i < optionTexts.Length && i < question.options.Length; i++)
        {
            optionTexts[i].text = question.options[i];
            optionButtons[i].gameObject.SetActive(true);
        }

        for (int i = question.options.Length; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(false);
        }
    }

    void SelectOption(int optionIndex)
    {
        if (currentQuestionIndex < 0) return;

        QuestionData question = questions[currentQuestionIndex];
        bool isCorrect = optionIndex == question.correctAnswer;

        // Muda a cor do painel para verde ou vermelho 
        if (questionPanel.TryGetComponent<Image>(out Image panelImage))
        {
            panelImage.color = isCorrect ? Color.green : Color.red;
        }

        // Mostra a resposta correta se errou 
        if (!isCorrect)
        {
            ColorBlock correctColors = optionButtons[question.correctAnswer].colors;
            correctColors.normalColor = Color.green;
            optionButtons[question.correctAnswer].colors = correctColors;
        }

        foreach (Button btn in optionButtons) ;
        // btn.interactable = false; 
    }

    void BackToWheel()
    {
        // Restaura a cor original do painel 
        if (questionPanel.TryGetComponent<Image>(out Image panelImage))
        {
            panelImage.color = originalPanelColor;
        }

        questionPanel.SetActive(false);
        spinButton.interactable = true;
        currentQuestionIndex = -1;
    }

    void UpdateWheelStatus()
    {
        int remaining = availableQuestions.Count;
        int total = questions.Length;
        wheelStatusText.text = $"Perguntas restantes: {remaining}/{total}";

        if (remaining == 0)
            wheelStatusText.text = "?? Todas as perguntas foram respondidas! ??";
    }

    public void ResetGame()
    {
        availableQuestions.Clear();
        usedQuestions.Clear();
        availableQuestionNumbers.Clear();

        for (int i = 0; i < questions.Length; i++)
            availableQuestions.Add(i);

        availableQuestionNumbers.AddRange(questionNumbers);

        questionPanel.SetActive(false);
        spinButton.interactable = true;
        UpdateWheelStatus();
        wheelTransform.eulerAngles = Vector3.zero;
    }


}

