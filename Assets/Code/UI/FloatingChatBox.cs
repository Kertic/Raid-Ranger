using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class FloatingChatBox : MonoBehaviour
    {
        [SerializeField] private TextMeshPro text;
        [SerializeField] private float timePerCharacter;

        private string _textBuffer;
        private int _displayedIndex;
        private bool _isStillTyping;
        private float _elapsedTimeSinceLastCharacter;

        void Update()
        {
            if (_isStillTyping)
            {
                _elapsedTimeSinceLastCharacter += Time.deltaTime;
                if (_elapsedTimeSinceLastCharacter > timePerCharacter)
                {
                    _displayedIndex++;
                    _elapsedTimeSinceLastCharacter = 0;
                }

                text.text = _textBuffer.Substring(0, _displayedIndex);
                if (_displayedIndex >= _textBuffer.Length)
                {
                    _isStillTyping = false;
                }
            }
        }

        public void SetText(string newText)
        {
            if (newText == _textBuffer)
            {
                return;
            }

            _textBuffer = newText;
            _displayedIndex = 0;
            _elapsedTimeSinceLastCharacter = 0;
            _isStillTyping = true;
        }
    }
}