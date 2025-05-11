using UnityEngine;
using UnityEngine.InputSystem;

namespace AnticTest.Data.Gameplay.Inputs
{
    [CreateAssetMenu(fileName = "New Selection Inputs", menuName = "AnticTest/Data/Gameplay/Inputs/Selection Inputs")]
    public class SelectionInputs : ScriptableObject
    {
        public InputAction leftPointerTapInput;
        public InputAction rightPointerTapInput;
        public InputAction pointerPositionInput;
    }
}
