﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Base.Packets;
using Bussiness;
using Game.Logic;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.GAME_PAIRUP_CANCEL, "撮合取消")]
    public class GamePairUpCancelHandler:IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client.Player.CurrentRoom != null && client.Player.CurrentRoom.BattleServer != null)
            {
                client.Player.CurrentRoom.BattleServer.RemoveRoom(client.Player.CurrentRoom);
            }
            if (client.Player != client.Player.CurrentRoom.Host)
            {
                client.Player.CurrentRoom.Host.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("Game.Server.SceneGames.PairUp.Failed"));
                RoomMgr.UpdatePlayerState(client.Player, 0);
            }
            else
            {
                RoomMgr.UpdatePlayerState(client.Player, 2);
            }

            return 0;
        }
    }
}
