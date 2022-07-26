using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Cards
{
    public class CardController : MonoBehaviour
    {
        public CardData Data { get; private set; }
        public CardMovement Movement { get; private set; }

        private void Awake()
        {
            Data = GetComponent<CardData>();
            Movement = GetComponent<CardMovement>();
        }
    }

    public class CardFactory : PlaceholderFactory<GameObject, CardController>
    {
    }
}