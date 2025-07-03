namespace Mod.CuongLe
{
    public class mobProMore
    {
    	public static bool findMobComplete;

    	public static bool DeSuaLapem;

    	static mobProMore()
    	{
    	}

    	public static bool checkDeKeu(string s)
    	{
    		return s.ToLower().Contains("sao sư phụ không đánh đi?");
    	}

    	public static void findMobforPet()
    	{
    		findMobComplete = false;
    		MyVector myVector = new MyVector();
    		for (int i = 0; i <= GameScr.vMob.size(); i++)
    		{
    			Mob mob = (Mob)GameScr.vMob.elementAt(i);
    			if (Math.abs(mob.x - Char.myCharz().cx) > 350)
    			{
    				findMobComplete = true;
    				myVector.addElement(mob);
    				Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
    				return;
    			}
    		}
    		if (!findMobComplete)
    		{
    			findMobforPet();
    		}
    	}
    }
}
