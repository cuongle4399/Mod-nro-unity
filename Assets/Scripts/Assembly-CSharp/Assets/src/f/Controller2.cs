using System;
using Assets.src.g;

namespace Assets.src.f
{
	internal class Controller2
	{
		public static void readMessage(Message msg)
		{
			try
			{
				switch (msg.command)
				{
				case sbyte.MinValue:
					readInfoEffChar(msg);
					break;
				case sbyte.MaxValue:
					readInfoRada(msg);
					break;
				case 114:
					try
					{
						string text3 = msg.reader().readUTF();
						mSystem.curINAPP = msg.reader().readByte();
						mSystem.maxINAPP = msg.reader().readByte();
						break;
					}
					catch (Exception)
					{
						break;
					}
				case 113:
				{
					int loop = 0;
					int layer = 0;
					int id = 0;
					short x = 0;
					short y = 0;
					short loopCount = -1;
					try
					{
						loop = msg.reader().readByte();
						layer = msg.reader().readByte();
						id = msg.reader().readShort();
						x = msg.reader().readShort();
						y = msg.reader().readShort();
						loopCount = msg.reader().readShort();
					}
					catch (Exception)
					{
					}
					EffecMn.addEff(new Effect(id, x, y, layer, loop, loopCount));
					break;
				}
				case 48:
				{
					sbyte b10 = msg.reader().readByte();
					ServerListScreen.SetIpSelect(b10, false);
					GameCanvas.instance.doResetToLoginScr(GameCanvas.serverScreen);
					Session_ME.gI().close();
					GameCanvas.endDlg();
					ServerListScreen.waitToLogin = true;
					break;
				}
				case 31:
				{
					int num17 = msg.reader().readInt();
					sbyte b17 = msg.reader().readByte();
					if (b17 == 1)
					{
						short smallID = msg.reader().readShort();
						sbyte b18 = -1;
						int[] array = null;
						short wimg = 0;
						short himg = 0;
						try
						{
							b18 = msg.reader().readByte();
							if (b18 > 0)
							{
								sbyte b19 = msg.reader().readByte();
								array = new int[b19];
								for (int num18 = 0; num18 < b19; num18++)
								{
									array[num18] = msg.reader().readByte();
								}
								wimg = msg.reader().readShort();
								himg = msg.reader().readShort();
							}
						}
						catch (Exception)
						{
						}
						if (num17 == Char.myCharz().charID)
						{
							Char.myCharz().petFollow = new PetFollow();
							Char.myCharz().petFollow.smallID = smallID;
							if (b18 > 0)
							{
								Char.myCharz().petFollow.SetImg(b18, array, wimg, himg);
							}
							break;
						}
						Char obj3 = GameScr.findCharInMap(num17);
						obj3.petFollow = new PetFollow();
						obj3.petFollow.smallID = smallID;
						if (b18 > 0)
						{
							obj3.petFollow.SetImg(b18, array, wimg, himg);
						}
					}
					else if (num17 == Char.myCharz().charID)
					{
						Char.myCharz().petFollow.remove();
						Char.myCharz().petFollow = null;
					}
					else
					{
						Char obj4 = GameScr.findCharInMap(num17);
						obj4.petFollow.remove();
						obj4.petFollow = null;
					}
					break;
				}
				case -89:
					GameCanvas.open3Hour = msg.reader().readByte() == 1;
					break;
				case 42:
				{
					GameCanvas.endDlg();
					LoginScr.isContinueToLogin = false;
					Char.isLoadingMap = false;
					sbyte haveName = msg.reader().readByte();
					if (GameCanvas.registerScr == null)
					{
						GameCanvas.registerScr = new RegisterScreen(haveName);
					}
					GameCanvas.registerScr.switchToMe();
					break;
				}
				case 52:
				{
					sbyte b23 = msg.reader().readByte();
					if (b23 == 1)
					{
						int num25 = msg.reader().readInt();
						if (num25 == Char.myCharz().charID)
						{
							Char.myCharz().setMabuHold(true);
							Char.myCharz().cx = msg.reader().readShort();
							Char.myCharz().cy = msg.reader().readShort();
						}
						else
						{
							Char obj5 = GameScr.findCharInMap(num25);
							if (obj5 != null)
							{
								obj5.setMabuHold(true);
								obj5.cx = msg.reader().readShort();
								obj5.cy = msg.reader().readShort();
							}
						}
					}
					if (b23 == 0)
					{
						int num26 = msg.reader().readInt();
						if (num26 == Char.myCharz().charID)
						{
							Char.myCharz().setMabuHold(false);
						}
						else
						{
							Char obj6 = GameScr.findCharInMap(num26);
							if (obj6 != null)
							{
								obj6.setMabuHold(false);
							}
						}
					}
					if (b23 == 2)
					{
						int charId2 = msg.reader().readInt();
						int id3 = msg.reader().readInt();
						Mabu mabu2 = (Mabu)GameScr.findCharInMap(charId2);
						mabu2.eat(id3);
					}
					if (b23 == 3)
					{
						GameScr.mabuPercent = msg.reader().readByte();
					}
					break;
				}
				case 51:
				{
					int charId = msg.reader().readInt();
					Mabu mabu = (Mabu)GameScr.findCharInMap(charId);
					sbyte id2 = msg.reader().readByte();
					short x2 = msg.reader().readShort();
					short y2 = msg.reader().readShort();
					sbyte b20 = msg.reader().readByte();
					Char[] array2 = new Char[b20];
					long[] array3 = new long[b20];
					for (int num19 = 0; num19 < b20; num19++)
					{
						int num20 = msg.reader().readInt();
						Res.outz("char ID=" + num20);
						array2[num19] = null;
						if (num20 != Char.myCharz().charID)
						{
							array2[num19] = GameScr.findCharInMap(num20);
						}
						else
						{
							array2[num19] = Char.myCharz();
						}
						array3[num19] = msg.reader().readLong();
					}
					mabu.setSkill(id2, x2, y2, array2, array3);
					break;
				}
				case -127:
					readLuckyRound(msg);
					break;
				case -126:
				{
					sbyte b29 = msg.reader().readByte();
					Res.outz("type quay= " + b29);
					if (b29 == 1)
					{
						sbyte b30 = msg.reader().readByte();
						string num40 = msg.reader().readUTF();
						string finish = msg.reader().readUTF();
						GameScr.gI().showWinNumber(num40, finish);
					}
					if (b29 == 0)
					{
						GameScr.gI().showYourNumber(msg.reader().readUTF());
					}
					break;
				}
				case -122:
				{
					short id4 = msg.reader().readShort();
					Npc npc2 = GameScr.findNPCInMap(id4);
					sbyte b28 = msg.reader().readByte();
					npc2.duahau = new int[b28];
					Res.outz("N DUA HAU= " + b28);
					for (int num39 = 0; num39 < b28; num39++)
					{
						npc2.duahau[num39] = msg.reader().readShort();
					}
					npc2.setStatus(msg.reader().readByte(), msg.reader().readInt());
					break;
				}
				case 102:
				{
					sbyte b24 = msg.reader().readByte();
					if (b24 == 0 || b24 == 1 || b24 == 2 || b24 == 6)
					{
						BigBoss2 bigBoss2 = Mob.getBigBoss2();
						if (bigBoss2 == null)
						{
							break;
						}
						if (b24 == 6)
						{
							bigBoss2.x = (bigBoss2.y = (bigBoss2.xTo = (bigBoss2.yTo = (bigBoss2.xFirst = (bigBoss2.yFirst = -1000)))));
							break;
						}
						sbyte b25 = msg.reader().readByte();
						Char[] array7 = new Char[b25];
						long[] array8 = new long[b25];
						for (int num32 = 0; num32 < b25; num32++)
						{
							int num33 = msg.reader().readInt();
							array7[num32] = null;
							if (num33 != Char.myCharz().charID)
							{
								array7[num32] = GameScr.findCharInMap(num33);
							}
							else
							{
								array7[num32] = Char.myCharz();
							}
							array8[num32] = msg.reader().readLong();
						}
						bigBoss2.setAttack(array7, array8, b24);
					}
					if (b24 == 3 || b24 == 4 || b24 == 5 || b24 == 7)
					{
						BachTuoc bachTuoc = Mob.getBachTuoc();
						if (bachTuoc == null)
						{
							break;
						}
						if (b24 == 7)
						{
							bachTuoc.x = (bachTuoc.y = (bachTuoc.xTo = (bachTuoc.yTo = (bachTuoc.xFirst = (bachTuoc.yFirst = -1000)))));
							break;
						}
						if (b24 == 3 || b24 == 4)
						{
							sbyte b26 = msg.reader().readByte();
							Char[] array9 = new Char[b26];
							long[] array10 = new long[b26];
							for (int num34 = 0; num34 < b26; num34++)
							{
								int num35 = msg.reader().readInt();
								array9[num34] = null;
								if (num35 != Char.myCharz().charID)
								{
									array9[num34] = GameScr.findCharInMap(num35);
								}
								else
								{
									array9[num34] = Char.myCharz();
								}
								array10[num34] = msg.reader().readLong();
							}
							bachTuoc.setAttack(array9, array10, b24);
						}
						if (b24 == 5)
						{
							short xMoveTo = msg.reader().readShort();
							bachTuoc.move(xMoveTo);
						}
					}
					if (b24 > 9 && b24 < 30)
					{
						readActionBoss(msg, b24);
					}
					break;
				}
				case 101:
				{
					Res.outz("big boss--------------------------------------------------");
					BigBoss bigBoss = Mob.getBigBoss();
					if (bigBoss == null)
					{
						break;
					}
					sbyte b21 = msg.reader().readByte();
					if (b21 == 0 || b21 == 1 || b21 == 2 || b21 == 4 || b21 == 3)
					{
						if (b21 == 3)
						{
							bigBoss.xTo = (bigBoss.xFirst = msg.reader().readShort());
							bigBoss.yTo = (bigBoss.yFirst = msg.reader().readShort());
							bigBoss.setFly();
						}
						else
						{
							sbyte b22 = msg.reader().readByte();
							Res.outz("CHUONG nChar= " + b22);
							Char[] array4 = new Char[b22];
							long[] array5 = new long[b22];
							for (int num21 = 0; num21 < b22; num21++)
							{
								int num22 = msg.reader().readInt();
								Res.outz("char ID=" + num22);
								array4[num21] = null;
								if (num22 != Char.myCharz().charID)
								{
									array4[num21] = GameScr.findCharInMap(num22);
								}
								else
								{
									array4[num21] = Char.myCharz();
								}
								array5[num21] = msg.reader().readLong();
							}
							bigBoss.setAttack(array4, array5, b21);
						}
					}
					if (b21 == 5)
					{
						bigBoss.haftBody = true;
						bigBoss.status = 2;
					}
					if (b21 == 6)
					{
						bigBoss.getDataB2();
						bigBoss.x = msg.reader().readShort();
						bigBoss.y = msg.reader().readShort();
					}
					if (b21 == 7)
					{
						bigBoss.setAttack(null, null, b21);
					}
					if (b21 == 8)
					{
						bigBoss.xTo = (bigBoss.xFirst = msg.reader().readShort());
						bigBoss.yTo = (bigBoss.yFirst = msg.reader().readShort());
						bigBoss.status = 2;
					}
					if (b21 == 9)
					{
						bigBoss.x = (bigBoss.y = (bigBoss.xTo = (bigBoss.yTo = (bigBoss.xFirst = (bigBoss.yFirst = -1000)))));
					}
					break;
				}
				case -120:
				{
					long num24 = mSystem.currentTimeMillis();
					Service.logController = num24 - Service.curCheckController;
					Service.gI().sendCheckController();
					break;
				}
				case -121:
				{
					long num27 = mSystem.currentTimeMillis();
					Service.logMap = num27 - Service.curCheckMap;
					Service.gI().sendCheckMap();
					break;
				}
				case 100:
				{
					sbyte b31 = msg.reader().readByte();
					sbyte b32 = msg.reader().readByte();
					Item item2 = null;
					if (b31 == 0)
					{
						item2 = Char.myCharz().arrItemBody[b32];
					}
					if (b31 == 1)
					{
						item2 = Char.myCharz().arrItemBag[b32];
					}
					short num41 = msg.reader().readShort();
					if (num41 == -1)
					{
						break;
					}
					item2.template = ItemTemplates.get(num41);
					item2.quantity = msg.reader().readInt();
					item2.info = msg.reader().readUTF();
					item2.content = msg.reader().readUTF();
					sbyte b33 = msg.reader().readByte();
					if (b33 != 0)
					{
						item2.itemOption = new ItemOption[b33];
						for (int num42 = 0; num42 < item2.itemOption.Length; num42++)
						{
							ItemOption itemOption3 = Controller.gI().readItemOption(msg);
							if (itemOption3 != null)
							{
								item2.itemOption[num42] = itemOption3;
							}
						}
					}
					if (item2.quantity <= 0)
					{
						item2 = null;
					}
					break;
				}
				case -123:
				{
					int charId3 = msg.reader().readInt();
					if (GameScr.findCharInMap(charId3) != null)
					{
						GameScr.findCharInMap(charId3).perCentMp = msg.reader().readByte();
					}
					break;
				}
				case -119:
					Char.myCharz().rank = msg.reader().readInt();
					break;
				case -117:
					GameScr.gI().tMabuEff = 0;
					GameScr.gI().percentMabu = msg.reader().readByte();
					if (GameScr.gI().percentMabu == 100)
					{
						GameScr.gI().mabuEff = true;
					}
					if (GameScr.gI().percentMabu == 101)
					{
						Npc.mabuEff = true;
					}
					break;
				case -116:
					GameScr.canAutoPlay = msg.reader().readByte() == 1;
					break;
				case -115:
					Char.myCharz().setPowerInfo(msg.reader().readUTF(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort());
					break;
				case -113:
				{
					sbyte[] array6 = new sbyte[10];
					for (int num29 = 0; num29 < 10; num29++)
					{
						array6[num29] = msg.reader().readByte();
						Res.outz("vlue i= " + array6[num29]);
					}
					GameScr.gI().onKSkill(array6);
					GameScr.gI().onOSkill(array6);
					GameScr.gI().onCSkill(array6);
					break;
				}
				case -111:
				{
					short num10 = msg.reader().readShort();
					ImageSource.vSource = new MyVector();
					for (int l = 0; l < num10; l++)
					{
						string iD = msg.reader().readUTF();
						sbyte version = msg.reader().readByte();
						ImageSource.vSource.addElement(new ImageSource(iD, version));
					}
					ImageSource.checkRMS();
					ImageSource.saveRMS();
					break;
				}
				case 125:
				{
					sbyte fusion = msg.reader().readByte();
					int num11 = msg.reader().readInt();
					if (num11 == Char.myCharz().charID)
					{
						Char.myCharz().setFusion(fusion);
					}
					else if (GameScr.findCharInMap(num11) != null)
					{
						GameScr.findCharInMap(num11).setFusion(fusion);
					}
					break;
				}
				case 124:
				{
					short num23 = msg.reader().readShort();
					string text4 = msg.reader().readUTF();
					Res.outz("noi chuyen = " + text4 + "npc ID= " + num23);
					Npc npc = GameScr.findNPCInMap(num23);
					if (npc != null)
					{
						npc.addInfo(text4);
					}
					break;
				}
				case 123:
				{
					Res.outz("SET POSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSss");
					int num3 = msg.reader().readInt();
					short xPos = msg.reader().readShort();
					short yPos = msg.reader().readShort();
					sbyte b4 = msg.reader().readByte();
					Char obj = null;
					if (num3 == Char.myCharz().charID)
					{
						obj = Char.myCharz();
					}
					else if (GameScr.findCharInMap(num3) != null)
					{
						obj = GameScr.findCharInMap(num3);
					}
					if (obj != null)
					{
						ServerEffect.addServerEffect((b4 != 0) ? 173 : 60, obj, 1);
						obj.setPos(xPos, yPos, b4);
					}
					break;
				}
				case 122:
				{
					short num28 = msg.reader().readShort();
					Res.outz("second login = " + num28);
					LoginScr.timeLogin = num28;
					LoginScr.currTimeLogin = (LoginScr.lastTimeLogin = mSystem.currentTimeMillis());
					GameCanvas.endDlg();
					break;
				}
				case 121:
					mSystem.publicID = msg.reader().readUTF();
					mSystem.strAdmob = msg.reader().readUTF();
					Res.outz("SHOW AD public ID= " + mSystem.publicID);
					mSystem.createAdmob();
					break;
				case -124:
				{
					sbyte b7 = msg.reader().readByte();
					sbyte b8 = msg.reader().readByte();
					if (b8 == 0)
					{
						if (b7 == 2)
						{
							int num4 = msg.reader().readInt();
							if (num4 == Char.myCharz().charID)
							{
								Char.myCharz().removeEffect();
							}
							else if (GameScr.findCharInMap(num4) != null)
							{
								GameScr.findCharInMap(num4).removeEffect();
							}
						}
						int num5 = msg.reader().readUnsignedByte();
						int num6 = msg.reader().readInt();
						if (num5 == 32)
						{
							if (b7 == 1)
							{
								int num7 = msg.reader().readInt();
								if (num6 == Char.myCharz().charID)
								{
									Char.myCharz().holdEffID = num5;
									GameScr.findCharInMap(num7).setHoldChar(Char.myCharz());
								}
								else if (GameScr.findCharInMap(num6) != null && num7 != Char.myCharz().charID)
								{
									GameScr.findCharInMap(num6).holdEffID = num5;
									GameScr.findCharInMap(num7).setHoldChar(GameScr.findCharInMap(num6));
								}
								else if (GameScr.findCharInMap(num6) != null && num7 == Char.myCharz().charID)
								{
									GameScr.findCharInMap(num6).holdEffID = num5;
									Char.myCharz().setHoldChar(GameScr.findCharInMap(num6));
								}
							}
							else if (num6 == Char.myCharz().charID)
							{
								Char.myCharz().removeHoleEff();
							}
							else if (GameScr.findCharInMap(num6) != null)
							{
								GameScr.findCharInMap(num6).removeHoleEff();
							}
						}
						if (num5 == 33)
						{
							if (b7 == 1)
							{
								if (num6 == Char.myCharz().charID)
								{
									Char.myCharz().protectEff = true;
								}
								else if (GameScr.findCharInMap(num6) != null)
								{
									GameScr.findCharInMap(num6).protectEff = true;
								}
							}
							else if (num6 == Char.myCharz().charID)
							{
								Char.myCharz().removeProtectEff();
							}
							else if (GameScr.findCharInMap(num6) != null)
							{
								GameScr.findCharInMap(num6).removeProtectEff();
							}
						}
						if (num5 == 39)
						{
							if (b7 == 1)
							{
								if (num6 == Char.myCharz().charID)
								{
									Char.myCharz().huytSao = true;
								}
								else if (GameScr.findCharInMap(num6) != null)
								{
									GameScr.findCharInMap(num6).huytSao = true;
								}
							}
							else if (num6 == Char.myCharz().charID)
							{
								Char.myCharz().removeHuytSao();
							}
							else if (GameScr.findCharInMap(num6) != null)
							{
								GameScr.findCharInMap(num6).removeHuytSao();
							}
						}
						if (num5 == 40)
						{
							if (b7 == 1)
							{
								if (num6 == Char.myCharz().charID)
								{
									Char.myCharz().blindEff = true;
								}
								else if (GameScr.findCharInMap(num6) != null)
								{
									GameScr.findCharInMap(num6).blindEff = true;
								}
							}
							else if (num6 == Char.myCharz().charID)
							{
								Char.myCharz().removeBlindEff();
							}
							else if (GameScr.findCharInMap(num6) != null)
							{
								GameScr.findCharInMap(num6).removeBlindEff();
							}
						}
						if (num5 == 41)
						{
							if (b7 == 1)
							{
								if (num6 == Char.myCharz().charID)
								{
									Char.myCharz().sleepEff = true;
								}
								else if (GameScr.findCharInMap(num6) != null)
								{
									GameScr.findCharInMap(num6).sleepEff = true;
								}
							}
							else if (num6 == Char.myCharz().charID)
							{
								Char.myCharz().removeSleepEff();
							}
							else if (GameScr.findCharInMap(num6) != null)
							{
								GameScr.findCharInMap(num6).removeSleepEff();
							}
						}
						if (num5 == 42)
						{
							if (b7 == 1)
							{
								if (num6 == Char.myCharz().charID)
								{
									Char.myCharz().stone = true;
								}
							}
							else if (num6 == Char.myCharz().charID)
							{
								Char.myCharz().stone = false;
							}
						}
					}
					if (b8 != 1)
					{
						break;
					}
					int num8 = msg.reader().readUnsignedByte();
					sbyte b9 = msg.reader().readByte();
					Res.outz("modbHoldID= " + b9 + " skillID= " + num8 + "eff ID= " + b7);
					if (num8 == 32)
					{
						if (b7 == 1)
						{
							int num9 = msg.reader().readInt();
							if (num9 == Char.myCharz().charID)
							{
								GameScr.findMobInMap(b9).holdEffID = num8;
								Char.myCharz().setHoldMob(GameScr.findMobInMap(b9));
							}
							else if (GameScr.findCharInMap(num9) != null)
							{
								GameScr.findMobInMap(b9).holdEffID = num8;
								GameScr.findCharInMap(num9).setHoldMob(GameScr.findMobInMap(b9));
							}
						}
						else
						{
							GameScr.findMobInMap(b9).removeHoldEff();
						}
					}
					if (num8 == 40)
					{
						if (b7 == 1)
						{
							GameScr.findMobInMap(b9).blindEff = true;
						}
						else
						{
							GameScr.findMobInMap(b9).removeBlindEff();
						}
					}
					if (num8 == 41)
					{
						if (b7 == 1)
						{
							GameScr.findMobInMap(b9).sleepEff = true;
						}
						else
						{
							GameScr.findMobInMap(b9).removeSleepEff();
						}
					}
					break;
				}
				case -125:
				{
					ChatTextField.gI().isShow = false;
					string text = msg.reader().readUTF();
					Res.outz("titile= " + text);
					sbyte b5 = msg.reader().readByte();
					ClientInput.gI().setInput(b5, text);
					for (int k = 0; k < b5; k++)
					{
						ClientInput.gI().tf[k].name = msg.reader().readUTF();
						sbyte b6 = msg.reader().readByte();
						if (b6 == 0)
						{
							ClientInput.gI().tf[k].setIputType(TField.INPUT_TYPE_NUMERIC);
						}
						if (b6 == 1)
						{
							ClientInput.gI().tf[k].setIputType(TField.INPUT_TYPE_ANY);
						}
						if (b6 == 2)
						{
							ClientInput.gI().tf[k].setIputType(TField.INPUT_TYPE_PASSWORD);
						}
					}
					break;
				}
				case -110:
				{
					sbyte b27 = msg.reader().readByte();
					if (b27 == 1)
					{
						int num36 = msg.reader().readInt();
						sbyte[] array11 = Rms.loadRMS(num36 + string.Empty);
						if (array11 == null)
						{
							Service.gI().sendServerData(1, -1, null);
						}
						else
						{
							Service.gI().sendServerData(1, num36, array11);
						}
					}
					if (b27 == 0)
					{
						int num37 = msg.reader().readInt();
						short num38 = msg.reader().readShort();
						sbyte[] data = new sbyte[num38];
						msg.reader().read(ref data, 0, num38);
						Rms.saveRMS(num37 + string.Empty, data);
					}
					break;
				}
				case 93:
				{
					string str = msg.reader().readUTF();
					str = Res.changeString(str);
					GameScr.gI().chatVip(str);
					break;
				}
				case -106:
				{
					short num30 = msg.reader().readShort();
					int num31 = msg.reader().readShort();
					if (ItemTime.isExistItem(num30))
					{
						ItemTime.getItemById(num30).initTime(num31);
						break;
					}
					ItemTime o = new ItemTime(num30, num31);
					Char.vItemTime.addElement(o);
					break;
				}
				case -105:
					TransportScr.gI().time = 0;
					TransportScr.gI().maxTime = msg.reader().readShort();
					TransportScr.gI().last = (TransportScr.gI().curr = mSystem.currentTimeMillis());
					TransportScr.gI().type = msg.reader().readByte();
					TransportScr.gI().switchToMe();
					break;
				case -103:
				{
					sbyte b12 = msg.reader().readByte();
					if (b12 == 0)
					{
						GameCanvas.panel.vFlag.removeAllElements();
						sbyte b13 = msg.reader().readByte();
						for (int m = 0; m < b13; m++)
						{
							Item item = new Item();
							short num12 = msg.reader().readShort();
							if (num12 != -1)
							{
								item.template = ItemTemplates.get(num12);
								sbyte b14 = msg.reader().readByte();
								if (b14 != -1)
								{
									item.itemOption = new ItemOption[b14];
									for (int n = 0; n < item.itemOption.Length; n++)
									{
										ItemOption itemOption2 = Controller.gI().readItemOption(msg);
										if (itemOption2 != null)
										{
											item.itemOption[n] = itemOption2;
										}
									}
								}
							}
							GameCanvas.panel.vFlag.addElement(item);
						}
						GameCanvas.panel.setTypeFlag();
						GameCanvas.panel.show();
					}
					else if (b12 == 1)
					{
						int num13 = msg.reader().readInt();
						sbyte b15 = msg.reader().readByte();
						Res.outz("---------------actionFlag1:  " + num13 + " : " + b15);
						if (num13 == Char.myCharz().charID)
						{
							Char.myCharz().cFlag = b15;
						}
						else if (GameScr.findCharInMap(num13) != null)
						{
							GameScr.findCharInMap(num13).cFlag = b15;
						}
						GameScr.gI().getFlagImage(num13, b15);
					}
					else
					{
						if (b12 != 2)
						{
							break;
						}
						sbyte b16 = msg.reader().readByte();
						int num14 = msg.reader().readShort();
						PKFlag pKFlag = new PKFlag();
						pKFlag.cflag = b16;
						pKFlag.IDimageFlag = num14;
						GameScr.vFlag.addElement(pKFlag);
						for (int num15 = 0; num15 < GameScr.vFlag.size(); num15++)
						{
							PKFlag pKFlag2 = (PKFlag)GameScr.vFlag.elementAt(num15);
							Res.outz("i: " + num15 + "  cflag: " + pKFlag2.cflag + "   IDimageFlag: " + pKFlag2.IDimageFlag);
						}
						for (int num16 = 0; num16 < GameScr.vCharInMap.size(); num16++)
						{
							Char obj2 = (Char)GameScr.vCharInMap.elementAt(num16);
							if (obj2 != null && obj2.cFlag == b16)
							{
								obj2.flagImage = num14;
							}
						}
						if (Char.myCharz().cFlag == b16)
						{
							Char.myCharz().flagImage = num14;
						}
					}
					break;
				}
				case -102:
				{
					sbyte b11 = msg.reader().readByte();
					if (b11 != 0 && b11 == 1)
					{
						GameCanvas.loginScr.isLogin2 = false;
						Service.gI().login(Rms.loadRMSString("acc"), Rms.loadRMSString("pass"), GameMidlet.VERSION, 0);
						LoginScr.isLoggingIn = true;
					}
					break;
				}
				case -101:
				{
					GameCanvas.loginScr.isLogin2 = true;
					GameCanvas.connect();
					string text2 = msg.reader().readUTF();
					Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, text2);
					Service.gI().setClientType();
					Service.gI().login(text2, string.Empty, GameMidlet.VERSION, 1);
					break;
				}
				case -100:
				{
					InfoDlg.hide();
					bool flag = false;
					if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
					{
						flag = true;
					}
					sbyte b = msg.reader().readByte();
					if (b < 0)
					{
						break;
					}
					Res.outz("t Indxe= " + b);
					GameCanvas.panel.maxPageShop[b] = msg.reader().readByte();
					GameCanvas.panel.currPageShop[b] = msg.reader().readByte();
					Res.outz("max page= " + GameCanvas.panel.maxPageShop[b] + " curr page= " + GameCanvas.panel.currPageShop[b]);
					int num = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemShop[b] = new Item[num];
					for (int i = 0; i < num; i++)
					{
						short num2 = msg.reader().readShort();
						if (num2 == -1)
						{
							continue;
						}
						Res.outz("template id= " + num2);
						Char.myCharz().arrItemShop[b][i] = new Item();
						Char.myCharz().arrItemShop[b][i].template = ItemTemplates.get(num2);
						Char.myCharz().arrItemShop[b][i].itemId = msg.reader().readShort();
						Char.myCharz().arrItemShop[b][i].buyCoin = msg.reader().readInt();
						Char.myCharz().arrItemShop[b][i].buyGold = msg.reader().readInt();
						Char.myCharz().arrItemShop[b][i].buyType = msg.reader().readByte();
						Char.myCharz().arrItemShop[b][i].quantity = msg.reader().readInt();
						Char.myCharz().arrItemShop[b][i].isMe = msg.reader().readByte();
						Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy;
						sbyte b2 = msg.reader().readByte();
						if (b2 != -1)
						{
							Char.myCharz().arrItemShop[b][i].itemOption = new ItemOption[b2];
							for (int j = 0; j < Char.myCharz().arrItemShop[b][i].itemOption.Length; j++)
							{
								ItemOption itemOption = Controller.gI().readItemOption(msg);
								if (itemOption != null)
								{
									Char.myCharz().arrItemShop[b][i].itemOption[j] = itemOption;
									Char.myCharz().arrItemShop[b][i].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemShop[b][i]);
								}
							}
						}
						sbyte b3 = msg.reader().readByte();
						if (b3 == 1)
						{
							int headTemp = msg.reader().readShort();
							int bodyTemp = msg.reader().readShort();
							int legTemp = msg.reader().readShort();
							int bagTemp = msg.reader().readShort();
							Char.myCharz().arrItemShop[b][i].setPartTemp(headTemp, bodyTemp, legTemp, bagTemp);
						}
						if (GameMidlet.intVERSION >= 237)
						{
							Char.myCharz().arrItemShop[b][i].nameNguoiKyGui = msg.reader().readUTF();
							Res.err("nguoi ki gui  " + Char.myCharz().arrItemShop[b][i].nameNguoiKyGui);
						}
					}
					if (flag)
					{
						GameCanvas.panel2.setTabKiGui();
					}
					GameCanvas.panel.setTabShop();
					GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = 0);
					break;
				}
				}
			}
			catch (Exception ex4)
			{
				Res.outz("=====> Controller2 " + ex4.StackTrace);
			}
		}

		private static void readLuckyRound(Message msg)
		{
			try
			{
				sbyte b = msg.reader().readByte();
				if (b == 0)
				{
					sbyte b2 = msg.reader().readByte();
					short[] array = new short[b2];
					for (int i = 0; i < b2; i++)
					{
						array[i] = msg.reader().readShort();
					}
					sbyte b3 = msg.reader().readByte();
					int price = msg.reader().readInt();
					short idTicket = msg.reader().readShort();
					CrackBallScr.gI().SetCrackBallScr(array, (byte)b3, price, idTicket);
				}
				else if (b == 1)
				{
					sbyte b4 = msg.reader().readByte();
					short[] array2 = new short[b4];
					for (int j = 0; j < b4; j++)
					{
						array2[j] = msg.reader().readShort();
					}
					CrackBallScr.gI().DoneCrackBallScr(array2);
				}
			}
			catch (Exception)
			{
			}
		}

		private static void readInfoRada(Message msg)
		{
			try
			{
				sbyte b = msg.reader().readByte();
				if (b == 0)
				{
					RadarScr.gI();
					MyVector myVector = new MyVector(string.Empty);
					short num = msg.reader().readShort();
					int num2 = 0;
					for (int i = 0; i < num; i++)
					{
						Info_RadaScr info_RadaScr = new Info_RadaScr();
						int id = msg.reader().readShort();
						int no = i + 1;
						int idIcon = msg.reader().readShort();
						sbyte rank = msg.reader().readByte();
						sbyte amount = msg.reader().readByte();
						sbyte max_amount = msg.reader().readByte();
						short templateId = -1;
						Char charInfo = null;
						sbyte b2 = msg.reader().readByte();
						if (b2 == 0)
						{
							templateId = msg.reader().readShort();
						}
						else
						{
							int head = msg.reader().readShort();
							int body = msg.reader().readShort();
							int leg = msg.reader().readShort();
							int bag = msg.reader().readShort();
							charInfo = Info_RadaScr.SetCharInfo(head, body, leg, bag);
						}
						string name = msg.reader().readUTF();
						string info = msg.reader().readUTF();
						sbyte b3 = msg.reader().readByte();
						sbyte use = msg.reader().readByte();
						sbyte b4 = msg.reader().readByte();
						ItemOption[] array = null;
						if (b4 != 0)
						{
							array = new ItemOption[b4];
							for (int j = 0; j < array.Length; j++)
							{
								ItemOption itemOption = Controller.gI().readItemOption(msg);
								sbyte activeCard = msg.reader().readByte();
								if (itemOption != null)
								{
									array[j] = itemOption;
									array[j].activeCard = activeCard;
								}
							}
						}
						info_RadaScr.SetInfo(id, no, idIcon, rank, b2, templateId, name, info, charInfo, array);
						info_RadaScr.SetLevel(b3);
						info_RadaScr.SetUse(use);
						info_RadaScr.SetAmount(amount, max_amount);
						myVector.addElement(info_RadaScr);
						if (b3 > 0)
						{
							num2++;
						}
					}
					RadarScr.gI().SetRadarScr(myVector, num2, num);
					RadarScr.gI().switchToMe();
				}
				else if (b == 1)
				{
					int id2 = msg.reader().readShort();
					sbyte use2 = msg.reader().readByte();
					if (Info_RadaScr.GetInfo(RadarScr.list, id2) != null)
					{
						Info_RadaScr.GetInfo(RadarScr.list, id2).SetUse(use2);
					}
					RadarScr.SetListUse();
				}
				else if (b == 2)
				{
					int num3 = msg.reader().readShort();
					sbyte level = msg.reader().readByte();
					int num4 = 0;
					for (int k = 0; k < RadarScr.list.size(); k++)
					{
						Info_RadaScr info_RadaScr2 = (Info_RadaScr)RadarScr.list.elementAt(k);
						if (info_RadaScr2 != null)
						{
							if (info_RadaScr2.id == num3)
							{
								info_RadaScr2.SetLevel(level);
							}
							if (info_RadaScr2.level > 0)
							{
								num4++;
							}
						}
					}
					RadarScr.SetNum(num4, RadarScr.list.size());
					if (Info_RadaScr.GetInfo(RadarScr.listUse, num3) != null)
					{
						Info_RadaScr.GetInfo(RadarScr.listUse, num3).SetLevel(level);
					}
				}
				else if (b == 3)
				{
					int id3 = msg.reader().readShort();
					sbyte amount2 = msg.reader().readByte();
					sbyte max_amount2 = msg.reader().readByte();
					if (Info_RadaScr.GetInfo(RadarScr.list, id3) != null)
					{
						Info_RadaScr.GetInfo(RadarScr.list, id3).SetAmount(amount2, max_amount2);
					}
					if (Info_RadaScr.GetInfo(RadarScr.listUse, id3) != null)
					{
						Info_RadaScr.GetInfo(RadarScr.listUse, id3).SetAmount(amount2, max_amount2);
					}
				}
				else if (b == 4)
				{
					int num5 = msg.reader().readInt();
					short idAuraEff = msg.reader().readShort();
					Char obj = null;
					obj = ((num5 != Char.myCharz().charID) ? GameScr.findCharInMap(num5) : Char.myCharz());
					if (obj != null)
					{
						obj.idAuraEff = idAuraEff;
						obj.idEff_Set_Item = msg.reader().readByte();
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private static void readInfoEffChar(Message msg)
		{
			try
			{
				sbyte b = msg.reader().readByte();
				int num = msg.reader().readInt();
				Char obj = null;
				obj = ((num != Char.myCharz().charID) ? GameScr.findCharInMap(num) : Char.myCharz());
				if (b == 0)
				{
					int id = msg.reader().readShort();
					int layer = msg.reader().readByte();
					int loop = msg.reader().readByte();
					short loopCount = msg.reader().readShort();
					sbyte isStand = msg.reader().readByte();
					if (obj != null)
					{
						obj.addEffChar(new Effect(id, obj, layer, loop, loopCount, isStand));
					}
				}
				else if (b == 1)
				{
					int id2 = msg.reader().readShort();
					if (obj != null)
					{
						obj.removeEffChar(0, id2);
					}
				}
				else if (b == 2 && obj != null)
				{
					obj.removeEffChar(-1, 0);
				}
			}
			catch (Exception)
			{
			}
		}

		private static void readActionBoss(Message msg, int actionBoss)
		{
			try
			{
				sbyte idBoss = msg.reader().readByte();
				NewBoss newBoss = Mob.getNewBoss(idBoss);
				if (newBoss == null)
				{
					return;
				}
				if (actionBoss == 10)
				{
					short xMoveTo = msg.reader().readShort();
					short yMoveTo = msg.reader().readShort();
					newBoss.move(xMoveTo, yMoveTo);
				}
				if (actionBoss >= 11 && actionBoss <= 20)
				{
					sbyte b = msg.reader().readByte();
					Char[] array = new Char[b];
					long[] array2 = new long[b];
					for (int i = 0; i < b; i++)
					{
						int num = msg.reader().readInt();
						array[i] = null;
						if (num != Char.myCharz().charID)
						{
							array[i] = GameScr.findCharInMap(num);
						}
						else
						{
							array[i] = Char.myCharz();
						}
						array2[i] = msg.reader().readLong();
					}
					sbyte dir = msg.reader().readByte();
					newBoss.setAttack(array, array2, (sbyte)(actionBoss - 10), dir);
				}
				if (actionBoss == 21)
				{
					newBoss.xTo = msg.reader().readShort();
					newBoss.yTo = msg.reader().readShort();
					newBoss.setFly();
				}
				if (actionBoss == 22)
				{
				}
				if (actionBoss == 23)
				{
					newBoss.setDie();
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
