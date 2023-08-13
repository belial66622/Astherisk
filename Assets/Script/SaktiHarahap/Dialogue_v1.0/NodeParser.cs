using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XNode;

public class NodeParser : MonoBehaviour
{
    public DialogueGraph graph;
    Coroutine _parser;
    public TextMeshProUGUI speaker;
    public TextMeshProUGUI dialogue;
    public Image speakerImage;

    [SerializeField] int currentDialogue;

    [SerializeField] GameObject dialogueCanva;
    void Start()
    {
        foreach (BaseNode b in graph.nodes)
        {
            if(b.GetString() == "Start")
            {
                graph.current = b; 
                break;
            }
        }
            _parser = StartCoroutine(ParseNode());

        Debug.Log("total node = " + graph.nodes.Count);
    }

    IEnumerator ParseNode()
    {
        BaseNode baseNode = graph.current;
        string data = baseNode.GetString();
        string[] dataParts = data.Split('/');
        if (dataParts[0] == "Start")
        {
            NextNode("exit");
        }
        if (dataParts[0] == "DialogueNode")
        {
            speaker.text = dataParts[1];
            dialogue.text = dataParts[2];
            yield return new WaitUntil (() => Input.GetMouseButtonDown(0));
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
            currentDialogue++;
            int totalNodes = graph.nodes.Count - 1;

            if(currentDialogue < totalNodes)
            {
                 NextNode("exit");
            }
            else
            {
                dialogueCanva.SetActive(false);
            }
        }
    }

    public void NextNode(string fieldName)
    {
        if(_parser != null)
        {
            StopCoroutine(_parser);
            _parser = null;
        }
        foreach (NodePort p in graph.current.Ports) 
        {
            if(p.fieldName == fieldName)
            {
                graph.current = p.Connection.node as BaseNode;
                break;
            }
        }
        _parser = StartCoroutine(ParseNode());
    }

}
