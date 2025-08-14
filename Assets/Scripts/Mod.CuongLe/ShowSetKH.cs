using System.Threading;

namespace Mod.CuongLe
{
    public class ShowSetKH
    {
    	public static string InDanhSachDoKH()
    	{
    		string text = "";
    		int[] array = new int[5];
    		int[] array2 = new int[5];
    		int[] array3 = new int[5];
    		int[] array4 = new int[5];
    		int[] array5 = new int[5];
    		DemItems(Char.myCharz().arrItemBag, array, array2, array3, array4, array5);
    		DemItems(Char.myCharz().arrItemBox, array, array2, array3, array4, array5);
    		DemItems(Char.myCharz().arrItemBody, array, array2, array3, array4, array5);
    		if (Char.myCharz().getGender().Equals("TĐ"))
    		{
    			text += DinhDangOutput("Sgk", array);
    			text += DinhDangOutput("Kok", array2);
    			text += DinhDangOutput("txh", array3);
    			text += DinhDangOutput("Gohan", array4);
    			return text + DinhDangOutput("Kirin", array5);
    		}
    		if (Char.myCharz().getGender().Equals("XD"))
    		{
    			text += DinhDangOutput("Kkr", array);
    			text += DinhDangOutput("Ca Đíc", array2);
    			text += DinhDangOutput("Cađic M", array3);
    			text += DinhDangOutput("Gohan", array4);
    			return text + DinhDangOutput("Nappa", array5);
    		}
    		text += DinhDangOutput("Picolo", array);
    		text += DinhDangOutput("Daimao", array2);
    		text += DinhDangOutput("Ốc tiêu", array3);
    		text += DinhDangOutput("Gohan", array4);
    		return text + DinhDangOutput("Nail", array5);
    	}

    	private static void DemItems(Item[] items, int[] set1, int[] set2, int[] set3, int[] set4, int[] set5)
    	{
    		Item[] array;
    		if (Char.myCharz().getGender().Equals("TĐ"))
    		{
    			array = items;
    			Item[] array2 = array;
    			foreach (Item item in array2)
    			{
    				try
    				{
    					if (item != null && item.itemOption[1] != null && item.itemOption[2] != null && item.template.type <= 4)
    					{
    						if (item.itemOption[1].optionTemplate.name.StartsWith("Set Sôngôku") || item.itemOption[2].optionTemplate.name.StartsWith("Set Sôngôku"))
    						{
    							TangCount(set1, item);
    						}
    						else if (item.itemOption[1].optionTemplate.name.StartsWith("Set Thần Vũ Trụ") || item.itemOption[2].optionTemplate.name.StartsWith("Set Thần Vũ Trụ"))
    						{
    							TangCount(set2, item);
    						}
    						else if (item.itemOption[1].optionTemplate.name.StartsWith("Set Thên") || item.itemOption[2].optionTemplate.name.StartsWith("Set Thên"))
    						{
    							TangCount(set3, item);
    						}
    						else if (item.itemOption[1].optionTemplate.name.StartsWith("Set Gohan") || item.itemOption[2].optionTemplate.name.StartsWith("Set Gohan"))
    						{
    							TangCount(set4, item);
    						}
    						else if (item.itemOption[1].optionTemplate.name.StartsWith("Set Kirin") || item.itemOption[2].optionTemplate.name.StartsWith("Set Kirin"))
    						{
    							TangCount(set5, item);
    						}
    					}
    				}
    				catch
    				{
    				}
    			}
    			return;
    		}
    		if (Char.myCharz().getGender().Equals("XD"))
    		{
    			array = items;
    			Item[] array3 = array;
    			foreach (Item item2 in array3)
    			{
    				try
    				{
    					if (item2 != null && item2.itemOption[1] != null && item2.itemOption[2] != null && item2.template.type <= 4)
    					{
    						if (item2.itemOption[1].optionTemplate.name.StartsWith("Set Kakarot") || item2.itemOption[2].optionTemplate.name.StartsWith("Set Kakarot"))
    						{
    							TangCount(set1, item2);
    						}
    						else if (item2.itemOption[1].optionTemplate.name.StartsWith("Set Ca Đíc") || item2.itemOption[2].optionTemplate.name.StartsWith("Set Ca Đíc"))
    						{
    							TangCount(set2, item2);
    						}
    						else if (item2.itemOption[1].optionTemplate.name.StartsWith("Set Cađic M") || item2.itemOption[2].optionTemplate.name.StartsWith("Set Cađic M"))
    						{
    							TangCount(set3, item2);
    						}
    						else if (item2.itemOption[1].optionTemplate.name.StartsWith("Set Gohan") || item2.itemOption[2].optionTemplate.name.StartsWith("Set Gohan"))
    						{
    							TangCount(set4, item2);
    						}
    						else if (item2.itemOption[1].optionTemplate.name.StartsWith("Set Nappa") || item2.itemOption[2].optionTemplate.name.StartsWith("Set Nappa"))
    						{
    							TangCount(set5, item2);
    						}
    					}
    				}
    				catch
    				{
    				}
    			}
    			return;
    		}
    		array = items;
    		Item[] array4 = array;
    		foreach (Item item3 in array4)
    		{
    			try
    			{
    				if (item3 != null && item3.itemOption[1] != null && item3.itemOption[2] != null && item3.template.type <= 4)
    				{
    					if (item3.itemOption[1].optionTemplate.name.StartsWith("Set Picolo") || item3.itemOption[2].optionTemplate.name.StartsWith("Set Picolo"))
    					{
    						TangCount(set1, item3);
    					}
    					else if (item3.itemOption[1].optionTemplate.name.StartsWith("Set Pikkoro Daimao") || item3.itemOption[2].optionTemplate.name.StartsWith("Set Pikkoro Daimao"))
    					{
    						TangCount(set2, item3);
    					}
    					else if (item3.itemOption[1].optionTemplate.name.StartsWith("Set Ốc tiêu") || item3.itemOption[2].optionTemplate.name.StartsWith("Set Ốc tiêu"))
    					{
    						TangCount(set3, item3);
    					}
    					else if (item3.itemOption[1].optionTemplate.name.StartsWith("Set Gohan") || item3.itemOption[2].optionTemplate.name.StartsWith("Set Gohan"))
    					{
    						TangCount(set4, item3);
    					}
    					else if (item3.itemOption[1].optionTemplate.name.StartsWith("Set Nail chiến binh") || item3.itemOption[2].optionTemplate.name.StartsWith("Nail chiến binh"))
    					{
    						TangCount(set5, item3);
    					}
    				}
    			}
    			catch
    			{
    			}
    		}
    	}

    	private static void TangCount(int[] array, Item item)
    	{
    		array[item.template.type]++;
    	}

    	private static string DinhDangOutput(string name, int[] array)
    	{
    		bool flag = true;
    		for (int i = 0; i < array.Length; i++)
    		{
    			if (array[i] != 0)
    			{
    				flag = false;
    				break;
    			}
    		}
    		if (flag)
    		{
    			return string.Empty;
    		}
    		return string.Concat(name + " [", array[0].ToString(), " ao| ", array[1].ToString(), " w| ", array[2].ToString(), " gang| ", array[3].ToString(), " jay| ", array[4].ToString(), " rd]\n");
    	}

    	public static bool checkKH(Item item)
    	{
    		for (int i = 0; i < item.itemOption.Length; i++)
    		{
    			if (item.itemOption[i].optionTemplate.id == 107)
    			{
    				return true;
    			}
    			if (item.itemOption[i].optionTemplate.name.StartsWith("$"))
    			{
    				return true;
    			}
    			if (item.template.type < 4)
    			{
    				return true;
    			}
    		}
    		return false;
    	}

    	public static int soluongsao(Item item)
    	{
    		for (int i = 0; i < item.itemOption.Length; i++)
    		{
    			if (item.itemOption[i].optionTemplate.id == 107)
    			{
    				return item.itemOption[i].param;
    			}
    		}
    		return 0;
    	}

        public static void SellTrashItem()
        {
            ModProCuongLe.tieuDietNguoiBatCo = false;
            ModProCuongLe.tanCongBoss = false;
            bool isTrain = false;
            bool isGoback = false;
            while (ModProCuongLe.banDo)
            {
                if (AutoTrain.isGoBack)
                {
                    isGoback = true;
                    AutoTrain.isGoBack = false;
                }
                if (ModProCuongLe.isFULLBag())
                {
                    if (AutoTrain.isAutoTrain)
                    {
                        AutoTrain.isAutoTrain = false;
                        isTrain = true;
                    }
                    Thread.Sleep(200);
                    if (TileMap.mapID != 26)
                    {
                        AutoMap.StartRunToMapId(26);
                    }
                    while (AutoMap.isXmaping)
                    {
                        Thread.Sleep(1100);
                    }
                    ModProCuongLe.teleNPC(16);
                    Thread.Sleep(500);
                    for (int num = Char.myCharz().arrItemBag.Length - 1; num >= 0; num--)
                    {
                        Item item = Char.myCharz().arrItemBag[num];
                        if (item != null && !itemKH(item) && !itemStar(item) && !itemTL(item) && item.template.id < 200 && (item.template.type == 0 || item.template.type == 1 || item.template.type == 2 || item.template.type == 3 || item.template.type == 4))
                        {
                            Service.gI().saleItem(0, 1, (short)num);
                            Service.gI().saleItem(1, 1, (short)num);
                            Thread.Sleep(800);
                        }
                        Thread.Sleep(200);
                    }
                    Thread.Sleep(1000);
                    if (isGoback)
                    {
                        isGoback = false;
                        AutoTrain.isGoBack = true;
                    }
                    if (isTrain)
                    {
                        AutoTrain.isAutoTrain = true;
                        isTrain = false;
                    }
                }
                Thread.Sleep(3000);
            }
        }

        public static bool itemKH(Item item)
    	{
    		bool result = false;
    		for (int i = 0; i < item.itemOption.Length; i++)
    		{
    			if (item.itemOption[i].optionTemplate.name.StartsWith("$"))
    			{
    				return true;
    			}
    		}
    		return result;
    	}

    	public static bool itemStar(Item item)
    	{
    		bool result = false;
    		for (int i = 0; i < item.itemOption.Length; i++)
    		{
    			try
    			{
    				if (item.itemOption[i].optionTemplate.name.StartsWith("#") && item.itemOption[i].param > 0)
    				{
    					return true;
    				}
    			}
    			catch
    			{
    			}
    		}
    		return result;
    	}

        public static void CatDo()
        {
            while (ModProCuongLe.catDoVIP)
            {
                int num = ModProCuongLe.countEmptyBox();
                if (num == 0)
                {
                    GameScr.info1.addInfo("|2| Rương FULL rồi", 0);
                    ModProCuongLe.catDoVIP = false;
                    break;
                }
                if (ModProCuongLe.isFULLBag() && !ModProCuongLe.isFULLBox())
                {
                    ModProCuongLe.tieuDietNguoiBatCo = false;
                    ModProCuongLe.tanCongBoss = false;
                    bool flag = false;
                    bool isGoback = false;
                    if (AutoTrain.isAutoTrain)
                    {
                        AutoTrain.isAutoTrain = false;
                        flag = true;
                    }
                    if (AutoTrain.isGoBack)
                    {
                        isGoback = true;
                        AutoTrain.isGoBack = false;
                    }
                    if (TileMap.mapID != 21 || TileMap.mapID != 22 || TileMap.mapID != 23)
                    {
                        if (Char.myCharz().getGender() == "TĐ")
                        {
                            AutoMap.StartRunToMapId(21);
                        }
                        else if (Char.myCharz().getGender() == "NM")
                        {
                            AutoMap.StartRunToMapId(22);
                        }
                        else
                        {
                            AutoMap.StartRunToMapId(23);
                        }
                    }
                    while (AutoMap.isXmaping)
                    {
                        Thread.Sleep(1000);
                    }
    				if(TileMap.mapID != 21 || TileMap.mapID != 22 || TileMap.mapID != 23)
    				{
                        ModProCuongLe.catDoVIP = false;
                        return;
    				}
                    ModProCuongLe.teleNPC(3);
                    Service.gI().openMenu(3);
                    for (int num2 = Char.myCharz().arrItemBag.Length - 1; num2 >= 0; num2--)
                    {
                        Item item = Char.myCharz().arrItemBag[num2];
                        if (item != null && item.template.type <= 4 && (itemKH(item) || itemStar(item) || itemTL(item) || item.template.id > 200))
                        {
                            Service.gI().getItem(1, (sbyte)num2);
                            Thread.Sleep(700);
                        }
                        Thread.Sleep(400);
                        if (ModProCuongLe.isFULLBox())
                        {
                            ModProCuongLe.catDoVIP = false;
                            break;
                        }
                    }
                    if (num == ModProCuongLe.countEmptyBox())
                    {
                        GameScr.info1.addInfo("|2| Hành trang không có đồ đủ tiêu chuẩn để cất", 0);
                        ModProCuongLe.catDoVIP = false;
                    }
                    if (isGoback)
                    {
                        isGoback = false;
                        AutoTrain.isGoBack = true;
                    }
                    if (flag)
                    {
                        AutoTrain.isAutoTrain = true;
                    }
                }
                Thread.Sleep(5000);
            }
        }

        public static bool itemTL(Item item)
    	{
    		if (item.template.id != 555 && item.template.id != 557 && item.template.id != 559 && item.template.id != 556 && item.template.id != 562 && item.template.id != 563 && item.template.id != 561 && item.template.id != 558 && item.template.id != 560 && item.template.id != 564 && item.template.id != 566 && item.template.id != 567)
    		{
    			return item.template.id == 565;
    		}
    		return true;
    	}
    }
}
