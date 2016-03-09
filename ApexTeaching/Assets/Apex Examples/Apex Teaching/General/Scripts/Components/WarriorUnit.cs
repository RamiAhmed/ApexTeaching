namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class WarriorUnit : UnitBase
    {
        public override UnitType type
        {
            get { return UnitType.Warrior; }
        }

        protected override void InternalAttack(float dmg)
        {
            // fighters hit only one unit or nest
            var hits = Physics.OverlapSphere(this.transform.position, _attackRadius, Layers.mortal);
            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (ReferenceEquals(hit.gameObject, this.gameObject))
                {
                    // don't damage self
                    continue;
                }

                var unit = hit.GetComponent<UnitBase>();
                if (unit != null)
                {
                    unit.ReceiveDamage(dmg);
                    this.transform.LookAt(new Vector3(unit.transform.position.x, this.transform.position.y, unit.transform.position.z));
                    return;
                }

                var nest = hit.GetComponent<NestStructure>();
                if (nest != null)
                {
                    nest.ReceiveDamage(dmg);
                    this.transform.LookAt(new Vector3(unit.transform.position.x, this.transform.position.y, unit.transform.position.z));
                    return;
                }
            }
        }
    }
}