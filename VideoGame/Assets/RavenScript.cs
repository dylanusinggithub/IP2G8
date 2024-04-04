using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RavenScript : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;
    private int currentLine = 0;
    private bool dialogueActive = false;

    [Header("Interaction Settings")]
    public GameObject interactPrompt;
    public float interactionRange = 3f;
    private Transform player;

    public float letterSpeed = 0.025f;

    private bool skipPrinting = false;

    private PlayerControls playerControls;

    void Start()
    {
        dialogueText.gameObject.SetActive(false);
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerControls = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= interactionRange && !dialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartDialogue();
            }
            interactPrompt.SetActive(true);
        }
        else
        {
            if (dialogueActive && Input.GetKeyDown(KeyCode.Space))
            {
                skipPrinting = true;
            }
            interactPrompt.SetActive(false);
        }


    }

    void StartDialogue()
    {
        dialogueActive = true;
        dialogueText.gameObject.SetActive(true);
        interactPrompt.SetActive(false);
        currentLine = 0;
        StartCoroutine(PrintDialogue(dialogueLines[currentLine]));
    }

    IEnumerator PrintDialogue(string text)
    {
        dialogueText.text = "";
        skipPrinting = false;

        for (int i = 0; i < text.Length; i++)
        {
            dialogueText.text += text[i];
            if (!skipPrinting)
            {
                yield return new WaitForSeconds(letterSpeed);
            }
        }

        if (currentLine < dialogueLines.Length - 1) 
        {
            dialogueText.text = text; 
            currentLine++;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            StartCoroutine(PrintDialogue(dialogueLines[currentLine]));
        }
        else
        {
            dialogueText.text = text; 
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            EndDialogue();
        }
    }


    void EndDialogue()
    {
        dialogueActive = false;
        dialogueText.gameObject.SetActive(false);
    }
}
