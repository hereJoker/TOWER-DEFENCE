using System;
using System.Collections.Generic;
using System.Threading;
using _Project.Scripts.Database;
using _Project.Scripts.Gameplay;
using _Project.Scripts.Movables;
using _Project.Scripts.SpawnSystems;
using _Project.Scripts.Unity;
using _Project.Scripts.Unity.Effects;
using _Project.Scripts.Unity.UI;
using UnityEngine;

namespace _Project.Scripts
{
    public class EntryPoint : MonoBehaviour, IWinLoseListener
    {
        [SerializeField] private WaveConfig _waveConfig;
        [SerializeField] private UnitConfig _unitConfig;

        [SerializeField] private PathAdapter _path;
        [SerializeField] private PlaceForTower _placeForTower;
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _placeMask;
        [SerializeField] private TowerPlacerUI _towerPlacerUI;
        [SerializeField] private TutorialUI _tutorial;
        [SerializeField] private HudUI _hud;
        [SerializeField] private UnityEffects _effects;

        private CancellationTokenSource _token;

        private TowerPlacerUnityController _placer;

        private readonly List<IUpdatable> _updatables = new List<IUpdatable>();

        private bool _update = true;

        private void Awake()
        {
            _token = new CancellationTokenSource();
            var router = new RouterWaveToMovable();
            var winLose = new WinLoseChecker(_waveConfig.TotalLives);
            var move = new MovableController(_path);
            var unitFabric = new UnitFabric(_unitConfig);
            var wave = new WaveController(_waveConfig.WaveSettings, unitFabric, _token.Token);
            var place = new ObjectPlacer.ObjectBoundsPlacer(_placeForTower.GetBounds());
            _placer = new TowerPlacerUnityController(place, _camera, _placeMask, _effects);


            wave.RegisterListener(router);
            wave.RegisterListener(winLose);

            move.Register(winLose);

            winLose.RegisterListener(_hud);
            winLose.RegisterListener(this);

            _hud.OnUpdateLives(winLose.TotalLives);


            _towerPlacerUI.Register(_placer);


            router.Init(move);


            _updatables.Add(wave);
            _updatables.Add(move);
            _updatables.Add(_placer);

        }

        private void Update()
        {
            if (_update == false)
                return;

            if (_placer.AmountOfTowerPlaced == 0)
            {
                _placer.Update(Time.deltaTime);
                //wait for tutorial to finish
                return;
            }

            _tutorial.Finish();
            var deltaTime = Time.deltaTime;
            foreach (var updatable in _updatables)
            {
                updatable.Update(deltaTime);
            }
        }

        private void OnDestroy()
        {
            _token.Cancel();
        }

        public void OnWin()
        {
            _update = false;
            _token.Cancel();
        }

        public void OnLose()
        {
            _update = false;
            _token.Cancel();
        }

        public void OnUpdateLives(in int lives)
        {
        }
    }

    public interface IUpdatable
    {
        void Update(in float deltaTime);
    }
}