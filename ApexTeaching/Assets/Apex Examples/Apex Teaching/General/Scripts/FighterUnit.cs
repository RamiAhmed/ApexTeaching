namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class FighterUnit : UnitBase
    {
        public override UnitType type
        {
            get { return UnitType.Fighter; }
        }

        protected override void InternalAttack(float dmg)
        {
            // fighters hit only one unit or nest
            var hits = Physics.OverlapSphere(this.transform.position, _attackRadius, Layers.units | Layers.structures);
            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                var unit = hit.GetComponent(typeof(UnitBase)) as UnitBase;
                if (unit != null)
                {
                    unit.ReceiveDamage(dmg);
                    return;
                }

                var nest = hit.GetComponent<NestStructure>();
                if (nest != null)
                {
                    nest.ReceiveDamage(dmg);
                    return;
                }
            }
        }
    }
}