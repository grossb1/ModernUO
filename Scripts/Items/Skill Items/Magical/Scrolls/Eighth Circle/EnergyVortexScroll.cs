namespace Server.Items
{
  public class EnergyVortexScroll : SpellScroll
  {
    [Constructible]
    public EnergyVortexScroll(int amount = 1) : base(57, 0x1F66, amount)
    {
    }

    public EnergyVortexScroll(Serial serial) : base(serial)
    {
    }

    public override void Serialize(GenericWriter writer)
    {
      base.Serialize(writer);

      writer.Write(0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
      base.Deserialize(reader);

      int version = reader.ReadInt();
    }
  }
}
