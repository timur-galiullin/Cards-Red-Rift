using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Cards
{
    public class ImageService : MonoBehaviour
    {
        private const string URL = "https://picsum.photos/";
        private int _imageWidth = 390;
        private int _imageHeight = 606;
        public Action OnDownloadEnd { get; set; }

        public Action OnStartDownload { get; set; }
        public List<Sprite> Sprites { get; private set; }

        public void DownloadImages(int count)
        {
            OnStartDownload?.Invoke();
            StartCoroutine(DownloadImagesCo(count));
        }

        private IEnumerator DownloadImagesCo(int count)
        {
            var list = new List<Sprite>();
            for (int i = 0; i < count; i++)
            {
                yield return StartCoroutine(DownloadImage());
            }

            Sprites = list;
            OnDownloadEnd?.Invoke();

            IEnumerator DownloadImage()
            {
                var request = UnityWebRequestTexture.GetTexture($"{URL}/{_imageWidth}/{_imageHeight}");
                request.useHttpContinue = false;
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    OnDownloadEnd.Invoke();
                    StopAllCoroutines();
                }
                else
                {
                    var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    list.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
                }
            }
        }
    }
}