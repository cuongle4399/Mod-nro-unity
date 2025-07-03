using Assets.src.e;
using System.Collections.Generic;
using System.Threading;

namespace Mod.CuongLe
{
    public class AutoTrain : IActionListener, IChatable
    {
        private static AutoTrain _Instance;

        private static bool isAvoidSuperMob;

        public static bool isGoBack;

        public static bool isGobackCoordinate;

        public static int gobackX;

        public static int gobackY;

        public static int gobackMapID;

        public static int gobackZoneID;

        public static bool isAutoTrain;

        private static int minimumMPGoHome;

        private static string[] inputMPPercentGoHome;

        public static List<int> listMobIds;

        public static long lastTimeAddNewMob;

        private static long lastTimeTeleportToMob;

        private static bool AutoChangeClothes;

        private static bool NextDiaHinh;

        private static int typeMobChange = -1;

        public static AutoTrain getInstance()
        {
            if (_Instance == null)
            {
                _Instance = new AutoTrain();
            }
            return _Instance;
        }

        public static void Update()
        {
            if (Char.myCharz().mobFocus != null && (Char.myCharz().mobFocus == null || !Char.myCharz().mobFocus.isMobMe) && (Char.myCharz().mobFocus.hp <= 0 || Char.myCharz().mobFocus.status == 1 || Char.myCharz().mobFocus.status == 0 || !isMeCanAttack(Char.myCharz().mobFocus)))
            {
                Char.myCharz().mobFocus = null;
            }
            if (GameScr.isAutoPlay && (GameScr.canAutoPlay || isAutoTrain) && GameCanvas.gameTick % 20 == 0)
            {
                DoIt();
            }
            if (Char.myCharz().cStamina <= 5 && GameCanvas.gameTick % 100 == 0)
            {
                UseGrape();
            }
            if (!isGoBack)
            {
                return;
            }
            if (Char.myCharz().meDead && GameCanvas.gameTick % 100 == 0)
            {
                Service.gI().returnTownFromDead();
            }
            if (isMeOutOfMP())
            {
                int num = 21 + Char.myCharz().cgender;
                if (TileMap.mapID != num)
                {
                    GameScr.isAutoPlay = false;
                    Char.myCharz().mobFocus = null;
                    if (GameCanvas.gameTick % 50 == 0)
                    {
                        AutoMap.StartRunToMapId(num);
                    }
                }
            }
            else
            {
                if (isMeOutOfMP())
                {
                    return;
                }
                if (TileMap.mapID != gobackMapID)
                {
                    GameScr.isAutoPlay = false;
                    AutoMap.StartRunToMapId(gobackMapID);
                }
                if (TileMap.mapID == gobackMapID)
                {
                    if (!isGobackCoordinate && GameCanvas.gameTick % 100 == 0)
                    {
                        GameScr.isAutoPlay = true;
                    }
                    if (TileMap.zoneID != gobackZoneID && !Char.ischangingMap && !Controller.isStopReadMessage && GameCanvas.gameTick % 80 == 0)
                    {
                        Service.gI().requestChangeZone(gobackZoneID, -1);
                    }
                    if (isGobackCoordinate && (Char.myCharz().cx != gobackX || Char.myCharz().cy != gobackY) && GameCanvas.gameTick % 100 == 0)
                    {
                        TeleportTo(gobackX, gobackY);
                    }
                }
            }
        }

        public void onChatFromMe(string text, string to)
        {
            if (ChatTextField.gI().tfChat.getText() != null && !ChatTextField.gI().tfChat.getText().Equals(string.Empty) && !text.Equals(string.Empty) && text != null)
            {
                if (ChatTextField.gI().strChat.Equals(inputMPPercentGoHome[0]))
                {
                    try
                    {
                        int num = (minimumMPGoHome = int.Parse(ChatTextField.gI().tfChat.getText()));
                        GameScr.info1.addInfo("Về Nhà Khi MP Dưới\n[" + num + "%]", 0);
                    }
                    catch
                    {
                        GameScr.info1.addInfo("%MP Không Hợp Lệ, Vui Lòng Nhập Lại", 0);
                    }
                    ResetChatTextField();
                }
            }
            else
            {
                ChatTextField.gI().isShow = false;
            }
        }

        public void onCancelChat()
        {
        }

        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    {
                        int num = (int)p;
                        listMobIds.Clear();
                        for (int j = 0; j < GameScr.vMob.size(); j++)
                        {
                            Mob mob2 = (Mob)GameScr.vMob.elementAt(j);
                            if (!mob2.isMobMe && mob2.templateId == num)
                            {
                                listMobIds.Add(mob2.mobId);
                            }
                        }
                        TurnOnAutoTrain();
                        break;
                    }
                case 2:
                    {
                        listMobIds.Clear();
                        for (int i = 0; i < GameScr.vMob.size(); i++)
                        {
                            Mob mob = (Mob)GameScr.vMob.elementAt(i);
                            if (!mob.isMobMe)
                            {
                                listMobIds.Add(mob.mobId);
                            }
                        }
                        TurnOnAutoTrain();
                        break;
                    }
                case 3:
                    TurnOnAutoTrain();
                    break;
                case 4:
                    isAvoidSuperMob = !isAvoidSuperMob;
                    GameScr.info1.addInfo("Né Siêu Quái\n" + (isAvoidSuperMob ? "[STATUS: OFF]" : "[STATUS: ON]"), 0);
                    break;
                case 5:
                    ShowMenuGoback();
                    break;
                case 6:
                    listMobIds.Clear();
                    isAutoTrain = false;
                    GameScr.info1.addInfo("Đã Clear Danh Sách Train!", 0);
                    break;
                case 7:
                    if (Char.myCharz().mobFocus == null)
                    {
                        GameScr.info1.addInfo("Vui Lòng Chọn Quái!", 0);
                    }
                    if (Char.myCharz().mobFocus != null)
                    {
                        listMobIds.Add(Char.myCharz().mobFocus.mobId);
                        GameScr.info1.addInfo("Đã Thêm Quái: " + Char.myCharz().mobFocus.mobId, 0);
                    }
                    break;
                case 8:
                    isAutoTrain = false;
                    Char.myCharz().mobFocus = null;
                    GameScr.info1.addInfo("Đã Tắt Auto Train!", 0);
                    break;
                case 9:
                    if (isGoBack)
                    {
                        isGoBack = false;
                        GameScr.info1.addInfo("Goback\n[STATUS: OFF]", 0);
                    }
                    else if (!isGoBack)
                    {
                        isGobackCoordinate = false;
                        isGoBack = true;
                        gobackMapID = TileMap.mapID;
                        gobackZoneID = TileMap.zoneID;
                        GameScr.info1.addInfo("Goback\n[" + TileMap.mapNames[gobackMapID] + "]\n[" + gobackZoneID + "]", 0);
                    }
                    break;
                case 10:
                    if (isGoBack)
                    {
                        isGoBack = false;
                        GameScr.info1.addInfo("Goback\n[STATUS: OFF]", 0);
                    }
                    else if (!isGoBack)
                    {
                        isGobackCoordinate = true;
                        isGoBack = true;
                        gobackMapID = TileMap.mapID;
                        gobackZoneID = TileMap.zoneID;
                        gobackX = Char.myCharz().cx;
                        gobackY = Char.myCharz().cy;
                        GameScr.info1.addInfo("Goback Tọa Độ\n[" + gobackX + "-" + gobackY + "]", 0);
                    }
                    break;
                case 11:
                    ChatTextField.gI().strChat = inputMPPercentGoHome[0];
                    ChatTextField.gI().tfChat.name = inputMPPercentGoHome[1];
                    ChatTextField.gI().startChat2(getInstance(), string.Empty);
                    break;
                case 12:
                    if (Char.myCharz().getGender() == "TĐ" || Char.myCharz().getGender() == "NM")
                    {
                        GameScr.info1.addInfo("Chỉ dành cho xd", 0);
                        break;
                    }
                    if (AutoItem.set1.Count == 0 || AutoItem.set2.Count == 0)
                    {
                        GameScr.info1.addInfo("Vui lòng thêm đồ cho set 1 và sét 2", 0);
                        break;
                    }
                    AutoChangeClothes = !AutoChangeClothes;
                    GameScr.info1.addInfo("|0| Auto mặc sét 1 khi khỉ, khỉ sịt mặc sét 2: " + (AutoChangeClothes ? "Bật" : "Tắt"), 0);
                    if (AutoChangeClothes)
                    {
                        new Thread(changeclothes).Start();
                    }
                    break;
                case 13:
                    NextDiaHinh = !NextDiaHinh;
                    GameScr.info1.addInfo("Train Fake tdlt vượt địa hình: " + (NextDiaHinh ? "Bật" : "Tắt"), 0);
                    break;
            }
        }

        public static void ShowMenu()
        {
            MyVector myVector = new MyVector();
            List<Mob> list = new List<Mob>();
            if (isAutoTrain && !GameScr.canAutoPlay)
            {
                myVector.addElement(new Command("Tắt Auto Train", getInstance(), 8, null));
            }
            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob mob = (Mob)GameScr.vMob.elementAt(i);
                if (mob.isMobMe)
                {
                    continue;
                }
                bool flag = false;
                for (int j = 0; j < list.Count; j++)
                {
                    if (mob.templateId == list[j].templateId)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    list.Add(mob);
                    myVector.addElement(new Command("Tàn Sát\n" + mob.getTemplate().name + "\n[" + NinjaUtil.getMoneys(mob.maxHp) + "HP]", getInstance(), 1, mob.templateId));
                }
            }
            myVector.addElement(new Command("Tàn Sát Tất Cả", getInstance(), 2, null));
            myVector.addElement(new Command("Tàn Sát Theo Vị Trí", getInstance(), 3, null));
            myVector.addElement(new Command("Né Siêu Quái\n" + (isAvoidSuperMob ? "[STATUS: OFF]" : "[STATUS: ON]"), getInstance(), 4, null));
            myVector.addElement(new Command("Train Fake TDLT\n" + (NextDiaHinh ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 13, null));
            myVector.addElement(new Command("Đang khỉ set 1, ngược set 2: " + (AutoChangeClothes ? "ON" : "OFF"), getInstance(), 12, null));
            myVector.addElement(new Command("Goback", getInstance(), 5, null));
            myVector.addElement(new Command("Clear Danh Sách Train", getInstance(), 6, null));
            if (Char.myCharz().mobFocus != null)
            {
                myVector.addElement(new Command("Thêm\n[" + Char.myCharz().mobFocus.getTemplate().name + "]\n[" + Char.myCharz().mobFocus.mobId + "]", getInstance(), 7, null));
            }
            GameCanvas.menu.startAt(myVector, 3);
        }

        private static void ShowMenuGoback()
        {
            MyVector myVector = new MyVector();
            myVector.addElement(new Command("Goback\n" + (isGoBack ? ("[" + TileMap.mapNames[gobackMapID] + "]\n[" + gobackZoneID + "]") : "[STATUS: OFF]"), getInstance(), 9, null));
            myVector.addElement(new Command("Goback Tọa Độ\n" + ((!isGoBack || !isGobackCoordinate) ? "[STATUS: OFF]" : ("[" + gobackX + "-" + gobackY + "]")), getInstance(), 10, null));
            myVector.addElement(new Command("Về Nhà Khi MP Dưới\n[" + minimumMPGoHome + "%]", getInstance(), 11, null));
            GameCanvas.menu.startAt(myVector, 3);
        }

        private static void ResetChatTextField()
        {
            ChatTextField.gI().strChat = "Chat";
            ChatTextField.gI().tfChat.name = "chat";
            ChatTextField.gI().isShow = false;
        }

        private static void TeleportTo(int x, int y)
        {
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();
            Char.myCharz().cx = x;
            Char.myCharz().cy = y + 1;
            Service.gI().charMove();
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();
        }

        private static bool isMeCanAttack(Mob mob)
        {
            if (!GameScr.canAutoPlay && mob.checkIsBoss())
            {
                if (mob.checkIsBoss())
                {
                    return isAvoidSuperMob;
                }
                return false;
            }
            return true;
        }

        private static bool isMeOutOfMP()
        {
            return Char.myCharz().cMP < Char.myCharz().cMPFull * minimumMPGoHome / 100;
        }

        private static Mob GetNextMob(int type)
        {
            if (type == 1)
            {
                long num = mSystem.currentTimeMillis();
                Mob result = null;
                for (int i = 0; i < listMobIds.Count; i++)
                {
                    Mob mob = (Mob)GameScr.vMob.elementAt(listMobIds[i]);
                    long cTimeDie = mob.cTimeDie;
                    if (!mob.isMobMe && cTimeDie < num)
                    {
                        result = mob;
                        num = cTimeDie;
                    }
                }
                return result;
            }
            Mob result2 = null;
            int minDistance = int.MaxValue;
            for (int j = 0; j < listMobIds.Count; j++)
            {
                Mob mob2 = (Mob)GameScr.vMob.elementAt(listMobIds[j]);
                if (mob2.status != 0 && mob2.status != 1 && mob2.hp > 0 && !mob2.isMobMe && isMeCanAttack(mob2))
                {
                    int dx = Math.abs(Char.myCharz().cx - mob2.xFirst);
                    int dy = Math.abs(Char.myCharz().cy - mob2.yFirst);
                    int distance = dx + dy; // Khoảng cách Manhattan
                    if (distance < minDistance)
                    {
                        result2 = mob2;
                        minDistance = distance;
                    }
                }
            }
            return result2;
        }

        public static void TuMoTDLT()
        {
            try
            {
                if (!ModProCuongLe.ExistItemBag(521))
                {
                    return;
                }
                for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
                {
                    Item item = Char.myCharz().arrItemBag[i];
                    if (item == null || item.template.id != 521)
                    {
                        continue;
                    }
                    for (int j = 0; j < Char.vItemTime.size(); j++)
                    {
                        if (((ItemTime)Char.vItemTime.elementAt(j)).idIcon == 4387)
                        {
                            return;
                        }
                    }
                    Service.gI().useItem(0, 1, (sbyte)i, -1);
                    break;
                }
            }
            catch
            {
            }
        }



        private static void TurnOnAutoTrain()
        {
            if (listMobIds.Count == 0)
            {
                GameScr.info1.addInfo("Danh Sách Tàn Sát Trống!", 0);
                isAutoTrain = false;
                return;
            }
            if (!GameScr.canAutoPlay)
            {
                isAutoTrain = true;
            }
            else
            {
                isAutoTrain = false;
            }
            TuMoTDLT();
            GameScr.isAutoPlay = true;
        }

        static AutoTrain()
        {
            listMobIds = new List<int>();
            minimumMPGoHome = 5;
            inputMPPercentGoHome = new string[2] { "Nhập %MP", "%MP" };
        }

        private static void DoIt()
        {
            bool flag = (!AutoTrain.isAutoTrain && !GameScr.canAutoPlay) || Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5 || ModProCuongLe.tanCongBoss || ModProCuongLe.tieuDietNguoiBatCo;
            if (!flag)
            {
                bool flag2 = AutoTrain.listMobIds.Count == 0;
                if (flag2)
                {
                    bool flag3 = mSystem.currentTimeMillis() - AutoTrain.lastTimeAddNewMob > 5000L;
                    if (flag3)
                    {
                        AutoTrain.lastTimeAddNewMob = mSystem.currentTimeMillis();
                        GameScr.info1.addInfo("Danh Sách Tàn Sát Trống!", 0);
                    }
                    AutoTrain.isAutoTrain = false;
                }
                else
                {
                    bool flag4 = Char.myCharz().mobFocus != null && (Char.myCharz().mobFocus == null || !Char.myCharz().mobFocus.isMobMe);
                    if (flag4)
                    {
                        bool flag5 = Char.myCharz().mobFocus.hp <= 0L || Char.myCharz().mobFocus.status == 1 || Char.myCharz().mobFocus.status == 0 || !AutoTrain.isMeCanAttack(Char.myCharz().mobFocus);
                        if (flag5)
                        {
                            Char.myCharz().mobFocus = null;
                        }
                    }
                    else
                    {
                        bool flag6 = !GameScr.canAutoPlay && AutoPick.isAutoPick;
                        if (flag6)
                        {
                            AutoPick.FocusToNearestItem();
                            bool flag7 = Char.myCharz().itemFocus != null;
                            if (flag7)
                            {
                                AutoPick.PickIt();
                                AutoPick.FocusToNearestItem();
                            }
                        }
                        else
                        {
                            Char.myCharz().itemFocus = null;
                        }
                        bool flag8 = Char.myCharz().itemFocus == null;
                        if (flag8)
                        {
                            Mob nextMob = AutoTrain.GetNextMob(0);
                            Char.myCharz().clearFocus(0);
                            bool flag9 = nextMob == null;
                            if (flag9)
                            {
                                nextMob = AutoTrain.GetNextMob(1);
                                Char.myCharz().clearFocus(0);
                                bool flag10 = !GameScr.canAutoPlay;
                                if (flag10)
                                {
                                    Char.myCharz().currentMovePoint = new MovePoint(nextMob.xFirst, nextMob.yFirst);
                                    Char.myCharz().endMovePointCommand = new Command(null, null, 8002, null);
                                }
                            }
                            else
                            {
                                Char.myCharz().mobFocus = nextMob;
                                bool canAutoPlay = GameScr.canAutoPlay;
                                if (canAutoPlay)
                                {
                                    Char.myCharz().cx = nextMob.x;
                                    Char.myCharz().cy = nextMob.y;
                                    Service.gI().charMove();
                                }
                            }
                        }
                    }
                    bool flag11 = Char.myCharz().mobFocus == null || (Char.myCharz().skillInfoPaint() != null && Char.myCharz().indexSkill < Char.myCharz().skillInfoPaint().Length && Char.myCharz().dart != null && Char.myCharz().arr != null);
                    if (!flag11)
                    {
                        bool flag12 = Char.myCharz().mobFocus != null && GameScr.canAutoPlay && (Math.abs(Char.myCharz().mobFocus.x - Char.myCharz().cx) > 50 || Math.abs(Char.myCharz().mobFocus.y - Char.myCharz().cy) > 50) && mSystem.currentTimeMillis() - AutoTrain.lastTimeTeleportToMob > 100L;
                        if (flag12)
                        {
                            AutoTrain.lastTimeTeleportToMob = mSystem.currentTimeMillis();
                            Char.myCharz().cx = Char.myCharz().mobFocus.x;
                            Char.myCharz().cy = Char.myCharz().mobFocus.y;
                            Service.gI().charMove();
                        }
                        Skill skill = null;
                        for (int i = 0; i < GameScr.keySkill.Length; i++)
                        {
                            bool flag13 = GameScr.keySkill[i] == null || GameScr.keySkill[i].paintCanNotUseSkill || GameScr.keySkill[i].template.id == 10 || GameScr.keySkill[i].template.id == 11 || GameScr.keySkill[i].template.id == 14 || GameScr.keySkill[i].template.id == 23 || GameScr.keySkill[i].template.id == 7 || GameScr.keySkill[i].template.id == 3 || GameScr.keySkill[i].template.id == 1 || GameScr.keySkill[i].template.id == 5 || GameScr.keySkill[i].template.id == 8 || GameScr.keySkill[i].template.id == 20 || GameScr.keySkill[i].template.id == 24 || GameScr.keySkill[i].template.id == 25 || GameScr.keySkill[i].template.id == 26 || GameScr.keySkill[i].template.id == 9 || GameScr.keySkill[i].template.id == 22 || GameScr.keySkill[i].template.id == 18 || (Char.myCharz().cgender == 1 && (Char.myCharz().cgender != 1 || (Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[5]) != null && (Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[5]) == null || GameScr.keySkill[i].template.id == 2)))) || Char.myCharz().skillInfoPaint() != null || (Char.myCharz().isMonkey == 1 && GameScr.keySkill[i].template.id == 13);
                            if (!flag13)
                            {
                                int num = (int)((GameScr.keySkill[i].template.manaUseType == 2) ? 1L : ((GameScr.keySkill[i].template.manaUseType == 1) ? ((long)GameScr.keySkill[i].manaUse * Char.myCharz().cMPFull / 100L) : ((long)GameScr.keySkill[i].manaUse)));
                                bool flag14 = Char.myCharz().cMP >= (long)num;
                                if (flag14)
                                {
                                    bool flag15 = skill == null;
                                    if (flag15)
                                    {
                                        skill = GameScr.keySkill[i];
                                    }
                                    else
                                    {
                                        bool flag16 = skill.coolDown < GameScr.keySkill[i].coolDown;
                                        if (flag16)
                                        {
                                            skill = GameScr.keySkill[i];
                                        }
                                    }
                                }
                            }
                        }
                        bool flag17 = skill != null;
                        if (flag17)
                        {
                            bool nextDiaHinh = AutoTrain.NextDiaHinh;
                            if (nextDiaHinh && Res.distance(Char.myCharz().cx, Char.myCharz().cy,
                                    Char.myCharz().mobFocus.xFirst, Char.myCharz().mobFocus.yFirst) <= 1000)
                            {
                                bool flag18 =
                                     !GameScr.canAutoPlay;
                                if (flag18)
                                {
                                    //if(Char.myCharz().mobFocus.getTemplate().type == Mob.TYPE_BAY)
                                    //{
                                    //    Char.myCharz().cx = Char.myCharz().mobFocus.xFirst;
                                    //    Char.myCharz().cy = ModProCuongLe.GetClosestGroundY(Char.myCharz().mobFocus.xFirst, Char.myCharz().mobFocus.yFirst);
                                    //    Service.gI().charMove();
                                    //}
                                    bool flag19 = AutoTrain.typeMobChange != global::Char.myCharz().mobFocus.mobId;
                                    if (flag19)
                                    {
                                        AutoMap.TeleportTo(global::Char.myCharz().mobFocus.xFirst, global::Char.myCharz().mobFocus.yFirst);
                                        AutoTrain.typeMobChange = global::Char.myCharz().mobFocus.mobId;
                                    }
                                }
                            }
                            GameScr.gI().doSelectSkill(skill, true);
                            //int idSkill = skill.template.id;
                            //if (idSkill == 0 || idSkill == 2 || idSkill == 4 || idSkill == 17)
                            //{
                            //    if (Char.myCharz().myskill != skill)
                            //    {
                            //        GameScr.gI().doSelectSkill(skill, true);
                            //    }
                            //    GameScr.gI().doDoubleClickToObj(Char.myCharz().mobFocus);
                            //    if (Res.distance(Char.myCharz().cx, Char.myCharz().cy,
                            //        Char.myCharz().mobFocus.xFirst, Char.myCharz().mobFocus.yFirst) <= 100)
                            //    {
                            //        AutoSkill.AutoSendAttack();
                            //    }
                            //}
                            //else
                            //{
                            //    GameScr.gI().doSelectSkill(skill, true);
                            //}

                        }
                        else
                        {
                            SkillAk();
                        }
                    }
                }
            }
        }
        public static void SkillAk()
        {
            Skill SkillDemo = null;
            for (int i = 0; i < Char.myCharz().vSkill.size(); i++)
            {
                if (Char.myCharz().vSkill.elementAt(i) != null)
                {
                    Skill skill = ((Skill)(Char.myCharz().vSkill.elementAt(i)));
                    if (skill.template.id == 4 || skill.template.id == 0 || skill.template.id == 17)
                    {
                        GameScr.gI().doSelectSkill(skill, true);
                        return;
                    }
                    else if (skill.template.id == 2)
                    {
                        SkillDemo = skill;
                    }
                }
            }
            GameScr.gI().doSelectSkill(SkillDemo, true);
        }
        public static void UseGrape()
        {
            for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
            {
                Item item = Char.myCharz().arrItemBag[i];
                if (item != null && item.template.id == 212)
                {
                    Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
                    return;
                }
            }
            for (int j = 0; j < Char.myCharz().arrItemBag.Length; j++)
            {
                Item item2 = Char.myCharz().arrItemBag[j];
                if (item2 != null && item2.template.id == 211)
                {
                    Service.gI().useItem(0, 1, (sbyte)item2.indexUI, -1);
                    break;
                }
            }
        }
        public static void changeclothes()
        {
            while (AutoChangeClothes)
            {
                if (Char.myCharz().meDead)
                {
                    Thread.Sleep(1000);
                }
                else if (Char.myCharz().isWaitMonkey)
                {
                    Thread.Sleep(1000);
                }
                else if (Char.myCharz().isMonkey == 1)
                {
                    AutoItem.useSet(0);
                    Thread.Sleep(2000);
                }
                else
                {
                    AutoItem.useSet(1);
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
