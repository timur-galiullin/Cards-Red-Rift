using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Cards
{
    [DefaultExecutionOrder(2)]
    public class CardView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _titleText;
        [SerializeField] private TextMeshPro _descriptionText;
        [SerializeField] private TextMeshPro _hpText;
        [SerializeField] private TextMeshPro _attackText;
        [SerializeField] private TextMeshPro _manaText;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private SpriteRenderer _hpRenderer;
        [SerializeField] private SortingGroup _sortingGroup;
        [SerializeField] private SpriteRenderer _outlineRender;

        [Inject] private ImageService _imageService;
        [Inject] private Hand _hand;

        private CardData _data;
        private Sequence _hpSequence;
        private Tween _outlineTween;

        private void UpdateSortingGroup()
        {
            if (_data.State != CardState.Drag)
            {
                _sortingGroup.sortingOrder = _data.Index;
            }
        }

        private void Awake()
        {
            _data = GetComponent<CardData>();
        }

        private void Start()
        {
            SetSprite();
            _hpText.text = _data.Hp.ToString();
            _attackText.text = _data.Attack.ToString();
            _manaText.text = _data.Mana.ToString();
            _titleText.text = _data.Title;
            _descriptionText.text = _data.Description;
        }

        private void OnEnable()
        {
            _data.OnUpdateHp += ChangeHpAnimation;
            _data.OnUpdateState += CheckState;
            _hand.OnSort += UpdateSortingGroup;
        }

        private void OnDisable()
        {
            _data.OnUpdateHp -= ChangeHpAnimation;
            _data.OnUpdateState -= CheckState;
            _hand.OnSort -= UpdateSortingGroup;
        }

        private void CheckState()
        {
            switch (_data.State)
            {
                case CardState.Drag:
                    _outlineRender.gameObject.SetActive(true);
                    _outlineRender.color = Color.white;
                    _outlineTween = _outlineRender.DOFade(0f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    _sortingGroup.sortingOrder = 10;
                    break;
                case CardState.Drop:
                case CardState.Deck:
                    _outlineRender.gameObject.SetActive(false);
                    _outlineTween.Kill();
                    break;
                case CardState.Hand:
                    _outlineRender.gameObject.SetActive(false);
                    UpdateSortingGroup();
                    break;
            }
        }

        private void SetSprite()
        {
            if (_imageService.Sprites != null)
            {
                _renderer.sprite = _imageService.Sprites[_data.Index];
            }
        }

        private void ChangeHpAnimation()
        {
            _hpSequence.Kill();
            _hpRenderer.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            var startHp = int.Parse(_hpText.text);
            var currentHp = startHp;
            var newHp = _data.Hp;
            var duration = 1.25f;
            _hpSequence = DOTween.Sequence()
                .Append(DOTween.To(() => currentHp, x =>
                {
                    currentHp = x;
                    _hpText.text = currentHp.ToString();
                    if (currentHp <= 0 && currentHp != startHp)
                    {
                        _data.State = CardState.Drop;
                    }
                }, newHp, duration))
                .Join(_hpRenderer.transform.DOScale(2f, duration / 2f).SetLoops(2, LoopType.Yoyo));
        }
    }
}