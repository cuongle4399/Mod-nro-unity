using System;

namespace Mod.CuongLe
{
    public class Boss
    {
    	public string NameBoss;

    	public string MapName;

    	public int MapId;

    	public DateTime AppearTime;

    	public Boss()
    	{
    	}

    	public Boss(string chatVip)
    	{
    		chatVip = chatVip.Replace("BOSS ", "").Replace(" vừa xuất hiện tại ", "|").Replace(" appear at ", "|");
    		string[] array = chatVip.Split('|');
    		NameBoss = array[0].Trim();
    		MapName = array[1].Trim();
    		MapId = GetMapID(MapName);
    		AppearTime = DateTime.Now;
    	}

    	public int GetMapID(string mapName)
    	{
    		for (int i = 0; i < TileMap.mapNames.Length; i++)
    		{
    			if (TileMap.mapNames[i].ToLower().Trim().Replace("  ", " ")
    				.Equals(mapName.ToLower().Trim().Replace("  ", " ")))
    			{
    				if (NameBoss.ToLower().Trim().Replace("  ", " ")
    					.StartsWith("tiểu đội trưởng") || NameBoss.ToLower().Trim().Replace("  ", " ")
    					.StartsWith("số"))
    				{
    					return 25;
    				}
    				return i;
    			}
    		}
    		return -1;
    	}

    	public void Paint(mGraphics g, int x, int y, int align)
    	{
    		TimeSpan timeSpan = DateTime.Now.Subtract(AppearTime);
    		int num = (int)timeSpan.TotalSeconds;
    		_ = mFont.tahoma_7_yellow;
    		if (TileMap.mapID == MapId)
    		{
    			_ = mFont.tahoma_7_red;
    			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
    			{
    				if (((Char)GameScr.vCharInMap.elementAt(i)).cName.Equals(NameBoss))
    				{
    					_ = mFont.tahoma_7b_red;
    					break;
    				}
    			}
    		}
    		if (GetMapID(MapName) != TileMap.mapID)
    		{
    			mFont.tahoma_7b_yellow.drawString(g, NameBoss + " - " + MapName + " - " + ((num < 60) ? (num + "s") : (timeSpan.Minutes + "ph")) + " trước", x, y, align, mFont.tahoma_7_grey);
    		}
    		else
    		{
    			mFont.tahoma_7b_yellow.drawString(g, "Đang trong map có Boss " + NameBoss + " - " + ((num < 60) ? (num + "s") : (timeSpan.Minutes + "ph")) + " trước", x, y, align, mFont.tahoma_7_greySmall);
    		}
    	}
    }
}
