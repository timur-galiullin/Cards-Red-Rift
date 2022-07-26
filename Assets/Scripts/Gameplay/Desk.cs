using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cards
{
    public class Desk : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private float _betweenCardDistance;

        private List<CardController> _cards;

        public Action OnSort { get; set; }

        public void SetCards(List<CardController> cards)
        {
            _cards = cards;
            foreach (var card in _cards)
            {
                card.Data.OnUpdateState += Sort;
            }
        }

        private void Sort()
        {
            if (_cards.Count(x => x.Data.State == CardState.Drag) > 0)
            {
                _renderer.gameObject.SetActive(true);
            }
            else
            {
                _renderer.gameObject.SetActive(false);
            }

            var cards = _cards.Where(x => x.Data.State == CardState.Deck).ToList();
            cards = cards.OrderBy(x => x.transform.position.x).ToList();
            var left = (0.5f - cards.Count / 2f) * _betweenCardDistance;
            for (int i = 0; i < cards.Count; i++)
            {
                var x = left + i * _betweenCardDistance;
                cards[i].Movement.Move(transform.position + new Vector3(x, 0f), 0);
            }

            OnSort?.Invoke();
        }
    }
}