using System;

namespace Mod.CuongLe
{
    public class DoHoa : IActionListener
    {
    	private static DoHoa _Instance;

    	public static bool HienThiLogo;

    	public static bool HienThiBackground;

    	public static bool isShowCharsInMap;

    	public static bool isHuntingBoss;

    	public static bool MapLuoi;

    	public static mFont DrawFont;

        public static bool isShowMenuVIP = false;

        public static Image imgCheck = GameCanvas.loadImage("/mainImage/myTexture2dcheck.png");

    	public static Image imgBoxItem =GameCanvas.loadImage("/mainImage/BoxMenu.png");

    	public static Image imgBoxOpenItem = GameCanvas.loadImage("/mainImage/BoxMenuOpen.png");

        public static Image imgLogo = mSystem.loadImage("/mainImage/logo1E.png");

        public static Image imgSetting = GameCanvas.loadImage("/pc/myTexture2dSettings.png");

        public static DoHoa getInstance()
    	{
    		if (_Instance == null)
    		{
    			_Instance = new DoHoa();
    		}
    		return _Instance;
    	}

    	public static void Update()
    	{
    	}

    	public static void Paint(mGraphics g)
    	{
            if (HienThiLogo)
            {
                g.drawImage(DoHoa.imgLogo, GameCanvas.w / 2, 20, 3);
            }
            if (isShowMenuVIP)
    		{
                g.drawImage(imgBoxOpenItem, GameScr.imgPanel.getWidth() + 5 + DoHoa.imgSetting.getWidth() + 5, 3);
            }
    		else
    		{
                g.drawImage(imgBoxItem, GameScr.imgPanel.getWidth() + 5 + DoHoa.imgSetting.getWidth() + 5, 3);
            }
            g.drawImage(imgSetting, GameScr.imgPanel.getWidth() + 5 , 3);
        }

    	public void perform(int idAction, object p)
    	{
    		switch (idAction)
    		{
    		case 1:
    			HienThiLogo = !HienThiLogo;
    			Rms.saveRMSInt("saveLogo", HienThiLogo ? 1 : 0);
    			break;
    		case 2:
    			HienThiBackground = !HienThiBackground;
    			Rms.saveRMSInt("saveBackground", HienThiBackground ? 1 : 0);
    			break;
    		case 3:
    			isHuntingBoss = !isHuntingBoss;
    			Rms.saveRMSInt("sanboss", isHuntingBoss ? 1 : 0);
    			break;
    		case 4:
    			isShowCharsInMap = !isShowCharsInMap;
    			Rms.saveRMSInt("showchar", isShowCharsInMap ? 1 : 0);
    			break;
    		case 5:
    			MapLuoi = !MapLuoi;
    			Rms.saveRMSInt("mapLuoi", MapLuoi ? 1 : 0);
    			break;
    		}
    	}

    	public static void ShowMenuDoHoa()
    	{
    		MyVector myVector = new MyVector();
    		myVector.addElement(new Command("Logo game: " + (HienThiLogo ? "ON" : "OFF"), getInstance(), 1, null));
    		myVector.addElement(new Command("Background: " + (HienThiBackground ? "ON" : "OFF"), getInstance(), 2, null));
    		myVector.addElement(new Command("Thông báo Boss: " + (isHuntingBoss ? "ON" : "OFF"), getInstance(), 3, null));
    		myVector.addElement(new Command("Danh sách nhân vật: " + (isShowCharsInMap ? "ON" : "OFF"), getInstance(), 4, null));
    		myVector.addElement(new Command("Địa hình dạng lưới: " + (MapLuoi ? "ON" : "OFF"), getInstance(), 5, null));
    		GameCanvas.menu.startAt(myVector, 3);
    	}

    	public static void loadData()
    	{
    		HienThiLogo = Rms.loadRMSInt("saveLogo") == 1;
    		HienThiBackground = Rms.loadRMSInt("saveBackground") == 1;
            isHuntingBoss = Rms.loadRMSInt("sanboss") == 1;
    		isShowCharsInMap = Rms.loadRMSInt("showchar") == 1;
    		MapLuoi = Rms.loadRMSInt("mapLuoi") == 1;
            DrawFont = mFont.tahoma_7;
        }

    	static DoHoa()
    	{
    		HienThiBackground = true;
    		isShowCharsInMap = true;
    		isHuntingBoss = true;
    		DrawFont = mFont.tahoma_7;
    	}

        public string getOptionInfo(Item item)
        {
            if (item == null || !getLogicOPT(item) || item.template.type == 5 || (item.template.type > 5 && item.template.type != 32))
            {
                return string.Empty;
            }

            ItemOption[] itemOption = item.itemOption;
            string result = string.Empty;

            foreach (ItemOption opt in itemOption)
            {
                int id = opt.optionTemplate.id;
                int param = opt.param;
                switch (id)
                {
                    case 50:
                        result += param + "% SĐ ";
                        break;
                    case 77:
                        result += param + "% HP ";
                        break;
                    case 80:
                        result += param + "% HP/30s ";
                        break;
                    case 81:
                        result += param + "% KI/30s ";
                        break;
                    case 94:
                        result += param + "% Giáp ";
                        break;
                    case 95:
                        result += param + "% HM ";
                        break;
                    case 96:
                        result += param + "% HK ";
                        break;
                    case 97:
                        result += param + "% PST ";
                        break;
                    case 100:
                        result += param + "% COIN ";
                        break;
                    case 101:
                        result += param + "% EXP ";
                        break;
                    case 103:
                        result += param + "% KI ";
                        break;
                }
            }

            return result.Trim();
        }


        private bool getLogicOPT(Item item)
    	{
    		if (item == null)
    		{
    			return false;
    		}
    		if (item.itemOption == null)
    		{
    			return false;
    		}
    		ItemOption[] itemOption = item.itemOption;
    		for (int i = 0; i < itemOption.Length; i++)
    		{
    			switch (itemOption[i].optionTemplate.id)
    			{
    			case 50:
    			case 77:
    				return true;
    			case 78:
    			case 79:
    			case 80:
    			case 81:
    				return true;
    			case 94:
    			case 95:
    			case 96:
    			case 97:
    			case 100:
    			case 101:
    			case 103:
    			case 104:
    				return true;
    			}
    		}
    		return false;
    	}

    	public void paintInfoOption(mGraphics g, Item item, int x, int y)
    	{
    		if (getLogicOPT(item))
    		{
    			mFont.tahoma_7_blue.drawString(g, getOptionInfo(item), x - mFont.tahoma_7_blue.getWidth(getOptionInfo(item)), y, mFont.LEFT);
    		}
    	}
   

    }
}
