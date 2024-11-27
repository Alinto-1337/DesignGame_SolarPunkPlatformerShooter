using UnityEngine;

namespace PunkPlatformerGame
{
    [CreateAssetMenu(fileName = "InputManagerSO", menuName = "SingletonSO/InputManagerSO")]
    public class InputManagerSO : ScriptableObject
    {
        InputSystem_Actions inputActions;

        private void OnValidate()
        {
            inputActions = new InputSystem_Actions();
        }

        // On Move Event..
    }
}
