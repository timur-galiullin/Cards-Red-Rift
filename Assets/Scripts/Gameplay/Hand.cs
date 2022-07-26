using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cards
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] [Range(0f, 10f)] private float _height;
        [SerializeField] [Range(0f, 10f)] private float _width;
        [SerializeField] [Range(0f, 10f)] private float _betweenCardDistance;
        [SerializeField] [Range(0f, 1f)] private float _angleModifier;

        private List<CardController> _cards;

        public Action OnSort { get; set; }

        public void SetCards(List<CardController> cards)
        {
            _cards = cards;
            foreach (var card in _cards)
            {
                card.transform.SetParent(transform);
                card.Data.State = CardState.Hand;
                card.Data.OnUpdateState += SortHand;
            }

            SortHand();
        }

        private void SortHand()
        {
            var cards = _cards.Where(x => x.Data.State == CardState.Hand).ToList();
            cards = cards.OrderBy(x => x.transform.position.x).ToList();
            var left = (0.5f - cards.Count / 2f) * _betweenCardDistance;
            for (int i = 0; i < cards.Count; i++)
            {
                var x = left + i * _betweenCardDistance;
                var y = Mathf.Sqrt((1 - x * x / (_width * _width)) * _height * _height);
                var angle = -Mathf.Atan2(x, y) * Mathf.Rad2Deg * _angleModifier;
                cards[i].Movement.Move(transform.position + new Vector3(x, y), angle);
                cards[i].Data.Index = i;
            }

            OnSort?.Invoke();
        }
    }
}