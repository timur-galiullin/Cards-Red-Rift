using UnityEngine;
using Zenject;

namespace Cards
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Hand _hand;
        [SerializeField] private Desk _desk;
        [SerializeField] private GameService _gameService;
        [SerializeField] private ImageService _imageService;

        public override void InstallBindings()
        {
            Container.BindInstance(_hand).AsSingle();
            Container.BindInstance(_desk).AsSingle();
            Container.BindInstance(_gameService).AsSingle();
            Container.BindInstance(_imageService).AsSingle();

            Container.BindFactory<GameObject, CardController, CardFactory>().FromFactory<PrefabFactory<CardController>>();
        }
    }
}