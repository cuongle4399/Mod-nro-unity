using System.Threading;

namespace Mod.CuongLe
{
    public class Yardat : IActionListener
    {
        private static Yardat _Instance;

        public static bool autoPotara;

        public static bool autoChangeFocus;

        public static bool petGoHome;

        public static Yardat getInstance()
        {
            if (_Instance == null)
            {
                _Instance = new Yardat();
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
                    new Thread(AutoChangeFocus).Start();
                    autoChangeFocus = !autoChangeFocus;
                    GameScr.info1.addInfo("Auto đổi mục tiêu :  " + (autoChangeFocus ? "ON" : "Off"), 0);
                    break;
                    break;
                case 2:
                    new Thread(AutoPotaraFusion).Start();
                    autoPotara = !autoPotara;
                    GameScr.info1.addInfo("Auto bông tai :  " + (autoPotara ? "ON" : "Off"), 0);
                    break;
                    break;
                case 3:
                    petGoHome = !petGoHome;
                    if (petGoHome && !Char.myCharz().havePet)
                    {
                        GameScr.info1.addInfo("Mày làm gì có đệ ???", 0);
                        petGoHome = false;
                        return;
                    }
                    GameScr.info1.addInfo("Auto Đệ về nhà :  " + (petGoHome ? "ON" : "Off"), 0);
                    break;
                default: break;
            }
        }
        public static void AutoChangeFocus()
        {
            while (autoChangeFocus)
            {
                GameCanvas.gI().keyPressedz(-7);
                Thread.Sleep(5000);
            }
        }
        public static void AutoPotaraFusion()
        {
            while (autoPotara)
            {
                if (!Char.myCharz().meDead)
                {
                    Service.gI().useItem(0, 1, (sbyte)ModProCuongLe.FindItemBag(454).indexUI, -1);
                    if (Char.myCharz().isNhapThe)
                    {
                        Char.myCharz().isNhapThe = false;
                        Thread.Sleep(11000);
                    }
                    else
                    {
                        Char.myCharz().isNhapThe = true;
                        Thread.Sleep(2000);

                    }
                }
                Thread.Sleep(100);
            }
        }
        public static void ShowMenu()
        {
            MyVector myVector3 = new MyVector();
            myVector3.addElement(new Command(autoChangeFocus ? "Auto đổi mục tiêu : ON" : "Auto đổi mục tiêu: OFF", getInstance(), 1, null));
            myVector3.addElement(new Command(autoPotara ? "Auto bông tai liên tục: ON" : "Auto bông tai liên tục: OFF", getInstance(), 2, null));
            myVector3.addElement(new Command(petGoHome ? "Auto Đệ về nhà: ON" : "Auto Đệ về nhà: OFF", getInstance(), 3, null));
            GameCanvas.menu.startAt(myVector3, 3);
        }
         public static void update()
        {
            if (petGoHome && Char.myPetz().petStatus != 3 && !Char.myCharz().isNhapThe && GameCanvas.gameTick % 55 == 0)
            {
                Service.gI().petStatus(3);
            }
        }
        public static void loadData()
        {
        }
    }
}
