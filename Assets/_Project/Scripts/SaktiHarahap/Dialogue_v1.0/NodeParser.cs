using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XNode;
using ThePatient;
using UnityEngine.Events;

public class NodeParser : InspectInteractable
{
    public DialogueGraph graph;
    Coroutine _parser;
    public TextMeshProUGUI speaker;
    public TextMeshProUGUI dialogue;
    public Image speakerImage;

    [SerializeField] int currentDialogue;

    [SerializeField] GameObject dialogueCanva;

    [SerializeField] UnityEvent OnDialogueEnd;
    IEnumerator ParseNode()
    {
        BaseNode baseNode = graph.current;
        string data = baseNode.GetString();
        string[] dataParts = data.Split('/');
        if (dataParts[0] == "Start")
        {
            dialogueCanva.SetActive(true);
            gameObject.GetComponent<Collider>().enabled = false;
            _input.DisablePlayerControll();
            _input.EnableDialogueControll();
            currentDialogue = 0;
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
                gameObject.GetComponent<Collider>().enabled = true;
                _input.EnablePlayerControll();
                _input.DisableDialogueControll();
                dialogueCanva.SetActive(false);

                OnDialogueEnd?.Invoke();
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

    public override bool Interact()
    {
        foreach (BaseNode b in graph.nodes)
        {
            if (b.GetString() == "Start")
            {
                graph.current = b;
                break;
            }
        }
        _parser = StartCoroutine(ParseNode());
        return false;
    }

    public override void OnFinishInteractEvent()
    {
        EventAggregate<InteractionTextEventArgs>.Instance.TriggerEvent(new InteractionTextEventArgs(false, ""));
    }

    public override void OnInteractEvent(string name)
    {
        EventAggregate<InteractionTextEventArgs>.Instance.TriggerEvent(
            new InteractionTextEventArgs(true, "[ E ]\nSpeak To " + name));
    }
}
