using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    [System.Serializable]
    public class STrigger
    {
        public EEventData _name;
        public bool _isActive;
        public EEventData[] _activate,_deactivate;
        public bool _hasEnable, _HasDisable;
        public EEventData[] _enableGameObject,_disableGameObject;

        public void SetActive(bool isActive)
        {
            _isActive= isActive;
        }

        public STrigger Clone()
        {
            var _trigger = (STrigger)MemberwiseClone();
            return _trigger;
        }
    }
}
