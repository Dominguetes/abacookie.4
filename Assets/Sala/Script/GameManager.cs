using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // para trocar de cena


using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Sprite bgImage;
    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new List<Sprite>();
    public List<Button> btns = new List<Button>();
    private bool firstGuess, secondGuess;
    private int firstGuessIndex, secondGuessIndex;
    private int countGuesses; // total de tentativas (quando o jogador vira o segundo cartão)
    private int countCorrectGuesses;
    private int gameGuesses; // número de pares
    private string firstGuessPuzzle, secondGuessPuzzle;

    [Header("UI Win Popup")]
    public GameObject GameWinPopUp;          // painel que aparece ao ganhar
    public TextMeshProUGUI winTitleText;                // "VOCÊ GANHOU!" (opcional)
    public TextMeshProUGUI attemptsText;                // texto que mostra as tentativas
    public Button backToMenuButton;          // botão para voltar à tela inicial

    private void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("materials/jogo-da-memoria");
    }
    void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);

        gameGuesses = gamePuzzles.Count / 2;
        GameWinPopUp.SetActive(false);

        // Se quiser ligar o botão de voltar ao menu por código:
        if (backToMenuButton != null)
            backToMenuButton.onClick.AddListener(BackToMainMenu);
    }
    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("puzzleBtn");
        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    void AddGamePuzzles()
    {
        int looper = btns.Count;
        int index = 0;
        for (int i = 0; i < looper; i++)
        {
            if (index == looper / 2)
            {
                index = 0;
            }
            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }
    void AddListeners()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => PickPuzzle());
        }
    }
    public void PickPuzzle()
    {
        // pega o nome do objeto clicado (assumindo que o nome é o índice)
        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;
            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            // Conta uma tentativa sempre que virar o segundo cartão
            countGuesses++;

            if (firstGuessPuzzle == secondGuessPuzzle)
            {
                Debug.Log("puzzles Match");
            }
            else
            {
                Debug.Log("Puzzle don't match");
            }
            StartCoroutine(checkThePuzzleMatch());
        }
    }
    IEnumerator checkThePuzzleMatch()
    {
        yield return new WaitForSeconds(0.2f);

        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(0.2f);
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            // Deixar transparentes (ou destrua se preferir)
            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

            CheckTheGameFinished();
        }
        else
        {
            // volta para o fundo
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }
        yield return new WaitForSeconds(0.5f);
        firstGuess = secondGuess = false;
    }
    void CheckTheGameFinished()
    {
        countCorrectGuesses++;

        if (countCorrectGuesses == gameGuesses)
        {
            Debug.Log("game finish");
            ShowWinPopup();
            Debug.Log("It took you " + countGuesses + " attempts");
        }
    }

    void ShowWinPopup()
    {
        if (GameWinPopUp != null)
        {
            GameWinPopUp.SetActive(true);

            if (winTitleText != null)
                winTitleText.text = "VOCÊ GANHOU!";

            if (attemptsText != null)
                attemptsText.text = "Tentativas: " + countGuesses.ToString();
        }
    }

    // Método público que pode ser ligado ao botão de voltar no inspector
    public void BackToMainMenu()
    {
        // troque "MainMenu" pelo nome da cena inicial do seu projeto
        SceneManager.LoadScene("MainMenu");
    }

    public void NextBtnClick()
    {
        Debug.Log("next Click");
    }
    public void RetryBtnClick()
    {
        Debug.Log("retry click");
    }
    void Shuffle(List<Sprite> List)
    {
        for (int i = 0; i < List.Count; i++)
        {
            Sprite temp = List[i];
            int randomIndex = Random.Range(i, List.Count);
            List[i] = List[randomIndex];
            List[randomIndex] = temp;

        }
    }

}
