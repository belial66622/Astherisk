using UnityEngine;

namespace ThePatient
{
    public class TestSave : MonoBehaviour, ISaveable
    {
        public object CaptureState()
        {
            return name;
        }

        public void RestoreState(object state)
        {
            state = (string)state;
            Debug.Log(state);
        }
    }
}