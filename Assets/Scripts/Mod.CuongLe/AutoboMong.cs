using System;
using System.Threading;
using Mod.CuongLe;
using UnityEngine;

public class AutoboMong : IActionListener, IChatable
{
    private static AutoboMong _Instance;

    private static readonly object _lock;

    public static bool autoboMong;

    public static bool waitAuto;

    public static int level;

    public static string checkInfo;

    public static bool trainning;

    public static string checknv;

    public static MobInfo[] arr;

    public static bool killCharing;

    public static bool trainVang;

    public static bool nextnvVang;

    public static bool nextnvNguoi;

    public static bool nextnvQuai;

    public static bool skipDone;

    public static bool chooseTypeGod;

    public static bool PickGoding;

    public static int IdMapTrainVang;

    public static int InputMapTrainVang;

    public static AutoboMong getInstance()
    {
        if (_Instance == null)
        {
            lock (_lock)
            {
                if (_Instance == null)
                {
                    _Instance = new AutoboMong();
                }
            }
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
                autoboMong = !autoboMong;
                if (!autoboMong)
                {
                    trainVang = false;
                    killCharing = false;
                    trainning = false;
                    AutoTrain.isGoBack = false;
                    InfoMe.FinishBoMong = false;
                    skipDone = false;
                    AutoMap.isEatChicken = true;
                    AutoPick.isAutoPick = false;
                    AutoPick.pickByList = 0;
                }
                else if (!InfoMe.EndNvBoMong)
                {
                    new Thread(StartAuto).Start();
                }
                else
                {
                    autoboMong = false;
                    ChatPopup.addChatPopupMultiLineGame("Đã hết nv hằng ngày rồi mà", 0, null);
                }
                break;
            case 2:
                switch (level)
                {
                    case 0:
                        level = 1;
                        ChatPopup.addChatPopupMultiLineGame("Đã đổi sang mức độ khó", 0, null);
                        break;
                    case 1:
                        level = 2;
                        ChatPopup.addChatPopupMultiLineGame("Đã đổi sang mức độ dễ", 0, null);
                        break;
                    case 2:
                        level = 0;
                        ChatPopup.addChatPopupMultiLineGame("Đã đổi sang mức độ siêu khó", 0, null);
                        break;
                }
                break;
            case 3:
                nextnvVang = !nextnvVang;
                if (nextnvVang)
                {
                    ChatPopup.addChatPopupMultiLineGame("Đã bỏ nv nhặt vàng", 0, null);
                }
                else
                {
                    ChatPopup.addChatPopupMultiLineGame("OK", 0, null);
                }
                break;
            case 4:
                ChatPopup.addChatPopupMultiLineGameline("Mặc định sẽ bỏ qua nv ăn trộm \nMod sẽ tự bật tdlt nếu chưa bật. Tránh gặp lỗi train quái địa hình phức tạp.Làm nhiệm vụ nhặt vàng có 2 cách: 1 đi train quái;2 tự đấm bản thân xok lụm vàng, hoặc có thể ấn bỏ luôn nv vàng cx dc\n nv pem người sẽ âm thầm bug đấm bản thân, yên tâm mod sẽ tự đi trốn rồi mới bật auto. CUỐI CÙNG CÓ TDLT VÀ CAPSUN BAY LÀ 1 LỢI THẾ: CHỐNG BAN + LAG!!!", 0, null, 5);
                break;
            case 5:
                nextnvNguoi = !nextnvNguoi;
                if (nextnvNguoi)
                {
                    ChatPopup.addChatPopupMultiLineGame("Đã bỏ nv pem người", 0, null);
                }
                else
                {
                    ChatPopup.addChatPopupMultiLineGame("OK", 0, null);
                }
                break;
            case 6:
                nextnvQuai = !nextnvQuai;
                if (nextnvQuai)
                {
                    ChatPopup.addChatPopupMultiLineGame("Đã bỏ nv pem quái", 0, null);
                }
                else
                {
                    ChatPopup.addChatPopupMultiLineGame("OK", 0, null);
                }
                break;
            case 7:
                Rms.saveRMSInt("nextnvVang", nextnvVang ? 1 : 0);
                Rms.saveRMSInt("nextnvNguoi", nextnvNguoi ? 1 : 0);
                Rms.saveRMSInt("nextnvQuai", nextnvQuai ? 1 : 0);
                Rms.saveRMSInt("ChooseTypeGod", chooseTypeGod ? 1 : 0);
                Rms.saveRMSInt("level", level);
                ChatPopup.addChatPopupMultiLineGame("Đã lưu cài đặt", 0, null);
                break;
            case 8:
                chooseTypeGod = !chooseTypeGod;
                if (chooseTypeGod)
                {
                    ChatPopup.addChatPopupMultiLineGame("Đã chọn tự pem lụm vàng bản thân", 0, null);
                }
                else
                {
                    ChatPopup.addChatPopupMultiLineGame("Đã chọn train quái lụm vàng", 0, null);
                }
                break;
            case 9:
                ChatTextField.gI().strChat = "idMapVang";
                ChatTextField.gI().tfChat.name = "Nhập ID Map làm nv up vàng";
                ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                ChatTextField.gI().startChat2(getInstance(), string.Empty);
                break;
        }
    }

    public static void ShowMenu()
    {
        MyVector myVector = new MyVector();
        myVector.addElement(new Command((!autoboMong) ? "Bắt đầu" : "Dừng", getInstance(), 1, null));
        myVector.addElement(new Command("Đổi mức độ\nHiện tại: " + ((level == 0) ? "Siêu Khó" : ((level == 1) ? "Khó" : "Dễ")), getInstance(), 2, null));
        myVector.addElement(new Command("Bỏ nv nhặt vàng: " + (nextnvVang ? "ON" : "OFF"), getInstance(), 3, null));
        myVector.addElement(new Command("Bỏ nv pem người: " + (nextnvNguoi ? "ON" : "OFF"), getInstance(), 5, null));
        myVector.addElement(new Command("Bỏ nv pem quái: " + (nextnvQuai ? "ON" : "OFF"), getInstance(), 6, null));
        myVector.addElement(new Command("Chọn kiểu NV vàng: " + (chooseTypeGod ? "Nhặt vàng" : "Up Quái"), getInstance(), 8, null));
        myVector.addElement(new Command("Đổi map up vàng: " + TileMap.mapNames[(IdMapTrainVang != -1) ? IdMapTrainVang : ((Char.myCharz().taskMaint.taskId < 22) ? 10 : ((Char.myCharz().taskMaint.taskId == 22 || Char.myCharz().taskMaint.taskId == 23) ? 68 : ((Char.myCharz().taskMaint.taskId <= 25) ? 77 : ((Char.myCharz().taskMaint.taskId >= 33 && Char.myCharz().cPower >= 60000000000L) ? 155 : 80))))], getInstance(), 9, null));
        myVector.addElement(new Command("Lưu cài đặt", getInstance(), 7, null));
        myVector.addElement(new Command("Xem cách hoạt động", getInstance(), 4, null));
        GameCanvas.menu.startAt(myVector, 3);
    }

    public static void loadData()
    {
        nextnvVang = Rms.loadRMSInt("nextnvVang") == 1;
        nextnvNguoi = Rms.loadRMSInt("nextnvNguoi") == 1;
        nextnvQuai = Rms.loadRMSInt("nextnvQuai") == 1;
        chooseTypeGod = Rms.loadRMSInt("ChooseTypeGod") == 1;
        int num = Rms.loadRMSInt("level");
        if (num >= 0 && num <= 2)
        {
            level = num;
        }
        else
        {
            level = 0;
        }
    }

    public static void StartAuto()
    {
        trainVang = false;
        killCharing = false;
        trainning = false;
        AutoTrain.isGoBack = false;
        InfoMe.FinishBoMong = false;
        skipDone = false;
        AutoMap.isEatChicken = true;
        AutoPick.isAutoPick = false;
        AutoPick.pickByList = 0;
        AutoTrain.TuMoTDLT();
        while (autoboMong)
        {
            InfoMe.NhanTinHieu = false;
            try
            {
                Thread.Sleep(200);
                while (TileMap.mapID != 47 && autoboMong)
                {
                    AutoMap.StartRunToMapId(47);
                    while (AutoMap.isXmaping)
                    {
                        Thread.Sleep(1000);
                    }
                }
                Thread.Sleep(150);
                ModProCuongLe.teleNPC(17);
                Thread.Sleep(150);
                Service.gI().openMenu(17);
                Thread.Sleep(150);
                Service.gI().confirmMenu(17, 2);
                Service.gI().confirmMenu(17, (sbyte)level);
                GameCanvas.menu.doCloseMenu();
                Thread.Sleep(1000);
                while (!InfoMe.NhanTinHieu && !trainning && !killCharing && !trainVang && autoboMong)
                {
                    ModProCuongLe.teleNPC(17);
                    Thread.Sleep(150);
                    Service.gI().openMenu(17);
                    Thread.Sleep(150);
                    Service.gI().confirmMenu(17, 2);
                    Service.gI().confirmMenu(17, 0);
                    GameCanvas.menu.doCloseMenu();
                    Thread.Sleep(2000);
                }
                if (!InfoMe.EndNvBoMong)
                {
                    DoneNV();
                }
            }
            catch
            {
            }
        }
    }

    static AutoboMong()
    {
        IdMapTrainVang = -1;
        _lock = new object();
        arr = new MobInfo[95]
        {
            new MobInfo("Mộc nhân", 14, 0),
            new MobInfo("Khủng long", 1, 1),
            new MobInfo("Lợn lòi", 8, 2),
            new MobInfo("Quỷ đất", 15, 3),
            new MobInfo("Khủng long mẹ", 2, 4),
            new MobInfo("Lợn lòi mẹ", 9, 5),
            new MobInfo("Quỷ đất mẹ", 16, 6),
            new MobInfo("Thằn lằn bay", 3, 7),
            new MobInfo("Phi long", 11, 8),
            new MobInfo("Quỷ bay", 17, 9),
            new MobInfo("Thằn lằn mẹ", 4, 10),
            new MobInfo("Phi long mẹ", 12, 11),
            new MobInfo("Quỷ bay mẹ", 18, 12),
            new MobInfo("Ốc mượn hồn", 29, 13),
            new MobInfo("Ốc sên", 33, 14),
            new MobInfo("Heo Xayda mẹ", 37, 15),
            new MobInfo("Heo rừng", 28, 16),
            new MobInfo("Heo da xanh", 32, 17),
            new MobInfo("Heo Xayda", 36, 18),
            new MobInfo("Heo rừng mẹ", 6, 19),
            new MobInfo("Heo xanh mẹ", 10, 20),
            new MobInfo("Alien", 19, 21),
            new MobInfo("Bulon", 30, 22),
            new MobInfo("Ukulele", 34, 23),
            new MobInfo("Quỷ mập", 38, 24),
            new MobInfo("Tambourine", 6, 25),
            new MobInfo("Drum", 10, 26),
            new MobInfo("Akkuman", 19, 27),
            new MobInfo("Thằn lằn bay 2", -1, 28),
            new MobInfo("Phi long 2", -1, 29),
            new MobInfo("Quỷ bay 2", -1, 30),
            new MobInfo("Không tặc", 29, 31),
            new MobInfo("Quỷ đầu to", 33, 32),
            new MobInfo("Quỷ địa ngục", 37, 33),
            new MobInfo("Lính độc nhãn", -1, 34),
            new MobInfo("Lính độc nhãn", -1, 35),
            new MobInfo("Sói xám", -1, 36),
            new MobInfo("Robot bay", -1, 37),
            new MobInfo("Robot thép", -1, 38),
            new MobInfo("Nappa", 68, 39),
            new MobInfo("Soldier", 70, 40),
            new MobInfo("Appule", 71, 41),
            new MobInfo("Raspberry", 71, 42),
            new MobInfo("Thằn lằn xanh", 72, 43),
            new MobInfo("Quỷ đầu nhọn", 64, 44),
            new MobInfo("Quỷ đầu vàng", 63, 45),
            new MobInfo("Quỷ da tím", 66, 46),
            new MobInfo("Quỷ già", 67, 47),
            new MobInfo("Cá sấu", 73, 48),
            new MobInfo("Dơi da xanh", 67, 49),
            new MobInfo("Quỷ chim", 81, 50),
            new MobInfo("Lính đầu trọc", 74, 51),
            new MobInfo("Lính tai dài", 76, 52),
            new MobInfo("Lính vũ trụ", 77, 53),
            new MobInfo("Khỉ lông đen", 82, 54),
            new MobInfo("Khỉ giáp sắt", 83, 55),
            new MobInfo("Khỉ lông đỏ", 79, 56),
            new MobInfo("Khỉ lông vàng", 80, 57),
            new MobInfo("Xên con cấp 1", 92, 58),
            new MobInfo("Xên con cấp 2", 93, 59),
            new MobInfo("Xên con cấp 3", 94, 60),
            new MobInfo("Xên con cấp 4", 96, 61),
            new MobInfo("Xên con cấp 5", 97, 62),
            new MobInfo("Xên con cấp 6", 98, 63),
            new MobInfo("Xên con cấp 7", 99, 64),
            new MobInfo("Xên con cấp 8", 100, 65),
            new MobInfo("Tai tím", 106, 66),
            new MobInfo("Abo", 107, 67),
            new MobInfo("Kado", 109, 68),
            new MobInfo("Da xanh", 110, 69),
            new MobInfo("Hirudegarn", -1, 70),
            new MobInfo("Vua Bạch Tuộc", -1, 71),
            new MobInfo("Rôbốt bảo vệ", -1, 72),
            new MobInfo("Kawazu", -1, 73),
            new MobInfo("Kinkarn", -1, 74),
            new MobInfo("Arbee", -1, 75),
            new MobInfo("Cỗ máy hủy diệt", -1, 76),
            new MobInfo("Gấu tướng cướp", -1, 77),
            new MobInfo("Khỉ lông xanh", 155, 78),
            new MobInfo("Taburine Đỏ", 155, 79),
            new MobInfo("Cabira", -1, 80),
            new MobInfo("Tobi", -1, 81),
            new MobInfo("Voi Chín Ngà", -1, 82),
            new MobInfo("Gà Chín Cựa", -1, 83),
            new MobInfo("Ngựa Chín Lmao", -1, 84),
            new MobInfo("Piano", -1, 85),
            new MobInfo("Ếch mặt đỏ", 166, 86),
            new MobInfo("Jinai", 166, 87),
            new MobInfo("Quỷ đỏ", -1, 88),
            new MobInfo("Quỷ xanh", -1, 89),
            new MobInfo("Quỷ xanh lá", -1, 90),
            new MobInfo("Quỷ vàng", -1, 91),
            new MobInfo("Godzilla", -1, 92),
            new MobInfo("Kong", -1, 93),
            new MobInfo("Máy đo sức mạnh", 42, 94)
        };
        waitAuto = true;
        checkInfo = "thời gian nhận nhiệm vụ";
        checknv = "đã hết nhiệm vụ cho hôm nay, hãy chờ đến ngày mai";
    }

    public static int typeNV(string x)
    {
        if (x.Contains("địa điểm"))
        {
            return 0;
        }
        if (x.Contains("vàng"))
        {
            return 1;
        }
        if (x.Contains("người"))
        {
            return 2;
        }
        if (x.Contains("ăn trộm"))
        {
            return 3;
        }
        return 4;
    }

    public static void huyNV()
    {
        Thread.Sleep(500);
        Service.gI().openMenu(17);
        Thread.Sleep(150);
        Service.gI().confirmMenu(17, 2);
        Thread.Sleep(50);
        Service.gI().confirmMenu(17, 1);
        Thread.Sleep(50);
        GameCanvas.menu.doCloseMenu();
        InfoMe.FinishBoMong = true;
        skipDone = true;
    }

    public static void DoneNV()
    {
        try
        {
            while (!InfoMe.FinishBoMong)
            {
                Thread.Sleep(2000);
            }
            if (skipDone)
            {
                trainVang = false;
                killCharing = false;
                trainning = false;
                AutoTrain.isGoBack = false;
                InfoMe.FinishBoMong = false;
                skipDone = false;
                AutoMap.isEatChicken = true;
                AutoPick.isAutoPick = false;
                AutoPick.pickByList = 0;
                return;
            }
            if (AutoPick.isAutoPick)
            {
                AutoPick.isAutoPick = false;
                AutoPick.pickByList = 0;
            }
            if (!AutoMap.isEatChicken)
            {
                AutoMap.isEatChicken = true;
            }
            AutoTrain.isGoBack = false;
            AutoTrain.getInstance().perform(8, null);
            while (TileMap.mapID != 47 && autoboMong)
            {
                AutoMap.StartRunToMapId(47);
                while (AutoMap.isXmaping)
                {
                    Thread.Sleep(1200);
                }
            }
            ModProCuongLe.teleNPC(17);
            Thread.Sleep(150);
            Service.gI().openMenu(17);
            Service.gI().confirmMenu(17, 2);
            Service.gI().confirmMenu(17, 0);
            GameCanvas.menu.doCloseMenu();
            trainVang = false;
            killCharing = false;
            trainning = false;
            AutoTrain.isGoBack = false;
            InfoMe.FinishBoMong = false;
        }
        catch
        {
        }
    }

    public void nvTrainQuai()
    {
        try
        {
            trainning = true;
            while (string.IsNullOrEmpty(InfoMe.nameMap))
            {
                Thread.Sleep(1000);
            }
            string mapModID = GetMapModID(InfoMe.nameMap);
            if (mapModID == "-1|-1")
            {
                autoboMong = false;
                trainning = false;
                ChatPopup.addChatPopupMultiLineGame("Lỗi tìm map quái", 0, null);
                return;
            }
            string[] array = mapModID.Split('|');
            if (!int.TryParse(array[0], out var result) || !int.TryParse(array[1], out var result2))
            {
                autoboMong = false;
                trainning = false;
                ChatPopup.addChatPopupMultiLineGame("Lỗi tìm map quái", 0, null);
                return;
            }
            while (TileMap.mapID != result)
            {
                AutoMap.StartRunToMapId(result);
                Thread.Sleep(1000);
                while (AutoMap.isXmaping)
                {
                    Thread.Sleep(1000);
                }
            }
            if (TileMap.mapID >= 63 && TileMap.mapID <= 83)
            {
                while (TileMap.zoneID != 1 && autoboMong)
                {
                    Service.gI().requestChangeZone(1, -1);
                    Thread.Sleep(1100);
                }
            }
            else
            {
                int zoneId = RandomZoneFrom1To14();
                while (TileMap.zoneID <= 1 && autoboMong)
                {
                    Service.gI().requestChangeZone(zoneId, -1);
                    Thread.Sleep(1100);
                }
            }
            Thread.Sleep(200);
            AutoTrain.isGobackCoordinate = false;
            AutoTrain.isGoBack = true;
            AutoTrain.gobackMapID = TileMap.mapID;
            AutoTrain.gobackZoneID = TileMap.zoneID;
            AutoTrain.getInstance().perform(1, result2);
        }
        catch (Exception)
        {
            autoboMong = false;
            trainning = false;
            ChatPopup.addChatPopupMultiLineGame("Lỗi tìm map quái", 0, null);
        }
    }

    public static string GetMapModID(string NameMob)
    {
        if (string.IsNullOrEmpty(NameMob))
        {
            return "-1|-1";
        }
        string result = "-1|-1";
        int num = -1;
        string text = NameMob.ToLower().Trim();
        MobInfo[] array = arr;
        MobInfo[] array2 = array;
        MobInfo[] array3 = array2;
        MobInfo[] array4 = array3;
        foreach (MobInfo mobInfo in array4)
        {
            string text2 = mobInfo.NameMob.ToLower().Trim();
            if (text.Contains(text2))
            {
                int length = text2.Length;
                if (length > num)
                {
                    num = length;
                    result = mobInfo.IdMap + "|" + mobInfo.IdMob;
                }
            }
        }
        return result;
    }

    public void nvPemNguoi()
    {
        killCharing = true;
        AutoMap.isEatChicken = false;
        int num = 44;
        if (TileMap.mapID != 42 || TileMap.mapID != 43 || TileMap.mapID != 44)
        {
            num = ((Char.myCharz().getGender() == "TĐ") ? 42 : ((!(Char.myCharz().getGender() == "XD")) ? 43 : 44));
        }
        while (TileMap.mapID != num && autoboMong)
        {
            AutoMap.StartRunToMapId(num);
            Thread.Sleep(1000);
            while (AutoMap.isXmaping)
            {
                Thread.Sleep(1000);
            }
        }
        int zoneId = RandomZoneFrom1To14();
        while (TileMap.zoneID <= 1 && autoboMong)
        {
            Service.gI().requestChangeZone(zoneId, -1);
            Thread.Sleep(1100);
        }
        AutoTrain.isGobackCoordinate = false;
        AutoTrain.isGoBack = true;
        AutoTrain.gobackMapID = TileMap.mapID;
        while (!InfoMe.FinishBoMong && autoboMong)
        {
            if (!ModProCuongLe.suicide && (TileMap.mapID == 43 || TileMap.mapID == 42 || TileMap.mapID == 44))
            {
                while (TileMap.zoneID <= 1)
                {
                    Service.gI().requestChangeZone(zoneId, -1);
                    Thread.Sleep(1100);
                }
                selectSkill();
                Thread.Sleep(300);
                ModProCuongLe.toimuonchet();
            }
            Thread.Sleep(2300);
        }
    }

    public void nvTrainVang()
    {
        trainVang = true;
        int taskId = Char.myCharz().taskMaint.taskId;
        long cPower = Char.myCharz().cPower;
        int num = ((IdMapTrainVang != -1) ? IdMapTrainVang : ((taskId < 22) ? 10 : ((taskId != 22 && taskId != 23) ? ((taskId <= 25) ? 77 : ((taskId < 33) ? 80 : ((taskId < 33 || cPower < 60000000000L) ? 80 : 155))) : 68)));
        while (TileMap.mapID != num && autoboMong)
        {
            AutoMap.StartRunToMapId(num);
            Thread.Sleep(1000);
            while (AutoMap.isXmaping)
            {
                Thread.Sleep(1000);
            }
        }
        if (TileMap.mapID != 10 || TileMap.mapID != 68 || TileMap.mapID != 77)
        {
            int zoneId = RandomZoneFrom1To14();
            while (TileMap.zoneID < 1 && autoboMong)
            {
                Service.gI().requestChangeZone(zoneId, -1);
                Thread.Sleep(1100);
            }
        }
        else
        {
            while (TileMap.zoneID != 1 && autoboMong)
            {
                Service.gI().requestChangeZone(1, -1);
                Thread.Sleep(1100);
            }
        }
        AutoTrain.isGobackCoordinate = false;
        AutoTrain.isGoBack = true;
        AutoTrain.gobackMapID = TileMap.mapID;
        AutoTrain.gobackZoneID = TileMap.zoneID;
        AutoPick.isAutoPick = true;
        AutoMap.isEatChicken = true;
        AutoPick.pickByList = 0;
        Thread.Sleep(200);
        switch (num)
        {
            case 77:
                AutoTrain.getInstance().perform(1, 53);
                break;
            case 68:
                AutoTrain.getInstance().perform(1, 39);
                break;
            default:
                AutoTrain.getInstance().perform(2, null);
                break;
        }
    }

    public void nvTrainVang2()
    {
        trainVang = true;
        AutoMap.isEatChicken = false;
        int num = 44;
        if (TileMap.mapID != 42 || TileMap.mapID != 43 || TileMap.mapID != 44)
        {
            num = ((Char.myCharz().getGender() == "TĐ") ? 42 : ((!(Char.myCharz().getGender() == "XD")) ? 43 : 44));
        }
        while (TileMap.mapID != num)
        {
            AutoMap.StartRunToMapId(num);
            Thread.Sleep(1000);
            while (AutoMap.isXmaping)
            {
                Thread.Sleep(1000);
            }
        }
        int zoneId = RandomZoneFrom1To14();
        while (TileMap.zoneID <= 1 && autoboMong)
        {
            Service.gI().requestChangeZone(zoneId, -1);
            Thread.Sleep(1100);
        }
        AutoTrain.isGobackCoordinate = false;
        AutoTrain.isGoBack = true;
        AutoTrain.gobackMapID = TileMap.mapID;
        AutoTrain.gobackZoneID = TileMap.zoneID;
        while (!InfoMe.FinishBoMong && autoboMong)
        {
            if (!ModProCuongLe.suicide && (TileMap.mapID == 43 || TileMap.mapID == 42 || TileMap.mapID == 44))
            {
                while (TileMap.zoneID <= 1 && autoboMong)
                {
                    Service.gI().requestChangeZone(zoneId, -1);
                    Thread.Sleep(1100);
                }
                Thread.Sleep(100);
                LumVang();
                while (PickGoding)
                {
                    Thread.Sleep(1000);
                }
                ModProCuongLe.toimuonchet();
            }
            Thread.Sleep(3000);
        }
    }

    public void LumVang()
    {
        PickGoding = true;
        while (checkGod())
        {
            for (int i = 0; i < GameScr.vItemMap.size(); i++)
            {
                ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
                if (Math.abs(Char.myCharz().cx - itemMap.x) <= 100 && itemMap != null && (itemMap.template.id == 76 || itemMap.template.id == 188 || itemMap.template.id == 189 || itemMap.template.id == 190))
                {
                    Char.myCharz().mobFocus = null;
                    Char.myCharz().charFocus = null;
                    Char.myCharz().npcFocus = null;
                    Char.myCharz().itemFocus = itemMap;
                    Service.gI().pickItem(itemMap.itemMapID);
                }
            }
            Thread.Sleep(200);
        }
        PickGoding = false;
    }

    private bool checkGod()
    {
        for (int i = 0; i < GameScr.vItemMap.size(); i++)
        {
            ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
            if (Math.abs(Char.myCharz().cx - itemMap.x) <= 100 && itemMap != null && (itemMap.template.id == 76 || itemMap.template.id == 188 || itemMap.template.id == 189 || itemMap.template.id == 190))
            {
                return true;
            }
        }
        return false;
    }

    public void selectSkill()
    {
        try
        {
            Skill skill = null;
            for (int i = 0; i < GameScr.keySkill.Length; i++)
            {
                if (GameScr.keySkill[i] == null || GameScr.keySkill[i].paintCanNotUseSkill || ((!(Char.myCharz().getGender() == "XD") || GameScr.keySkill[i].template.id != 4) && (!(Char.myCharz().getGender() == "NM") || GameScr.keySkill[i].template.id != 2) && (!(Char.myCharz().getGender() == "TĐ") || GameScr.keySkill[i].template.id != 0)))
                {
                    continue;
                }
                int num = (int)((GameScr.keySkill[i].template.manaUseType == 2) ? 1 : ((GameScr.keySkill[i].template.manaUseType == 1) ? (GameScr.keySkill[i].manaUse * Char.myCharz().cMPFull / 100) : GameScr.keySkill[i].manaUse));
                if (Char.myCharz().cMP >= num)
                {
                    if (skill == null)
                    {
                        skill = GameScr.keySkill[i];
                    }
                    else if (skill.coolDown < GameScr.keySkill[i].coolDown)
                    {
                        skill = GameScr.keySkill[i];
                    }
                }
            }
            if (skill != null)
            {
                GameScr.gI().doSelectSkill(skill, isShortcut: true);
            }
        }
        catch
        {
        }
    }

    public static int RandomZoneFrom1To14()
    {
        return UnityEngine.Random.Range(1, 15);
    }

    public void onChatFromMe(string text, string to)
    {
        if (ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Equals(string.Empty) || text.Equals(string.Empty) || text == null)
        {
            ChatTextField.gI().isShow = false;
            ResetChatTextField();
        }
        else if (ChatTextField.gI().strChat.Equals("idMapVang"))
        {
            try
            {
                if (string.IsNullOrEmpty(text.Trim()))
                {
                    GameScr.info1.addInfo("Nhập đi chứ dm!", 0);
                    return;
                }
                if (int.TryParse(text.Trim(), out var result))
                {
                    if (result < 0 || result >= TileMap.mapNames.Length || TileMap.mapNames[result] == null)
                    {
                        GameScr.info1.addInfo("Id Map up vàng không tồn tại !", 0);
                        return;
                    }
                    IdMapTrainVang = result;
                    GameScr.info1.addInfo("Đã đổi map up vàng thành " + TileMap.mapNames[IdMapTrainVang], 0);
                }
            }
            catch
            {
                GameScr.info1.addInfo("Vui lòng nhập đúng định dạng Id Map", 0);
            }
            ResetChatTextField();
        }
        else
        {
            Service.gI().chat(text);
            ChatTextField.gI().isShow = false;
        }
    }

    private static void ResetChatTextField()
    {
        ChatTextField.gI().strChat = "Chat";
        ChatTextField.gI().tfChat.name = "chat";
        ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
        ChatTextField.gI().isShow = false;
    }

    public void onCancelChat()
    {
    }
}
