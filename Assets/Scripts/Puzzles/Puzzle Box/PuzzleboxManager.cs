using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleboxManager : MonoBehaviour
{
    //Singleton - There should only be one puzzlebox in the scene
    public static PuzzleboxManager instance;

    [SerializeField] List<PuzzleboxPanel> panels = new List<PuzzleboxPanel>();
    [SerializeField] FadeObject lid;
    public int step;

    bool interactable = true;

    bool startDisabled = true;

    //Victory rotation
    [SerializeField] Vector3 victoryRotation = new Vector3();

    //Reward
    [SerializeField] InventoryItem rewardItem;

    //Sounds
    [SerializeField] List<AudioClip> interactSounds = new List<AudioClip>();
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("PuzzleboxManager initialized!");
            instance = this;
            //Shouldn't persist between scenes
            // DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < panels.Count; i++) {
            panels[i].setPuzzleboxManager(this, i);
        }
        if (startDisabled) {
            this.gameObject.SetActive(false);
        }
    }

    // OnDestroy, clear the singleton
    void OnDestroy()
    {
        Debug.Log("PuzzleboxManager destroyed!");
        if (instance == this)
        {
            Debug.Log("PuzzleboxManager de-initialized!");
            instance = null;        
        }
    }

    //Call this to start a puzzle in the scene
    public static void startPuzzle()
    {
        if (instance == null)
        {
            Debug.LogError("PuzzleboxManager.startPuzzle(): no instance in scene");
            return;
        }
        instance.gameObject.SetActive(true);
        GamestateManager.setState(GamestateManager.Gamestate.inMenu);
    }

    public static void quitPuzzle()
    {
        if (instance == null)
        {
            Debug.LogError("PuzzleboxManager.startPuzzle(): no instance in scene");
            return;
        }
        instance.gameObject.SetActive(false);
        GamestateManager.setState(GamestateManager.Gamestate.firstPerson);
    }
    
    public bool panelInteract(int index)
    {
        if (!interactable)
            return false;
        
        if (index == step)
        {
            playInteractSound();
            step++;
            checkWin();
            return true;
        }
        else if (index == (step - 1))
        {
            playInteractSound();
            step--;
            //checkWin() unnecessary
            return true;
        }

        return false;
    }

    public bool checkWin()
    {
        if (step >= panels.Count)
        {
            Debug.Log("Puzzlebox solved! Awarding key...");
            interactable = false;
            StartCoroutine(winSequence());
            return true;
        }
        return false;
    }

    public IEnumerator winSequence()
    {
        lid.FadeOut();

        float elapsedTime = 0.0f;
        float winDuration = 5.0f;
        while (elapsedTime < winDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / winDuration);

            yield return null;
        }
        playWinSound();
        this.gameObject.SetActive(false);
        GamestateManager.setState(GamestateManager.Gamestate.firstPerson);
        if (rewardItem != null)
            InventoryManager.addItem(rewardItem);
        InventoryManager.removeItem("puzzleboxItem", 1);
    }

    void playInteractSound()
    {
        int randomValue = Random.Range(0, interactSounds.Count);
        audioSource.PlayOneShot(interactSounds[randomValue]);
    }

    void playWinSound()
    {
        audioSource.PlayOneShot(winSound);
    }
}
