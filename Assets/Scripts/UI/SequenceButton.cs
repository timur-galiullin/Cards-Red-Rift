using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Cards
{
    [RequireComponent(typeof(Button))]
    public class SequenceButton : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _startSprite;
        [SerializeField] private Sprite _stopSprite;
        [SerializeField] private TextMeshProUGUI _text;

        [Inject] private GameService _gameService;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (_gameService.IsSequenceActive)
                {
                    _gameService.StopSequence();
                }
                else
                {
                    _gameService.StartSequence();
                }
            });
        }

        private void OnEnable()
        {
            _gameService.OnSequenceUpdate += UpdateButton;
        }

        private void OnDisable()
        {
            _gameService.OnSequenceUpdate -= UpdateButton;
        }

        private void UpdateButton()
        {
            if (_gameService.IsSequenceActive)
            {
                _image.sprite = _stopSprite;
                _text.text = "Stop";
            }
            else
            {
                _image.sprite = _startSprite;
                _text.text = "Start";
            }
        }
    }
}