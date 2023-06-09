using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.SkillHandlers
{
    public static class Anatomy
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Anatomy].Callback = OnUse;
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.Target = new InternalTarget();

            m.SendLocalizedMessage(500321); // Whom shall I examine?

            return TimeSpan.FromSeconds(1.0);
        }

        private class InternalTarget : Target
        {
            public InternalTarget() : base(8, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (from == targeted)
                {
                    from.LocalOverheadMessage(
                        MessageType.Regular,
                        0x3B2,
                        500324
                    ); // You know yourself quite well enough already.
                }
                else if (targeted is TownCrier crier)
                {
                    crier.PrivateOverheadMessage(
                        MessageType.Regular,
                        0x3B2,
                        500322,
                        from.NetState
                    ); // This person looks fine to me, though he may have some news...
                }
                else if (targeted is BaseVendor vendor && vendor.IsInvulnerable)
                {
                    vendor.PrivateOverheadMessage(
                        MessageType.Regular,
                        0x3B2,
                        500326,
                        from.NetState
                    ); // That can not be inspected.
                }
                else if (targeted is Mobile targ)
                {
                    var marginOfError = Math.Max(0, 25 - (int)(from.Skills.Anatomy.Value / 4));

                    var str = targ.Str + Utility.RandomMinMax(-marginOfError, +marginOfError);
                    var dex = targ.Dex + Utility.RandomMinMax(-marginOfError, +marginOfError);
                    var stm = targ.Stam * 100 / Math.Max(targ.StamMax, 1) +
                              Utility.RandomMinMax(-marginOfError, +marginOfError);

                    var strMod = str / 10;
                    var dexMod = dex / 10;
                    var stmMod = stm / 10;

                    if (strMod < 0)
                    {
                        strMod = 0;
                    }
                    else if (strMod > 10)
                    {
                        strMod = 10;
                    }

                    if (dexMod < 0)
                    {
                        dexMod = 0;
                    }
                    else if (dexMod > 10)
                    {
                        dexMod = 10;
                    }

                    if (stmMod > 10)
                    {
                        stmMod = 10;
                    }
                    else if (stmMod < 0)
                    {
                        stmMod = 0;
                    }

                    if (from.CheckTargetSkill(SkillName.Anatomy, targ, 0, 100))
                    {
                        targ.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            1038045 + strMod * 11 + dexMod,
                            from.NetState
                        ); // That looks [strong] and [dexterous].

                        if (from.Skills.Anatomy.Base >= 65.0)
                        {
                            targ.PrivateOverheadMessage(
                                MessageType.Regular,
                                0x3B2,
                                1038303 + stmMod,
                                from.NetState
                            ); // That being is at [10,20,...] percent endurance.
                        }
                    }
                    else
                    {
                        targ.PrivateOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            1042666,
                            from.NetState
                        ); // You can not quite get a sense of their physical characteristics.
                    }
                }
                else
                {
                    (targeted as Item)?.SendLocalizedMessageTo(from, 500323); // Only living things have anatomies!
                }
            }
        }
    }
}
