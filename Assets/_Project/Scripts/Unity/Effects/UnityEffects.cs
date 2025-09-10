using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Unity.Effects
{
    public class UnityEffects : MonoBehaviour
    {
        [SerializeField] private float _trajectorySpeed = 10f;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Material[] _rangeMaterials;
        [SerializeField] private ParticleSystem RedEffect;
        [SerializeField] private ParticleSystem BlueEffect;
        [SerializeField] private ParticleSystem GreenEffect;
        private readonly List<Transform> _list = new List<Transform>();

        private readonly Vector4[] _towerPos = new Vector4[100];
        private readonly float[] _towerRange = new float[100];
        private int _towerIndex = -1;
        private static readonly int TowerPoses = Shader.PropertyToID("_TowerPoses");
        private static readonly int TowerRanges = Shader.PropertyToID("_TowerRanges");
        private static readonly int TowerAmount = Shader.PropertyToID("_TowerAmount");

        private void Awake()
        {
            UpdateRangeMaterials();
        }

        public void OnAddTower(Vector3 pos, float range)
        {
            _towerIndex++;
            UpdateLastTower(pos, range);
        }

        public void RemoveLast()
        {
            _towerIndex--;
            UpdateRangeMaterials();
        }

        public void UpdateLastTower(Vector3 pos, float range)
        {
            _towerPos[_towerIndex] = pos;
            _towerRange[_towerIndex] = range;
            UpdateRangeMaterials();
        }

        private void UpdateRangeMaterials()
        {
            foreach (var material in _rangeMaterials)
            {
                material.SetVectorArray(TowerPoses, _towerPos);
                material.SetFloatArray(TowerRanges, _towerRange);
                material.SetInt(TowerAmount, _towerIndex + 1);
            }
        }

        public void ProjectileAttack(Vector3 startPos, Transform target, System.Action onFinish)
        {
            var o = GetObject();
            StartCoroutine(ProjectileRoutineToTransform(startPos, target, o.transform, () =>
            {
                SetObject(o);
                onFinish?.Invoke();
            }));
        }
        
        public void ProjectileAoeAttack(Vector3 startPos, Vector3 finish, System.Action onFinish)
        {
            var o = GetObject();
            StartCoroutine(ProjectileRoutine(startPos, finish, o.transform, () =>
            {
                RedEffect.transform.position = finish;
                RedEffect.Play();
                SetObject(o);
                onFinish?.Invoke();
            }));
        }
        
        public void ProjectileSlow(Vector3 startPos, Transform target, System.Action onFinish)
        {
            var o = GetObject();
            StartCoroutine(ProjectileRoutineToTransform(startPos, target, o.transform, () =>
            {
                SetObject(o);
                onFinish?.Invoke();
            }));

        }

        public void ProjectileAoeSlow(Vector3 startPos, Vector3 finish, System.Action onFinish)
        {
            var o = GetObject();
            StartCoroutine(ProjectileRoutine(startPos, finish, o.transform, () =>
            {
                BlueEffect.transform.position = finish;
                BlueEffect.Play();
                SetObject(o);
                onFinish?.Invoke();
            }));
        }

        public void ProjectileAoeGreen(Vector3 startPos, Vector3 finish, System.Action onFinish)
        {
            var o = GetObject();
            StartCoroutine(ProjectileRoutine(startPos, finish, o.transform, () =>
            {
                GreenEffect.transform.position = finish;
                GreenEffect.Play();
                SetObject(o);
                onFinish?.Invoke();
            }));
        }

        private Transform GetObject()
        {
            Transform o = null;
            if (_list.Count == 0)
            {
                o = Instantiate(_prefab).transform;
            }
            else
            {
                o = _list[0].transform;
                _list.RemoveAt(0);
                o.gameObject.SetActive(true);
            }

            return o;
        }

        private void SetObject(Transform o)
        {
            o.gameObject.SetActive(false);
            _list.Add(o);
        }


        private IEnumerator ProjectileRoutine(Vector3 start, Vector3 end,
            Transform tr, System.Action finish)
        {
            var lerpTime = 0f;
            while (lerpTime < 1f)
            {
                lerpTime += Time.deltaTime * _trajectorySpeed;
                tr.position = Vector3.Lerp(start, end, lerpTime);
                yield return null;
            }
            
            finish?.Invoke();
        }
        
        private IEnumerator ProjectileRoutineToTransform(Vector3 start, Transform end,
            Transform tr, System.Action finish)
        {
            var lerpTime = 0f;
            while (lerpTime < 1f)
            {
                lerpTime += Time.deltaTime * _trajectorySpeed;
                tr.position = Vector3.Lerp(start, end.position, lerpTime);
                yield return null;
            }
            
            finish?.Invoke();
        }
    }
}
