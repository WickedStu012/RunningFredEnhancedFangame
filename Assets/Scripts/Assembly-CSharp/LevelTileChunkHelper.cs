public class LevelTileChunkHelper
{
	public static int ToInt(LevelTileChunk ltc)
	{
		switch (ltc)
		{
		case LevelTileChunk.UNKNOWN:
			return -1;
		case LevelTileChunk.PLANE:
			return 0;
		case LevelTileChunk.START_RAMP:
			return 1;
		case LevelTileChunk.HALF_ELEVATED:
			return 2;
		case LevelTileChunk.NARROW_PLANE_MIDDLE:
			return 3;
		case LevelTileChunk.NARROW_PLANE_SIDE:
			return 4;
		case LevelTileChunk.START_NARROWER:
			return 5;
		case LevelTileChunk.START_NARROW_TUNNEL:
			return 6;
		case LevelTileChunk.NARROW_TUNNEL_1:
			return 7;
		case LevelTileChunk.NARROW_TUNNEL_2:
			return 8;
		case LevelTileChunk.START_BRIDGE:
			return 9;
		case LevelTileChunk.BRIDGE_1:
			return 10;
		case LevelTileChunk.BRIDGE_2:
			return 11;
		case LevelTileChunk.BRIDGE_BROKEN:
			return 12;
		case LevelTileChunk.STAIRS:
			return 13;
		default:
			return -1;
		}
	}

	public static LevelTileChunk ToEnum(int ltc)
	{
		switch (ltc)
		{
		case -1:
			return LevelTileChunk.UNKNOWN;
		case 0:
			return LevelTileChunk.PLANE;
		case 1:
			return LevelTileChunk.START_RAMP;
		case 2:
			return LevelTileChunk.HALF_ELEVATED;
		case 3:
			return LevelTileChunk.NARROW_PLANE_MIDDLE;
		case 4:
			return LevelTileChunk.NARROW_PLANE_SIDE;
		case 5:
			return LevelTileChunk.START_NARROWER;
		case 6:
			return LevelTileChunk.START_NARROW_TUNNEL;
		case 7:
			return LevelTileChunk.NARROW_TUNNEL_1;
		case 8:
			return LevelTileChunk.NARROW_TUNNEL_2;
		case 9:
			return LevelTileChunk.START_BRIDGE;
		case 10:
			return LevelTileChunk.BRIDGE_1;
		case 11:
			return LevelTileChunk.BRIDGE_2;
		case 12:
			return LevelTileChunk.BRIDGE_BROKEN;
		case 13:
			return LevelTileChunk.STAIRS;
		default:
			return LevelTileChunk.UNKNOWN;
		}
	}
}
