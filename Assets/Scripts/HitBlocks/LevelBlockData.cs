using System;
using HitBlockLevels;
using UnityEngine;

namespace HitBlockLevels
{
    [Serializable]
    public class LevelBlockData
    {
        [SerializeField]
        public GameObject _block;
        private IBlockShowable _blockShowable;
        public IBlockShowable BlockShowable
        {
            get
            {
                if (_blockShowable == null)
                    _blockShowable = _block.GetComponent<IBlockShowable>();

                if (_blockShowable == null)
                {
                    Debug.LogError("Error: Componet is not inherited from Iterface \"IBlockShowable\"");
                }

                return _blockShowable;
            }
            set
            {
                if (value is not IBlockShowable)
                {
                    Debug.LogError("Error: Componet is not inherited from Iterface \"IBlockShowable\"");
                }
                else
                {
                    _blockShowable = value;
                    if (_blockShowable is MonoBehaviour mono)
                    {
                        _block = mono.gameObject;
                    }
                }
            }
        }
        public int EaseInTime;
        public int EaseOutTime;

        public Vector3 Position;
        public float Rotation;
    }
}