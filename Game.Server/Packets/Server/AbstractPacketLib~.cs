﻿using System;
using System.Collections.Generic;
using log4net;
using System.Reflection;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;
using Game.Server;
using Game.Server.Packets;
using Bussiness;
using Game.Server.Rooms;
using Game.Server.GameUtils;
using Game.Server.SceneMarryRooms;
using Game.Server.HotSpringRooms;
using Game.Server.Quests;
using Game.Server.Buffer;
using System.Configuration;

namespace Game.Base.Packets
{
    [PacketLib(1)]
    public class AbstractPacketLib : IPacketLib
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected readonly GameClient m_gameClient;

        public AbstractPacketLib(GameClient client)
        {
            m_gameClient = client;
        }

        public GSPacketIn SendContinuation(GamePlayer player, HotSpringRoomInfo hotSpringRoomInfo)
        {
            throw new NotImplementedException();
        }

        public GSPacketIn SendHotSpringRoomInfo(GamePlayer player, HotSpringRoom[] allHotRoom)
        {
            GSPacketIn packet = new GSPacketIn((byte)ePackageType.HOTSPRING_ROOM_LIST_GET, player.PlayerCharacter.ID);
            packet.WriteInt(allHotRoom.Length);
            foreach (HotSpringRoom room in allHotRoom)
            {
                packet.WriteInt(room.Info.RoomNumber);
                packet.WriteInt(room.Info.RoomID);
                packet.WriteString(room.Info.RoomName);
                packet.WriteString(room.Info.Pwd);
                packet.WriteInt(room.Info.AvailTime);
                packet.WriteInt(room.Count);
                packet.WriteInt(player.PlayerCharacter.ID);
                packet.WriteString(player.PlayerCharacter.UserName);
                packet.WriteDateTime(room.Info.BeginTime);
                packet.WriteString(room.Info.RoomIntroduction);
                packet.WriteInt(room.Info.RoomType);
                packet.WriteInt(room.Info.MaxCount);
            }
            SendTCP(packet);
            return packet;
        }
        public GSPacketIn SendOpenVIP(GamePlayer Player)
        {
            GSPacketIn packet = new GSPacketIn((byte)ePackageType.VIP_RENEWAL, Player.PlayerCharacter.ID);
            packet.WriteBoolean(Player.PlayerCharacter.IsVIP);
            packet.WriteInt(Player.PlayerCharacter.VIPLevel);
            packet.WriteInt(Player.PlayerCharacter.VIPExp);
            packet.WriteDateTime(Player.PlayerCharacter.VIPExpireDay);
            packet.WriteDateTime(Player.PlayerCharacter.VIPLastDate);
            packet.WriteInt(Player.PlayerCharacter.VIPNextLevelDaysNeeded);
            packet.WriteBoolean(Player.PlayerCharacter.CanTakeVipReward);
            SendTCP(packet);
            return packet;
        }
        public static IPacketLib CreatePacketLibForVersion(int rawVersion, GameClient client)
        {
            foreach (Type t in ScriptMgr.GetDerivedClasses(typeof(IPacketLib)))
            {
                foreach (PacketLibAttribute attr in t.GetCustomAttributes(typeof(PacketLibAttribute), false))
                {
                    if (attr.RawVersion == rawVersion)
                    {
                        try
                        {
                            IPacketLib lib = (IPacketLib)Activator.CreateInstance(t, new object[] { client });
                            return lib;
                        }
                        catch (Exception e)
                        {
                            if (log.IsErrorEnabled)
                                log.Error("error creating packetlib (" + t.FullName + ") for raw version " + rawVersion, e);
                        }
                    }
                }
            }
            return null;
        }

        public void SendTCP(GSPacketIn packet)
        {
            m_gameClient.SendTCP(packet);
        }

        public void SendLoginFailed(string msg)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.LOGIN);
            pkg.WriteByte(1);
            pkg.WriteString(msg);
            SendTCP(pkg);
        }
               
        public void SendLoginSuccess()
        {
            if (m_gameClient.Player == null)
                return;

            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.LOGIN, m_gameClient.Player.PlayerCharacter.ID);
            pkg.WriteByte(0);
            pkg.WriteInt(4);//_loc_3.ZoneID = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Attack);//_loc_3.Attack = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Defence);//_loc_3.Defence = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Agility); //_loc_3.Agility = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Luck);//_loc_3.Luck = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.GP);//_loc_3.GP = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Repute);//_loc_3.Repute = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Gold);//_loc_3.Gold = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Money);//_loc_3.Money = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.GiftToken);//_loc_3.DDTMoney = _loc_2.readInt();
            pkg.WriteInt(1);//_loc_3.Score = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Hide);//_loc_3.Hide = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.FightPower);//_loc_3.FightPower = _loc_2.readInt();
            pkg.WriteInt(5);//_loc_3.apprenticeshipState
            pkg.WriteInt(-1);//_loc_3.masterID            
            pkg.WriteString("Master");// _loc_3.setMasterOrApprentices
            pkg.WriteInt(0);//_loc_3.graduatesCount
            pkg.WriteString("HoNorMaster");//_loc_3.honourOfMaster
            pkg.WriteDateTime(DateTime.Now.AddDays(50));//_loc_3.freezesDate = _loc_2.readDate();
            pkg.WriteBoolean(m_gameClient.Player.PlayerCharacter.IsVIP); //_loc_3.typeVIP = _loc_2.readByte();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.VIPLevel);
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.VIPExp);
            pkg.WriteDateTime(m_gameClient.Player.PlayerCharacter.VIPExpireDay);
            pkg.WriteDateTime(m_gameClient.Player.PlayerCharacter.LastDate);
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.VIPOnlineDays);
            pkg.WriteDateTime(DateTime.Now);
            pkg.WriteBoolean(m_gameClient.Player.PlayerCharacter.CanTakeVipReward);
            pkg.WriteInt(0);
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.AchievementPoint);
            pkg.WriteString("Tân binh Gunny");
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.OnlineTime);
            pkg.WriteBoolean(m_gameClient.Player.PlayerCharacter.Sex);
            pkg.WriteString(m_gameClient.Player.PlayerCharacter.Style + "&" + m_gameClient.Player.PlayerCharacter.Colors);
            pkg.WriteString(m_gameClient.Player.PlayerCharacter.Skin);
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.ConsortiaID);
            pkg.WriteString(m_gameClient.Player.PlayerCharacter.ConsortiaName);
            pkg.WriteInt(0);//_loc_3.badgeID = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.DutyLevel);
            pkg.WriteString(m_gameClient.Player.PlayerCharacter.DutyName);
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Right);
            pkg.WriteString(m_gameClient.Player.PlayerCharacter.ChairmanName);
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.ConsortiaHonor);
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.ConsortiaRiches);
            pkg.WriteBoolean(m_gameClient.Player.PlayerCharacter.HasBagPassword);
            pkg.WriteString(m_gameClient.Player.PlayerCharacter.PasswordQuest1);
            pkg.WriteString(m_gameClient.Player.PlayerCharacter.PasswordQuest2);
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.FailedPasswordAttemptCount);
            pkg.WriteString(m_gameClient.Player.PlayerCharacter.UserName);
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Nimbus);
            pkg.WriteString(m_gameClient.Player.PlayerCharacter.PvePermission);
            pkg.WriteString(m_gameClient.Player.PlayerCharacter.FightLabPermission);
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.AnswerSite);
            pkg.WriteInt(1);//receiebox = _loc_2.readInt();
            pkg.WriteInt(6);//receieGrade = _loc_2.readInt();
            pkg.WriteInt(30);//needGetBoxTime = _loc_2.readInt();
            pkg.WriteDateTime(m_gameClient.Player.PlayerCharacter.LastSpaDate);
            pkg.WriteDateTime(DateTime.Now.AddDays(-5));
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.UseOffer = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.matchInfo.dailyScore = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.matchInfo.dailyWinCount = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.matchInfo.dailyGameCount = _loc_2.readInt();
            pkg.WriteBoolean(m_gameClient.Player.PlayerCharacter.Sex);//_loc_3.DailyLeagueFirst = _loc_2.readBoolean();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.DailyLeagueLastScore = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.matchInfo.weeklyScore = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.matchInfo.weeklyGameCount = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.matchInfo.weeklyRanking = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.spdTexpExp = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.attTexpExp = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.defTexpExp = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.hpTexpExp = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.lukTexpExp = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.texpTaskCount = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.texpCount = _loc_2.readInt();
            pkg.WriteDateTime(DateTime.Now.AddDays(-5));//_loc_3.texpTaskDate = _loc_2.readDate();
            pkg.WriteBoolean(false);//_loc_3.isOldPlayerHasValidEquitAtLogin = _loc_2.readBoolean();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.badLuckNumber = _loc_2.readInt();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.luckyNum = _loc_2.readInt();
            pkg.WriteDateTime(DateTime.Now.AddDays(-5));//_loc_3.lastLuckyNumDate = _loc_2.readDate();
            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Offer);//_loc_3.lastLuckNum = _loc_2.readInt();
            pkg.WriteBoolean(m_gameClient.Player.PlayerCharacter.IsOldPlayer);//_loc_3.isOld = _loc_2.readBoolean();
            pkg.WriteInt(0);//_loc_3.CardSoul = _loc_2.readInt();
            pkg.WriteInt(0);//_loc_3.uesedFinishTime = _loc_2.readInt();
            SendTCP(pkg);
        }
       
        public void SendRSAKey(byte[] m, byte[] e)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.RSAKEY);
            pkg.Write(m);
            pkg.Write(e);
            SendTCP(pkg);
        }

        public void SendCheckCode()
        {
            if (m_gameClient.Player == null || m_gameClient.Player.PlayerCharacter.CheckCount < GameProperties.CHECK_MAX_FAILED_COUNT)
                return;

            if (m_gameClient.Player.PlayerCharacter.CheckError == 0)
            {
                m_gameClient.Player.PlayerCharacter.CheckCount += 10000;
            }

            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.CHECK_CODE, m_gameClient.Player.PlayerCharacter.ID, 10240);
            // pkg.WriteBoolean(true);
            if (m_gameClient.Player.PlayerCharacter.CheckError < 1)
            {
                pkg.WriteByte(0);
            }
            else
            {
                pkg.WriteByte(2);
            }
            pkg.WriteBoolean(true);
            m_gameClient.Player.PlayerCharacter.CheckCode = CheckCode.GenerateCheckCode();
            pkg.Write(CheckCode.CreateImage(m_gameClient.Player.PlayerCharacter.CheckCode));

            //string[] codes = CheckCode.GenerateCheckCode(4);
            //int index = ThreadSafeRandom.NextStatic(codes.Length);
            //m_gameClient.Player.PlayerCharacter.CheckIndex = index + 1;
            //for (int i = 0; i < codes.Length; i++)
            //{
            //    pkg.WriteString(codes[i]);
            //}

            //pkg.Write(CheckCode.CreateCheckCodeImage(codes[index]));
            SendTCP(pkg);
        }

        public void SendKitoff(string msg)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.KIT_USER);
            pkg.WriteString(msg);
            SendTCP(pkg);
        }

        public void SendEditionError(string msg)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.EDITION_ERROR);
            pkg.WriteString(msg);
            SendTCP(pkg);
        }

        public void SendWaitingRoom(bool result)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.SCENE_LOGIN);
            pkg.WriteByte((byte)(result ? 1 : 0));
            SendTCP(pkg);
        }

        public GSPacketIn SendPlayerState(int id, byte state)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.CHANGE_STATE, id);
            pkg.WriteByte(state);
            SendTCP(pkg);
            return pkg;
        }

        public virtual GSPacketIn SendMessage(eMessageType type, string message)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.SYS_MESS);
            pkg.WriteInt((int)type);
            pkg.WriteString(message);
            SendTCP(pkg);
            return pkg;
        }

        public void SendReady()
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.SERVER_READY);
            SendTCP(pkg);
        }

        public void SendUpdatePrivateInfo(PlayerInfo info)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.UPDATE_PRIVATE_INFO, info.ID);
            pkg.WriteInt(info.Money);
            pkg.WriteInt(info.GiftToken); //this._self.DDTMoney = event.pkg.readInt();
            pkg.WriteInt(1);//this._self.Score = event.pkg.readInt();
            pkg.WriteInt(info.Gold);
            pkg.WriteInt(1);//_self.badLuckNumber = event.pkg.readInt();
            pkg.WriteInt(1);//_self.damageScores = event.pkg.readInt();
            
            SendTCP(pkg);
        }

        public GSPacketIn SendUpdatePublicPlayer(PlayerInfo info)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.UPDATE_PlAYER_INFO, info.ID);
            pkg.WriteInt(info.GP);
            pkg.WriteInt(info.Offer);
            pkg.WriteInt(info.RichesOffer);
            pkg.WriteInt(info.RichesRob);
            pkg.WriteInt(info.Win);
            pkg.WriteInt(info.Total);
            pkg.WriteInt(info.Escape);
            pkg.WriteInt(info.Attack);
            pkg.WriteInt(info.Defence);
            pkg.WriteInt(info.Agility);
            pkg.WriteInt(info.Luck);
            pkg.WriteInt(info.hp);//info.hp = pkg.readInt();
            pkg.WriteInt(info.Hide);
            pkg.WriteString(info.Style);
            pkg.WriteString(info.Colors);
            pkg.WriteString(info.Skin);
            pkg.WriteBoolean(false);//info.IsShowConsortia = pkg.readBoolean();
            pkg.WriteInt(info.ConsortiaID);
            pkg.WriteString(info.ConsortiaName);
            pkg.WriteInt(0);//info.badgeID = pkg.readInt();
            pkg.WriteInt(info.ConsortiaLevel);//unknown1 = pkg.readInt();
            pkg.WriteInt(info.ConsortiaRepute);//unknown2 = pkg.readInt(); 
            pkg.WriteInt(info.Nimbus);
            pkg.WriteString(info.PvePermission);
            pkg.WriteString(info.FightLabPermission);
            pkg.WriteInt(info.FightPower);
            pkg.WriteInt(5);//apprenticeshipState = pkg.readInt();
            pkg.WriteInt(-1);//masterID = pkg.readInt();
            pkg.WriteString("Master");//setMasterOrApprentices(pkg.readUTF());
            pkg.WriteInt(0);//graduatesCount = pkg.readInt();
            pkg.WriteString("HoNorMaster");//honourOfMaster = pkg.readUTF();
            pkg.WriteInt(info.AchievementPoint);
            pkg.WriteString("Danh hiệu Gunny"); //honor = pkg.readUTF();
            pkg.WriteDateTime((DateTime)info.LastSpaDate);
            pkg.WriteInt(100);//charmgp
            pkg.WriteInt(100); //unknown3 = pkg.readInt();
            pkg.WriteDateTime(DateTime.MinValue); //info.shopFinallyGottenTime
            pkg.WriteInt(info.Offer);//info.UseOffer = pkg.readInt();
            pkg.WriteInt(info.Offer);//info.matchInfo.dailyScore = pkg.readInt();
            pkg.WriteInt(info.Offer);//info.matchInfo.dailyWinCount = pkg.readInt();
            pkg.WriteInt(info.Offer);//info.matchInfo.dailyGameCount = pkg.readInt();
            pkg.WriteInt(info.Offer);//info.matchInfo.weeklyScore = pkg.readInt();
            pkg.WriteInt(info.Offer);//info.matchInfo.weeklyGameCount = pkg.readInt();
            pkg.WriteInt(info.Offer);//info.spdTexpExp = pkg.readInt();
            pkg.WriteInt(info.Offer);//info.attTexpExp = pkg.readInt();
            pkg.WriteInt(info.Offer);//info.defTexpExp = pkg.readInt();
            pkg.WriteInt(info.Offer);//info.hpTexpExp = pkg.readInt();
            pkg.WriteInt(info.Offer);//info.lukTexpExp = pkg.readInt();
            pkg.WriteInt(info.Offer);//info.texpTaskCount = pkg.readInt();
            pkg.WriteInt(info.Offer);//info.texpCount = pkg.readInt();
            pkg.WriteDateTime(DateTime.Now.AddDays(-5));//info.texpTaskDate = pkg.readDate();
            pkg.WriteInt(1);//len = pkg.readInt();
            for (int i = 0; i < 1; i++)
            {
                pkg.WriteInt(1);//mapId = pkg.readInt();
                pkg.WriteByte(1);//flag = pkg.readByte();
            }
            SendTCP(pkg);
            return pkg;
        }


        public void SendPingTime(GamePlayer player)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.PING);
            player.PingStart = DateTime.Now.Ticks;
            pkg.WriteInt(player.PlayerCharacter.AntiAddiction);
            SendTCP(pkg);
        }

        public GSPacketIn SendNetWork(int id, long delay)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.NETWORK, id);
            pkg.WriteInt((int)delay / 1000 / 10);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendUserEquip(PlayerInfo player, List<ItemInfo> items)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ITEM_EQUIP, player.ID, 10240);
            pkg.WriteInt(player.ID);
            pkg.WriteString(player.NickName);
            pkg.WriteInt(player.Agility);
            pkg.WriteInt(player.Attack);
            pkg.WriteString(player.Colors);
            pkg.WriteString(player.Skin);
            pkg.WriteInt(player.Defence);
            pkg.WriteInt(player.GP);
            pkg.WriteInt(player.Grade);
            pkg.WriteInt(player.Luck);
            pkg.WriteInt(player.hp);//_loc_5.hp = _loc_2.readInt();
            pkg.WriteInt(player.Hide);
            pkg.WriteInt(player.Repute);
            pkg.WriteBoolean(player.Sex);
            pkg.WriteString(player.Style);
            pkg.WriteInt(player.Offer);            
            pkg.WriteBoolean(player.IsVIP);
            pkg.WriteInt(player.VIPLevel);
            pkg.WriteInt(player.Win);
            pkg.WriteInt(player.Total);
            pkg.WriteInt(player.Escape);
            pkg.WriteInt(player.ConsortiaID);
            pkg.WriteString(player.ConsortiaName);
            pkg.WriteInt(0);//_loc_5.badgeID = _loc_2.readInt();
            pkg.WriteInt(player.RichesOffer);
            pkg.WriteInt(player.RichesRob);
            pkg.WriteBoolean(player.IsMarried);
            pkg.WriteInt(player.SpouseID);
            pkg.WriteString(player.SpouseName);
            pkg.WriteString(player.DutyName);
            pkg.WriteInt(player.Nimbus);
            pkg.WriteInt(player.FightPower);
            pkg.WriteInt(5);//apprenticeshipState = _loc_2.readInt();
            pkg.WriteInt(-1);//masterID = _loc_2.readInt();
            pkg.WriteString("Master");//setMasterOrApprentices(_loc_2.readUTF());
            pkg.WriteInt(0);//graduatesCount = _loc_2.readInt();
            pkg.WriteString("HoNorMaster");//honourOfMaster = _loc_2.readUTF();
            pkg.WriteInt(player.AchievementPoint);//
            pkg.WriteString("Tân Binh");//honor = _loc_2.readUTF();
            pkg.WriteDateTime(DateTime.Now.AddDays(-2));
            pkg.WriteInt(player.Offer);    //_loc_5.spdTexpExp = _loc_2.readInt();
            pkg.WriteInt(player.Offer);    //_loc_5.attTexpExp = _loc_2.readInt();
            pkg.WriteInt(player.Offer);    //_loc_5.defTexpExp = _loc_2.readInt();
            pkg.WriteInt(player.Offer);    //_loc_5.hpTexpExp = _loc_2.readInt();
            pkg.WriteInt(player.Offer);    //_loc_5.lukTexpExp = _loc_2.readInt();
            pkg.WriteBoolean(false);    //_loc_5.DailyLeagueFirst = _loc_2.readBoolean();
            pkg.WriteInt(player.Offer);    //_loc_5.DailyLeagueLastScore = _loc_2.readInt();
            pkg.WriteInt(items.Count);
            foreach (ItemInfo info in items)
            {
                pkg.WriteByte((byte)info.BagType);
                pkg.WriteInt(info.UserID);
                pkg.WriteInt(info.ItemID);
                pkg.WriteInt(info.Count);
                pkg.WriteInt(info.Place);
                pkg.WriteInt(info.TemplateID);
                pkg.WriteInt(info.AttackCompose);
                pkg.WriteInt(info.DefendCompose);
                pkg.WriteInt(info.AgilityCompose);
                pkg.WriteInt(info.LuckCompose);
                pkg.WriteInt(info.StrengthenLevel);
                pkg.WriteBoolean(info.IsBinds);
                pkg.WriteBoolean(info.IsJudge);
                pkg.WriteDateTime(info.BeginDate);
                pkg.WriteInt(info.ValidDate);
                pkg.WriteString(info.Color);
                pkg.WriteString(info.Skin);
                pkg.WriteBoolean(info.IsUsed);
                pkg.WriteInt(info.Hole1);
                pkg.WriteInt(info.Hole2);
                pkg.WriteInt(info.Hole3);
                pkg.WriteInt(info.Hole4);
                pkg.WriteInt(info.Hole5);
                pkg.WriteInt(info.Hole6);
                pkg.WriteString(info.Template.Pic);
                pkg.WriteInt(info.Template.RefineryLevel);
                pkg.WriteDateTime(DateTime.Now);
                pkg.WriteByte((byte)info.Hole5Level);
                pkg.WriteInt(info.Hole5Exp);
                pkg.WriteByte((byte)info.Hole6Level);
                pkg.WriteInt(info.Hole6Exp);
                //_loc_8.isGold = _loc_2.readBoolean();
                if (info.StrengthenLevel > 12)
                {
                    pkg.WriteBoolean(true);
                    pkg.WriteDateTime(DateTime.Now.AddDays(365));//_loc_8.goldValidDate = _loc_2.readInt();
                    pkg.WriteDateTime(DateTime.Now);//_loc_8.goldBeginTime = _loc_2.readDateString();
                }
                else { pkg.WriteBoolean(false); }
            }
            pkg.Compress();
            SendTCP(pkg);
            return pkg;
        }

        public void SendDateTime()
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.SYS_DATE);
            pkg.WriteDateTime(DateTime.Now);
            SendTCP(pkg);
        }

        /// <summary>
        /// 给用户每日赠送物品
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public GSPacketIn SendDailyAward(GamePlayer player)
        {
            bool result = false;
            if (DateTime.Now.Date != player.PlayerCharacter.LastAward.Date)
            {
                result = true;
            }
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.DAILY_AWARD);
            pkg.WriteBoolean(result);
            pkg.WriteInt(0);
            SendTCP(pkg);
            return pkg;

        }

        #region IPacketLib 房间列表

        public GSPacketIn SendUpdateRoomList(BaseRoom room)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GAME_ROOMLIST_UPDATE);
            pkg.WriteInt(1);
            pkg.WriteInt(1);
            pkg.WriteInt(room.RoomId);
            pkg.WriteByte((byte)room.RoomType);
            pkg.WriteByte((byte)room.TimeMode);
            pkg.WriteByte((byte)room.PlayerCount);
            pkg.WriteByte((byte)room.PlacesCount);
            pkg.WriteBoolean(string.IsNullOrEmpty(room.Password) ? false : true);
            pkg.WriteInt(room.MapId);
            pkg.WriteBoolean(room.IsPlaying);
            pkg.WriteString(room.Name);
            pkg.WriteByte((byte)room.GameType);
            pkg.WriteByte((byte)room.HardLevel);
            pkg.WriteInt(room.LevelLimits);
            SendTCP(pkg);
            return pkg;
        }
        public GSPacketIn SendUpdateRoomList(List<BaseRoom> roomlist)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GAME_ROOMLIST_UPDATE);
            pkg.WriteInt(roomlist.Count);
            var length = roomlist.Count < 10 ? roomlist.Count : 10;
            pkg.WriteInt(length);
            for (int i = 0; i < roomlist.Count; i++)
            {
                BaseRoom room = roomlist[i];
                pkg.WriteInt(room.RoomId);
                pkg.WriteByte((byte)room.RoomType);
                pkg.WriteByte((byte)room.TimeMode);
                pkg.WriteByte((byte)room.PlayerCount);
                pkg.WriteByte((byte)room.PlacesCount);
                pkg.WriteBoolean(string.IsNullOrEmpty(room.Password) ? false : true);
                pkg.WriteInt(room.MapId);
                pkg.WriteBoolean(room.IsPlaying);
                pkg.WriteString(room.Name);
                pkg.WriteByte((byte)room.GameType);
                pkg.WriteByte((byte)room.HardLevel);
                pkg.WriteInt(room.LevelLimits);
            }
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendSceneAddPlayer(GamePlayer player)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.SCENE_ADD_USER, player.PlayerCharacter.ID);
            pkg.WriteInt(player.PlayerCharacter.Grade);
            pkg.WriteBoolean(player.PlayerCharacter.Sex);
            pkg.WriteString(player.PlayerCharacter.NickName);
            pkg.WriteBoolean(player.PlayerCharacter.IsVIP);
            pkg.WriteInt(player.PlayerCharacter.VIPLevel);
            pkg.WriteString(player.PlayerCharacter.ConsortiaName);
            pkg.WriteInt(player.PlayerCharacter.Offer);
            pkg.WriteInt(player.PlayerCharacter.Win);
            pkg.WriteInt(player.PlayerCharacter.Total);
            pkg.WriteInt(player.PlayerCharacter.Escape);
            pkg.WriteInt(player.PlayerCharacter.ConsortiaID);
            pkg.WriteInt(player.PlayerCharacter.Repute);
            pkg.WriteBoolean(player.PlayerCharacter.IsMarried);

            if (player.PlayerCharacter.IsMarried)
            {
                pkg.WriteInt(player.PlayerCharacter.SpouseID);// player.SpouseID = pkg.readInt();
                pkg.WriteString(player.PlayerCharacter.SpouseName);// player.SpouseName = pkg.readUTF();
            }

            pkg.WriteString(player.PlayerCharacter.UserName); //player.LoginName = pkg.readUTF();
            pkg.WriteInt(player.PlayerCharacter.FightPower);
            //_loc_3.apprenticeshipState = _loc_2.readInt();
            pkg.WriteInt(5);

            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendSceneRemovePlayer(Game.Server.GameObjects.GamePlayer player)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.SCENE_REMOVE_USER, player.PlayerCharacter.ID);
            SendTCP(pkg);
            return pkg;
        }

        #endregion

        #region IPacketLib 房间

        public GSPacketIn SendRoomPlayerAdd(GamePlayer player)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GAME_PLAYER_ENTER, player.PlayerId);
            bool isInGame = false;
            if (player.CurrentRoom.Game != null)
            {
                isInGame = true;
            }
            pkg.WriteBoolean(isInGame); //_loc_4 = _loc_2.readBoolean();
            //_loc_5 = _loc_2.readByte(); //_loc_14.place
            //_loc_6 = _loc_2.readByte(); //_loc_14.team
            //_loc_7 = _loc_2.readInt();  //_loc_13.Grade
            //_loc_8 = _loc_2.readInt();  //_loc_13.Offer
            //_loc_9 = _loc_2.readInt();  //_loc_13.Hide
            //_loc_10 = _loc_2.readInt(); //_loc_13.Repute
            //_loc_11 = _loc_2.readInt(); //_loc_14.webSpeedInfo.delay
            //_loc_12 = _loc_2.readInt(); //_loc_13.ZoneID
            pkg.WriteByte((byte)player.CurrentRoomIndex);
            pkg.WriteByte((byte)player.CurrentRoomTeam);
            pkg.WriteInt(player.PlayerCharacter.Grade);
            pkg.WriteInt(player.PlayerCharacter.Offer);
            pkg.WriteInt(player.PlayerCharacter.Hide);
            pkg.WriteInt(player.PlayerCharacter.Repute);
            pkg.WriteInt((int)player.PingTime / 1000 / 10);
            pkg.WriteInt(4); //pkg.WriteInt(player.ServerID);            
            pkg.WriteInt(player.PlayerCharacter.ID);
            pkg.WriteString(player.PlayerCharacter.NickName);
            pkg.WriteBoolean(player.PlayerCharacter.IsVIP);
            pkg.WriteInt(player.PlayerCharacter.VIPLevel);
            pkg.WriteBoolean(player.PlayerCharacter.Sex);
            pkg.WriteString(player.PlayerCharacter.Style);
            pkg.WriteString(player.PlayerCharacter.Colors);
            pkg.WriteString(player.PlayerCharacter.Skin);
            //_loc_13.WeaponID = _loc_2.readInt();
            //_loc_13.DeputyWeaponID = _loc_2.readInt();
            //_loc_13.Repute = _loc_10;
            //_loc_13.Grade = _loc_7;
            //_loc_13.Offer = _loc_8;
            //_loc_13.Hide = _loc_9;
            //_loc_13.ConsortiaID = _loc_2.readInt();
            ItemInfo item = player.MainBag.GetItemAt(6);
            pkg.WriteInt(item == null ? -1 : item.TemplateID);
            if (player.SecondWeapon == null)
            {
                pkg.WriteInt(0);
                //pkg.WriteInt(10001);
            }
            else
            {
                pkg.WriteInt(player.SecondWeapon.TemplateID);
            }            
            pkg.WriteInt(player.PlayerCharacter.ConsortiaID);
            pkg.WriteString(player.PlayerCharacter.ConsortiaName);
            pkg.WriteInt(player.PlayerCharacter.Win);
            pkg.WriteInt(player.PlayerCharacter.Total);
            pkg.WriteInt(player.PlayerCharacter.Escape);
            pkg.WriteInt(player.PlayerCharacter.ConsortiaLevel);
            pkg.WriteInt(player.PlayerCharacter.ConsortiaRepute);
            pkg.WriteBoolean(player.PlayerCharacter.IsMarried);
            if (player.PlayerCharacter.IsMarried)
            {
                pkg.WriteInt(player.PlayerCharacter.SpouseID);
                pkg.WriteString(player.PlayerCharacter.SpouseName);
            }
            pkg.WriteString(player.PlayerCharacter.UserName);
            pkg.WriteInt(player.PlayerCharacter.Nimbus);
            pkg.WriteInt(player.PlayerCharacter.FightPower);
            //_loc_13.apprenticeshipState = _loc_2.readInt();
            //_loc_13.masterID = _loc_2.readInt();
            //_loc_13.setMasterOrApprentices(_loc_2.readUTF());
            //_loc_13.graduatesCount = _loc_2.readInt();
            pkg.WriteInt(1);
            pkg.WriteInt(0);
            pkg.WriteString("Master");
            pkg.WriteInt(5);
            pkg.WriteString("HonorOfMaster");
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendRoomPlayerRemove(GamePlayer player)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GAME_PLAYER_EXIT, player.PlayerId);
             //_loc_2 = event.pkg.clientId;
             //_loc_3 = event.pkg.readInt();
             //_loc_4 = this._current.findPlayerByID(_loc_2, _loc_3);
            pkg.Parameter1 = player.PlayerId;
            pkg.WriteInt(4);
            pkg.WriteInt(4);
            SendTCP(pkg);
            return pkg;
        }       

        public GSPacketIn SendRoomUpdatePlayerStates(byte[] states)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.PLAYER_STATE);
            for (int i = 0; i < states.Length; i++)
            {
                pkg.WriteByte(states[i]);
            }
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendRoomUpdatePlacesStates(int[] states)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GAME_ROOM_UPDATE_PLACE);
            for (int i = 0; i < states.Length; i++)
            {
                pkg.WriteInt(states[i]);
            }
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendRoomPlayerChangedTeam(GamePlayer player)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GAME_TEAM, player.PlayerId);
            pkg.WriteByte((byte)player.CurrentRoomTeam);
            pkg.WriteByte((byte)player.CurrentRoomIndex);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendRoomCreate(BaseRoom room)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GAME_ROOM_CREATE);
            pkg.WriteInt(room.RoomId);
            pkg.WriteByte((byte)room.RoomType);
            pkg.WriteByte((byte)room.HardLevel);
            pkg.WriteByte((byte)room.TimeMode);
            pkg.WriteByte((byte)room.PlayerCount);
            pkg.WriteByte((byte)room.PlacesCount);
            pkg.WriteBoolean(string.IsNullOrEmpty(room.Password) ? false : true);
            pkg.WriteInt(room.MapId);
            pkg.WriteBoolean(room.IsPlaying);
            pkg.WriteString(room.Name);
            pkg.WriteByte((byte)room.GameType);
            pkg.WriteInt(room.LevelLimits);
            pkg.WriteBoolean(false);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendRoomLoginResult(bool result)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GAME_ROOM_LOGIN);
            pkg.WriteBoolean(result);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendRoomPairUpStart(BaseRoom room)
        {
            
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GAME_PAIRUP_START);
            SendTCP(pkg);
            return pkg;
        }        

        public GSPacketIn SendRoomType(GamePlayer player, BaseRoom game)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GAME_PAIRUP_ROOM_SETUP);
            pkg.WriteByte((byte)game.GameStyle);
            pkg.WriteInt((int)game.GameType);

            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendRoomPairUpCancel(BaseRoom room)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GAME_PAIRUP_CANCEL);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendRoomClear(GamePlayer player, BaseRoom game)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GAME_ROOM_CLEAR, player.PlayerCharacter.ID);
            pkg.WriteInt(game.RoomId);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendEquipChange(GamePlayer player, int place, int goodsID, string style)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.EQUIP_CHANGE, player.PlayerCharacter.ID);
            pkg.WriteByte((byte)place);
            pkg.WriteInt(goodsID);
            pkg.WriteString(style);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendRoomChange(BaseRoom room)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GAME_ROOM_SETUP_CHANGE);
            pkg.WriteInt(room.MapId);
            pkg.WriteByte((byte)room.RoomType);
            pkg.WriteByte((byte)room.TimeMode);
            pkg.WriteByte((byte)room.HardLevel);
            pkg.WriteInt(room.LevelLimits);
            pkg.WriteBoolean(false);
            SendTCP(pkg);
            return pkg;
        }

        #endregion

        #region IPacketLib 熔炼
        public GSPacketIn SendFusionPreview(GamePlayer player, Dictionary<int, double> previewItemList, bool isbind, int MinValid)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ITEM_FUSION_PREVIEW, player.PlayerCharacter.ID);
            pkg.WriteInt(previewItemList.Count);
            foreach (KeyValuePair<int, double> p in previewItemList)
            {
                pkg.WriteInt(p.Key);
                pkg.WriteInt(MinValid);
                int value = Convert.ToInt32(p.Value);
                pkg.WriteInt(value > 100 ? 100 : value < 0 ? 0 : value);

            }

            pkg.WriteBoolean(isbind);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendFusionResult(GamePlayer player, bool result)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ITEM_FUSION, player.PlayerCharacter.ID);
            pkg.WriteBoolean(result);           
            SendTCP(pkg);
            return pkg;
        }
        #endregion

        public GSPacketIn SendOpenHoleComplete(GamePlayer player, int type, bool result)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.OPEN_FIVE_SIX_HOLE, player.PlayerCharacter.ID);
            pkg.WriteInt(type);
            pkg.WriteBoolean(result);   
            SendTCP(pkg);
            return pkg;
        }
        #region IPacketLib 炼化
        public GSPacketIn SendRefineryPreview(GamePlayer player, int templateid, bool isbind, ItemInfo item)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ITEM_REFINERY_PREVIEW, player.PlayerCharacter.ID);

            pkg.WriteInt(templateid);

            pkg.WriteInt(item.ValidDate);
            pkg.WriteBoolean(isbind);
            pkg.WriteInt(item.AgilityCompose);
            pkg.WriteInt(item.AttackCompose);
            pkg.WriteInt(item.DefendCompose);
            pkg.WriteInt(item.LuckCompose);

            SendTCP(pkg);
            return pkg;
        }

        #endregion

        #region IPacketLib 背包/战利品
        
        public void SendUpdateInventorySlot(PlayerInventory bag, int[] updatedSlots)
        {
            if (m_gameClient.Player == null)
                return;
            if (bag.BagType == (int)eBageType.Card)
            {
                SendUpdateCardData(bag, updatedSlots);
                return;
            }
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GRID_GOODS, m_gameClient.Player.PlayerCharacter.ID, 10240);
            pkg.WriteInt(bag.BagType);
            pkg.WriteInt(updatedSlots.Length);
          
            foreach (int i in updatedSlots)
            {
                pkg.WriteInt(i);

                ItemInfo item = bag.GetItemAt(i);
                if (item == null)
                {
                    pkg.WriteBoolean(false);
                }
                else
                {
                    pkg.WriteBoolean(true);
                    pkg.WriteInt(item.UserID);
                    pkg.WriteInt(item.ItemID);
                    pkg.WriteInt(item.Count);
                    pkg.WriteInt(item.Place);
                    pkg.WriteInt(item.TemplateID);
                    pkg.WriteInt(item.AttackCompose);
                    pkg.WriteInt(item.DefendCompose);
                    pkg.WriteInt(item.AgilityCompose);
                    pkg.WriteInt(item.LuckCompose);
                    pkg.WriteInt(item.StrengthenLevel);
                    pkg.WriteInt(0);//item.StrengthenExp = pkg.readInt();
                    pkg.WriteBoolean(item.IsBinds);
                    pkg.WriteBoolean(item.IsJudge);
                    pkg.WriteDateTime(item.BeginDate);
                    pkg.WriteInt(item.ValidDate);
                    pkg.WriteString(item.Color == null ? "" : item.Color);
                    pkg.WriteString(item.Skin == null ? "" : item.Skin);
                    pkg.WriteBoolean(item.IsUsed);
                    pkg.WriteInt(item.Hole1);
                    pkg.WriteInt(item.Hole2);
                    pkg.WriteInt(item.Hole3);
                    pkg.WriteInt(item.Hole4);
                    pkg.WriteInt(item.Hole5);
                    pkg.WriteInt(item.Hole6);
                    pkg.WriteString(item.Template.Pic);
                    pkg.WriteInt(item.Template.RefineryLevel);
                    pkg.WriteDateTime(DateTime.Now.AddDays(365));                    
                    pkg.WriteInt(item.StrengthenTimes);
                    pkg.WriteByte((byte)item.Hole5Level);
                    pkg.WriteInt(item.Hole5Exp);
                    pkg.WriteByte((byte)item.Hole6Level);
                    pkg.WriteInt(item.Hole6Exp);
                    //item.isGold = pkg.readBoolean();
                    if (item.StrengthenLevel > 12)
                    {
                        pkg.WriteBoolean(true);
                        pkg.WriteDateTime(DateTime.Now.AddDays(365));//item.goldValidDate = pkg.readInt();
                        pkg.WriteDateTime(DateTime.Now);//item.goldBeginTime = pkg.readDateString();
                    }
                    else { pkg.WriteBoolean(false); }
                }
            }

            SendTCP(pkg);
        }
        public void SendUpdateCardData(PlayerInventory bag, int[] updatedSlots)
        {

            if (bag.BagType == (int)eBageType.Card)
            {

                GSPacketIn pkg = new GSPacketIn((byte)ePackageType.CARDS_DATA);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.ID);
                pkg.WriteInt(updatedSlots.Length);
                foreach (int i in updatedSlots)
                {
                    pkg.WriteInt(i);
                    if (bag.GetItemAt(i) != null)
                    {
                        pkg.WriteBoolean(true);
                        var item = bag.GetItemAt(i);
                        pkg.WriteInt(item.ItemID);//_loc_9.CardID = _loc_2.readInt();
                        pkg.WriteInt(1);//_loc_9.CardType = _loc_2.readInt();
                        pkg.WriteInt(m_gameClient.Player.PlayerCharacter.ID);
                        pkg.WriteInt(item.Place);
                        pkg.WriteInt(item.Template.TemplateID);
                        pkg.WriteBoolean(item.IsGold); //_loc_9.isFirstGet = _loc_2.readBoolean();
                        pkg.WriteInt(item.Attack);
                        pkg.WriteInt(item.Defence);
                        pkg.WriteInt(item.Agility);
                        pkg.WriteInt(item.Luck);
                        pkg.WriteInt(item.AttackCompose);// _loc_9.Damage = _loc_2.readInt();
                        pkg.WriteInt(item.DefendCompose); //_loc_9.Guard = _loc_2.readInt();                        
                        
                    }
                    else
                    {
                        pkg.WriteBoolean(false);
                    }
                }
                SendTCP(pkg);

            }
                return;
                
        }
        public GSPacketIn SendPlayerCardInfo(PlayerInfo player, List<ItemInfo> items)
        {

            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.CARDS_DATA, player.ID, 10240);
                    pkg.WriteInt(player.ID);                
                    pkg.WriteInt(items.Count);
                    foreach (ItemInfo item in items)
                    {
                            pkg.WriteInt(item.Place);                        
                            pkg.WriteBoolean(true);
                            pkg.WriteInt(item.ItemID);//_loc_9.CardID = _loc_2.readInt();
                            pkg.WriteInt(1);//_loc_9.CardType = _loc_2.readInt();
                            pkg.WriteInt(m_gameClient.Player.PlayerCharacter.ID);
                            pkg.WriteInt(item.Place);
                            pkg.WriteInt(item.Template.TemplateID);
                            pkg.WriteBoolean(item.IsGold); //_loc_9.isFirstGet = _loc_2.readBoolean();
                            pkg.WriteInt(item.Attack);
                            pkg.WriteInt(item.Defence);
                            pkg.WriteInt(item.Agility);
                            pkg.WriteInt(item.Luck);
                            pkg.WriteInt(item.AttackCompose);// _loc_9.Damage = _loc_2.readInt();
                            pkg.WriteInt(item.DefendCompose); //_loc_9.Guard = _loc_2.readInt();     
                    }
                
            //pkg.Compress();
            SendTCP(pkg);
            return pkg;
        }
        #endregion

        #region IPacketLib 好友

        public GSPacketIn SendFriendRemove(int FriendID)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.FRIEND_REMOVE, FriendID);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendFriendState(int playerID, bool state)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.FRIEND_STATE, playerID);
            pkg.WriteInt(1);
            pkg.WriteBoolean(state);
            pkg.WriteInt(playerID);
            SendTCP(pkg);
            return pkg;
        }

        #endregion

        #region IPacketLib 任务

        /// <summary>
        /// 发送当前用户的任务数据
        /// </summary>
        /// <param name="player"></param>
        /// <param name="infos"></param>
        /// <returns></returns>
        public GSPacketIn SendUpdateQuests(GamePlayer player, byte[] states, BaseQuest[] infos)
        {
            //TODO:完成任务列表的同步
            if (m_gameClient.Player == null)
                return null;


            try
            {
                var length = 0;
                var numSend = infos.Length;
                var j = 0;
                do
                {
                    GSPacketIn pkg = new GSPacketIn((byte)ePackageType.QUEST_UPDATE, m_gameClient.Player.PlayerCharacter.ID);
                    length = (numSend > 7) ? 7 : numSend;
                    pkg.WriteInt(length);
                    for (int i = 0; i < length; i++, j++)
                    {
                        var info = infos[j];
                        if (info.Data.IsExist)
                        {
                            pkg.WriteInt(info.Data.QuestID);           //任务编号
                            pkg.WriteBoolean(info.Data.IsComplete);    //是否完成
                            pkg.WriteInt(info.Data.Condition1);        //用户条件一
                            pkg.WriteInt(info.Data.Condition2);        //用户条件二
                            pkg.WriteInt(info.Data.Condition3);        //用户条件三
                            pkg.WriteInt(info.Data.Condition4);        //用户条件四
                            pkg.WriteDateTime(info.Data.CompletedDate);//用户条件完成日期
                            pkg.WriteInt(info.Data.RepeatFinish);      //该任务剩余接受次数。
                            pkg.WriteInt(info.Data.RandDobule);        //用户接受任务机会
                            pkg.WriteBoolean(info.Data.IsExist);         //是否为新任务
                        }
                    }
                    //输出所有的任务
                    for (int i = 0; i < states.Length; i++)
                    {
                        pkg.WriteByte(states[i]);
                    }
                    numSend -= length;
                    SendTCP(pkg);
                } while (j < infos.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }

            return new GSPacketIn((byte)ePackageType.QUEST_UPDATE, m_gameClient.Player.PlayerCharacter.ID);
        }

        #endregion

        #region IPacketLib Buffers

        public GSPacketIn SendUpdateBuffer(GamePlayer player, BufferInfo[] infos)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.BUFF_UPDATE, player.PlayerId);
            //pkg.WriteInt(infos.Length);
            foreach (BufferInfo info in infos)
            {
                pkg.WriteInt(info.Type);
                pkg.WriteBoolean(info.IsExist);
                pkg.WriteDateTime(info.BeginDate);
                pkg.WriteInt(info.ValidDate);
                pkg.WriteInt(info.Value);
            }
            SendTCP(pkg);

            return pkg;
        }

        public GSPacketIn SendBufferList(GamePlayer player, List<AbstractBuffer> infos)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.BUFF_OBTAIN, player.PlayerId);
            pkg.WriteInt(infos.Count);
            foreach (AbstractBuffer bufferInfo in infos)
            {
                BufferInfo info = bufferInfo.Info;
                pkg.WriteInt(info.Type);
                pkg.WriteBoolean(info.IsExist);
                pkg.WriteDateTime(info.BeginDate);
                pkg.WriteInt(info.ValidDate);
                pkg.WriteInt(info.Value);
            }
            SendTCP(pkg);

            return pkg;
        }

        #endregion

        #region IPacketLib Return

        //type:1加载收件邮，2加载发件邮，3加载全部
        public GSPacketIn SendMailResponse(int playerID, eMailRespose type)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.MAIL_RESPONSE);
            pkg.WriteInt(playerID);
            pkg.WriteInt((int)type);
            GameServer.Instance.LoginServer.SendPacket(pkg);
            return pkg;
        }

        #endregion

        #region IPacketLib Auction

        public GSPacketIn SendAuctionRefresh(AuctionInfo info, int auctionID, bool isExist, ItemInfo item)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.AUCTION_REFRESH);
            pkg.WriteInt(auctionID);
            pkg.WriteBoolean(isExist);
            if (isExist)
            {
                pkg.WriteInt(info.AuctioneerID);
                pkg.WriteString(info.AuctioneerName);
                pkg.WriteDateTime(info.BeginDate);
                pkg.WriteInt(info.BuyerID);
                pkg.WriteString(info.BuyerName);
                pkg.WriteInt(info.ItemID);
                pkg.WriteInt(info.Mouthful);
                pkg.WriteInt(info.PayType);
                pkg.WriteInt(info.Price);
                pkg.WriteInt(info.Rise);
                pkg.WriteInt(info.ValidDate);
                pkg.WriteBoolean(item != null);
                if (item != null)
                {
                    pkg.WriteInt(item.Count);
                    pkg.WriteInt(item.TemplateID);
                    pkg.WriteInt(item.AttackCompose);
                    pkg.WriteInt(item.DefendCompose);
                    pkg.WriteInt(item.AgilityCompose);
                    pkg.WriteInt(item.LuckCompose);
                    pkg.WriteInt(item.StrengthenLevel);
                    pkg.WriteBoolean(item.IsBinds);
                    pkg.WriteBoolean(item.IsJudge);
                    pkg.WriteDateTime(item.BeginDate);
                    pkg.WriteInt(item.ValidDate);
                    pkg.WriteString(item.Color);
                    pkg.WriteString(item.Skin);
                    pkg.WriteBoolean(item.IsUsed);
                }
            }
            pkg.Compress();
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendAASState(bool result)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.AAS_STATE_GET);
            pkg.WriteBoolean(result);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendIDNumberCheck(bool result)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.AAS_IDNUM_CHECK);
            pkg.WriteBoolean(result);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendAASInfoSet(bool result)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.AAS_INFO_SET);
            pkg.WriteBoolean(result);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendAASControl(bool result, bool IsAASInfo, bool IsMinor)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.AAS_CTRL);
            //pkg.WriteBoolean(result);
            //pkg.WriteBoolean(IsAASInfo);
            //pkg.WriteBoolean(IsMinor);
            //DDTank
            pkg.WriteBoolean(true);
            pkg.WriteInt(1);
            pkg.WriteBoolean(true);
            pkg.WriteBoolean(IsMinor);
            SendTCP(pkg);
            return pkg;
        }
        #endregion

        #region MarryInfo
        public GSPacketIn SendMarryRoomInfo(GamePlayer player, MarryRoom room)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.MARRY_ROOM_CREATE, player.PlayerCharacter.ID);
            bool result = room != null;
            pkg.WriteBoolean(result);
            if (result)
            {
                pkg.WriteInt(room.Info.ID);
                pkg.WriteBoolean(room.Info.IsHymeneal);
                pkg.WriteString(room.Info.Name);
                pkg.WriteBoolean(room.Info.Pwd == "" ? false : true);
                pkg.WriteInt(room.Info.MapIndex);
                pkg.WriteInt(room.Info.AvailTime);
                pkg.WriteInt(room.Count);
                pkg.WriteInt(room.Info.PlayerID);
                pkg.WriteString(room.Info.PlayerName);
                pkg.WriteInt(room.Info.GroomID);
                pkg.WriteString(room.Info.GroomName);
                pkg.WriteInt(room.Info.BrideID);
                pkg.WriteString(room.Info.BrideName);
                pkg.WriteDateTime(room.Info.BeginTime);
                pkg.WriteByte((byte)room.RoomState);
                pkg.WriteString(room.Info.RoomIntroduction);
            }

            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendMarryRoomLogin(GamePlayer player, bool result)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.MARRY_ROOM_LOGIN, player.PlayerCharacter.ID);
            pkg.WriteBoolean(result);
            if (result)
            {
                pkg.WriteInt(player.CurrentMarryRoom.Info.ID);
                pkg.WriteString(player.CurrentMarryRoom.Info.Name);
                pkg.WriteInt(player.CurrentMarryRoom.Info.MapIndex);
                pkg.WriteInt(player.CurrentMarryRoom.Info.AvailTime);
                pkg.WriteInt(player.CurrentMarryRoom.Count);
                //pkg.WriteInt(room.Player.PlayerCharacter.ID);
                //pkg.WriteInt(room.Groom.PlayerCharacter.ID);
                //pkg.WriteInt(room.Bride.PlayerCharacter.ID);
                pkg.WriteInt(player.CurrentMarryRoom.Info.PlayerID);
                pkg.WriteString(player.CurrentMarryRoom.Info.PlayerName);
                pkg.WriteInt(player.CurrentMarryRoom.Info.GroomID);
                pkg.WriteString(player.CurrentMarryRoom.Info.GroomName);
                pkg.WriteInt(player.CurrentMarryRoom.Info.BrideID);
                pkg.WriteString(player.CurrentMarryRoom.Info.BrideName);

                pkg.WriteDateTime(player.CurrentMarryRoom.Info.BeginTime);
                pkg.WriteBoolean(player.CurrentMarryRoom.Info.IsHymeneal);
                pkg.WriteByte((byte)player.CurrentMarryRoom.RoomState);
                pkg.WriteString(player.CurrentMarryRoom.Info.RoomIntroduction);
                pkg.WriteBoolean(player.CurrentMarryRoom.Info.GuestInvite);
                pkg.WriteInt(player.MarryMap);
                pkg.WriteBoolean(player.CurrentMarryRoom.Info.IsGunsaluteUsed);
            }

            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendPlayerEnterMarryRoom(Game.Server.GameObjects.GamePlayer player)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.PLAYER_ENTER_MARRY_ROOM, player.PlayerCharacter.ID);
            pkg.WriteInt(player.PlayerCharacter.Grade);
            pkg.WriteInt(player.PlayerCharacter.Hide);
            pkg.WriteInt(player.PlayerCharacter.Repute);
            pkg.WriteInt(player.PlayerCharacter.ID);
            pkg.WriteString(player.PlayerCharacter.NickName);
            pkg.WriteBoolean(player.PlayerCharacter.IsVIP);
            pkg.WriteInt(player.PlayerCharacter.VIPLevel);
            pkg.WriteBoolean(player.PlayerCharacter.Sex);
            pkg.WriteString(player.PlayerCharacter.Style);
            pkg.WriteString(player.PlayerCharacter.Colors);
            pkg.WriteString(player.PlayerCharacter.Skin);
            pkg.WriteInt(player.X);
            pkg.WriteInt(player.Y);
            pkg.WriteInt(player.PlayerCharacter.FightPower);
            pkg.WriteInt(player.PlayerCharacter.Win);
            pkg.WriteInt(player.PlayerCharacter.Total);
            pkg.WriteInt(player.PlayerCharacter.Offer);
            
            SendTCP(pkg);

            return pkg;
        }


        public GSPacketIn SendMarryInfoRefresh(MarryInfo info, int ID, bool isExist)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.MARRYINFO_REFRESH);
            pkg.WriteInt(ID);
            pkg.WriteBoolean(isExist);
            if (isExist)
            {
                pkg.WriteInt(info.UserID);
                pkg.WriteBoolean(info.IsPublishEquip);
                pkg.WriteString(info.Introduction);
            }
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendPlayerMarryStatus(GamePlayer player, int userID, bool isMarried)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.MARRY_STATUS, player.PlayerCharacter.ID);
            pkg.WriteInt(userID);
            pkg.WriteBoolean(isMarried);
            SendTCP(pkg);
            return pkg;

        }

        public GSPacketIn SendPlayerMarryApply(GamePlayer player, int userID, string userName, string loveProclamation, int id)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.MARRY_APPLY, player.PlayerCharacter.ID);
            pkg.WriteInt(userID);//求婚者的ID
            pkg.WriteString(userName);//求婚者的昵称
            pkg.WriteString(loveProclamation);
            pkg.WriteInt(id);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendPlayerDivorceApply(GamePlayer player, bool result, bool isProposer)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.DIVORCE_APPLY, player.PlayerCharacter.ID);
            pkg.WriteBoolean(result);
            //判断是主动提出离婚者还是被动离婚者
            pkg.WriteBoolean(isProposer);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendMarryApplyReply(GamePlayer player, int UserID, string UserName, bool result, bool isApplicant, int id)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.MARRY_APPLY_REPLY, player.PlayerCharacter.ID);
            pkg.WriteInt(UserID);//对方的ID
            pkg.WriteBoolean(result);
            pkg.WriteString(UserName);//对方的名称
            pkg.WriteBoolean(isApplicant);
            pkg.WriteInt(id);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendBigSpeakerMsg(GamePlayer player, string msg)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.B_BUGLE, player.PlayerCharacter.ID);
            pkg.WriteInt(player.PlayerCharacter.ID);
            pkg.WriteString(player.PlayerCharacter.NickName);
            pkg.WriteString(msg);

            GameServer.Instance.LoginServer.SendPacket(pkg);

            GamePlayer[] players = Game.Server.Managers.WorldMgr.GetAllPlayers();
            foreach (GamePlayer p in players)
            {
                p.Out.SendTCP(pkg);
            }

            return pkg;
        }

        public GSPacketIn SendPlayerLeaveMarryRoom(GamePlayer player)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.PLAYER_EXIT_MARRY_ROOM, player.PlayerCharacter.ID);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendMarryRoomInfoToPlayer(GamePlayer player, bool state, MarryRoomInfo info)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.MARRY_ROOM_STATE, player.PlayerCharacter.ID);
            pkg.WriteBoolean(state);
            if (state)
            {
                pkg.WriteInt(info.ID);
                pkg.WriteString(info.Name);
                pkg.WriteInt(info.MapIndex);
                pkg.WriteInt(info.AvailTime);
                //pkg.WriteInt(info.Count);
                //pkg.WriteInt(room.Player.PlayerCharacter.ID);
                //pkg.WriteInt(room.Groom.PlayerCharacter.ID);
                //pkg.WriteInt(room.Bride.PlayerCharacter.ID);
                pkg.WriteInt(info.PlayerID);
                pkg.WriteInt(info.GroomID);
                pkg.WriteInt(info.BrideID);

                pkg.WriteDateTime(info.BeginTime);
                //pkg.WriteBoolean(info.IsHymeneal);
                pkg.WriteBoolean(info.IsGunsaluteUsed);
            }
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendMarryInfo(GamePlayer player, MarryInfo info)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.MARRYINFO_GET, player.PlayerCharacter.ID);
            pkg.WriteString(info.Introduction);
            pkg.WriteBoolean(info.IsPublishEquip);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendContinuation(GamePlayer player, MarryRoomInfo info)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.MARRY_CMD, player.PlayerCharacter.ID);
            pkg.WriteByte((byte)MarryCmdType.CONTINUATION);
            pkg.WriteInt(info.AvailTime);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendMarryProp(GamePlayer player, MarryProp info)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.MARRYPROP_GET, player.PlayerCharacter.ID);
            pkg.WriteBoolean(info.IsMarried);
            pkg.WriteInt(info.SpouseID);
            pkg.WriteString(info.SpouseName);
            pkg.WriteBoolean(info.IsCreatedMarryRoom);
            pkg.WriteInt(info.SelfMarryRoomID);
            pkg.WriteBoolean(info.IsGotRing);
            SendTCP(pkg);
            return pkg;
        }


        #endregion

    }
}
