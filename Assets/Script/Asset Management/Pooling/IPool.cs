using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mvvm
{
    public interface IPool<in TPoolObjectType> where TPoolObjectType : IPoolObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="poolObject"></param>
        void PoolObjectReturn(TPoolObjectType poolObject);
    }

    public interface IPool
    {
        void PoolObjectReturned(PoolObject poolObject);
    }
}