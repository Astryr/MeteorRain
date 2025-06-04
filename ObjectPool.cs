using System;
using System.Collections.Generic;

namespace MyGame
{
    public class ObjectPool<T> where T : IPoolable
    {
        private readonly Func<T> factory;
        private readonly List<T> pool = new List<T>();

        public ObjectPool(Func<T> factory, int initialSize = 10)
        {
            this.factory = factory;
            for (int i = 0; i < initialSize; i++)
                pool.Add(factory());
        }

        public T Get(float x, float y, float dx, float dy, Image sprite)
        {
            foreach (var obj in pool)
            {
                if (!obj.IsActive)
                {
                    obj.Reset(x, y, dx, dy, sprite);
                    obj.IsActive = true;
                    return obj;
                }
            }
            var newObj = factory();
            newObj.Reset(x, y, dx, dy, sprite);
            newObj.IsActive = true;
            pool.Add(newObj);
            return newObj;
        }

        public void Release(T obj)
        {
            obj.IsActive = false;
        }

        public IEnumerable<T> ActiveObjects => pool.FindAll(o => o.IsActive);
    }
}