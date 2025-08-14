using System.Threading;

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
    	 public static void FindMobForPet()
        {
            findMobComplete = false;
            MyVector selectedMobs = new MyVector();
            Mob closestMob = null;
            float minDistanceSquared = float.MaxValue;
            bool goback= false;
            if (AutoTrain.isGoBack)
            {
                goback = true;
                AutoTrain.isGoBack = false;
            }
            int charX = Char.myCharz().cx;
            int charY = Char.myCharz().cy;

            // Duyệt qua tất cả Mob
            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob mob = (Mob)GameScr.vMob.elementAt(i);
                if (mob == null || mob.getTemplate().type == Mob.TYPE_BAY) continue;

                // Tính bình phương khoảng cách Euclidean
                int dx = mob.x - charX;
                int dy = mob.y - charY;
                int distanceSquared = dx * dx + dy * dy;

                // Kiểm tra điều kiện khoảng cách > 350
                if (distanceSquared < minDistanceSquared)
                {
                    minDistanceSquared = distanceSquared;
                    closestMob = mob;
                }
            }

            // Nếu tìm thấy Mob
            if (closestMob != null)
            {
                AutoMap.TeleportTo(closestMob.x, closestMob.y);
                selectedMobs.addElement(closestMob);
                Service.gI().sendPlayerAttack(selectedMobs, new MyVector(), 1);
                Thread.Sleep(500);
                Service.gI().sendPlayerAttack(selectedMobs, new MyVector(), 1);
            }
            if (goback)
            {
                AutoTrain.isGoBack = true;
            }
            else
            {
                AutoMap.TeleportTo(charX, charY);
            }
        }
    	//public static void findMobforPet()
    	//{
    	//	findMobComplete = false;
    	//	MyVector myVector = new MyVector();
    	//	for (int i = 0; i <= GameScr.vMob.size(); i++)
    	//	{
    	//		Mob mob = (Mob)GameScr.vMob.elementAt(i);
    	//		if (Math.abs(mob.x - Char.myCharz().cx) > 350)
    	//		{
    	//			findMobComplete = true;
    	//			myVector.addElement(mob);
    	//			Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
    	//			return;
    	//		}
    	//	}
    	//	if (!findMobComplete)
    	//	{
    	//		findMobforPet();
    	//	}
    	//}
    }
}
