using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _Project.Scripts.SpawnSystems
{

    public sealed class WaveController : IUpdatable
    {
        public int CurrentWave { get; private set; }
        public bool IsFinishAllWaves { get; private set; }
        public bool IsSpawning { get; private set; }

        private readonly WaveSettings _settings;
        private readonly IUnitFabric _fabric;

        private readonly List<IUnit> _list = new List<IUnit>();

        private readonly List<IWaveUnitListener> _unitListener = new List<IWaveUnitListener>();
        private readonly List<IWaveStateListener> _stateListener = new List<IWaveStateListener>();


        private CancellationToken _token;

        public WaveController(WaveSettings settings, IUnitFabric fabric, CancellationToken token)
        {
            _settings = settings;
            _fabric = fabric;
            _token = token;
            CurrentWave = -1; //not started
        }

        public void RegisterListener(IWaveUnitListener l) => _unitListener.Add(l);
        public void RemoveListener(IWaveUnitListener l) => _unitListener.Remove(l);
        
        public void RegisterListener(IWaveStateListener l) => _stateListener.Add(l);
        public void RemoveListener(IWaveStateListener l) => _stateListener.Remove(l);

        public void Update(in float deltaTime)
        {
            if (IsFinishAllWaves)
                return;
            
            if (IsSpawning)
                return;
            
            if (IsAtLeastOneAlive())
                return;
            
            // clear all previous units
            if (CurrentWave >= 0)
            {
                var wave = _settings.Waves[CurrentWave];

                foreach (var unit in _list)
                {
                    _fabric.DestroyUnit(wave.Type, unit);
                }
                _list.Clear();
                OnFinishWave();
            }

            IsSpawning = true;
            _ = SpawnAsync();
        }

        private async Task SpawnAsync()
        {
            if (_token.IsCancellationRequested)
                return;
            CurrentWave++;
            if (CurrentWave >= _settings.Waves.Count)
            {
                IsFinishAllWaves = true;
                IsSpawning = false;
                OnFinishAllWaves();
                return;
            }
            
            OnStartWave();

            var wave = _settings.Waves[CurrentWave];

            for (int i = 0; i < wave.Amount; i++)
            {
                var nextDate = DateTime.Now.Add(TimeSpan.FromSeconds(wave.Delay));
                while (DateTime.Now < nextDate)
                {
                    await Task.Yield();
                }
                
                if (_token.IsCancellationRequested)
                    break;
                var obj = _fabric.CreateUnit(wave.Type);
                _list.Add(obj);
                OnUnitCreate(obj);
            }
            
            IsSpawning = false;
        }

        private void OnUnitCreate(IUnit unit)
        {
            foreach (var l in _unitListener)
            {
                l.OnCreateUnit(unit);
            }
        }

        private void OnStartWave()
        {
            foreach (var l in _stateListener)
            {
                l.OnStartWave(CurrentWave);
            }
        }
        
        private void OnFinishWave()
        {
            foreach (var l in _stateListener)
            {
                l.OnFinishWave(CurrentWave);
            }
        }

        private void OnFinishAllWaves()
        {
            foreach (var l in _stateListener)
            {
                l.OnFinishAllWaves();
            }
        }
        

        private bool IsAtLeastOneAlive()
        {
            if (_token.IsCancellationRequested)
                return false;
            foreach (var unit in _list)
            {
                if (unit.IsOnPath)
                    return true;
            }

            return false;
        }
    }

    public interface IWaveUnitListener
    {
        void OnCreateUnit(IUnit unit);
    }
    
    public interface IWaveStateListener
    {
        void OnStartWave(int index);
        void OnFinishWave(int index);
        void OnFinishAllWaves();
    }

    public interface IUnitFabric
    {
        IUnit CreateUnit(int type);
        void DestroyUnit(int type, IUnit obj);
    }
    
    public interface IUnit
    {
        bool IsAlive { get; }
        
        bool IsOnPath { get; }
    }

}