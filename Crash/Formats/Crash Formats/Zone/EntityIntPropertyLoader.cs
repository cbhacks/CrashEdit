namespace CrashEdit.Crash
{

	[EntityPropertyType(EntityPropertyType.Int8)]
	public sealed class EntityInt8PropertyLoader : EntityBasicPropertyLoader<sbyte>
	{
		protected override byte ElementSize => 1;

		protected override sbyte LoadElement(byte[] data)
		{
			return (sbyte)data[0];
		}

		protected override EntityProperty Load(IEnumerable<EntityPropertyRow<sbyte>> rows)
		{
			return new EntityInt8Property(rows);
		}
	}

	[EntityPropertyType(EntityPropertyType.Int16)]
	public sealed class EntityInt16PropertyLoader : EntityBasicPropertyLoader<short>
	{
		protected override byte ElementSize => 2;

		protected override short LoadElement(byte[] data)
		{
			return BitConv.FromInt16(data, 0);
		}

		protected override EntityProperty Load(IEnumerable<EntityPropertyRow<short>> rows)
		{
			return new EntityInt16Property(rows);
		}
	}

	[EntityPropertyType(EntityPropertyType.Int32)]
	public sealed class EntityInt32PropertyLoader : EntityBasicPropertyLoader<int>
	{
		protected override byte ElementSize => 4;

		protected override int LoadElement(byte[] data)
		{
			return BitConv.FromInt32(data, 0);
		}

		protected override EntityProperty Load(IEnumerable<EntityPropertyRow<int>> rows)
		{
			return new EntityInt32Property(rows);
		}
	}

	[EntityPropertyType(EntityPropertyType.UInt8)]
	public sealed class EntityUInt8PropertyLoader : EntityBasicPropertyLoader<byte>
	{
		protected override byte ElementSize => 1;

		protected override byte LoadElement(byte[] data)
		{
			return data[0];
		}

		protected override EntityProperty Load(IEnumerable<EntityPropertyRow<byte>> rows)
		{
			return new EntityUInt8Property(rows);
		}
	}

	[EntityPropertyType(EntityPropertyType.UInt16)]
	public sealed class EntityUInt16PropertyLoader : EntityBasicPropertyLoader<ushort>
	{
		protected override byte ElementSize => 2;

		protected override ushort LoadElement(byte[] data)
		{
			return (ushort)BitConv.FromInt16(data, 0);
		}

		protected override EntityProperty Load(IEnumerable<EntityPropertyRow<ushort>> rows)
		{
			return new EntityUInt16Property(rows);
		}
	}

	[EntityPropertyType(EntityPropertyType.UInt32)]
	public sealed class EntityUInt32PropertyLoader : EntityBasicPropertyLoader<uint>
	{
		protected override byte ElementSize => 4;

		protected override uint LoadElement(byte[] data)
		{
			return (uint)BitConv.FromInt32(data, 0);
		}

		protected override EntityProperty Load(IEnumerable<EntityPropertyRow<uint>> rows)
		{
			return new EntityUInt32Property(rows);
		}
	}
}
