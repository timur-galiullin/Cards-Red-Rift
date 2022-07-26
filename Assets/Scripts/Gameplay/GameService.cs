using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Cards
{
    public class GameService : MonoBehaviour
    {
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private Hand _hand;

        [Inject] private Desk _desk;
        [Inject] private CardFactory _cardFactory;
        [Inject] private ImageService _imageService;

        private List<string> _titles = new List<string>
        {
            "Jaxson Schultz",
            "Anaya Barnes",
            "Maeve Evans",
            "Jordan Thomas",
            "Penelope Cook",
            "Emilia Davis",
            "Joel Fowler",
            "Hayden White",
            "Calvin Ward",
            "Emily Perkins"
        };

        private List<string> _descriptions = new List<string>
        {
            "White, dreadlocks neatly coiffured to reveal a strong, menacing face",
            "Chestnut, shoulder-length hair slightly reveals a skinny, warm face",
            "Black, curly hair tight in a ponytail reveals a full, worried face",
            "Light blue, long hair gently hangs over a craggy, menacing face",
            "Gray, short hair hangs over a furrowed, tense face",
            "Brown, short hair gently hangs over a fine, charming face",
            "Chestnut, straight hair clumsily hangs over a bony, friendly face",
            "Light green, layered hair tight in a bun reveals a round, sad face",
            "Brown, greasy hair neatly coiffured to reveal a craggy, lived-in face",
            "Red, coily hair awkwardly hangs over a fresh, lively face"
        };

        private List<CardController> _cards;
        private int _cardCount;
        private WaitForSeconds _sequenceDelay = new WaitForSeconds(1.5f);
        private IEnumerator _sequence;
        private bool _isSequenceActive;

        public bool IsSequenceActive
        {
            get => _isSequenceActive;
            private set
            {
                _isSequenceActive = value;
                OnSequenceUpdate?.Invoke();
            }
        }

        public Action OnSequenceUpdate { get; set; }

        public void StartSequence()
        {
            IsSequenceActive = true;
            _sequence = Co();
            StartCoroutine(_sequence);

            IEnumerator Co()
            {
                while (_cards.Count > 0)
                {
                    _cards = _cards.Where(x => x.Data.State != CardState.Drop)
                        .OrderBy(x => x.transform.position.x)
                        .ToList();

                    for (int i = 0; i < _cards.Count; i++)
                    {
                        yield return _sequenceDelay;
                        var newHp = Random.Range(-2, 10);
                        while (newHp == _cards[i].Data.Hp)
                        {
                            newHp = Random.Range(-2, 10);
                        }

                        _cards[i].Data.Hp = newHp;
                    }
                }

                StopSequence();
            }
        }

        public void StopSequence()
        {
            IsSequenceActive = false;
            if (_sequence != null)
            {
                StopCoroutine(_sequence);
            }
        }

        private void Awake()
        {
            _cardCount = Random.Range(4, 7);
            _imageService.DownloadImages(_cardCount);
        }

        private void OnEnable()
        {
            _imageService.OnDownloadEnd += CreateCards;
        }

        private void OnDisable()
        {
            _imageService.OnDownloadEnd -= CreateCards;
        }

        private void CreateCards()
        {
            _cards = new List<CardController>();
            for (int i = 0; i < _cardCount; i++)
            {
                var card = _cardFactory.Create(_cardPrefab);
                card.Data.Index = i;
                card.Data.Attack = Random.Range(1, 10);
                card.Data.Hp = Random.Range(1, 10);
                card.Data.Mana = Random.Range(1, 10);
                card.Data.Title = _titles[Random.Range(0, _titles.Count)];
                card.Data.Description = _descriptions[Random.Range(0, _descriptions.Count)];
                _cards.Add(card);
            }

            _hand.SetCards(_cards);
            _desk.SetCards(_cards);
        }
    }
}