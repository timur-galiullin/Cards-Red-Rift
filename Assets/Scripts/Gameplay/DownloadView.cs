using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Cards
{
    public class DownloadView : MonoBehaviour
    {
        [SerializeField] private Image _fade;
        [SerializeField] private Image _arrow;
        [SerializeField] private float _arrowRotateSpeed;

        [Inject] private ImageService _imageService;

        private bool _isActive;

        private void OnEnable()
        {
            _imageService.OnStartDownload += Show;
            _imageService.OnDownloadEnd += Hide;
        }

        private void OnDisable()
        {
            _imageService.OnStartDownload -= Show;
            _imageService.OnDownloadEnd -= Hide;
        }

        private void Update()
        {
            if (!_isActive)
            {
                return;
            }
            
            _arrow.transform.Rotate(new Vector3(0f,0f ,_arrowRotateSpeed * Time.deltaTime));
        }

        private void Show()
        {
            _fade.gameObject.SetActive(true);
            _isActive = true;
        }

        private void Hide()
        {
            _fade.gameObject.SetActive(false);
            _isActive = false;
        }
    }
}
