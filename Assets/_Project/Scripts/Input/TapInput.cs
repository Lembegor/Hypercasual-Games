using System;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Input
{
    public class TapInput : ITickable
    {
        public event Action Pressed;
        public event Action Released;

        public void Tick()
        {
            Pointer pointer = Pointer.current;

            if (pointer == null)
                return;

            if (pointer.press.wasPressedThisFrame)
                Pressed?.Invoke();

            if (pointer.press.wasReleasedThisFrame)
                Released?.Invoke();
        }
    }
}
