namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class BlasterUnit : UnitBase
    {
        [SerializeField, Tooltip("Blaster units automatically explode if another unit or building is within this range")]
        private float _explodeRadius = 2f;

        [SerializeField]
        private ParticleSystem _explodeEffect;

        public override UnitType type
        {
            get { return UnitType.Blaster; }
        }

        private void Update()
        {
            var rangeSqr = _explodeRadius * _explodeRadius;
            var count = _observations.Count;
            for (int i = 0; i < count; i++)
            {
                var obsCanDie = _observations[i].GetComponent<ICanDie>();
                if (obsCanDie == null)
                {
                    continue;
                }

                if ((obsCanDie.transform.position - this.transform.position).sqrMagnitude < rangeSqr)
                {
                    Explode(GetDamage());
                    return;
                }
            }
        }

        protected override void InternalAttack(float dmg)
        {
            Explode(dmg);
        }

        private void Explode(float dmg)
        {
            // exploders hit all units and possible nest - within range
            var hits = Physics.OverlapSphere(this.transform.position, _attackRadius, Layers.units | Layers.structures);
            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                var unit = hit.GetComponent<UnitBase>();
                if (unit != null)
                {
                    unit.ReceiveDamage(dmg);
                }

                var nest = hit.GetComponent<NestStructure>();
                if (nest != null)
                {
                    nest.ReceiveDamage(dmg);
                }
            }

            PlayEffect(_explodeEffect);

            // exploders kill themselves
            ReceiveDamage(this.currentHealth + 1f);
        }
    }
}