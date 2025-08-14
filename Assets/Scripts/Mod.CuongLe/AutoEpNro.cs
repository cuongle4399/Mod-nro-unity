using System.Threading;

namespace Mod.CuongLe
{
    public class AutoEpNro : IActionListener
    {
    	private static AutoEpNro _Instance;

    	public static bool upgrading;

    	public static AutoEpNro getInstance()
    	{
    		if (_Instance == null)
    		{
    			_Instance = new AutoEpNro();
    		}
    		return _Instance;
    	}

    	public static void ShowMenu()
    	{
    		MyVector myVector = new MyVector();
    		myVector.addElement(new Command("Ép toàn bộ nro về 1 sao", getInstance(), 1, null));
    		myVector.addElement(new Command("Ép toàn bộ nro về 2 sao", getInstance(), 2, null));
    		myVector.addElement(new Command("Ép toàn bộ nro về 3 sao", getInstance(), 3, null));
    		myVector.addElement(new Command("Ép toàn bộ nro về 4 sao", getInstance(), 4, null));
    		myVector.addElement(new Command("Ép toàn bộ nro về 5 sao", getInstance(), 5, null));
    		myVector.addElement(new Command("Ép toàn bộ nro về 6 sao", getInstance(), 6, null));
    		GameCanvas.menu.startAt(myVector, 3);
    	}

    	public void perform(int idAction, object p)
    	{
    		switch (idAction)
    		{
    		case 1:
    			return;
    		case 2:
    			return;
    		case 3:
    			return;
    		case 4:
    			return;
    		case 5:
    			return;
    		}
    		new Thread((ThreadStart)delegate
    		{
    			ep7Ve6(20);
    		}).Start();
    	}

    	public static int indexMenu(string caption)
    	{
    		for (int i = 0; i < GameCanvas.menu.menuItems.size(); i++)
    		{
    			Menu menu = (Menu)GameCanvas.menu.menuItems.elementAt(i);
    			if (menu != null && menu.Equals(caption))
    			{
    				return i;
    			}
    		}
    		return -1;
    	}

    	public static bool checkItempara7(int id)
    	{
    		Item item = ModProCuongLe.FindItemBag(id);
    		if (item != null)
    		{
    			return item.quantity >= 7;
    		}
    		return false;
    	}

    	public static void epNro(int idItem)
    	{
    		upgrading = true;
    		if (TileMap.mapID != 44)
    		{
    			AutoMap.StartRunToMapId(44);
    		}
    		while (AutoMap.isXmaping)
    		{
    			Thread.Sleep(1000);
    		}
    		if (soLuongItem(idItem - 1) == 0 && ModProCuongLe.countEmptyBag() == 0)
    		{
    			GameScr.info1.addInfo("Vui lòng dọn dẹp hành trang để ép", 0);
    			upgrading = false;
    			return;
    		}
    		int num = soLuongItem(idItem);
    		int num2 = num + 1;
    		while (checkItempara7(idItem) && num < num2)
    		{
    			num = soLuongItem(idItem);
    			ModProCuongLe.teleNPC(21);
    			Service.gI().openMenu(21);
    			Service.gI().confirmMenu(21, 6);
    			GameCanvas.panel.vItemCombine.addElement(ModProCuongLe.FindItemBag(idItem));
    			Thread.Sleep(100);
    			Service.gI().combine(1, GameCanvas.panel.vItemCombine);
    			Thread.Sleep(100);
    			Service.gI().confirmMenu(21, 0);
    			Thread.Sleep(100);
    			GameCanvas.menu.doCloseMenu();
    			Thread.Sleep(100);
    			num2 = soLuongItem(idItem);
    		}
    		upgrading = false;
    	}

    	public static int soLuongItem(int Iditem)
    	{
    		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
    		{
    			Item item = Char.myCharz().arrItemBag[i];
    			if (item.template.id == Iditem)
    			{
    				return item.quantity;
    			}
    		}
    		return 0;
    	}

    	public static void ep7Ve6(int idItem)
    	{
    		while (ModProCuongLe.FindItemBag(idItem) != null)
    		{
    			epNro(idItem);
    			Thread.Sleep(1000);
    		}
    		GameScr.info1.addInfo("Đã auto ép ngọc rồng xok", 0);
    	}
    }
}
