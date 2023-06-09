using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.SkillHandlers
{
    public static class ArmsLore
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.ArmsLore].Callback = OnUse;
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.Target = new InternalTarget();

            m.SendLocalizedMessage(500349); // What item do you wish to get information about?

            return TimeSpan.FromSeconds(1.0);
        }

        [PlayerVendorTarget]
        private class InternalTarget : Target
        {
            public InternalTarget() : base(2, false, TargetFlags.None) => AllowNonlocal = true;

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseWeapon weap)
                {
                    if (from.CheckTargetSkill(SkillName.ArmsLore, weap, 0, 100))
                    {
                        if (weap.MaxHitPoints != 0)
                        {
                            var hp = Math.Clamp((int)(weap.HitPoints / (double)weap.MaxHitPoints * 10), 0, 9);

                            from.SendLocalizedMessage(1038285 + hp);
                        }

                        var damage = (weap.MaxDamage + weap.MinDamage) / 2;
                        var hand = weap.Layer == Layer.OneHanded ? 0 : 1;

                        if (damage < 3)
                        {
                            damage = 0;
                        }
                        else
                        {
                            damage = (int)Math.Ceiling(Math.Min(damage, 30) / 5.0);
                        }

                        var type = weap.Type;

                        if (type == WeaponType.Ranged)
                        {
                            from.SendLocalizedMessage(1038224 + damage * 9);
                        }
                        else if (type == WeaponType.Piercing)
                        {
                            from.SendLocalizedMessage(1038218 + hand + damage * 9);
                        }
                        else if (type == WeaponType.Slashing)
                        {
                            from.SendLocalizedMessage(1038220 + hand + damage * 9);
                        }
                        else if (type == WeaponType.Bashing)
                        {
                            from.SendLocalizedMessage(1038222 + hand + damage * 9);
                        }
                        else
                        {
                            from.SendLocalizedMessage(1038216 + hand + damage * 9);
                        }

                        if (weap.Poison != null && weap.PoisonCharges > 0)
                        {
                            from.SendLocalizedMessage(1038284); // It appears to have poison smeared on it.
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(500353); // You are not certain...
                    }
                }
                else if (targeted is BaseArmor arm)
                {
                    if (from.CheckTargetSkill(SkillName.ArmsLore, arm, 0, 100))
                    {
                        if (arm.MaxHitPoints != 0)
                        {
                            var hp = Math.Clamp((int)(arm.HitPoints / (double)arm.MaxHitPoints * 10), 0, 9);

                            from.SendLocalizedMessage(1038285 + hp);
                        }

                        from.SendLocalizedMessage(1038295 + (int)Math.Ceiling(Math.Min(arm.ArmorRating, 35) / 5.0));
                    }
                    else
                    {
                        from.SendLocalizedMessage(500353); // You are not certain...
                    }
                }
                else if (targeted is SwampDragon pet && pet.HasBarding)
                {
                    if (from.CheckTargetSkill(SkillName.ArmsLore, pet, 0, 100))
                    {
                        var perc = Math.Clamp(4 * pet.BardingHP / pet.BardingMaxHP, 0, 4);
                        pet.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 1053021 - perc, from.NetState);
                    }
                    else
                    {
                        from.SendLocalizedMessage(500353); // You are not certain...
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500352); // This is neither weapon nor armor.
                }
            }
        }
    }
}
