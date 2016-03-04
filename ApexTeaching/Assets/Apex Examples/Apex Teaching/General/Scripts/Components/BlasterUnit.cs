namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class BlasterUnit : UnitBase
    {
        public override UnitType type
        {
            get { return UnitType.Blaster; }
        }

        protected override void InternalAttack(float dmg)
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

            // exploders kill themselves
            ReceiveDamage(this.currentHealth + 1f);
        }
    }
}