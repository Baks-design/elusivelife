using System;
using EngineRoom.Examples.Interfaces;
using UnityEngine;

namespace EngineRoom.Examples.Views
{
    public class TargetView : MonoBehaviour, ITargetView
    {
        [SerializeField] GameObject target;
        [SerializeField] Collider coll;

        public bool HasTarget => target.activeInHierarchy;

        public event Action TargetGotHit;

        void Awake() => DespawnTarget();

        void OnTriggerEnter(Collider other)
        {
            if (!HasTarget)
                return;

            TargetGotHit?.Invoke();
        }

        public void SpawnTarget()
        {
            target.SetActive(true);
            coll.enabled = true;
        }

        public void DespawnTarget()
        {
            target.SetActive(false);
            coll.enabled = false;
        }
    }
}
