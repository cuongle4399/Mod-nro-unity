using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mod.CuongLe
{
    public class ModProCuongLe : IActionListener
    {
        private static ModProCuongLe _Instance;

        public static bool HealingPower;

        public static bool autoFlag;

        public static bool DoBoss;

        public static bool findBossMod;

        public static bool aGimBoss;

        public static bool aWhis;

        public static float timeScale;

        public static bool isAttackBoss;

        public static long timeCanAttack;

        public static bool petw;

        public static bool charw;

        public static bool isKOK;

        public static bool DoSatBossNapa;

        public static int ntMin;

        public static int ntNow;

        public static bool AutoteleBoss;

        public static bool tieuDietNguoiBatCo;

        public static List<Char> listNguoiCoDen;

        public static List<Char> listBossTrongKhu;

        public static bool tanCongBoss;

        public static bool hienThiDoKH;

        public static int songlamccginua;

        public static bool banDo;

        public static bool catDoVIP;

        public static bool suicide;

        public static ModProCuongLe getInstance()
        {
            if (_Instance == null)
            {
                _Instance = new ModProCuongLe();
            }
            return _Instance;
        }

        public static void ShowMenu()
        {
            MyVector myVector = new MyVector();
            myVector.addElement(new Command("Auto boss", getInstance(), 5, null));
            myVector.addElement(new Command("Yardat", getInstance(), 8, null));
            myVector.addElement(new Command("Auto vứt vật phẩm", getInstance(), 29, null));
            myVector.addElement(new Command("Auto bò mộng", getInstance(), 32, null));
            myVector.addElement(new Command("Nâng cao", getInstance(), 16, null));
            GameCanvas.menu.startAt(myVector, 3);
        }

        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    HealingPower = !HealingPower;
                    new Thread(AutoHealingPower).Start();
                    break;
                case 2:
                    autoFlag = !autoFlag;
                    new Thread(autoCoDen).Start();
                    GameScr.info1.addInfo(autoFlag ? "Auto Cờ đen chống địch: ON" : "Auto Cờ đen chống địch: OFF", 0);
                    break;
                case 3:
                    DoBoss = !DoBoss;
                    if (DoBoss)
                    {
                        new Thread(autoDoBoss).Start();
                    }
                    GameScr.info1.addInfo(DoBoss ? "Dò boss: ON" : "Dò boss: OFF", 0);
                    break;
                case 4:
                    break;
                case 5:
                    ShowMenuBoss();
                    break;
                case 6:
                    //new Thread(AutoChangeFocus).Start();
                    //autoChangeFocus = !autoChangeFocus;
                    //GameScr.info1.addInfo("Auto đổi mục tiêu :  " + (autoChangeFocus ? "ON" : "Off"), 0);
                    break;
                case 7:
                    //new Thread(AutoPotaraFusion).Start();
                    //autoPotara = !autoPotara;
                    //GameScr.info1.addInfo("Auto bông tai + đệ về nhà :  " + (autoPotara ? "ON" : "Off"), 0);
                    break;
                case 8:

                    Yardat.ShowMenu();
                    break;
                
                case 9:
                    aGimBoss = !aGimBoss;
                    new Thread(AutoFocusBoss).Start();
                    GameScr.info1.addInfo("Auto gim boss: " + (aGimBoss ? "Bật" : "Tắt"), 0);
                    break;
                case 10:
                    aWhis = !aWhis;
                    if (aWhis)
                    {
                        new Thread(AutoWhis).Start();
                    }
                    GameScr.info1.addInfo("Auto leo Tháp Whis: " + (aWhis ? "Bật" : "Tắt"), 0);
                    break;
                case 12:
                    break;
                case 16:
                    {
                        MyVector myVector2 = new MyVector();
                        myVector2.addElement(new Command("Auto trị thương bản thân(namek) " + (HealingPower ? "Bật" : "Tắt"), getInstance(), 1, null));
                        myVector2.addElement(new Command("Auto cờ đen chống địch: " + (autoFlag ? "Bật" : "Tắt"), getInstance(), 2, null));
                        myVector2.addElement(new Command("Auto up đệ KOK: " + (isKOK ? "Bật" : "Tắt"), getInstance(), 18, null));
                        myVector2.addElement(new Command("Tiêu diệt all người bật cờ: " + (tieuDietNguoiBatCo ? "Bật" : "Tắt"), getInstance(), 22, null));
                        myVector2.addElement(new Command("Tự đấm bản thân đến chết", getInstance(), 25, null));
                        myVector2.addElement(new Command("Tự động pem khi đệ sủa: " + (mobProMore.DeSuaLapem ? "Bật" : "Tắt"), getInstance(), 26, null));
                        myVector2.addElement(new Command("Bán đồ rác khi full ht: " + (banDo ? "Bật" : "Tắt"), getInstance(), 27, null));
                        myVector2.addElement(new Command("Cất đồ sao,kh,TL khi full ht: " + (catDoVIP ? "Bật" : "Tắt"), getInstance(), 28, null));
                        GameCanvas.menu.startAt(myVector2, 3);
                        break;
                    }
                case 17:
                    AutoteleBoss = !AutoteleBoss;
                    new Thread(teleBoss).Start();
                    GameScr.info1.addInfo("Auto dịch theo Boss\n" + (AutoteleBoss ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
                    break;
                case 18:
                    isKOK = !isKOK;
                    new Thread(autoDeKOK).Start();
                    GameScr.info1.addInfo("Auto Up Kaioken: " + (isKOK ? "[STATUS:ON]" : "[STATUS:OFF]"), 0);
                    break;
                case 19:
                    {
                        MyVector myVector = new MyVector();
                        myVector.addElement(new Command("Thông tin Sư phụ" + (charw ? "\n[STATUS: ON] " : "\n[STATUS: OFF]"), getInstance(), 20, null));
                        myVector.addElement(new Command("Thông Tin Đệ tử" + (petw ? "\n[STATUS: ON] " : "\n[STATUS: OFF]"), getInstance(), 21, null));
                        myVector.addElement(new Command("Thông Tin up vàng" + (MainMod.infoTrainGold ? "\n[STATUS: ON] " : "\n[STATUS: OFF]"), getInstance(), 30, null));
                        myVector.addElement(new Command("Danh sách sét KH: " + (hienThiDoKH ? "ON" : "OFF"), getInstance(), 24, null));
                        GameCanvas.menu.startAt(myVector, 3);
                        break;
                    }
                case 20:
                    charw = !charw;
                    if (charw)
                    {
                        MainMod.infoTrainGold = false;
                        hienThiDoKH = false;
                        petw = false;
                    }
                    MainMod.GoldCurrent = 0L;
                    GameScr.info1.addInfo("Char View " + (charw ? "[STATUS: ON] " : "[STATUS: OFF]"), 0);
                    break;
                case 21:
                    petw = !petw;
                    if (petw && !Char.myCharz().havePet)
                    {
                        petw = false;
                        ChatPopup.addChatPopupMultiLineGame("Làm gì có đệ đâu mà show ba ????", 0, null);
                        break;
                    }
                    if (petw)
                    {
                        MainMod.infoTrainGold = false;
                        hienThiDoKH = false;
                        charw = false;
                        MainMod.GoldCurrent = 0L;
                    }
                    GameScr.info1.addInfo("Pet View " + (petw ? "[STATUS: ON] " : "[STATUS: OFF]"), 0);
                    break;
                case 22:
                    AutoteleBoss = false;
                    tanCongBoss = false;
                    tieuDietNguoiBatCo = !tieuDietNguoiBatCo;
                    if (!tieuDietNguoiBatCo)
                    {
                        listNguoiCoDen.Clear();
                    }
                    else if (Char.myCharz().cFlag == 0)
                    {
                        Service.gI().getFlag(1, 8);
                    }
                    Char.myCharz().mobFocus = null;
                    Char.myCharz().itemFocus = null;
                    Char.myCharz().npcFocus = null;
                    GameScr.info1.addInfo("Chế độ đồ sát người: " + (tieuDietNguoiBatCo ? " ON " : "OFF"), 0);
                    break;
                case 23:
                    tieuDietNguoiBatCo = false;
                    tanCongBoss = !tanCongBoss;
                    if (tanCongBoss)
                    {
                        Char.myCharz().mobFocus = null;
                        Char.myCharz().itemFocus = null;
                        Char.myCharz().npcFocus = null;
                        AutoteleBoss = true;
                        new Thread(teleBoss).Start();
                    }
                    else
                    {
                        listBossTrongKhu.Clear();
                        AutoteleBoss = false;
                    }
                    GameScr.info1.addInfo("Tấn công Boss: " + (tanCongBoss ? " ON " : "OFF"), 0);
                    break;
                case 24:
                    hienThiDoKH = !hienThiDoKH;
                    if (hienThiDoKH)
                    {
                        MainMod.infoTrainGold = false;
                        petw = false;
                        MainMod.GoldCurrent = 0L;
                        charw = false;
                    }
                    break;
                case 25:
                    GameScr.info1.addInfo("Tiến hành bug nv pem người bò mộng", 0);
                    new Thread(toimuonchet).Start();
                    break;
                case 26:
                    mobProMore.DeSuaLapem = !mobProMore.DeSuaLapem;
                    GameScr.info1.addInfo("Tự động pem khi đệ sủa: " + (mobProMore.DeSuaLapem ? "Bật" : "Tắt"), 0);
                    break;
                case 27:
                    banDo = !banDo;
                    if (banDo)
                    {
                        new Thread(ShowSetKH.SellTrashItem).Start();
                    }
                    GameScr.info1.addInfo("|0|Bán đồ đang: " + (banDo ? "Bật" : "Tắt"), 0);
                    break;
                case 28:
                    catDoVIP = !catDoVIP;
                    if (catDoVIP)
                    {
                        new Thread(ShowSetKH.CatDo).Start();
                    }
                    GameScr.info1.addInfo("|0|Cất đồ kh+ spl vào rương: " + (catDoVIP ? "Bật" : "Tắt"), 0);
                    break;
                case 29:
                    AutoVutDo.ShowMenu();
                    break;
                case 30:
                    MainMod.infoTrainGold = !MainMod.infoTrainGold;
                    if (MainMod.infoTrainGold)
                    {
                        hienThiDoKH = false;
                        petw = false;
                        MainMod.GoldCurrent = Char.myCharz().xu;
                        charw = false;
                        MainMod.GoldUpdate = Char.myCharz().xu;
                    }
                    else
                    {
                        MainMod.GoldUpdateRealTime = 0L;
                    }
                    break;
                case 32:
                    AutoboMong.ShowMenu();
                    break;
                case 11:
                    findBossMod = !findBossMod;
                    if (findBossMod)
                    {
                        GameScr.info1.addInfo("|0|Auto tìm boss Hirde " + (findBossMod ? "Bật" : "Tắt"), 0);
                        new Thread(findBossHi).Start();
                    }

                    break;

                case 13:
                    DoSatBossNapa = !DoSatBossNapa;
                    GameScr.info1.addInfo("|0|Auto đánh Boss Napa " + (DoSatBossNapa ? "Bật" : "Tắt"), 0);
                    if (DoSatBossNapa)
                    {
                        new Thread(AutoFarmBossNapa).Start();
                    }
                    break;
                case 14:
                case 15:
                case 31:
                    break;
            }
        }

        public static void SelectMe(Char player)
        {
            try
            {
                MyVector myVector = new MyVector();
                myVector.addElement(player);
                Service.gI().sendPlayerAttack(new MyVector(), myVector, -1);
            }
            catch (Exception)
            {
            }
        }

        public static void SkillHealing()
        {
            if (!Char.myCharz().meDead)
            {
                Service.gI().selectSkill(7);
                SelectMe(Char.myCharz());
                Service.gI().selectSkill(Char.myCharz().myskill.template.id);
            }
        }

        public static void AutoHealingPower()
        {
            if (!Char.myCharz().getGender().Equals("NM"))
            {
                HealingPower = false;
                GameScr.info1.addInfo("Chỉ namek mới có thể dùng !!", 0);
                return;
            }
            if (HealingPower)
            {
                GameScr.info1.addInfo("Auto trị thương bản thân: Bật", 0);
            }
            while (HealingPower)
            {
                SkillHealing();
                Thread.Sleep(((Skill)Char.myCharz().vSkillFight.elementAt(2)).coolDown);
            }
            HealingPower = false;
            GameScr.info1.addInfo("Auto trị thương bản thân:Tắt", 0);
        }
        public static void AutoHealingPower2()
        {
            while (HealingPower)
            {
                SkillHealing();
                int cooldown = ((Skill)Char.myCharz().vSkillFight.elementAt(2)).coolDown;

                for (int i = 0; i < cooldown / 1000; i++)
                {
                    if (!HealingPower) break;
                    Thread.Sleep(1000);
                }

            }
        }

        public static void autoCoDen()
        {
            while (autoFlag)
            {
                if (!checkCoDen() && Char.myCharz().cFlag == 0)
                {
                    Service.gI().getFlag(1, 8);
                }
                if (checkCoDen() && Char.myCharz().cFlag == 8)
                {
                    Service.gI().getFlag(1, 0);
                }
                Thread.Sleep(1000);
            }
            while (Char.myCharz().cFlag != 0)
            {
                Service.gI().getFlag(1, 0);
                Thread.Sleep(5000);
            }
        }

        public static bool checkCoDen()
        {
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char obj = (Char)GameScr.vCharInMap.elementAt(i);
                if (obj.cFlag != 0 && obj.charID > 0)
                {
                    return true;
                }
            }
            return false;
        }


        public static bool checkBoss()
        {
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char obj = (Char)GameScr.vCharInMap.elementAt(i);
                if (obj.cName != null && obj.cName != "" && !obj.isPet && !obj.isMiniPet && char.IsUpper(char.Parse(obj.cName.Substring(0, 1))) && obj.cName != "Trọng tài" && !obj.cName.StartsWith("Broly") && !obj.cName.StartsWith("#") && !obj.cName.StartsWith("$") && obj.cHP > 0
                    && obj.cx < TileMap.GetMapEndX())
                {
                    return true;
                }
            }
            return false;
        }


        public static void AutoFarmBossNapa()
        {
            int startMap = UnityEngine.Random.Range(68, 73);
            AutoTrain.TuMoTDLT();
            aGimBoss = AutoteleBoss = tanCongBoss = true;
            tieuDietNguoiBatCo = false;

            new Thread(teleBoss).Start();
            new Thread(AutoFocusBoss).Start();

            while (DoSatBossNapa)
            {
                if (TileMap.mapID != startMap)
                    AutoMap.StartRunToMapId(startMap);

                while (AutoMap.isXmaping)
                    Thread.Sleep(1000);

                int num = Math.max(TileMap.zoneID + 1, 2); // Bỏ khu 0,1

                while (num <= 14 && DoSatBossNapa && !Char.myCharz().meDead)
                {
                    if (TileMap.mapID != startMap)
                        AutoMap.StartRunToMapId(startMap);

                    while (AutoMap.isXmaping)
                        Thread.Sleep(1000);

                    Thread.Sleep(50);
                    Service.gI().requestChangeZone(num, -1);

                    while (TileMap.zoneID != num && DoSatBossNapa)
                    {
                        Thread.Sleep(1200);
                        Service.gI().requestChangeZone(num, -1);
                    }

                    if (checkBoss() && DoSatBossNapa)
                    {
                        Thread.Sleep(1000);
                        Char.myCharz().mobFocus = null;
                        Char.myCharz().itemFocus = null;
                        Char.myCharz().npcFocus = null;

                        while (checkBoss() && DoSatBossNapa)
                            Thread.Sleep(2000);

                        listBossTrongKhu.Clear();

                        if (TileMap.zoneID < 14 && DoSatBossNapa)
                        {
                            num = Math.max(TileMap.zoneID + 1, 2);
                            Thread.Sleep(2000);
                            continue;
                        }
                        else break;
                    }
                    if (DoSatBossNapa)
                    {
                        int i = 0;
                        while(i < 5 & DoSatBossNapa)
                        {
                            Thread.Sleep(1000);
                            i++;
                        }
                    }
                
                    num++;
                }

                startMap = (startMap >= 72) ? 68 : startMap + 1;
            }

            tanCongBoss = aGimBoss = AutoteleBoss = false;
            listBossTrongKhu.Clear();
        }

        public static void autoDoBoss()
        {
            try
            {
                if (TileMap.mapID == 23 || TileMap.mapID == 21 || TileMap.mapID == 22 || TileMap.mapID == 47 || TileMap.mapID == 48 || TileMap.mapID == 50 || TileMap.mapID == 116)
                {
                    DoBoss = false;
                    GameScr.info1.addInfo("Boss đâu ra trong map này ông thần ??", 0);
                    return;
                }
                if (checkBoss())
                {
                    GameScr.info1.addInfo("Đã tìm thấy boss", 0);
                    DoBoss = false;
                    return;
                }
                AutoTrain.TuMoTDLT();
                int num = TileMap.zoneID + 1;
                while (num <= 14 && DoBoss && !Char.myCharz().meDead)
                {
                    Thread.Sleep(50);
                    Service.gI().requestChangeZone(num, -1);
                    while (TileMap.zoneID != num && DoBoss)
                    {
                        Thread.Sleep(1500);
                        Service.gI().requestChangeZone(num, -1);
                    }
                    if (checkBoss() && DoBoss)
                    {
                        GameScr.info1.addInfo("Đã tìm thấy boss", 0);
                        break;
                    }
                    if (!checkItemTime(4380))
                    {
                        Thread.Sleep(5200);
                    }
                    Thread.Sleep(5200);
                    num++;

                }
                DoBoss = false;
            }
            catch
            {
                GameScr.info1.addInfo("Lỗi r", 0);
                DoBoss = false;
            }
        }


        public static Item FindItemBag(int id)
        {
            for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
            {
                if (Char.myCharz().arrItemBag[i] != null && Char.myCharz().arrItemBag[i].template.id == id)
                {
                    return Char.myCharz().arrItemBag[i];
                }
            }
            return null;
        }

        public static void AutoFocusBoss()
        {
            while (aGimBoss)
            {
                Char bossThapNhat = null;
                long hpMin = long.MaxValue;

                for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                {
                    Char obj = (Char)GameScr.vCharInMap.elementAt(i);
                    if (obj.cName != null && obj.cName != "" && !obj.isPet && !obj.isMiniPet && char.IsUpper(char.Parse(obj.cName.Substring(0, 1))) && obj.cName != "Trọng tài" && !obj.cName.StartsWith("Broly") && !obj.cName.StartsWith("#") && !obj.cName.StartsWith("$")
                        && obj.cHP > 0)
                    {
                        if (obj.cHP < hpMin)
                        {
                            hpMin = obj.cHP;
                            bossThapNhat = obj;
                        }
                    }
                }

                if (bossThapNhat != null)
                {
                    Char.myCharz().npcFocus = null;
                    Char.myCharz().charFocus = null;
                    Char.myCharz().mobFocus = null;
                    Char.myCharz().charFocus = bossThapNhat;
                }

                Thread.Sleep(500);
            }
        }


        public static void AutoWhis()
        {
            bool flag = false;
            if (TileMap.mapID != 154)
            {
                AutoMap.StartRunToMapId(154);
                while (AutoMap.isXmaping)
                {
                    Thread.Sleep(1000);
                }
            }
            tanCongBoss = true;
            tieuDietNguoiBatCo = false;
            while (aWhis && !Char.myCharz().meDead)
            {
                Service.gI().openMenu(56);
                Service.gI().confirmMenu(56, 3);
                Thread.Sleep(500);
                // GameCanvas.gI().keyPressedz(-7);
                GameCanvas.menu.performSelect();
                GameCanvas.menu.doCloseMenu();

                //ChatPopup.serverChatPopUp = null;
                while (checkBossWhis())
                {
                    AutoSkill.isAutoSendAttack = true;
                    Thread.Sleep(2000);
                    flag = true;
                }
                Thread.Sleep(3500);
                if (!flag)
                {
                    break;
                }
                flag = false;
            }
            tanCongBoss = false;
            GameScr.info1.addInfo("Auto Whis đã được ngắt", 0);
            AutoSkill.isAutoSendAttack = false;
            aWhis = false;
        }

        public static void useItem(int x)
        {
            for (sbyte b = 0; b < Char.myCharz().arrItemBag.Length; b++)
            {
                if (Char.myCharz().arrItemBag[b] != null && Char.myCharz().arrItemBag[b].template.id == x)
                {
                    Service.gI().useItem(0, 1, b, -1);
                    break;
                }
            }
        }


        public static void onChatFromMe(string text, string to)
        {
            if (text == "bpao")
            {
                if (GameScr.isAnalog == 1)
                {
                    GameScr.isAnalog = 0;
                }
                else
                {
                    GameScr.isAnalog = 1;
                }
                GameScr.info1.addInfo("Lệnh Admin : Mở bàn phím ảo: " + ((GameScr.isAnalog == 1) ? "ON" : "OFF"), 0);
                text = string.Empty;
            }
            if (text.StartsWith("k_"))
            {
                try
                {
                    int zoneId = int.Parse(text.Split('_')[1]);
                    Service.gI().requestChangeZone(zoneId, -1);
                }
                catch
                {
                }
            }
            if (text.StartsWith("s_"))
            {
                try
                {
                    int num = (MainMod.runSpeed = int.Parse(text.Split('_')[1]));
                    GameScr.info1.addInfo("Tốc Độ Di Chuyển: " + num, 0);
                }
                catch
                {
                }
            }
            if (text.StartsWith("cheat_"))
            {
                try
                {
                    float num = (Time.timeScale = float.Parse(text.Split('_')[1]));
                    GameScr.info1.addInfo("Cheat: " + num, 0);
                    text = string.Empty;
                }
                catch
                {
                }
            }
            if (text.Equals("ts"))
            {
                try
                {
                    AutoTrain.trainCuongLe = !AutoTrain.trainCuongLe;
                    GameScr.info1.addInfo("Auto Train Cường Lê: " + (AutoTrain.trainCuongLe ? "Bật" : "Tắt"), 0);
                    text = string.Empty;
                }
                catch
                {
                }
            }
        }

        static ModProCuongLe()
        {
            timeScale = 2.4f;
            listNguoiCoDen = new List<Char>();
            listBossTrongKhu = new List<Char>();
            songlamccginua = 500;
        }

        public static bool ExistItemBag(int x)
        {
            for (sbyte b = 0; b < Char.myCharz().arrItemBag.Length; b++)
            {
                if (Char.myCharz().arrItemBag[b] != null && Char.myCharz().arrItemBag[b].template.id == x)
                {
                    return true;
                }
            }
            return false;
        }

        public static void isMeCanAttack()
        {
            if (!AutoSkill.isAutoSendAttack)
            {
                AutoSkill.isAutoSendAttack = true;
            }
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char obj = (Char)GameScr.vCharInMap.elementAt(i);
                if (MainMod.isBoss(obj))
                {
                    Char.myCharz().charFocus = obj;
                    if ((Math.abs(Char.myCharz().cx - obj.cx) > 40 || Math.abs(Char.myCharz().cy - obj.cy) > 40) && mSystem.currentTimeMillis() - timeCanAttack >= 500)
                    {
                        timeCanAttack = mSystem.currentTimeMillis();
                        MainMod.TeleportTo(obj.cx, MainMod.GetYGround(obj.cx));
                        Service.gI().charMove();
                        Char.myCharz().cy = obj.cy;
                        Service.gI().charMove();
                    }
                    break;
                }
            }
        }

        public static void Update()
        {
            if (tanCongBoss && !Char.myCharz().meDead && GameCanvas.gameTick % 30 == 0)
            {
                updateListBoss();
                if (listBossTrongKhu.Contains(Char.myCharz().charFocus))
                {
                    ChienDau();
                }
            }
            if (tieuDietNguoiBatCo && !Char.myCharz().meDead && GameCanvas.gameTick % 30 == 0)
            {
                updateCoDen();
                for (int i = 0; i < listNguoiCoDen.Count; i++)
                {
                    Char obj = listNguoiCoDen[i];
                    if (GameScr.vCharInMap.contains(obj) && obj.cHP > 0)
                    {
                        Char.myCharz().mobFocus = null;
                        Char.myCharz().npcFocus = null;
                        Char.myCharz().itemFocus = null;
                        Char.myCharz().charFocus = obj;
                        while (!obj.meDead && obj.cHP > 0 && obj.cFlag != 0 && obj.charID > 0 && tieuDietNguoiBatCo && Char.myCharz().charFocus != obj)
                        {
                            AutoMap.TeleportTo(obj.cx, GetClosestGroundY(obj.cx, obj.cy));
                        }
                    }
                }
            }
            if (tieuDietNguoiBatCo && listNguoiCoDen.Contains(Char.myCharz().charFocus) && Char.myCharz().charFocus.cFlag != 0 && Char.myCharz().npcFocus == null && Char.myCharz().itemFocus == null && Char.myCharz().mobFocus == null)
            {
                ChienDau();
            }
            if (isAttackBoss)
            {
                isMeCanAttack();
            }
        }

        public static int checkTimeTDLT()
        {
            int result = 0;
            for (sbyte b = 0; b < Char.myCharz().arrItemBag.Length; b++)
            {
                if (Char.myCharz().arrItemBag[b].template.id == 521)
                {
                    result = Char.myCharz().arrItemBag[b].itemOption[0].param;
                    break;
                }
            }
            return result;
        }

        public static int areaWithFewPeople()
        {
            int result = TileMap.zoneID;
            int num = GameScr.gI().numPlayer[TileMap.zoneID];
            for (int i = 0; i < GameScr.gI().zones.Length - 1; i++)
            {
                if (num <= 1)
                {
                    break;
                }
                if (GameScr.gI().numPlayer[i] == 0)
                {
                    return i;
                }
                if (GameScr.gI().numPlayer[i] < num)
                {
                    result = i;
                }
            }
            return result;
        }

        public static void teleNPC(int idNpc)
        {
            for (int i = 0; i < GameScr.vNpc.size(); i++)
            {
                Npc npc = (Npc)GameScr.vNpc.elementAt(i);
                if (npc.template.npcTemplateId == idNpc)
                {
                    Char.myCharz().npcFocus = npc;
                    AutoMap.TeleportTo(npc.cx, npc.cy - 3);
                    break;
                }
            }
        }

        public static void muaTDLT()
        {
            _ = TileMap.mapID;
            if (TileMap.mapID != 5)
            {
                AutoMap.StartRunToMapId(5);
            }
            while (AutoMap.isXmaping)
            {
                Thread.Sleep(1000);
            }
            teleNPC(39);
            Service.gI().openMenu(39);
            Service.gI().confirmMenu(39, 0);
            Thread.Sleep(1000);
            Service.gI().buyItem(0, 521, 1);
        }

        public static int MyHPPercent()
        {
            return (int)(Char.myCharz().cHP * 100 / Char.myCharz().cHPFull);
        }

        public static bool XoaTauBay(object obj)
        {
            Teleport teleport = (Teleport)obj;

            // Chỉ xử lý nếu là tàu của mình
            if (teleport.isMe)
            {
                // Dừng hiệu ứng bay lên, bay xuống nếu có
                SoundMn.gI().pauseAirShip(); // <-- dòng này thêm

                Char.myCharz().isTeleport = false;

                if (teleport.type == 0)
                {
                    Controller.isStopReadMessage = false;
                    Char.ischangingMap = true;
                }

                // Xóa teleport khỏi danh sách
                Teleport.vTeleport.removeElement(teleport);
                return true;
            }

            return false;
        }



        public static void autoDeKOK()
        {
            int cx = Char.myCharz().cx;
            int cy = Char.myCharz().cy;
            while (isKOK)
            {
                if (Char.myCharz().meDead || AutoTrain.isAutoTrain)
                {
                    isKOK = false;
                    break;
                }
                while (AutoMap.isXmaping)
                {
                    Thread.Sleep(1000);
                }
                Char.myCharz().currentMovePoint = new MovePoint(cx + 10, cy);
                Thread.Sleep(500);
                Char.myCharz().currentMovePoint = new MovePoint(cx - 10, cy);
                Thread.Sleep(500);
            }
        }

        public static void teleBoss()
        {
            while (AutoteleBoss)
            {
                Char bossThapNhat = null;
                long hpMin = long.MaxValue;

                for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                {
                    Char obj = (Char)GameScr.vCharInMap.elementAt(i);
                    if (MainMod.isBoss(obj) && !Char.myCharz().meDead && obj.cHP > 0
                        && obj.cx > 10 && obj.cy < TileMap.GetMapEndY() && obj.cx < TileMap.GetMapEndX())
                    {
                        if (obj.cHP < hpMin)
                        {
                            hpMin = obj.cHP;
                            bossThapNhat = obj;
                        }
                    }
                }

                if (bossThapNhat != null)
                {
                    Char.myCharz().charFocus = bossThapNhat;
                    AutoMap.TeleportTo(bossThapNhat.cx, GetClosestGroundY(bossThapNhat.cx, bossThapNhat.cy));
                }

                Thread.Sleep(2000);
            }
        }


        public static void ChienDau()
        {
            Skill skill = null;
            for (int i = 0; i < GameScr.keySkill.Length; i++)
            {
                if (GameScr.keySkill[i] == null || GameScr.keySkill[i].paintCanNotUseSkill || GameScr.keySkill[i].template.id == 10 || GameScr.keySkill[i].template.id == 11 || GameScr.keySkill[i].template.id == 14 || GameScr.keySkill[i].template.id == 23 || GameScr.keySkill[i].template.id == 7 || GameScr.keySkill[i].template.id == 3 || GameScr.keySkill[i].template.id == 1 || GameScr.keySkill[i].template.id == 5 || GameScr.keySkill[i].template.id == 8 || GameScr.keySkill[i].template.id == 20 || GameScr.keySkill[i].template.id == 24 || GameScr.keySkill[i].template.id == 25 || GameScr.keySkill[i].template.id == 26 || GameScr.keySkill[i].template.id == 9 || GameScr.keySkill[i].template.id == 22 || GameScr.keySkill[i].template.id == 18 || (Char.myCharz().cgender == 1 && (Char.myCharz().cgender != 1 || (Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[5]) != null && (Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[5]) == null || GameScr.keySkill[i].template.id == 2)))) || Char.myCharz().skillInfoPaint() != null || (Char.myCharz().isMonkey == 1 && GameScr.keySkill[i].template.id == 13))
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
                //GameScr.gI().doDoubleClickToObj(Char.myCharz().charFocus);
                //AutoSkill.AutoSendAttack();
            }
        }

        public static void TeleNguoiCoDen()
        {
            AutoTrain.TuMoTDLT();
            if (Char.myCharz().cFlag == 0)
            {
                GameScr.info1.addInfo("Bật cờ lên (phím Z) ????", 0);
                tieuDietNguoiBatCo = false;
                return;
            }
            for (int i = 0; i < listNguoiCoDen.Count; i++)
            {
                Char obj2 = listNguoiCoDen[i];
                if (GameScr.vCharInMap.contains(obj2))
                {
                    Char.myCharz().mobFocus = null;
                    Char.myCharz().npcFocus = null;
                    Char.myCharz().itemFocus = null;
                    Char.myCharz().charFocus = obj2;
                    while (!obj2.meDead && obj2.cHP > 0 && obj2.cFlag != 0 && obj2.charID > 0 && tieuDietNguoiBatCo && Char.myCharz().charFocus != obj2)
                    {
                        AutoMap.TeleportTo(obj2.cx, MainMod.GetYGround(obj2.cx));
                    }
                }
            }
        }

        public static void updateCoDen()
        {
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char obj = (Char)GameScr.vCharInMap.elementAt(i);
                if (obj.cName != null && obj.cName != "" && !obj.isPet && !obj.isMiniPet && !obj.cName.StartsWith("#") && !obj.cName.StartsWith("$") && obj.cName != "Trọng tài" && obj.cFlag != 0 && !char.IsUpper(char.Parse(obj.cName.Substring(0, 1)))

                    && obj.cHP > 0)
                {
                    listNguoiCoDen.Add(obj);
                }
            }
        }

        public static void updateListBoss()
        {
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char obj = (Char)GameScr.vCharInMap.elementAt(i);
                if (obj.cName != null && obj.cName != "" && obj.cName != "Trọng tài" && obj.cName != "Broly" && MainMod.isBoss(obj)
                    && obj.cx > 10 && obj.cHP > 0 && obj.cy < TileMap.GetMapEndY() && obj.cx < TileMap.GetMapEndX())
                {
                    listBossTrongKhu.Add(obj);
                }
            }
        }

        public static bool ExistItemBox(int id)
        {
            for (int i = 0; i < Char.myCharz().arrItemBox.Length; i++)
            {
                if (Char.myCharz().arrItemBox[i] != null && Char.myCharz().arrItemBox[i].template.id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ExistItemBody(int id)
        {
            for (int i = 0; i < Char.myCharz().arrItemBody.Length; i++)
            {
                if (Char.myCharz().arrItemBody[i] != null && Char.myCharz().arrItemBody[i].template.id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public static void findBossHi()
        {
            AutoTrain.TuMoTDLT();
            if (TileMap.mapID != 19)
            {
                AutoMap.StartRunToMapId(19);
                while (AutoMap.isXmaping)
                {
                    Thread.Sleep(1000);
                }
            }
            while (findBossMod)
            {
                teleNPC(53);
                Service.gI().openMenu(53);
                Service.gI().confirmMenu(53, 0);
                Thread.Sleep(900);
                if (TileMap.mapID == 126)
                {
                    Thread.Sleep(1200);
                    if (CheckBossMob(70))
                    {
                        GameScr.info1.addInfo("Đã tìm thấy boss", 0);
                        findBossMod = false;
                        return;
                    }
                    //AutoTrain.getInstance().perform(1, 70);
                    //while (CheckBossMob(70))
                    //{
                    //    Thread.Sleep(3500);
                    //}
                    //AutoTrain.getInstance().perform(8, null);
                }

            }
        }
        public static bool checkItemTime(int idIcon)
        {
            for (int j = 0; j < Char.vItemTime.size(); j++)
            {
                if (((ItemTime)Char.vItemTime.elementAt(j)).idIcon == idIcon)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckBossMob(int idBossMod)
        {
            for (int j = 0; j < GameScr.vMob.size(); j++)
            {
                Mob mob2 = (Mob)GameScr.vMob.elementAt(j);
                if (!mob2.isMobMe && mob2.templateId == idBossMod && mob2.status != 0 && mob2.status != 1 && mob2.hp > 0
                    && !mob2.isDie && mob2.x > 0 && mob2.xFirst > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public static void toimuonchet()
        {
            suicide = true;
            while (Char.myCharz().cFlag == 0)
            {
                Service.gI().getFlag(1, 8);
                Thread.Sleep(1100);
            }
            while (Char.myCharz().cHP > 0 && !Char.myCharz().meDead)
            {
                MyVector myVector = new MyVector();
                myVector.addElement(Char.myCharz());
                Service.gI().sendPlayerAttack(new MyVector(), myVector, -1);
                Thread.Sleep(songlamccginua);
            }
            suicide = false;
        }

        public static bool isFULLBag()
        {
            for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
            {
                if (Char.myCharz().arrItemBag[i] == null)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool isFULLBox()
        {
            for (int i = 0; i < Char.myCharz().arrItemBox.Length; i++)
            {
                if (Char.myCharz().arrItemBox[i] == null)
                {
                    return false;
                }
            }
            return true;
        }

        public static int countEmptyBox()
        {
            int num = 0;
            for (int num2 = Char.myCharz().arrItemBox.Length - 1; num2 >= 0; num2--)
            {
                if (Char.myCharz().arrItemBox[num2] == null)
                {
                    num++;
                }
            }
            return num;
        }

        public static int countEmptyBag()
        {
            int num = 0;
            for (int num2 = Char.myCharz().arrItemBag.Length - 1; num2 >= 0; num2--)
            {
                if (Char.myCharz().arrItemBag[num2] == null)
                {
                    num++;
                }
            }
            return num;
        }

        public static void ShowMenuBoss()
        {
            MyVector myVector = new MyVector();
            myVector.addElement(new Command(DoBoss ? "Dò boss: ON" : "Dò boss: OFF", getInstance(), 3, null));
            myVector.addElement(new Command(aGimBoss ? "Auto gim boss : ON" : "Auto gim boss: OFF", getInstance(), 9, null));
            myVector.addElement(new Command(DoSatBossNapa ? "Farm boss napa : ON" : "Farm boss napa: OFF", getInstance(), 13, null));
            myVector.addElement(new Command(aWhis ? "Auto Leo top whis: ON" : "Auto Leo top whis: OFF", getInstance(), 10, null));
            myVector.addElement(new Command(findBossMod ? "Auto map Boss trứng mabu : ON" : "Auto map Boss trứng mabu: OFF", getInstance(), 11, null));
            myVector.addElement(new Command("Auto dịch theo Boss: " + (AutoteleBoss ? "ON" : "OFF"), getInstance(), 17, null));
            myVector.addElement(new Command("Auto dịch + Tấn công Boss " + (tanCongBoss ? "ON" : "OFF"), getInstance(), 23, null));
            GameCanvas.menu.startAt(myVector, 4);
        }

        public static bool checkBossWhis()
        {
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char obj = (Char)GameScr.vCharInMap.elementAt(i);
                if (obj.cName != null && obj.cName.StartsWith("Whis") && !obj.isPet && !obj.isMiniPet && char.IsUpper(char.Parse(obj.cName.Substring(0, 1))) && obj.cName != "Trọng tài" && !obj.cName.StartsWith("Broly") && !obj.cName.StartsWith("#") && !obj.cName.StartsWith("$"))
                {
                    return true;
                }
            }
            return false;
        }
        public static int GetClosestGroundY(int x, int targetY)
        {
            List<int> groundYs = new List<int>();
            int stepSize = 24; // Kích thước ô, đồng bộ với GetYGround
            int y = 50; // Bắt đầu từ Y=50, tương tự GetYGround
            int maxY = TileMap.GetMapEndY(); // Giới hạn chiều cao bản đồ

            // Tìm tất cả các tọa độ Y là mặt đất tại X
            while (y <= maxY)
            {
                if (TileMap.tileTypeAt(x, y, 2)) // Kiểm tra ô là mặt đất (type 2)
                {
                    if (y % 24 != 0) // Căn chỉnh với lưới ô
                    {
                        y -= y % 24;
                    }
                    groundYs.Add(y);
                }
                y += stepSize;
            }

            // Nếu không tìm thấy mặt đất, trả về Y của Boss
            if (groundYs.Count == 0)
            {
                GameScr.info1.addInfo($"Không tìm thấy mặt đất tại x={x}, sử dụng y={AutoMap.GetYGround(x)}", 0);
                return AutoMap.GetYGround(x); // tọa độ mặt đất nhưng ko đảm bảo là tọa độ mặt đất gần với boss nhất,
                                              // vì 1 map có thể có nhiều tầng mặt đất
            }

            // Tìm Y mặt đất gần nhất với targetY
            int closestY = groundYs[0];
            int minDistance = Math.abs(groundYs[0] - targetY);
            foreach (int groundY in groundYs)
            {
                int distance = Math.abs(groundY - targetY);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestY = groundY;
                }
            }

            return closestY;
        }
    }
}
