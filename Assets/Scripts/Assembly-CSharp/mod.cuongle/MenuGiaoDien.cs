namespace Mod.CuongLe
{
    public class MenuGiaoDien
    {
    	public static string[] menuMod = new string[8] { "Logo Game", "Background", "Thông Báo Boss", "Danh sách nhân vật", "Địa hình lưới", "Danh sách SKH", "Thông tin up vàng", "Thông tin sư phụ"};

    	public static bool[] getArrMod()
    	{
    		return new bool[8]
    		{
    			DoHoa.HienThiLogo,
    			DoHoa.HienThiBackground,
    			DoHoa.isHuntingBoss,
    			DoHoa.isShowCharsInMap,
    			DoHoa.MapLuoi,
    			ModProCuongLe.hienThiDoKH,
    			MainMod.infoTrainGold,
    			ModProCuongLe.charw
    		};
    	}
    }
}
