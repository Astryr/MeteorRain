using System;
using System.Collections.Generic;

namespace MyGame
{
    public class Collider
    {
        // Evento que se dispara cuando ocurre una colisión
        public event Action<GameObject> OnCollision;

        // Referencia al objeto dueño de este collider
        public GameObject Owner { get; }

        public Collider(GameObject owner)
        {
            this.Owner = owner;
        }

        // Chequea colisiones contra una lista de colliders
        public void CheckCollisions(IEnumerable<Collider> others)
        {
            foreach (var other in others)
            {
                if (other == this) continue;
                if (IsCollidingWith(other))
                {
                    OnCollision?.Invoke(other.Owner);
                }
            }
        }

        // Lógica simple de colisión (AABB)
        private bool IsCollidingWith(Collider other)
        {
            float ax = Owner.x, ay = Owner.y, aw = 50, ah = 50;
            float bx = other.Owner.x, by = other.Owner.y, bw = 50, bh = 50;

            // Si el objeto tiene tamaño personalizado
            if (Owner is Player) { aw = 50; ah = 50; }
            if (Owner is Asteroide a) { aw = ah = a.collisionRadius * 2; }
            if (Owner is Bullet) { aw = ah = 10; }

            if (other.Owner is Player) { bw = 50; bh = 50; }
            if (other.Owner is Asteroide a2) { bw = bh = a2.collisionRadius * 2; }
            if (other.Owner is Bullet) { bw = bh = 10; }

            return ax < bx + bw && ax + aw > bx && ay < by + bh && ay + ah > by;
        }
    }

}