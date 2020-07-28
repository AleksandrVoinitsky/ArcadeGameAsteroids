using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "InputManager", menuName = "Managers/InputManager")]
    public class InputManager : ManagerBase, IAwake
    {
        [SerializeField] InputType inputType;
        [Space(10)]
        [SerializeField] GameObject[] inputPreset;
        private GameObject currentInput;
        public InputType CurrentType { get; private set; }
        private List<GameObject> inputList;

        public void OnAwake()
        {
            Debug.Log("InputManager active");
            inputList = new List<GameObject>();
            CreateInputPresets();
        }

        public void CreateInputPresets()
        {
            for (int i = 0; i < inputPreset.Length; i++)
            {
                inputList.Add(Instantiate(inputPreset[i]));
                inputList[i].SetActive(false);
            }
            currentInput = inputPreset[(int)inputType];
        }

        public void SetInput()
        {
            for (int i = 0; i < inputPreset.Length; i++)
            {
                inputList[i].SetActive(false);
            }

            currentInput = inputList[(int)inputType];
            currentInput.SetActive(true);
        }

        public GameObject GetInput()
        {
            return currentInput;
        }

        public void SetInputType(InputType Type)
        {
            inputType = Type;
            SetInput();
        }
    }
}
