using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

namespace UI
{
    public class Dialog : MonoBehaviour
    {
        [Header("Groups")]
        [SerializeField] private CanvasGroup interactionText;
        [SerializeField] private CanvasGroup dialogContainer;
        
        [Header("Text Properties")]
        [SerializeField] private TMP_Text dialogText;
        [SerializeField] private Color specialDialogColor;

        private bool _hasInteracted = false;
        
        private string _currentText = "Esto es un @texto@ de prueba";
        private int _currentPosition = 0;
        private Coroutine _writeCo;
        
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.E)) return;
            _hasInteracted = true;
            interactionText.DOFade(0.0f, 0.5f);
            dialogContainer.DOFade(1.0f, 0.5f);
            StartDialog(0.05f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            interactionText.DOFade(1.0f, 0.5f);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            interactionText.DOFade(0.0f, 0.5f);
        }
        
        private IEnumerator WriteByLetter (float delay)
        {
            var textStyle = false;
            var ignoreStyle = false;

            while (_currentPosition < _currentText.Length)
            {
                string nextLetter = _currentText[_currentPosition++].ToString();

                if (nextLetter == "@")
                {
                    ignoreStyle = true;
                    textStyle = !textStyle;
                }

                if (!ignoreStyle) 
                {
                    var hexColor = ColorUtility.ToHtmlStringRGB(specialDialogColor);
                    
                    if (textStyle)
                        nextLetter = "<color=#"+hexColor+">"+nextLetter+"</color>";

                    dialogText.text += nextLetter;
                }

                ignoreStyle = false;
                yield return new WaitForSeconds(delay);
            }
            
            yield return new WaitForSeconds(2.0f);
            ResetDialog();
        }

        private void ResetDialog()
        {
            _hasInteracted = false;
            dialogText.text = "";
            _currentPosition = 0;
            
            dialogContainer.DOFade(0.0f, 0.3f);
        }

        public void StartDialog(float delay)
        {
            ResetDialog();
            
            if(_writeCo != null) StopCoroutine(_writeCo);
            _writeCo = StartCoroutine(WriteByLetter(delay));
        }
    }
}
