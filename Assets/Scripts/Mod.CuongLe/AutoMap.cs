using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.CuongLe
{
    public class AutoMap : IActionListener
    {
        public class NextMap
        {
            public int MapID;
            public int Npc;
            public int Index;
            private bool isEntering;
            private long enterDelayStart;
            private Waypoint enterPendingWaypoint;
            private bool isTeleNpc;
            private long teleNpcStartTime;
            public int Index2;
            public int Index3;
            public bool walk;
            public int x;
            public int y;
            private bool hasTeleported;
            private long teleportTime;
            private int teleportAttempts;

            public void GotoMap()
            {
                if (!walk)
                {
                    if (Index == -1 && Npc == -1)
                    {
                        Waypoint wayPoint = GetWayPoint();
                        if (wayPoint != null)
                        {
                            Enter(wayPoint);
                        }
                    }
                    else
                    {
                        if (Npc == -1 || Index == -1)
                        {
                            return;
                        }
                        if (!isTeleNpc)
                        {
                            ModProCuongLe.teleNPC(Npc);
                            isTeleNpc = true;
                            teleNpcStartTime = mSystem.currentTimeMillis();
                        }
                        else
                        {
                            if (mSystem.currentTimeMillis() - teleNpcStartTime < 300)
                            {
                                return;
                            }
                            Service.gI().openMenu(Npc);
                            bool flag = Char.myCharz().taskMaint.taskId > 30;
                            int num = Index;
                            if (TileMap.mapID == 19 && MapID == 68 && Char.myCharz().taskMaint.taskId == 23)
                            {
                                num = 0;
                            }
                            if (TileMap.mapID == 19 && MapID == 109 && !flag)
                            {
                                num = 1;
                            }
                            Service.gI().confirmMenu((short)Npc, (sbyte)num);
                            if (Index2 != -1)
                            {
                                Service.gI().confirmMenu((short)Npc, (sbyte)Index2);
                                if (Index3 != -1)
                                {
                                    Service.gI().confirmMenu((short)Npc, (sbyte)Index3);
                                }
                            }
                            isTeleNpc = false;
                        }
                    }
                }
                else if (x != -1 && y != -1)
                {
                    Char.myCharz().currentMovePoint = new MovePoint(x, y);
                }
            }

            public Waypoint GetWayPoint()
            {
                for (int i = 0; i < TileMap.vGo.size(); i++)
                {
                    Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
                    if (GetMapName().Equals(GetMapName(waypoint.popup)))
                    {
                        return waypoint;
                    }
                }
                return null;
            }

            public string GetMapName()
            {
                return TileMap.mapNames[MapID];
            }

            public void Enter(Waypoint waypoint)
            {
                if (!isEntering)
                {
                    isEntering = true;
                    enterDelayStart = mSystem.currentTimeMillis();
                    enterPendingWaypoint = waypoint;
                    hasTeleported = false;
                    teleportTime = 0;
                    teleportAttempts = 0;
                }
                else
                {
                    if (mSystem.currentTimeMillis() - enterDelayStart < 200)
                    {
                        return;
                    }

                    // Special case: If current map is 166 and destination is 155, directly load map left
                    if (TileMap.mapID == 166 && MapID == 155)
                    {
                        LoadMapLeft();
                        isEntering = false;
                        enterPendingWaypoint = null;
                        hasTeleported = false;
                        teleportTime = 0;
                        teleportAttempts = 0;
                        return;
                    }

                    int num = ((waypoint.maxX < 60) ? 15 : ((waypoint.minX <= TileMap.pxw - 60) ? ((waypoint.minX + waypoint.maxX) / 2) : (TileMap.pxw - 15)));
                    int maxY = waypoint.maxY;

                    if (num == -1 || maxY == -1)
                    {
                        isEntering = false;
                        enterPendingWaypoint = null;
                        hasTeleported = false;
                        teleportTime = 0;
                        teleportAttempts = 0;
                        return;
                    }

                    int distanceX = Math.abs(Char.myCharz().cx - num);
                    int distanceY = Math.abs(Char.myCharz().cy - maxY);
                    int teleportThreshold = 30;
                    int walkThreshold = 10;
                    bool isCenterMap = waypoint.maxX >= 60 && waypoint.minX <= TileMap.pxw - 60;
                    int targetX = num;
                    int targetY = maxY;
                    int teleportX = isCenterMap ? num : num + (Char.myCharz().cx < num ? -30 : 30);
                    int teleportY = maxY;

                    if (!hasTeleported && (distanceX > teleportThreshold || distanceY > teleportThreshold) && teleportAttempts < 3)
                    {
                        TeleportTo(teleportX, teleportY);
                        hasTeleported = true;
                        teleportTime = mSystem.currentTimeMillis();
                        enterDelayStart = mSystem.currentTimeMillis();
                        teleportAttempts++;
                    }
                    else if (hasTeleported && mSystem.currentTimeMillis() - teleportTime < 50)
                    {
                        return;
                    }
                    else if (!isCenterMap && (Math.abs(Char.myCharz().cx - targetX) > teleportThreshold || distanceY > teleportThreshold) && teleportAttempts < 3 && hasTeleported)
                    {
                        TeleportTo(teleportX, teleportY);
                        teleportTime = mSystem.currentTimeMillis();
                        enterDelayStart = mSystem.currentTimeMillis();
                        teleportAttempts++;
                    }
                    else if (teleportAttempts >= 3)
                    {
                        isEntering = false;
                        enterPendingWaypoint = null;
                        hasTeleported = false;
                        teleportTime = 0;
                        teleportAttempts = 0;
                        return;
                    }
                    else if (!isCenterMap && (distanceX > walkThreshold || distanceY > walkThreshold))
                    {
                        if (Char.myCharz().currentMovePoint == null)
                        {
                            Char.myCharz().currentMovePoint = new MovePoint(targetX, targetY);
                            return;
                        }
                        return;
                    }
                    else
                    {
                        if (waypoint.isOffline)
                        {
                            Service.gI().getMapOffline();
                        }
                        else
                        {
                            Service.gI().requestChangeMap();
                        }
                        isEntering = false;
                        enterPendingWaypoint = null;
                        hasTeleported = false;
                        teleportTime = 0;
                        teleportAttempts = 0;
                    }
                }
            }

            public string GetMapName(PopUp popup)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < popup.says.Length; i++)
                {
                    stringBuilder.Append(popup.says[i]);
                    stringBuilder.Append(" ");
                }
                return stringBuilder.ToString().Trim();
            }

            public void TeleportTo(int x, int y)
            {
                if (GameScr.canAutoPlay)
                {
                    Char.myCharz().cx = x;
                    Char.myCharz().cy = y;
                    Service.gI().charMove();
                    return;
                }
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

            public NextMap(int mapID, int npcID, int index, int index2, int index3, bool walk, int x, int y)
            {
                MapID = mapID;
                Npc = npcID;
                Index = index;
                Index2 = index2;
                Index3 = index3;
                this.walk = walk;
                this.x = x;
                this.y = y;
                hasTeleported = false;
                teleportTime = 0;
                teleportAttempts = 0;
            }
        }

        public static AutoMap _Instance;

        private static Dictionary<int, List<NextMap>> linkMaps;

        private static Dictionary<string, int[]> planetDictionary;

        public static bool isXmaping;

        public static int IdMapEnd;

        private static int[] wayPointMapLeft;

        private static int[] wayPointMapCenter;

        private static int[] wayPointMapRight;

        public static bool isEatChicken;

        private static bool isHarvestPean;

        private static bool isUseCapsule;

        private static bool isUsingCapsule;

        private static bool isOpeningPanel;

        private static long lastTimeOpenedPanel;

        private static bool isSaveData;

        private static long lastWaitTime;

        private static int[] idMapsNamek;

        private static int[] idMapsXayda;

        private static int[] idMapsTraiDat;

        private static int[] idMapsTuongLai;

        private static int[] idMapsCold;

        private static int[] idMapsNappa;

        private static int[] idMapsThapleo;

        private static int[] idMapsManhVoBT;

        private static int[] idMapsKhiGas;

        private static long lastErrorTime;

        private static long lastItemUseTime;

        private static bool isUsingItem;

        public static bool xmapErrr;

        public static AutoMap getInstance()
        {
            if (_Instance == null)
            {
                _Instance = new AutoMap();
            }
            return _Instance;
        }

        public static void Update()
        {
            if (Char.myCharz().meDead && GameCanvas.gameTick % 350 == 0)
            {
                if (AutoboMong.autoboMong)
                {
                    Service.gI().returnTownFromDead();
                }
                else
                {
                    lastWaitTime = mSystem.currentTimeMillis() + 1000;
                }
            }
            if (TileMap.mapID == IdMapEnd)
            {
                FinishXmap();
                return;
            }
            bool flag = false;
            if (isEatChicken && (TileMap.mapID == 21 || TileMap.mapID == 22 || TileMap.mapID == 23))
            {
                for (int i = 0; i < GameScr.vItemMap.size(); i++)
                {
                    ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
                    if ((itemMap.playerId == Char.myCharz().charID || itemMap.playerId == -1) && itemMap.template.id == 74)
                    {
                        flag = true;
                        Char.myCharz().itemFocus = itemMap;
                        if (mSystem.currentTimeMillis() - lastWaitTime > 600)
                        {
                            lastWaitTime = mSystem.currentTimeMillis();
                            Service.gI().pickItem(Char.myCharz().itemFocus.itemMapID);
                            return;
                        }
                    }
                }
            }
            if (!isXmaping || flag || mSystem.currentTimeMillis() - lastWaitTime <= 300 || GameCanvas.gameTick % 40 != 0)
            {
                return;
            }

            bool flag2 = true;

            if (isFutureMap(IdMapEnd))
            {
                if (Char.myCharz().taskMaint.taskId <= 24)
                {
                    FinishXmap();
                    return;
                }

                if (flag2 && GameScr.findNPCInMap(38) == null)
                {
                    switch (TileMap.mapID)
                    {
                        case 27:
                            flag2 = false;
                            UpdateXmap(28);
                            break;

                        case 28:
                            flag2 = false;
                            UpdateXmap(29);
                            break;

                        case 29:
                            flag2 = false;
                            UpdateXmap(28);
                            break;
                    }
                }
            }

            if (flag2)
            {
                UpdateXmap(IdMapEnd);
            }
        }

        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    ShowPlanetMenu();
                    break;
                case 2:
                    isEatChicken = !isEatChicken;
                    GameScr.info1.addInfo("Ăn Đùi Gà\n" + (isEatChicken ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
                    if (isSaveData)
                    {
                        Rms.saveRMSInt("AutoMapIsEatChicken", isEatChicken ? 1 : 0);
                    }
                    break;
                case 3:
                    isHarvestPean = !isHarvestPean;
                    GameScr.info1.addInfo("Thu Đậu\n" + (isHarvestPean ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
                    if (isSaveData)
                    {
                        Rms.saveRMSInt("AutoMapIsHarvestPean", isHarvestPean ? 1 : 0);
                    }
                    break;
                case 4:
                    isUseCapsule = !isUseCapsule;
                    GameScr.info1.addInfo("Sử Dụng Capsule\n" + (isUseCapsule ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
                    if (isSaveData)
                    {
                        Rms.saveRMSInt("AutoMapIsUseCsb", isUseCapsule ? 1 : 0);
                    }
                    break;
                case 5:
                    isSaveData = !isSaveData;
                    GameScr.info1.addInfo("Lưu Cài Đặt Auto Map\n" + (isSaveData ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
                    Rms.saveRMSInt("AutoMapIsSaveRms", isSaveData ? 1 : 0);
                    if (isSaveData)
                    {
                        SaveData();
                    }
                    break;
                case 6:
                    ShowMapsMenu((int[])p);
                    break;
                case 7:
                    isXmaping = true;
                    IdMapEnd = (int)p;
                    break;
                case 8:
                    FinishXmap();
                    break;
            }
        }

        public static void ShowMenu()
        {
            LoadData();
            MyVector myVector = new MyVector();
            myVector.addElement(new Command("Load Map", getInstance(), 1, null));
            if (isXmaping)
            {
                myVector.addElement(new Command("Dừng load map", getInstance(), 8, null));
            }
            myVector.addElement(new Command("Ăn Đùi Gà\n" + (isEatChicken ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 2, null));
            myVector.addElement(new Command("Thu Đậu\n" + (isHarvestPean ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 3, null));
            myVector.addElement(new Command("Sử Dụng Capsule\n" + (isUseCapsule ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 4, null));
            myVector.addElement(new Command("Lưu Cài Đặt\n" + (isSaveData ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 5, null));
            GameCanvas.menu.startAt(myVector, 3);
        }

        private static void ShowPlanetMenu()
        {
            MyVector myVector = new MyVector();
            foreach (KeyValuePair<string, int[]> item in planetDictionary)
            {
                myVector.addElement(new Command(item.Key, getInstance(), 6, item.Value));
            }
            GameCanvas.menu.startAt(myVector, 3);
        }

        private static void ShowMapsMenu(int[] mapIDs)
        {
            MyVector myVector = new MyVector();
            for (int i = 0; i < mapIDs.Length; i++)
            {
                if ((Char.myCharz().cgender != 0 || (mapIDs[i] != 22 && mapIDs[i] != 23)) && (Char.myCharz().cgender != 1 || (mapIDs[i] != 21 && mapIDs[i] != 23)) && (Char.myCharz().cgender != 2 || (mapIDs[i] != 21 && mapIDs[i] != 22)))
                {
                    myVector.addElement(new Command(GetMapName(mapIDs[i]), getInstance(), 7, mapIDs[i]));
                }
            }
            GameCanvas.menu.startAt(myVector, 3);
        }

        public static void StartRunToMapId(int mapID)
        {
            isXmaping = true;
            IdMapEnd = mapID;

        }

        public static void FinishXmap()
        {
            isXmaping = false;
            isUsingCapsule = false;
            isOpeningPanel = false;
            xmapErrr = false;
        }

        public static void UpdateXmap(int mapID)
        {
            if (linkMaps.ContainsKey(999))
            {
                linkMaps.Remove(999);
            }
            linkMaps.Add(999, new List<NextMap>());
            linkMaps[999].Add(new NextMap(24 + Char.myCharz().cgender, 10, 0, -1, -1, walk: false, -1, -1));
            if (IdMapEnd == 160 && !isUsingItem)
            {
                if (!ModProCuongLe.ExistItemBag(992))
                {
                    //GameScr.info1.addInfo(GetErrorMessage(IdMapEnd, new List<int[]>(), Char.myCharz().cPower), 0);
                    FinishXmap();
                }
                else
                {
                    isUsingItem = true;
                    lastItemUseTime = mSystem.currentTimeMillis();
                    ModProCuongLe.useItem(992);
                }
                return;
            }
            int[] array = FindWay(mapID);
            if (array == null)
            {
                long num = mSystem.currentTimeMillis();
                if (num - lastErrorTime >= 1000)
                {
                    string errorMessage = GetErrorMessage(mapID, new List<int[]>(), Char.myCharz().cPower);
                    //GameScr.info1.addInfo(errorMessage, 0);
                    lastErrorTime = num;
                    xmapErrr = true;
                }
                return;
            }
            if (isUseCapsule)
            {
                if (!isUsingCapsule && array.Length > 3)
                {
                    for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
                    {
                        Item item = Char.myCharz().arrItemBag[i];
                        if (item != null && (item.template.id == 194 || (item.template.id == 193 && item.quantity > 1)))
                        {
                            isUsingCapsule = true;
                            isOpeningPanel = false;
                            lastTimeOpenedPanel = mSystem.currentTimeMillis();
                            GameCanvas.panel.mapNames = null;
                            Service.gI().useItem(0, 1, -1, item.template.id);
                            return;
                        }
                    }
                }
                if (isUsingCapsule && !isOpeningPanel && mSystem.currentTimeMillis() - lastTimeOpenedPanel < 500)
                {
                    return;
                }
                if (isUsingCapsule && !isOpeningPanel && GameCanvas.panel.mapNames == null)
                {
                    isUsingCapsule = false;
                    isOpeningPanel = true;
                    return;
                }
                if (isUsingCapsule && !isOpeningPanel)
                {
                    for (int num2 = array.Length - 1; num2 >= 1; num2--)
                    {
                        for (int j = 0; j < GameCanvas.panel.mapNames.Length; j++)
                        {
                            if (GameCanvas.panel.mapNames[j].Contains(TileMap.mapNames[array[num2]]))
                            {
                                isOpeningPanel = true;
                                Service.gI().requestMapSelect(j);
                                return;
                            }
                        }
                    }
                    isOpeningPanel = true;
                }
            }
            if (TileMap.mapID != array[0] || Char.ischangingMap || Controller.isStopReadMessage)
            {
                return;
            }
            if (Char.myCharz().clan == null)
            {
                bool flag = false;
                int[] array2 = idMapsKhiGas;
                foreach (int num3 in array2)
                {
                    if (IdMapEnd == num3)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    array2 = idMapsManhVoBT;
                    foreach (int num4 in array2)
                    {
                        if (IdMapEnd == num4)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (!flag && IdMapEnd >= 53 && IdMapEnd <= 62)
                {
                    flag = true;
                }
                if (flag)
                {
                    //GameScr.info1.addInfo(GetErrorMessage(IdMapEnd, new List<int[]>(), Char.myCharz().cPower), 0);
                    FinishXmap();
                    return;
                }
            }
            if (!isUsingItem || mSystem.currentTimeMillis() - lastItemUseTime >= 500)
            {
                if (isUsingItem && TileMap.mapID == 160)
                {
                    isUsingItem = false;
                }
                Goto(array[1]);
            }
        }

        public static void LoadMapLeft()
        {
            LoadMap(0);
        }

        public static void LoadMapCenter()
        {
            LoadMap(2);
        }

        public static void LoadMapRight()
        {
            LoadMap(1);
        }

        private static void LoadData()
        {
            isSaveData = Rms.loadRMSInt("AutoMapIsSaveRms") == 1;
            if (isSaveData)
            {
                if (Rms.loadRMSInt("AutoMapIsEatChicken") == -1)
                {
                    isEatChicken = true;
                }
                else
                {
                    isEatChicken = Rms.loadRMSInt("AutoMapIsEatChicken") == 1;
                }
                if (Rms.loadRMSInt("AutoMapIsUseCsb") == -1)
                {
                    isUseCapsule = true;
                }
                else
                {
                    isUseCapsule = Rms.loadRMSInt("AutoMapIsUseCsb") == 1;
                }
                isHarvestPean = Rms.loadRMSInt("AutoMapIsHarvestPean") == 1;
            }
        }

        private static void SaveData()
        {
            Rms.saveRMSInt("AutoMapIsEatChicken", isEatChicken ? 1 : 0);
            Rms.saveRMSInt("AutoMapIsHarvestPean", isHarvestPean ? 1 : 0);
            Rms.saveRMSInt("AutoMapIsUseCsb", isUseCapsule ? 1 : 0);
        }

        private static void LoadLinkMapsXmap()
        {
            AddLinkMapsXmap(0, 21);
            AddLinkMapsXmap(1, 47);
            AddLinkMapsXmap(47, 111);
            AddLinkMapsXmap(2, 24);
            AddLinkMapsXmap(5, 29);
            AddLinkMapsXmap(7, 22);
            AddLinkMapsXmap(9, 25);
            AddLinkMapsXmap(13, 33);
            AddLinkMapsXmap(14, 23);
            AddLinkMapsXmap(16, 26);
            AddLinkMapsXmap(20, 37);
            AddLinkMapsXmap(39, 21);
            AddLinkMapsXmap(40, 22);
            AddLinkMapsXmap(41, 23);
            AddLinkMapsXmap(109, 105);
            AddLinkMapsXmap(109, 106);
            AddLinkMapsXmap(106, 107);
            AddLinkMapsXmap(108, 105);
            AddLinkMapsXmap(80, 105);
            AddLinkMapsXmap(3, 27, 28, 29, 30);
            AddLinkMapsXmap(11, 31, 32, 33, 34);
            AddLinkMapsXmap(17, 35, 36, 37, 38);
            AddLinkMapsXmap(109, 108, 107, 110, 106);
            AddLinkMapsXmap(47, 46, 45, 48);
            AddLinkMapsXmap(131, 132, 133);
            AddLinkMapsXmap(42, 0, 1, 2, 3, 4, 5, 6);
            AddLinkMapsXmap(43, 7, 8, 9, 11, 12, 13, 10);
            AddLinkMapsXmap(52, 44, 14, 15, 16, 17, 18, 20, 19);
            AddLinkMapsXmap(53, 58, 59, 60, 61, 62, 55, 56, 54, 57);
            AddLinkMapsXmap(68, 69, 70, 71, 72, 64, 65, 63, 66, 67, 73, 74, 75, 76, 77, 81, 82, 83, 79, 80);
            AddLinkMapsXmap(102, 92, 93, 94, 96, 97, 98, 99, 100, 103);
            AddLinkMapsXmap(153, 156, 157, 158, 159);
            AddLinkMapsXmap(46, 45, 48, 50, 154, 155, 166);
            AddLinkMapsXmap(149, 147, 152, 151, 148);
            AddLinkMapsXmap(139, 140);
            AddLinkMapsXmap(160, 161, 162, 163);
        }

        private static void LoadNPCLinkMapsXmap()
        {
            AddNPCLinkMapsXmap(19, 68, 12, 1);
            AddNPCLinkMapsXmap(19, 109, 12, 0);
            AddNPCLinkMapsXmap(19, 109, 12, 1);
            AddNPCLinkMapsXmap(24, 25, 10, 0);
            AddNPCLinkMapsXmap(24, 26, 10, 1);
            AddNPCLinkMapsXmap(24, 84, 10, 2);
            AddNPCLinkMapsXmap(25, 24, 11, 0);
            AddNPCLinkMapsXmap(25, 26, 11, 1);
            AddNPCLinkMapsXmap(25, 84, 11, 2);
            AddNPCLinkMapsXmap(26, 24, 12, 0);
            AddNPCLinkMapsXmap(26, 25, 12, 1);
            AddNPCLinkMapsXmap(26, 84, 12, 2);
            AddNPCLinkMapsXmap(27, 102, 38, 1);
            AddNPCLinkMapsXmap(27, 53, 25, 0);
            AddNPCLinkMapsXmap(28, 102, 38, 1);
            AddNPCLinkMapsXmap(29, 102, 38, 1);
            AddNPCLinkMapsXmap(45, 48, 19, 3);
            AddNPCLinkMapsXmap(52, 127, 44, 0);
            AddNPCLinkMapsXmap(52, 129, 23, 2);
            AddNPCLinkMapsXmap(52, 113, 23, 1);
            AddNPCLinkMapsXmap(68, 19, 12, 0);
            AddNPCLinkMapsXmap(80, 131, 60, 0);
            AddNPCLinkMapsXmap(102, 27, 38, 1);
            AddNPCLinkMapsXmap(113, 52, 22, 4);
            AddNPCLinkMapsXmap(127, 52, 44, 2);
            AddNPCLinkMapsXmap(129, 52, 23, 3);
            AddNPCLinkMapsXmap(131, 80, 60, 1);
            AddNPCLinkMapsXmap(5, 153, 13, 0, 2);
            AddNPCLinkMapsXmap(153, 156, 47, 2);
            AddNPCLinkMapsXmap(48, 50, 20, 3, 1);
            AddNPCLinkMapsXmap(50, 154, 44, 1);
            AddNPCLinkMapsXmap(154, 155, 44, 1);
            AddNPCLinkMapsXmap(50, 48, 44, 0);
            AddNPCLinkMapsXmap(48, 45, 20, 3, 0);
            AddNPCLinkMapsXmap(154, 50, 55, 0);
            AddNPCLinkMapsXmap(155, 154, 44, 0);
            AddNPCLinkMapsXmap(153, 5, 10, 2);
            AddNPCLinkMapsXmap(155, 166, -1, -1, -1, -1, walk: true, 1400, 600);
            AddNPCLinkMapsXmap(46, 47, -1, -1, -1, -1, walk: true, 80, 700);
            AddNPCLinkMapsXmap(45, 46, -1, -1, -1, -1, walk: true, 80, 700);
            AddNPCLinkMapsXmap(46, 45, -1, -1, -1, -1, walk: true, 380, 90);
            AddNPCLinkMapsXmap(0, 149, 67, 3, 0);
            AddNPCLinkMapsXmap(24, 139, 63, 0);
            AddNPCLinkMapsXmap(139, 24, 63, 0);
            AddNPCLinkMapsXmap(84, 26, 10, 0);
            AddNPCLinkMapsXmap(84, 25, 10, 0);
            AddNPCLinkMapsXmap(84, 24, 10, 0);
            AddNPCLinkMapsXmap(126, 19, 53, 0);
            AddNPCLinkMapsXmap(19, 126, 53, 0);
        }

        private static void AddPlanetXmap()
        {
            planetDictionary.Add("Trái đất", idMapsTraiDat);
            planetDictionary.Add("Namek", idMapsNamek);
            planetDictionary.Add("Xayda", idMapsXayda);
            planetDictionary.Add("Fide", idMapsNappa);
            planetDictionary.Add("Tương lai", idMapsTuongLai);
            planetDictionary.Add("Cold", idMapsCold);
            planetDictionary.Add("Tháp leo", idMapsThapleo);
            planetDictionary.Add("Khuc vực bang", idMapsManhVoBT);
            planetDictionary.Add("Khi Gas", idMapsKhiGas);
        }

        private static void AddLinkMapsXmap(params int[] link)
        {
            for (int i = 0; i < link.Length; i++)
            {
                if (!linkMaps.ContainsKey(link[i]))
                {
                    linkMaps.Add(link[i], new List<NextMap>());
                }
                if (i != 0)
                {
                    linkMaps[link[i]].Add(new NextMap(link[i - 1], -1, -1, -1, -1, walk: false, -1, -1));
                }
                if (i != link.Length - 1)
                {
                    linkMaps[link[i]].Add(new NextMap(link[i + 1], -1, -1, -1, -1, walk: false, -1, -1));
                }
            }
        }

        private static void Goto(int mapID)
        {
            NextMap nextMap = null;
            NextMap nextMap2 = null;
            foreach (NextMap item in linkMaps[TileMap.mapID])
            {
                if (item.MapID == mapID)
                {
                    if (item.Npc != -1 || item.walk)
                    {
                        nextMap = item;
                        break;
                    }
                    if (item.Npc == -1 && item.Index == -1 && !item.walk)
                    {
                        nextMap2 = item;
                    }
                }
            }
            if (nextMap != null)
            {
                nextMap.GotoMap();
            }
            else if (nextMap2 != null)
            {
                nextMap2.GotoMap();
            }
            else
            {
                //GameScr.info1.addInfo("Không thể thực hiện", 0);
            }
        }

        private static int[] FindWay(int mapID)
        {
            List<int[]> list = new List<int[]>();
            Queue<int[]> queue = new Queue<int[]>();
            HashSet<int> hashSet = new HashSet<int>();
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            queue.Enqueue(new int[1] { TileMap.mapID });
            hashSet.Add(TileMap.mapID);
            dictionary[TileMap.mapID] = -1;
            long cPower = Char.myCharz().cPower;
            bool flag = Char.myCharz().taskMaint.taskId > 30;
            while (queue.Count > 0)
            {
                int[] array = queue.Dequeue();
                int num = array[array.Length - 1];
                if (num == mapID)
                {
                    bool flag2 = true;
                    if (hasWayGoFutureAndBack(array))
                    {
                        flag2 = false;
                    }
                    if (!flag && hasWayGoToColdMap(array))
                    {
                        flag2 = false;
                    }
                    foreach (int num2 in array)
                    {
                        if (num2 != 155 && num2 >= 153 && num2 <= 159 && cPower < 40000000000L)
                        {
                            flag2 = false;
                            break;
                        }
                        if ((num2 == 155 || num2 == 166) && cPower < 60000000000L)
                        {
                            flag2 = false;
                            break;
                        }
                        foreach (int num3 in idMapsTuongLai)
                        {
                            if (Char.myCharz().taskMaint.taskId <= 24 && num2 == num3)
                            {
                                flag2 = false;
                                break;
                            }
                        }
                    }
                    if (flag2)
                    {
                        list.Add(array);
                    }
                }
                else
                {
                    if (!linkMaps.ContainsKey(num))
                    {
                        continue;
                    }
                    foreach (NextMap item in linkMaps[num])
                    {
                        int mapID2 = item.MapID;
                        if (num == 19 && mapID2 == 109 && !flag)
                        {
                            if (!hashSet.Contains(mapID2))
                            {
                                hashSet.Add(mapID2);
                                int[] array8 = new int[array.Length + 1];
                                Array.Copy(array, array8, array.Length);
                                array8[array.Length] = mapID2;
                                queue.Enqueue(array8);
                                dictionary[mapID2] = num;
                            }
                        }
                        else if (!hashSet.Contains(mapID2) && (flag || mapID2 < 105 || mapID2 > 110))
                        {
                            hashSet.Add(mapID2);
                            int[] array9 = new int[array.Length + 1];
                            Array.Copy(array, array9, array.Length);
                            array9[array.Length] = mapID2;
                            queue.Enqueue(array9);
                            dictionary[mapID2] = num;
                        }
                    }
                }
            }
            int num4 = int.MaxValue;
            int[] result = null;
            foreach (int[] item2 in list)
            {
                if (item2.Length < num4)
                {
                    num4 = item2.Length;
                    result = item2;
                }
            }
            return result;
        }

        private static bool hasWayGoFutureAndBack(int[] ways)
        {
            for (int i = 1; i < ways.Length - 1; i++)
            {
                if (ways[i] == 102 && ways[i + 1] == 24 && (ways[i - 1] == 27 || ways[i - 1] == 28 || ways[i - 1] == 29))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool hasWayGoToColdMap(int[] ways)
        {
            for (int i = 0; i < ways.Length; i++)
            {
                if (ways[i] >= 105 && ways[i] <= 110)
                {
                    return true;
                }
            }
            return false;
        }

        private static string GetMapName(int mapID)
        {
            string text = mapID switch
            {
                129 => TileMap.mapNames[mapID] + " 23\n[" + mapID + "]",
                113 => string.Concat(new object[3] { "Siêu hạng\n[", mapID, "]" }),
                _ => TileMap.mapNames[mapID] + "\n[" + mapID + "]",
            };
            return text;
        }

        private static void LoadWaypointsInMap()
        {
            ResetSavedWaypoints();
            int num = TileMap.vGo.size();
            if (num != 2)
            {
                for (int i = 0; i < num; i++)
                {
                    Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
                    if (waypoint.maxX < 60)
                    {
                        wayPointMapLeft[0] = waypoint.minX + 15;
                        wayPointMapLeft[1] = waypoint.maxY;
                    }
                    else if (waypoint.maxX > TileMap.pxw - 60)
                    {
                        wayPointMapRight[0] = waypoint.maxX - 15;
                        wayPointMapRight[1] = waypoint.maxY;
                    }
                    else
                    {
                        wayPointMapCenter[0] = waypoint.minX + 15;
                        wayPointMapCenter[1] = waypoint.maxY;
                    }
                }
                return;
            }
            Waypoint waypoint2 = (Waypoint)TileMap.vGo.elementAt(0);
            Waypoint waypoint3 = (Waypoint)TileMap.vGo.elementAt(1);
            if ((waypoint2.maxX < 60 && waypoint3.maxX < 60) || (waypoint2.minX > TileMap.pxw - 60 && waypoint3.minX > TileMap.pxw - 60))
            {
                wayPointMapLeft[0] = waypoint2.minX + 15;
                wayPointMapLeft[1] = waypoint2.maxY;
                wayPointMapRight[0] = waypoint3.maxX - 15;
                wayPointMapRight[1] = waypoint3.maxY;
            }
            else if (waypoint2.maxX < waypoint3.maxX)
            {
                wayPointMapLeft[0] = waypoint2.minX + 15;
                wayPointMapLeft[1] = waypoint2.maxY;
                wayPointMapRight[0] = waypoint3.maxX - 15;
                wayPointMapRight[1] = waypoint3.maxY;
            }
            else
            {
                wayPointMapLeft[0] = waypoint3.minX + 15;
                wayPointMapLeft[1] = waypoint3.maxY;
                wayPointMapRight[0] = waypoint2.maxX - 15;
                wayPointMapRight[1] = waypoint2.maxY;
            }
        }

        public static int GetYGround(int x)
        {
            int num = 50;
            int num2 = 0;
            while (num2 < 30)
            {
                num2++;
                num += 24;
                if (TileMap.tileTypeAt(x, num, 2))
                {
                    if (num % 24 != 0)
                    {
                        num -= num % 24;
                    }
                    break;
                }
            }
            return num;
        }

        public static void TeleportTo(int x, int y)
        {
            if (GameScr.canAutoPlay)
            {
                Char.myCharz().cx = x;
                Char.myCharz().cy = y;
                Service.gI().charMove();
                return;
            }
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

        private static void ResetSavedWaypoints()
        {
            wayPointMapLeft = new int[2];
            wayPointMapCenter = new int[2];
            wayPointMapRight = new int[2];
        }

        private static bool isNRDMap(int mapID)
        {
            if (mapID >= 85)
            {
                return mapID <= 91;
            }
            return false;
        }

        private static bool isFutureMap(int mapID)
        {
            for (int i = 0; i < idMapsTuongLai.Length; i++)
            {
                if (idMapsTuongLai[i] == mapID)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool isNRD(ItemMap mapID)
        {
            if (mapID.template.id >= 372)
            {
                return mapID.template.id <= 378;
            }
            return false;
        }

        private static void LoadMap(int position)
        {
            if (isNRDMap(TileMap.mapID))
            {
                TeleportInNRDMap(position);
                return;
            }
            LoadWaypointsInMap();
            switch (position)
            {
                case 0:
                    if (wayPointMapLeft[0] != 0 && wayPointMapLeft[1] != 0)
                    {
                        TeleportTo(wayPointMapLeft[0], wayPointMapLeft[1]);
                    }
                    else
                    {
                        TeleportTo(60, GetYGround(60));
                    }
                    break;
                case 1:
                    if (wayPointMapRight[0] != 0 && wayPointMapRight[1] != 0)
                    {
                        TeleportTo(wayPointMapRight[0], wayPointMapRight[1]);
                    }
                    else
                    {
                        TeleportTo(TileMap.pxw - 60, GetYGround(TileMap.pxw - 60));
                    }
                    break;
                case 2:
                    if (wayPointMapCenter[0] != 0 && wayPointMapCenter[1] != 0)
                    {
                        TeleportTo(wayPointMapCenter[0], wayPointMapCenter[1]);
                    }
                    else
                    {
                        TeleportTo(TileMap.pxw / 2, GetYGround(TileMap.pxw / 2));
                    }
                    break;
            }
            if (TileMap.mapID != 7 && TileMap.mapID != 14 && TileMap.mapID != 0)
            {
                Service.gI().requestChangeMap();
            }
            else
            {
                Service.gI().getMapOffline();
            }
        }

        private static void TeleportInNRDMap(int position)
        {
            switch (position)
            {
                case 0:
                    TeleportTo(60, GetYGround(60));
                    break;
                default:
                    TeleportTo(TileMap.pxw - 60, GetYGround(TileMap.pxw - 60));
                    break;
                case 2:
                    for (int i = 0; i < GameScr.vNpc.size(); i++)
                    {
                        Npc npc = (Npc)GameScr.vNpc.elementAt(i);
                        if (npc.template.npcTemplateId >= 30 && npc.template.npcTemplateId <= 36)
                        {
                            Char.myCharz().npcFocus = npc;
                            TeleportTo(npc.cx, npc.cy - 3);
                            break;
                        }
                    }
                    break;
            }
        }

        static AutoMap()
        {
            linkMaps = new Dictionary<int, List<NextMap>>();
            planetDictionary = new Dictionary<string, int[]>();
            isEatChicken = true;
            isUseCapsule = true;
            idMapsNamek = new int[15]
            {
                43, 22, 7, 8, 9, 11, 12, 13, 10, 31,
                32, 33, 34, 43, 25
            };
            idMapsXayda = new int[20]
            {
                44, 23, 14, 15, 16, 17, 18, 20, 19, 35,
                36, 37, 38, 52, 44, 26, 84, 113, 127, 129
            };
            idMapsTraiDat = new int[26]
            {
                42, 21, 0, 1, 2, 3, 4, 5, 6, 27,
                28, 29, 30, 47, 42, 24, 53, 58, 59, 60,
                61, 62, 55, 56, 54, 57
            };
            idMapsTuongLai = new int[10] { 102, 92, 93, 94, 96, 97, 98, 99, 100, 103 };
            idMapsCold = new int[6] { 109, 108, 107, 110, 106, 105 };
            idMapsNappa = new int[23]
            {
                68, 69, 70, 71, 72, 64, 65, 63, 66, 67,
                73, 74, 75, 76, 77, 81, 82, 83, 79, 80,
                131, 132, 133
            };
            idMapsThapleo = new int[7] { 46, 45, 48, 50, 154, 155, 166 };
            idMapsManhVoBT = new int[5] { 153, 156, 157, 158, 159 };
            idMapsKhiGas = new int[5] { 149, 147, 152, 151, 148 };
            LoadLinkMapsXmap();
            LoadNPCLinkMapsXmap();
            AddPlanetXmap();
            LoadData();
        }

        private static void AddNPCLinkMapsXmap(int currentMapID, int nextMapID, int npcID, int select, int select2 = -1, int select3 = -1, bool walk = false, int x = -1, int y = -1)
        {
            if (!linkMaps.ContainsKey(currentMapID))
            {
                linkMaps.Add(currentMapID, new List<NextMap>());
            }
            linkMaps[currentMapID].Add(new NextMap(nextMapID, npcID, select, select2, select3, walk, x, y));
        }

        private static string GetErrorMessage(int mapID, List<int[]> validPaths, long currentPower)
        {
            bool flag = false;
            string result = "";
            if (mapID == 154 && currentPower < 40000000000L)
            {
                result = $"Yêu cầu sức mạnh tối thiểu cho map 154: {40000000000L:N0}.";
                flag = true;
            }
            else if (mapID == 155 && currentPower < 60000000000L)
            {
                result = $"Yêu cầu sức mạnh tối thiểu cho map 155: {60000000000L:N0}.";
                flag = true;
            }
            else if (mapID == 166 && currentPower < 60000000000L)
            {
                result = $"Yêu cầu sức mạnh tối thiểu cho map 166: {60000000000L:N0}.";
                flag = true;
            }
            else if (mapID >= 153 && mapID <= 159 && mapID != 155 && currentPower < 40000000000L)
            {
                result = $"Yêu cầu sức mạnh tối thiểu cho map {mapID}: {40000000000L:N0}.";
                flag = true;
            }
            if (!flag)
            {
                foreach (int[] validPath in validPaths)
                {
                    foreach (int num in validPath)
                    {
                        if (num == 154 && currentPower < 40000000000L)
                        {
                            result = $"Không thể đi qua map 154 vì sức mạnh {currentPower:N0} < {40000000000L:N0}.";
                            flag = true;
                            break;
                        }
                        if (num == 155 && currentPower < 60000000000L)
                        {
                            result = $"Không thể đi qua map 155 vì sức mạnh {currentPower:N0} < {60000000000L:N0}.";
                            flag = true;
                            break;
                        }
                        if (num == 166 && currentPower < 60000000000L)
                        {
                            result = $"Không thể đi qua map 166 vì sức mạnh {currentPower:N0} < {60000000000L:N0}.";
                            flag = true;
                            break;
                        }
                        if (num >= 153 && num <= 159 && num != 155 && currentPower < 40000000000L)
                        {
                            result = $"Không thể đi qua map {num} vì sức mạnh {currentPower:N0} < {40000000000L:N0}.";
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }
            }
            if (!flag && isFutureMap(mapID) && Char.myCharz().taskMaint.taskId <= 24)
            {
                result = $"Hãy hoàn thành nhiệm vụ để vào map {mapID}.";
                flag = true;
            }
            if (!flag)
            {
                foreach (int[] validPath2 in validPaths)
                {
                    foreach (int num2 in validPath2)
                    {
                        if (isFutureMap(num2) && Char.myCharz().taskMaint.taskId <= 24)
                        {
                            result = $"Hãy hoàn thành nhiệm vụ để vào map {num2}.";
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }
            }
            if (!flag)
            {
                bool flag2 = false;
                foreach (int num3 in idMapsKhiGas)
                {
                    if (mapID == num3)
                    {
                        flag2 = true;
                        break;
                    }
                }
                if (!flag2)
                {
                    foreach (int num4 in idMapsManhVoBT)
                    {
                        if (mapID == num4)
                        {
                            flag2 = true;
                            break;
                        }
                    }
                }
                if (!flag2 && mapID >= 53 && mapID <= 62)
                {
                    flag2 = true;
                }
                if (flag2 && Char.myCharz().clan == null)
                {
                    result = $"Cần có pt để vào map {mapID}.";
                    flag = true;
                }
            }
            if (!flag)
            {
                foreach (int[] validPath3 in validPaths)
                {
                    foreach (int num5 in validPath3)
                    {
                        bool flag3 = false;
                        foreach (int num6 in idMapsKhiGas)
                        {
                            if (num5 == num6)
                            {
                                flag3 = true;
                                break;
                            }
                        }
                        if (!flag3)
                        {
                            foreach (int num8 in idMapsManhVoBT)
                            {
                                if (num5 == num8)
                                {
                                    flag3 = true;
                                    break;
                                }
                            }
                        }
                        if (!flag3 && num5 >= 53 && num5 <= 62)
                        {
                            flag3 = true;
                        }
                        if (flag3 && Char.myCharz().clan == null)
                        {
                            result = $"Cần có pt để vào map {num5}.";
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }
            }
            if (!flag && mapID == 160 && !ModProCuongLe.ExistItemBag(992))
            {
                result = "Không có Nhẫn thời không!";
                flag = true;
            }
            if (!flag)
            {
                result = string.Format("Không thể tìm thấy đường đi từ map {0} đến map {1}.", TileMap.mapID, mapID, validPaths.Count);
            }
            return result;
        }
    }
}