using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

namespace Assets.Scripts
{
    /// <summary>
    /// Класс обработки событий ввода с контроллера для 3D-объектов.
    /// </summary>
    public class VRInput : BaseInput
    {
        /// <summary>
        /// Камера на в начале луча на геймпаде.
        /// </summary>
        public Camera EventCamera = null;

        /// <summary>
        /// Источник событий - геймпад.
        /// </summary>
        public SteamVR_Input_Sources TargetSource = SteamVR_Input_Sources.RightHand;

        /// <summary>
        /// Действие на геймпаде, которое будем отслеживать.
        /// </summary>
        public SteamVR_Action_Boolean ClickAction = null;

        /// <summary>
        /// Инициализация используемых данных.
        /// Перезадаём источник ввода.
        /// </summary>
        protected override void Awake()
        {
            GetComponent<BaseInputModule>().inputOverride = this;
        }

        public override bool GetMouseButton(int button)
        {
            return ClickAction.state;
        }

        public override bool GetMouseButtonUp(int button)
        {
            return ClickAction.stateUp;
        }

        public override bool GetButtonDown(string buttonName)
        {
            return ClickAction.stateDown;
        }

        public override Vector2 mousePosition
        {
            get
            {
                return new Vector2(EventCamera.pixelWidth / 2, EventCamera.pixelHeight / 2);
            }
        }
    }
}
