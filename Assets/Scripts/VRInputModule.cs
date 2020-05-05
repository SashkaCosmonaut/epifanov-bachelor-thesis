using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

namespace Assets.Scripts
{
    /// <summary>
    /// Класс обработки событий взаимодействия с окружением.
    /// </summary>
    public class VRInputModule : BaseInputModule
    {
        /// <summary>
        /// Камера на в начале луча на геймпаде.
        /// </summary>
        public Camera Camera;

        public SteamVR_Input_Sources TargetSource;

        public SteamVR_Action_Boolean ClickAction;

        private GameObject CurrentObject = null;

        private PointerEventData Data = null;

        protected override void Awake()
        {
            base.Awake();

            Data = new PointerEventData(eventSystem);
        }

        public PointerEventData GetEventData()
        {
            return Data;
        }

        public override void Process()
        {
            // Reset data
            Data.Reset();
            Data.position = new Vector2(Camera.pixelWidth / 2, Camera.pixelHeight / 2);

            // Raycast
            eventSystem.RaycastAll(Data, m_RaycastResultCache);
            Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
            CurrentObject = Data.pointerCurrentRaycast.gameObject;

            // Clear raycast
            m_RaycastResultCache.Clear();

            // Hover
            HandlePointerExitAndEnter(Data, CurrentObject);

            // Press
            if (ClickAction.GetStateDown(TargetSource))
                ProcessPress(Data);

            // Release
            if (ClickAction.GetStateUp(TargetSource))
                ProcessRelease(Data);
        }


        private void ProcessPress(PointerEventData data)
        {
            Data.pointerPressRaycast = Data.pointerCurrentRaycast;

            var newPointerPress = ExecuteEvents.ExecuteHierarchy(CurrentObject, data, ExecuteEvents.pointerDownHandler);

            if (newPointerPress == null)
                newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(CurrentObject);

            data.pressPosition = data.position;
            data.pointerPress = newPointerPress;
            data.rawPointerPress = CurrentObject;
        }

        private void ProcessRelease(PointerEventData data)
        {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

            var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(CurrentObject);

            if (Data.pointerPress == pointerUpHandler)
                ExecuteEvents.Execute(Data.pointerPress, Data, ExecuteEvents.pointerClickHandler);

            eventSystem.SetSelectedGameObject(null);

            data.pressPosition = Vector2.zero;
            data.pointerPress = null;
            data.rawPointerPress = null;
        }
    }
}
