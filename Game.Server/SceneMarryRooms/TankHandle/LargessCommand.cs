﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets;
using SqlDataProvider.Data;
using Game.Server.Managers;
using Game.Server.SceneMarryRooms;
using Game.Server.SceneMarryRooms.TankHandle;
using Bussiness;
using Game.Server.Statics;

namespace Game.Server.SceneMarryRooms.TankHandle
{
    [MarryCommandAttbute((byte)MarryCmdType.LARGESS)]
    public class LargessCommand : IMarryCommandHandler
    {
        public bool HandleCommand(TankMarryLogicProcessor process, GamePlayer player, GSPacketIn packet)
        {
            if (player.CurrentMarryRoom == null /*|| player.CurrentMarryRoom.RoomState != eRoomState.FREE*/)
            {
                return false;
            }

            //if (player.PlayerCharacter.ID == player.CurrentMarryRoom.Info.GroomID || player.PlayerCharacter.ID == player.CurrentMarryRoom.Info.BrideID)
            //{
            //    return false;
            //}

            int num = packet.ReadInt();

            if (num > 0)
            {
                if (player.PlayerCharacter.Money >= num)
                {
                    //player.SetMoney(-num, MoneyAddType.Marry);
                    player.RemoveMoney(num);
                    LogMgr.LogMoneyAdd(LogMoneyType.Marry, LogMoneyType.Marry_Gift, player.PlayerCharacter.ID, num, player.PlayerCharacter.Money, 0, 0, 0,0, "", "", "");
                }
                else
                {
                    player.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("UserFirecrackersCommand.MoneyNotEnough"));
                    return false;
                }


                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    string content = LanguageMgr.GetTranslation("LargessCommand.Content",player.PlayerCharacter.NickName,num / 2);
                    string title = LanguageMgr.GetTranslation("LargessCommand.Title",player.PlayerCharacter.NickName);

                    MailInfo mail1 = new MailInfo();
                    mail1.Annex1 = "";
                    mail1.Content = content;
                    mail1.Gold = 0;
                    mail1.IsExist = true;
                    mail1.Money = num / 2;
                    mail1.Receiver = player.CurrentMarryRoom.Info.BrideName;
                    mail1.ReceiverID = player.CurrentMarryRoom.Info.BrideID;
                    mail1.Sender = LanguageMgr.GetTranslation("LargessCommand.Sender");
                    mail1.SenderID = 0;
                    mail1.Title = title;
                    mail1.Type = (int)eMailType.Marry;
                    pb.SendMail(mail1);

                    player.Out.SendMailResponse(mail1.ReceiverID, eMailRespose.Receiver);

                    MailInfo mail2 = new MailInfo();
                    mail2.Annex1 = "";
                    mail2.Content = content;
                    mail2.Gold = 0;
                    mail2.IsExist = true;
                    mail2.Money = num / 2;
                    mail2.Receiver = player.CurrentMarryRoom.Info.GroomName;
                    mail2.ReceiverID = player.CurrentMarryRoom.Info.GroomID;
                    mail2.Sender = LanguageMgr.GetTranslation("LargessCommand.Sender");
                    mail2.SenderID = 0;
                    mail2.Title = title;
                    mail2.Type = (int)eMailType.Marry;
                    pb.SendMail(mail2);

                    player.Out.SendMailResponse(mail2.ReceiverID, eMailRespose.Receiver);
                }

                player.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("LargessCommand.Succeed"));
                GSPacketIn msg = player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("LargessCommand.Notice", player.PlayerCharacter.NickName, num));
                player.CurrentMarryRoom.SendToPlayerExceptSelf(msg,player);

                return true;
            }
      
            return false;
        }
        
    }
}
