using System;
using UnityEngine;

namespace Cards
{
    public enum CardState
    {
        Hand,
        Drag,
        Deck,
        Drop
    }

    public class CardData : MonoBehaviour
    {
        private CardState _state;
        private int _hp;

        public int Hp
        {
            get => _hp;
            set
            {
                _hp = value;
                OnUpdateHp?.Invoke();
            }
        }

        public CardState State
        {
            get => _state;
            set
            {
                _state = value;
                OnUpdateState?.Invoke();
            }
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public int Attack { get; set; }
        public int Mana { get; set; }

        public int Index { get; set; }

        public Action OnUpdateHp { get; set; }
        public Action OnUpdateState { get; set; }
    }
}