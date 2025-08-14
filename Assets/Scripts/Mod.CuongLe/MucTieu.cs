namespace Mod.CuongLe
{
    public class MucTieu : IActionListener
    {
    	private static MucTieu _Instance;

    	public static bool deselectNpc;

    	public static bool deselectMob;

    	public static bool deselectChar;

    	public static bool deselectCharNoAttack;

    	public static MucTieu getInstance()
    	{
    		if (_Instance == null)
    		{
    			_Instance = new MucTieu();
    		}
    		return _Instance;
    	}

    	public static void Update()
    	{
    	}

    	public static void Paint(mGraphics g)
    	{
    	}

    	public void perform(int idAction, object p)
    	{
    		switch (idAction)
    		{
    		case 1:
    			deselectNpc = !deselectNpc;
    			GameScr.info1.addInfo("Bỏ chọn NPC: " + (deselectNpc ? "ON" : "OFF"), 0);
    			break;
    		case 2:
    			deselectMob = !deselectMob;
                GameScr.info1.addInfo("Bỏ chọn Mob: " + (deselectMob ? "ON" : "OFF"), 0);
                break;
    		case 3:
    			deselectChar = !deselectChar;
    			GameScr.info1.addInfo("Bỏ chọn Char: " + (deselectChar ? "ON" : "OFF"), 0);
    			break;
    		default: break;
    		}
    	}

    	public static void ShowMenu()
    	{
    		MyVector myVector = new MyVector();
    		myVector.addElement(new Command("Bỏ chọn NPC: " + (deselectNpc ? "ON" : "OFF"), getInstance(), 1, null));
    		myVector.addElement(new Command("Bỏ chọn Quái: " + (deselectMob ? "ON" : "OFF"), getInstance(), 2, null));
    		myVector.addElement(new Command("Bỏ chọn Người: "+ (deselectChar ? "ON" : "OFF"), getInstance(), 3, null));
    		GameCanvas.menu.startAt(myVector, 3);
    	}

    	public static void loadData()
    	{
    	}
    }
}
