using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Cards
{
    [DefaultExecutionOrder(1)]
    public class CardMovement : MonoBehaviour
    {
        [Inject] private GameService _gameService;

        private CardData _cardData;

        private Tween _moveTween;
        private Tween _rotateTween;

        private float _moveTime = 0.5f;
        private float _rotateTime = 0.5f;

        public void Move(Vector3 position, float angle)
        {
            _moveTween.Kill();
            _moveTween = transform.DOMove(position, _moveTime);
            Rotate(angle);
        }

        private void Awake()
        {
            _cardData = GetComponent<CardData>();
        }

        private void OnEnable()
        {
            _cardData.OnUpdateState += CheckState;
        }

        private void OnDisable()
        {
            _cardData.OnUpdateState -= CheckState;
        }

        private void Update()
        {
            if (_cardData.State != CardState.Drag)
            {
                return;
            }

            var newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0f;
            transform.position = Vector3.Lerp(transform.position, newPosition, 0.35f);
        }

        private void OnMouseDown()
        {
            if (_cardData.State == CardState.Drop || _gameService.IsSequenceActive)
            {
                return;
            }

            _cardData.State = CardState.Drag;
            Rotate(0f);
        }

        private void OnMouseUp()
        {
            if (_cardData.State == CardState.Drop || _gameService.IsSequenceActive)
            {
                return;
            }

            int layerMask = 1 << 6;
            var overlap = Physics2D.OverlapCircle(transform.position, 0.25f, layerMask);
            if (overlap)
            {
                _cardData.State = CardState.Deck;
            }
            else
            {
                _cardData.State = CardState.Hand;
            }
        }


        private void CheckState()
        {
            if (_cardData.State == CardState.Drop)
            {
                Move(new Vector3(0f, -15f), 0);
            }
        }

        private void Rotate(float angle)
        {
            _rotateTween.Kill();
            _rotateTween = transform.DOLocalRotate(new Vector3(0f, 0f, angle), _rotateTime);
        }
    }
}