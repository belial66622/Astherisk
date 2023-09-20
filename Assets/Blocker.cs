using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class Blocker : MonoBehaviour
    {
        [SerializeField]GameObject[] Block;
        private void OnDisable()
        {
            TriggerManager.Instance._deactivateSendSignal -= Deactivate;
            TriggerManager.Instance._activeSendSignal -= Activate;

        }
        // Start is called before the first frame update
        void Start()
        {
            TriggerManager.Instance._deactivateSendSignal += Deactivate;
            TriggerManager.Instance._activeSendSignal += Activate;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void Deactivate(EEventData disable)
        {
            foreach (GameObject child in Block)
            {
                if (child.GetComponent<Blockerobject>().DBlock == disable)
                { 
                    child.gameObject.SetActive(false); 
                }
            }
        }


        void Activate(EEventData enable)
        {
            foreach (GameObject child in Block)
            {
                if (child.GetComponent<Blockerobject>().EBlock == enable)
                {
                    child.gameObject.SetActive(true);
                }
            }

        }
    }
}
