using System;
using System.Security.Cryptography;
using System.Xml.Schema;
using UnityEngine;

namespace ThePatient
{
    public class LockObject : InspectPickup
    {
        [Header("Lock Parameter")]
        [SerializeField] int password;

        [SerializeField] Transform[] numberRolls;

        NumberRoll selectedRoll;

        NumberRoll[] numberRollsArray;

        class NumberRoll
        {
            public Transform transform;
            public int number;

            public NumberRoll(Transform transform, int number)
            {
                this.transform = transform;
                this.number = number;
            }

            public void UpdateMaterial(int active, float value)
            {
               transform.GetComponent<Renderer>().material.SetFloat("_Fresnel_Power", value);
               transform.GetComponent<Renderer>().material.SetInt("_Use_Fresnel", active);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OnInspectExit += CheckPassword;
            _input.InspectExit += ExitInspect;
            _input.SelectUp += SelectUp;
            _input.SelectDown += SelectDown;
            _input.NumberLeft += NumberLeft;
            _input.NumberRight += NumberRight;
            
            SetupLockPuzzle();
        }

        protected override void OnDisable()
        {
            OnInspectExit -= CheckPassword;
            _input.InspectExit -= ExitInspect;
            _input.SelectUp -= SelectUp;
            _input.SelectDown -= SelectDown;
            _input.NumberLeft -= NumberLeft;
            _input.NumberRight -= NumberRight;
        }

        void UpdateSelectedRollMaterial()
        {
            foreach(var roll in numberRollsArray) 
            {
                roll.UpdateMaterial(1, 100);
            }
            selectedRoll.UpdateMaterial(1, .5f);
        }

        void CheckPassword(string audio)
        {
            if(AppendRollNumber() == password)
            {
                Debug.Log("Password Correct");
                // Play Unlock Sound
                audio = "KeyPickup";
                Pickup(audio);
            }
            else
            {
                // play incorrect sound
                Debug.Log("Password Incorrect");
            }
        }

        int AppendRollNumber()
        {
            int appendedNumber = 0;
            foreach(var number in numberRollsArray)
            {
                int newNumber = int.Parse(appendedNumber.ToString() + number.number.ToString());
                appendedNumber = newNumber;
            }

            return appendedNumber;
        }

        int GetSelectedIndex()
        {
            for (int i = 0; i < numberRollsArray.Length; i++)
            {
                if (numberRollsArray[i] == selectedRoll)
                {
                    return i;
                }
            }

            return 0;
        }

        void SetupLockPuzzle()
        {
            numberRollsArray = new NumberRoll[numberRolls.Length];
            
            for (int i = 0; i < numberRolls.Length; i++)
            {
                numberRollsArray[i] = new NumberRoll(numberRolls[i], 0);
            }

            selectedRoll = numberRollsArray[0];
            UpdateSelectedRollMaterial();
        }

        private void SelectDown()
        {
            int index = GetSelectedIndex() + 1;
            if (index > numberRollsArray.Length - 1)
            {
                index = 0;
            }
            selectedRoll = numberRollsArray[index];
            UpdateSelectedRollMaterial();
        }

        private void SelectUp()
        {
            int index = GetSelectedIndex() - 1;
            if (index < 0)
            {
                index = numberRollsArray.Length - 1;
            }
            selectedRoll = numberRollsArray[index];
            UpdateSelectedRollMaterial();
        }
        private void NumberRight()
        {
            selectedRoll.transform.Rotate(Vector3.up, 30f, Space.Self);
            selectedRoll.number++;
            if(selectedRoll.number > 11)
            {
                selectedRoll.number = 0;
            }
        }

        private void NumberLeft()
        {
            selectedRoll.transform.Rotate(Vector3.up, -30f, Space.Self);
            selectedRoll.number--;
            if (selectedRoll.number < 0)
            {
                selectedRoll.number = 11;
            }
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnInspectEventStart()
        {
            InteractableManager.Instance.OnInteractionInspect(new InteractionLockPuzzleEventArgs(true));
            //InteractableManager.Instance.OnInteractionInspect(new InteractionIconEventArgs(true, $"Hint : {password}"));
        }

        protected override void OnInspectEventExit()
        {
            InteractableManager.Instance.OnInteractionInspect(new InteractionLockPuzzleEventArgs(false));
            //InteractableManager.Instance.OnInteractionInspect(new InteractionIconEventArgs(false, $""));
        }

        // Interface implementation

        public override bool Interact()
        {
            Inspect();
            selectedRoll = numberRollsArray[0];
            UpdateSelectedRollMaterial();
            _input.EnableLockPuzzleControl();
            return true;
        }

        public override void OnFinishInteractEvent()
        {
            EventAggregate<InteractionIconEventArgs>.Instance.TriggerEvent(new InteractionIconEventArgs(false, InteractionType.Default));
        }

        public override void OnInteractEvent()
        {
            EventAggregate<InteractionIconEventArgs>.Instance.TriggerEvent(
                new InteractionIconEventArgs(true, InteractionType.Inspect));
        }
    }
}
