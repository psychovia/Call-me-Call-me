using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic; 
using System.Text;  
using TMPro;

public class ScriptManager : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string fullScriptText = "My mother came from the south of this country. In her words, her hometown was a place where it never snowed, all year round. The streets were lined with palm trees. Some of them—as my mom claimed—were even older than her mother. She said the beaches there were piled high with white shining sand, and the sea was as green as an emerald. She said that in summer, the hillsides would burst into bloom with flowers, like an ocean of flowers. \"An ocean of flowers? Really?\" I'd always joke at this point. \"Well, that's lucky. Good thing no one in our family has pollen allergies.\" And that's when she would always launch into the story of how she and Dad drove for five straight days, moving the whole family north. Before I grew up and left home, we had this conversation countless times. Mostly, it was just me and Mom's chitchat. My dad would join in every now and then. And every single time, the conversation would end with her saying, \"You'll have to see it for yourself one day. It's nothing like the pictures.\" But I knew the truth. And I think my mom knew it, too. I was never really going to see it. And that was the absolute truth. But it's not surprising, actually. It wasn't just me. None of the kids in school had much interest in the dirt under their feet at that time. After all... it's not Earth's era anymore.";


    public string[] lines;
    public float textSpeed;

    private int index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lines = SplitIntoDialogueLines(fullScriptText, 50);

        textComponent.text = string.Empty;
        StartDialogue();        
    }

    // Update is called once per frame
    void Update()
    {
        //will change later to more complicated next line
        bool clicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        if (clicked)
        {
            if(textComponent.text == lines[index])
            {
                Debug.Log("Attempting to go to the next line");
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }
        string[] SplitIntoDialogueLines(string text, int maxWords)
    {
        if (string.IsNullOrWhiteSpace(text))
            return new string[0];

        List<string> sentences = SplitIntoSentences(text);
        List<string> lines = new List<string>();

        StringBuilder currentLine = new StringBuilder();
        int currentWordCount = 0;

        foreach (string sentence in sentences)
        {
            // count words in this sentence
            int sentenceWordCount = CountWords(sentence);

            // Edge case: single sentence longer than maxWords
            if (sentenceWordCount > maxWords)
            {
                //split this long sentence by words
                string[] chunks = SplitLongSentence(sentence, maxWords);
                foreach (string chunk in chunks)
                {
                    if (currentWordCount == 0)
                    {
                        currentLine.Append(chunk);
                        currentWordCount = CountWords(chunk);
                    }
                    else if (currentWordCount + CountWords(chunk) <= maxWords)
                    {
                        currentLine.Append(" ");
                        currentLine.Append(chunk);
                        currentWordCount += CountWords(chunk);
                    }
                    else
                    {
                        lines.Add(currentLine.ToString().Trim());
                        currentLine.Clear();
                        currentLine.Append(chunk);
                        currentWordCount = CountWords(chunk);
                    }
                }
                continue;
            }

            //if this sentence fits in the current line, append it
            if (currentWordCount == 0 || currentWordCount + sentenceWordCount <= maxWords)
            {
                if (currentWordCount > 0)
                    currentLine.Append(" ");

                currentLine.Append(sentence.Trim());
                currentWordCount += sentenceWordCount;
            }
            else
            {
                //finish current line and start a new one
                lines.Add(currentLine.ToString().Trim());
                currentLine.Clear();
                currentLine.Append(sentence.Trim());
                currentWordCount = sentenceWordCount;
            }
        }

        //add the last line if there's leftover
        if (currentLine.Length > 0)
        {
            lines.Add(currentLine.ToString().Trim());
        }

        return lines.ToArray();
    }

    List<string> SplitIntoSentences(string text)
    {
        List<string> sentences = new List<string>();
        StringBuilder sb = new StringBuilder();

        int i = 0;
        while (i < text.Length)
        {
            char c = text[i];
            sb.Append(c);

            // Check for sentence-ending punctuation
            if (c == '.' || c == '?' || c == '!')
            {
                // Handle "..." as a single sentence end
                if (c == '.' && i + 2 < text.Length && text[i + 1] == '.' && text[i + 2] == '.')
                {
                    sb.Append(".."); // we already appended one '.', add two more
                    i += 2;
                }

                string sentence = sb.ToString().Trim();
                if (!string.IsNullOrEmpty(sentence))
                {
                    sentences.Add(sentence);
                }

                sb.Clear();
            }

            i++;
        }

        string leftover = sb.ToString().Trim();
        if (!string.IsNullOrEmpty(leftover))
        {
            sentences.Add(leftover);
        }

        return sentences;
    }

    int CountWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        string[] words = text.Split(
            new char[] { ' ', '\t', '\n', '\r' },
            System.StringSplitOptions.RemoveEmptyEntries
        );

        return words.Length;
    }

    string[] SplitLongSentence(string sentence, int maxWords)
    {
        string[] words = sentence.Split(
            new char[] { ' ', '\t', '\n', '\r' },
            System.StringSplitOptions.RemoveEmptyEntries
        );

        List<string> chunks = new List<string>();
        StringBuilder sb = new StringBuilder();
        int count = 0;

        foreach (string word in words)
        {
            if (count >= maxWords)
            {
                chunks.Add(sb.ToString().Trim());
                sb.Clear();
                count = 0;
            }

            if (sb.Length > 0)
                sb.Append(" ");

            sb.Append(word);
            count++;
        }

        if (sb.Length > 0)
        {
            chunks.Add(sb.ToString().Trim());
        }

        return chunks.ToArray();
    }


    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length-1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
