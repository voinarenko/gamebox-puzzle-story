using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Toggle))]
    public class MuteToggle : MonoBehaviour
    {
        // Выключатель звука

        [SerializeField] private Sprite[] _images;      // массив изображений
        private Toggle _toggle;                         // переключатель звука

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            GetComponent<Image>().sprite = _images[1];
            _toggle = GetComponent<Toggle>();
            _toggle.isOn = true;
            if (AudioListener.volume == 0)
            {
                _toggle.isOn = false;
                GetComponent<Image>().sprite = _images[0];
            }
        }

        /// <summary>
        /// Метод включения/выключения звука
        /// </summary>
        /// <param name="audioIn"></param>
        public void ToggleAudioOnValueChange(bool audioIn)
        {
            if (audioIn)
            {
                AudioListener.volume = 1;
                GetComponent<Image>().sprite = _images[1];
            }
            else
            {
                AudioListener.volume = 0;
                GetComponent<Image>().sprite = _images[0];
            }
        }
    }
}
