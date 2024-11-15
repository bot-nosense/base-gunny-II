﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlDataProvider.Data;
using SqlDataProvider.BaseClass;
using System.Data;
using System.Data.SqlClient;
using DAL;
using log4net;
using System.Reflection;
using log4net.Util;
using Bussiness.Managers;

namespace Bussiness
{
    public class ProduceBussiness : BaseBussiness
    {
        public AchievementInfo[] GetALlAchievement()
        {
            List<AchievementInfo> list = new List<AchievementInfo>();
            SqlDataReader resultDataReader = null;
            try
            {
                base.db.GetReader(ref resultDataReader, "SP_Achievement_All");
                while (resultDataReader.Read())
                {
                    list.Add(InitAchievement(resultDataReader));
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetALlAchievement:", exception);
                }
            }
            finally
            {
                if (!((resultDataReader == null) || resultDataReader.IsClosed))
                {
                    resultDataReader.Close();
                }
            }
            return list.ToArray();
        }
        public AchievementConditionInfo[] GetALlAchievementCondition()
        {
            List<AchievementConditionInfo> list = new List<AchievementConditionInfo>();
            SqlDataReader resultDataReader = null;
            try
            {
                base.db.GetReader(ref resultDataReader, "SP_Achievement_Condition_All");
                while (resultDataReader.Read())
                {
                    list.Add(InitAchievementCondition(resultDataReader));
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetALlAchievementCondition:", exception);
                }
            }
            finally
            {
                if (!((resultDataReader == null) || resultDataReader.IsClosed))
                {
                    resultDataReader.Close();
                }
            }
            return list.ToArray();
        }
        public AchievementDataInfo[] GetAllAchievementData(int userID)
        {
            List<AchievementDataInfo> list = new List<AchievementDataInfo>();
            SqlDataReader resultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@UserID", userID) };
                base.db.GetReader(ref resultDataReader, "SP_Achievement_Data_All", sqlParameters);
                while (resultDataReader.Read())
                {
                    list.Add(InitAchievementData(resultDataReader));
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetAllAchievementData", exception);
                }
            }
            finally
            {
                if (!((resultDataReader == null) || resultDataReader.IsClosed))
                {
                    resultDataReader.Close();
                }
            }
            return list.ToArray();
        }
        public AchievementRewardInfo[] GetALlAchievementReward()
        {
            List<AchievementRewardInfo> list = new List<AchievementRewardInfo>();
            SqlDataReader resultDataReader = null;
            try
            {
                base.db.GetReader(ref resultDataReader, "SP_Achievement_Reward_All");
                while (resultDataReader.Read())
                {
                    list.Add(InitAchievementReward(resultDataReader));
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetALlAchievementReward", exception);
                }
            }
            finally
            {
                if (!((resultDataReader == null) || resultDataReader.IsClosed))
                {
                    resultDataReader.Close();
                }
            }
            return list.ToArray();
        }
        public ActiveAwardInfo[] GetAllActiveAwardInfo()
        {
            List<ActiveAwardInfo> list = new List<ActiveAwardInfo>();
            SqlDataReader resultDataReader = null;
            try
            {
                base.db.GetReader(ref resultDataReader, "SP_Active_Award");
                while (resultDataReader.Read())
                {
                    ActiveAwardInfo item = new ActiveAwardInfo
                    {
                        ID = (int)resultDataReader["ID"],
                        ActiveID = (int)resultDataReader["ActiveID"],
                        AgilityCompose = (int)resultDataReader["AgilityCompose"],
                        AttackCompose = (int)resultDataReader["AttackCompose"],
                        Count = (int)resultDataReader["Count"],
                        DefendCompose = (int)resultDataReader["DefendCompose"],
                        Gold = (int)resultDataReader["Gold"],
                        ItemID = (int)resultDataReader["ItemID"],
                        LuckCompose = (int)resultDataReader["LuckCompose"],
                        Mark = (int)resultDataReader["Mark"],
                        Money = (int)resultDataReader["Money"],
                        Sex = (int)resultDataReader["Sex"],
                        StrengthenLevel = (int)resultDataReader["StrengthenLevel"],
                        ValidDate = (int)resultDataReader["ValidDate"],
                        GiftToken = (int)resultDataReader["GiftToken"]
                    };
                    list.Add(item);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetAllActiveAwardInfo", exception);
                }
            }
            finally
            {
                if (!((resultDataReader == null) || resultDataReader.IsClosed))
                {
                    resultDataReader.Close();
                }
            }
            return list.ToArray();
        }
        public ActiveConditionInfo[] GetAllActiveConditionInfo()
        {
            List<ActiveConditionInfo> list = new List<ActiveConditionInfo>();
            SqlDataReader resultDataReader = null;
            try
            {
                base.db.GetReader(ref resultDataReader, "SP_Active_Condition");
                while (resultDataReader.Read())
                {
                    ActiveConditionInfo item = new ActiveConditionInfo
                    {
                        ID = (int)resultDataReader["ID"],
                        ActiveID = (int)resultDataReader["ActiveID"],
                        Conditiontype = (int)resultDataReader["Conditiontype"],
                        Condition = (int)resultDataReader["Condition"],
                        LimitGrade = (resultDataReader["LimitGrade"].ToString() == null) ? "" : resultDataReader["LimitGrade"].ToString(),
                        AwardId = (resultDataReader["AwardId"].ToString() == null) ? "" : resultDataReader["AwardId"].ToString(),
                        IsMult = (bool)resultDataReader["IsMult"],
                        StartTime = (DateTime)resultDataReader["StartTime"],
                        EndTime = (DateTime)resultDataReader["EndTime"]
                    };
                    list.Add(item);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetAllActiveConditionInfo", exception);
                }
            }
            finally
            {
                if (!((resultDataReader == null) || resultDataReader.IsClosed))
                {
                    resultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public List<BigBugleInfo> GetAllAreaBigBugleRecord()
        {
            SqlDataReader resultDataReader = null;
            List<BigBugleInfo> list = new List<BigBugleInfo>();
            try
            {
                base.db.GetReader(ref resultDataReader, "SP_Get_AreaBigBugle_Record");
                while (resultDataReader.Read())
                {
                    BigBugleInfo item = new BigBugleInfo
                    {
                        ID = (int)resultDataReader["ID"],
                        UserID = (int)resultDataReader["UserID"],
                        AreaID = (int)resultDataReader["AreaID"],
                        NickName = (resultDataReader["NickName"] == null) ? "" : resultDataReader["NickName"].ToString(),
                        Message = (resultDataReader["Message"] == null) ? "" : resultDataReader["Message"].ToString(),
                        State = (bool)resultDataReader["State"],
                        IP = (resultDataReader["IP"] == null) ? "" : resultDataReader["IP"].ToString()
                    };
                    list.Add(item);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetAllAreaBigBugleRecord", exception);
                }
            }
            finally
            {
                if (!((resultDataReader == null) || resultDataReader.IsClosed))
                {
                    resultDataReader.Close();
                }
            }
            return list;
        }
        public AchievementInfo InitAchievement(SqlDataReader reader)
        {
            return new AchievementInfo { 
                ID = (int)reader["ID"], 
                PlaceID = (int)reader["PlaceID"], 
                Title = (reader["Title"] == null) ? "" : reader["Title"].ToString(),
                Detail = (reader["Detail"] == null) ? "" : reader["Detail"].ToString(), 
                NeedMinLevel = (int)reader["NeedMinLevel"], 
                NeedMaxLevel = (int)reader["NeedMaxLevel"], 
                PreAchievementID = (reader["PreAchievementID"] == null) ? "" : reader["PreAchievementID"].ToString(), 
                IsOther = (int)reader["IsOther"], 
                AchievementType = (int)reader["AchievementType"], 
                CanHide = (bool)reader["CanHide"], 
                StartDate = (DateTime)reader["StartDate"], 
                EndDate = (DateTime)reader["EndDate"], 
                AchievementPoint = (int)reader["AchievementPoint"], 
                IsActive = (int)reader["IsActive"], 
                PicID = (int)reader["PicID"], 
                IsShare = (bool)reader["IsShare"] };
        }
        public AchievementConditionInfo InitAchievementCondition(SqlDataReader reader)
        {
            return new AchievementConditionInfo { 
                AchievementID = (int)reader["AchievementID"], 
                CondictionID = (int)reader["CondictionID"], 
                CondictionType = (int)reader["CondictionType"], 
                Condiction_Para1 = (reader["Condiction_Para1"] == null) ? "" : reader["Condiction_Para1"].ToString(), 
                Condiction_Para2 = (int)reader["Condiction_Para2"] };
        }
        public AchievementDataInfo InitAchievementData(SqlDataReader reader)
        {
            return new AchievementDataInfo { 
                UserID = (int)reader["UserID"], 
                AchievementID = (int)reader["AchievementID"], 
                IsComplete = (bool)reader["IsComplete"], 
                CompletedDate = (DateTime)reader["CompletedDate"] };
        }
        public AchievementRewardInfo InitAchievementReward(SqlDataReader reader)
        {
            return new AchievementRewardInfo { 
                AchievementID = (int)reader["AchievementID"], 
                RewardType = (int)reader["RewardType"], 
                RewardPara = (reader["RewardPara"] == null) ? "" : reader["RewardPara"].ToString(), 
                RewardValueId = (int)reader["RewardValueId"], 
                RewardCount = (int)reader["RewardCount"] };
        }
        public ItemRecordTypeInfo[] GetAllItemRecordType()
        {
            List<ItemRecordTypeInfo> list = new List<ItemRecordTypeInfo>();
            SqlDataReader resultDataReader = null;
            try
            {
                base.db.GetReader(ref resultDataReader, "SP_Item_Record_Type_All");
                while (resultDataReader.Read())
                {
                    list.Add(this.InitItemRecordType(resultDataReader));
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetAllItemRecordType:", exception);
                }
            }
            finally
            {
                if (!((resultDataReader == null) || resultDataReader.IsClosed))
                {
                    resultDataReader.Close();
                }
            }
            return list.ToArray();
        }
        public ItemRecordTypeInfo InitItemRecordType(SqlDataReader reader)
        {
            return new ItemRecordTypeInfo { 
                RecordID = (int)reader["RecordID"], 
                Name = (reader["Name"] == null) ? "" : reader["Name"].ToString(), 
                Description = (reader["Description"] == null) ? "" : reader["Description"].ToString() };
        }


        #region WindPic
        public VaneInfo InitVaneInfo(SqlDataReader reader)
        {
            VaneInfo info = new VaneInfo();
            info.ID = (int)reader["ID"];
            info.VaneId1 = (int)reader["VaneId1"];
            info.Param1 = (byte[])reader["Param1"];
            info.VaneId2 = (int)reader["VaneId2"];
            info.Param2 = (byte[])reader["Param2"];
            info.VaneId3 = (int)reader["VaneId3"];
            info.Param3 = (byte[])reader["Param3"];
            return info;
        }
        public VaneInfo[] GetAllVane()
        {
            List<VaneInfo> infos = new List<VaneInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Get_All_Vane");
                while (reader.Read())
                {
                    VaneInfo info = new VaneInfo();
                    info.ID = (int)reader["ID"];
                    info.VaneId1 = (int)reader["VaneId1"];
                    info.Param1 = (byte[])reader["Param1"];
                    info.VaneId2 = (int)reader["VaneId2"];
                    info.Param2 = (byte[])reader["Param2"];
                    info.VaneId3 = (int)reader["VaneId3"];
                    info.Param3 = (byte[])reader["Param3"];
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("ByteArray", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }
        public VaneInfo GetSingleVane(int VaneID)
        {
            SqlDataReader reader = null;
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@ID", SqlDbType.Int, 4);
                para[0].Value = VaneID;
                db.GetReader(ref reader, "SP_Single_Vane_ByID", para);
                while (reader.Read())
                {
                    return InitVaneInfo(reader);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }

            return null;
        }
        #endregion
        #region ItemTemplateInfo
        public ItemTemplateInfo[] GetAllGoodsASC()
        {
            List<ItemTemplateInfo> infos = new List<ItemTemplateInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Items_All_ASC");
                while (reader.Read())
                {
                    infos.Add(InitItemTemplateInfo(reader));
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }

            return infos.ToArray();
        }

        public ItemTemplateInfo[] GetAllGoods()
        {
            List<ItemTemplateInfo> infos = new List<ItemTemplateInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Items_All");
                while (reader.Read())
                {
                    infos.Add(InitItemTemplateInfo(reader));
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }

            return infos.ToArray();
        }
        public ShopGoodsShowListInfo InitShopGoodsShowListInfo(SqlDataReader reader)
        {
            ShopGoodsShowListInfo info = new ShopGoodsShowListInfo();
            info.Type = (int)reader["Type"];
            info.ShopId = (int)reader["ShopId"];
            return info;
        }
        public ShopGoodsShowListInfo[] GetAllShopGoodsShowList()
        {
            List<ShopGoodsShowListInfo> infos = new List<ShopGoodsShowListInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_ShopGoodsShowList_All");
                while (reader.Read())
                {
                    infos.Add(InitShopGoodsShowListInfo(reader));
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }

            return infos.ToArray();
        }
        public ItemBoxInfo[] GetSingleItemsBox(int DataID)
        {
            List<ItemBoxInfo> infos = new List<ItemBoxInfo>();
            SqlDataReader reader = null;
            try
            {

                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@ID", SqlDbType.Int, 4);
                para[0].Value = DataID;
                db.GetReader(ref reader, "SP_ItemsBox_Single", para);
                while (reader.Read())
                {
                    infos.Add(InitItemBoxInfo(reader));
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }

        public ItemTemplateInfo GetSingleGoods(int goodsID)
        {
            SqlDataReader reader = null;
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@ID", SqlDbType.Int, 4);
                para[0].Value = goodsID;
                db.GetReader(ref reader, "SP_Items_Single", para);
                while (reader.Read())
                {
                    return InitItemTemplateInfo(reader);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }

            return null;
        }

        public ItemTemplateInfo[] GetSingleCategory(int CategoryID)
        {
            List<ItemTemplateInfo> infos = new List<ItemTemplateInfo>();
            SqlDataReader reader = null;
            try
            {

                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
                para[0].Value = CategoryID;
                db.GetReader(ref reader, "SP_Items_Category_Single", para);
                while (reader.Read())
                {
                    infos.Add(InitItemTemplateInfo(reader));
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }
        public ItemTemplateInfo[] GetFusionType()
        {
            List<ItemTemplateInfo> infos = new List<ItemTemplateInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Items_FusionType");
                while (reader.Read())
                {
                    infos.Add(InitItemTemplateInfo(reader));
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }

            return infos.ToArray();
        }

        public ItemTemplateInfo InitItemTemplateInfo(SqlDataReader reader)
        {
            ItemTemplateInfo info = new ItemTemplateInfo();
            info.AddTime = reader["AddTime"].ToString();
            info.Agility = (int)reader["Agility"];
            info.Attack = (int)reader["Attack"];
            info.CanDelete = (bool)reader["CanDelete"];
            info.CanDrop = (bool)reader["CanDrop"];
            info.CanEquip = (bool)reader["CanEquip"];
            info.CanUse = (bool)reader["CanUse"];
            info.CategoryID = (int)reader["CategoryID"];
            info.Colors = reader["Colors"].ToString();
            info.Defence = (int)reader["Defence"];
            info.Description = reader["Description"].ToString();
            info.Level = (int)reader["Level"];
            info.Luck = (int)reader["Luck"];
            info.MaxCount = (int)reader["MaxCount"];
            info.Name = reader["Name"].ToString();
            info.NeedSex = (int)reader["NeedSex"];
            info.Pic = reader["Pic"].ToString();
            info.Data = reader["Data"] == null ? "" : reader["Data"].ToString();
            info.Property1 = (int)reader["Property1"];
            info.Property2 = (int)reader["Property2"];
            info.Property3 = (int)reader["Property3"];
            info.Property4 = (int)reader["Property4"];
            info.Property5 = (int)reader["Property5"];
            info.Property6 = (int)reader["Property6"];
            info.Property7 = (int)reader["Property7"];
            info.Property8 = (int)reader["Property8"];
            info.Quality = (int)reader["Quality"];
            info.Script = reader["Script"].ToString();
            info.TemplateID = (int)reader["TemplateID"];
            info.CanCompose = (bool)reader["CanCompose"];
            info.CanStrengthen = (bool)reader["CanStrengthen"];
            info.NeedLevel = (int)reader["NeedLevel"];
            info.BindType = (int)reader["BindType"];
            info.FusionType = (int)reader["FusionType"];
            info.FusionRate = (int)reader["FusionRate"];
            info.FusionNeedRate = (int)reader["FusionNeedRate"];
            info.Hole = reader["Hole"] == null ? "" : reader["Hole"].ToString();
            info.RefineryLevel = (int)reader["RefineryLevel"];
			info.ReclaimValue = (int)reader["ReclaimValue"];
            info.ReclaimType = (int)reader["ReclaimType"];
            info.CanRecycle = (int)reader["CanRecycle"];
            info.IsDirty = false;
            return info;
        }
        public ItemBoxInfo[] GetItemBoxInfos()
        {
            List<ItemBoxInfo> infos = new List<ItemBoxInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_ItemsBox_All");
                while (reader.Read())
                {
                    infos.Add(InitItemBoxInfo(reader));
                }
            }            
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init@Shop_Goods_Box：" + e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }

        public ItemBoxInfo InitItemBoxInfo(SqlDataReader reader)
        {
            ItemBoxInfo info = new ItemBoxInfo();
            info.Id = (int)reader["id"];
            info.DataId = (int)reader["DataId"];
            info.TemplateId = (int)reader["TemplateId"];
            info.IsSelect = (bool)reader["IsSelect"];
            info.IsBind = (bool)reader["IsBind"];
            info.ItemValid = (int)reader["ItemValid"];
            info.ItemCount = (int)reader["ItemCount"];
            info.StrengthenLevel = (int)reader["StrengthenLevel"];
            info.AttackCompose = (int)reader["AttackCompose"];
            info.DefendCompose = (int)reader["DefendCompose"];
            info.AgilityCompose = (int)reader["AgilityCompose"];
            info.LuckCompose = (int)reader["LuckCompose"];
            info.Random = (int)reader["Random"];
            info.IsTips = (int)reader["IsTips"];
            info.IsLogs = (bool)reader["IsLogs"];
            return info;
        }
        #endregion
        public bool UpdatePlayerInfoHistory(PlayerInfoHistory info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] { 
                    new SqlParameter("@UserID", info.UserID), 
                    new SqlParameter("@LastQuestsTime", info.LastQuestsTime), 
                    new SqlParameter("@LastTreasureTime", info.LastTreasureTime), 
                    new SqlParameter("@OutPut", SqlDbType.Int) };
                sqlParameters[3].Direction = ParameterDirection.Output;
                db.RunProcedure("SP_User_Update_History", sqlParameters);
                flag = ((int)sqlParameters[6].Value) == 1;
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("User_Update_BoxProgression", exception);
                }
            }
            return flag;
        }
        public CategoryInfo[] GetAllCategory()
        {
            List<CategoryInfo> infos = new List<CategoryInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Items_Category_All");
                while (reader.Read())
                {
                    CategoryInfo info = new CategoryInfo();
                    info.ID = (int)reader["ID"];
                    info.Name = reader["Name"] == null ? "" : reader["Name"].ToString();
                    info.Place = (int)reader["Place"];
                    info.Remark = reader["Remark"] == null ? "" : reader["Remark"].ToString();
                    infos.Add(info);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }

        public PropInfo[] GetAllProp()
        {
            List<PropInfo> infos = new List<PropInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Prop_All");
                while (reader.Read())
                {
                    PropInfo info = new PropInfo();
                    info.AffectArea = (int)reader["AffectArea"];
                    info.AffectTimes = (int)reader["AffectTimes"];
                    info.AttackTimes = (int)reader["AttackTimes"];
                    info.BoutTimes = (int)reader["BoutTimes"];
                    info.BuyGold = (int)reader["BuyGold"];
                    info.BuyMoney = (int)reader["BuyMoney"];
                    info.Category = (int)reader["Category"];
                    info.Delay = (int)reader["Delay"];
                    info.Description = reader["Description"].ToString();
                    info.Icon = reader["Icon"].ToString();
                    info.ID = (int)reader["ID"];
                    info.Name = reader["Name"].ToString();
                    info.Parameter = (int)reader["Parameter"];
                    info.Pic = reader["Pic"].ToString();
                    info.Property1 = (int)reader["Property1"];
                    info.Property2 = (int)reader["Property2"];
                    info.Property3 = (int)reader["Property3"];
                    info.Random = (int)reader["Random"];
                    info.Script = reader["Script"].ToString();
                    infos.Add(info);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }

        public BallInfo[] GetAllBall()
        {
            List<BallInfo> infos = new List<BallInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Ball_All");
                while (reader.Read())
                {
                    BallInfo info = new BallInfo();
                    info.Amount = (int)reader["Amount"];
                    info.ID = (int)reader["ID"];
                    info.Name = reader["Name"].ToString();
                    info.Crater = reader["Crater"] == null ? "" : reader["Crater"].ToString();
                    info.Power = (double)reader["Power"];
                    info.Radii = (int)reader["Radii"];
                    info.AttackResponse = (int)reader["AttackResponse"];
                    info.BombPartical = reader["BombPartical"].ToString();
                    info.FlyingPartical = reader["FlyingPartical"].ToString();
                    info.IsSpin = (bool)reader["IsSpin"];
                    info.Mass = (int)reader["Mass"];
                    info.SpinV = (int)reader["SpinV"];
                    info.SpinVA = (double)reader["SpinVA"];
                    info.Wind = (int)reader["Wind"];
                    info.DragIndex = (int)reader["DragIndex"];
                    info.Weight = (int)reader["Weight"];
                    info.Shake = (bool)reader["Shake"];
                    info.Delay = (int)reader["Delay"];
                    info.ShootSound = reader["ShootSound"] == null ? "" : reader["ShootSound"].ToString();
                    info.BombSound = reader["BombSound"] == null ? "" : reader["BombSound"].ToString();
                    info.ActionType = (int)reader["ActionType"];
                    info.HasTunnel = (bool)reader["HasTunnel"];
                    infos.Add(info);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }
        public BallConfigInfo[] GetAllBallConfig()
        {
            List<BallConfigInfo> infos = new List<BallConfigInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "[SP_Ball_Config_All]");
                while (reader.Read())
                {
                    BallConfigInfo info = new BallConfigInfo();
                    info.Common = (int)reader["Common"];
                    info.TemplateID = (int)reader["TemplateID"];
                    info.CommonAddWound = (int)reader["CommonAddWound"];
                    info.CommonMultiBall = (int)reader["CommonMultiBall"];
                    info.Special = (int)reader["Special"];
                    info.SpecialII = (int)reader["SpecialII"];

                 
                    infos.Add(info);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }
        public ShopItemInfo[] GetALllShop()
        {
            List<ShopItemInfo> infos = new List<ShopItemInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Shop_All");
                while (reader.Read())
                {
                    ShopItemInfo info = new ShopItemInfo();
                    info.ID = int.Parse(reader["ID"].ToString());
                    info.ShopID = int.Parse(reader["ShopID"].ToString());
                    info.GroupID = int.Parse(reader["GroupID"].ToString());
                    info.TemplateID = int.Parse(reader["TemplateID"].ToString());
                    info.BuyType = int.Parse(reader["BuyType"].ToString());
                    info.Sort = int.Parse(reader["Sort"].ToString());
                    info.IsVouch = int.Parse(reader["IsVouch"].ToString());;
                    info.Label = int.Parse(reader["Label"].ToString());
                    info.Beat = decimal.Parse(reader["Beat"].ToString());
                    info.AUnit = int.Parse(reader["AUnit"].ToString());
                    info.APrice1 = int.Parse(reader["APrice1"].ToString());
                    info.AValue1 = int.Parse(reader["AValue1"].ToString());
                    info.APrice2 = int.Parse(reader["APrice2"].ToString());
                    info.AValue2 = int.Parse(reader["AValue2"].ToString());
                    info.APrice3 = int.Parse(reader["APrice3"].ToString());
                    info.AValue3 = int.Parse(reader["AValue3"].ToString());

                    info.BUnit = int.Parse(reader["BUnit"].ToString());
                    info.BPrice1 = int.Parse(reader["BPrice1"].ToString());
                    info.BValue1 = int.Parse(reader["BValue1"].ToString());
                    info.BPrice2 = int.Parse(reader["BPrice2"].ToString());
                    info.BValue2 = int.Parse(reader["BValue2"].ToString());
                    info.BPrice3 = int.Parse(reader["BPrice3"].ToString());
                    info.BValue3 = int.Parse(reader["BValue3"].ToString());

                    info.CUnit = int.Parse(reader["CUnit"].ToString());
                    info.CPrice1 = int.Parse(reader["CPrice1"].ToString());
                    info.CValue1 = int.Parse(reader["CValue1"].ToString());
                    info.CPrice2 = int.Parse(reader["CPrice2"].ToString());
                    info.CValue2 = int.Parse(reader["CValue2"].ToString());
                    info.CPrice3 = int.Parse(reader["CPrice3"].ToString());
                    info.CValue3 = int.Parse(reader["CValue3"].ToString());
                    info.IsContinue = bool.Parse(reader["IsContinue"].ToString());
                    info.IsCheap = bool.Parse(reader["IsCheap"].ToString());
                    info.LimitCount = int.Parse(reader["LimitCount"].ToString());
                    info.StartDate = DateTime.Parse(reader["StartDate"].ToString());
                    info.EndDate = DateTime.Parse(reader["EndDate"].ToString());
                    infos.Add(info);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }
        public FusionInfo[] GetAllFusionDesc()
        {
            List<FusionInfo> infos = new List<FusionInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Fusion_All_Desc");
                while (reader.Read())
                {
                    FusionInfo info = new FusionInfo();
                    info.FusionID = (int)reader["FusionID"];
                    info.Item1 = (int)reader["Item1"];
                    info.Item2 = (int)reader["Item2"];
                    info.Item3 = (int)reader["Item3"];
                    info.Item4 = (int)reader["Item4"];
                    info.Formula = (int)reader["Formula"];
                    info.Reward = (int)reader["Reward"];
                    infos.Add(info);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("GetAllFusion", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }
        public FusionInfo[] GetAllFusion()
        {
            List<FusionInfo> infos = new List<FusionInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Fusion_All");
                while (reader.Read())
                {
                    FusionInfo info = new FusionInfo();
                    info.FusionID = (int)reader["FusionID"];
                    info.Item1 = (int)reader["Item1"];
                    info.Item2 = (int)reader["Item2"];
                    info.Item3 = (int)reader["Item3"];
                    info.Item4 = (int)reader["Item4"];
                    info.Formula = (int)reader["Formula"];
                    info.Reward = (int)reader["Reward"];
                    infos.Add(info);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("GetAllFusion", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }

        public StrengthenInfo[] GetAllStrengthen()
        {
            List<StrengthenInfo> infos = new List<StrengthenInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Item_Strengthen_All");
                while (reader.Read())
                {
                    StrengthenInfo info = new StrengthenInfo();
                    info.StrengthenLevel = (int)reader["StrengthenLevel"];
                    info.Random = (int)reader["Random"];
                    info.Rock = (int)reader["Rock"];
                    info.Rock1 = (int)reader["Rock1"];
                    info.Rock2 = (int)reader["Rock2"];
                    info.Rock3 = (int)reader["Rock3"];
                    info.StoneLevelMin = (int)reader["StoneLevelMin"];
                    infos.Add(info);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("GetAllStrengthen", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }

        public StrengthenGoodsInfo[] GetAllStrengthenGoodsInfo()
        {
            List<StrengthenGoodsInfo> infos = new List<StrengthenGoodsInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Item_StrengthenGoodsInfo_All");
                while (reader.Read())
                {
                    StrengthenGoodsInfo info = new StrengthenGoodsInfo();
                    info.ID = (int)reader["ID"];
                    info.Level = (int)reader["Level"];
                    info.CurrentEquip = (int)reader["CurrentEquip"];
                    info.GainEquip = (int)reader["GainEquip"];
                    info.OrginEquip = (int)reader["OrginEquip"];
                    infos.Add(info);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("GetAllStrengthenGoodsInfo", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }


        /// <summary>
        /// 炼化后强化
        /// </summary>
        /// <returns></returns>
        public StrengthenInfo[] GetAllRefineryStrengthen()
        {

            List<StrengthenInfo> infos = new List<StrengthenInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Item_Refinery_Strengthen_All");
                while (reader.Read())
                {
                    StrengthenInfo info = new StrengthenInfo();
                    info.StrengthenLevel = (int)reader["StrengthenLevel"];
                    info.Rock = (int)reader["Rock"];
                    infos.Add(info);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("GetAllRefineryStrengthen", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();

        }
        ///<summary>
        ///炼化公式
        ///</summary>
        public List<RefineryInfo> GetAllRefineryInfo()
        {
            List<RefineryInfo> infos = new List<RefineryInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Item_Refinery_All");
                while (reader.Read())
                {
                    RefineryInfo info = new RefineryInfo();
                    info.RefineryID = (int)reader["RefineryID"];

                    info.m_Equip.Add((int)reader["Equip1"]);
                    info.m_Equip.Add((int)reader["Equip2"]);
                    info.m_Equip.Add((int)reader["Equip3"]);
                    info.m_Equip.Add((int)reader["Equip4"]);
                    info.Item1 = (int)reader["Item1"];
                    info.Item2 = (int)reader["Item2"];
                    info.Item3 = (int)reader["Item3"];
                    info.Item1Count = (int)reader["Item1Count"];
                    info.Item2Count = (int)reader["Item2Count"];
                    info.Item3Count = (int)reader["Item3Count"];
                    info.m_Reward.Add((int)reader["Material1"]);
                    info.m_Reward.Add((int)reader["Operate1"]);
                    info.m_Reward.Add((int)reader["Reward1"]);
                    info.m_Reward.Add((int)reader["Material2"]);
                    info.m_Reward.Add((int)reader["Operate2"]);
                    info.m_Reward.Add((int)reader["Reward2"]);
                    info.m_Reward.Add((int)reader["Material3"]);
                    info.m_Reward.Add((int)reader["Operate3"]);
                    info.m_Reward.Add((int)reader["Reward3"]);
                    info.m_Reward.Add((int)reader["Material4"]);
                    info.m_Reward.Add((int)reader["Operate4"]);
                    info.m_Reward.Add((int)reader["Reward4"]);
                    infos.Add(info);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("GetAllRefineryInfo", e);
            }

            finally
            {

                if (reader != null && reader.IsClosed)
                    reader.Close();
            }

            return infos;
        }


        #region 任务信息

        /// <summary>
        /// 任务模板数据集
        /// </summary>
        /// <returns></returns>
        public QuestInfo[] GetALlQuest()
        {
            List<QuestInfo> infos = new List<QuestInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Quest_All");
                while (reader.Read())
                {
                    infos.Add(InitQuest(reader));
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }

        /// <summary>
        /// 任务奖励数据集
        /// </summary>
        /// <returns></returns>
        public QuestAwardInfo[] GetAllQuestGoods()
        {
            List<QuestAwardInfo> infos = new List<QuestAwardInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Quest_Goods_All");
                while (reader.Read())
                {
                    infos.Add(InitQuestGoods(reader));
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return infos.ToArray();
        }

        /// <summary>
        /// 任务条件数据集
        /// </summary>
        /// <returns></returns>
        public QuestConditionInfo[] GetAllQuestCondiction()
        {
            List<QuestConditionInfo> infos = new List<QuestConditionInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Quest_Condiction_All");
                while (reader.Read())
                {
                    infos.Add(InitQuestCondiction(reader));
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return infos.ToArray();
        }
        public QuestRateInfo[] GetAllQuestRate()
        {
            List<QuestRateInfo> infos = new List<QuestRateInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Quest_Rate_All");
                while (reader.Read())
                {
                    infos.Add(InitQuestRate(reader));
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return infos.ToArray();
        }

        /// <summary>
        /// 任务模板一条记录
        /// </summary>
        /// <param name="questID"></param>
        /// <returns></returns>
        public QuestInfo GetSingleQuest(int questID)
        {
            SqlDataReader reader = null;
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@QuestID", SqlDbType.Int, 4);
                para[0].Value = questID;
                db.GetReader(ref reader, "SP_Quest_Single", para);
                while (reader.Read())
                {
                    return InitQuest(reader);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return null;
        }

        /// <summary>
        /// 从任务模板表中读取数据
        /// </summary>
        /// <param name="reader">传入SqlDataReader</param>
        /// <returns>任务模板表</returns>
        public QuestInfo InitQuest(SqlDataReader reader)
        {
            QuestInfo info = new QuestInfo();
            info.ID = (int)reader["ID"];
            info.QuestID = (int)reader["QuestID"];
            info.Title = reader["Title"] == null ? "" : reader["Title"].ToString();
            info.Detail = reader["Detail"] == null ? "" : reader["Detail"].ToString();
            info.Objective = reader["Objective"] == null ? "" : reader["Objective"].ToString();
            info.NeedMinLevel = (int)reader["NeedMinLevel"];
            info.NeedMaxLevel = (int)reader["NeedMaxLevel"];
            info.PreQuestID = reader["PreQuestID"] == null ? "" : reader["PreQuestID"].ToString();
            info.NextQuestID = reader["NextQuestID"] == null ? "" : reader["NextQuestID"].ToString();
            info.IsOther = (int)reader["IsOther"];
            info.CanRepeat = (bool)reader["CanRepeat"];
            info.RepeatInterval = (int)reader["RepeatInterval"];
            info.RepeatMax = (int)reader["RepeatMax"];
            info.RewardGP = (int)reader["RewardGP"];
            info.RewardGold = (int)reader["RewardGold"];
            info.RewardBindMoney = (int)reader["RewardBindMoney"];
            info.RewardOffer = (int)reader["RewardOffer"];
            info.RewardRiches = (int)reader["RewardRiches"];
            info.RewardBuffID = (int)reader["RewardBuffID"];
            info.RewardBuffDate = (int)reader["RewardBuffDate"];
            info.RewardMoney = (int)reader["RewardMoney"];
            info.Rands = (decimal)reader["Rands"];
            info.RandDouble = (int)reader["RandDouble"];
            info.TimeMode = (bool)reader["TimeMode"];
            info.StartDate = (DateTime)reader["StartDate"];
            info.EndDate = (DateTime)reader["EndDate"];
            return info;
        }

        /// <summary>
        /// 从任务奖励表中读取数据
        /// </summary>
        /// <param name="reader">传入QuestGoods</param>
        /// <returns>任务奖励表</returns>
        public QuestAwardInfo InitQuestGoods(SqlDataReader reader)
        {
            QuestAwardInfo info = new QuestAwardInfo();
            info.QuestID = (int)reader["QuestID"];
            info.RewardItemID = (int)reader["RewardItemID"];
            info.IsSelect = (bool)reader["IsSelect"];
            info.RewardItemValid = (int)reader["RewardItemValid"];
            info.RewardItemCount1 = (int)reader["RewardItemCount1"];
            info.RewardItemCount2 = (int)reader["RewardItemCount2"];
            info.RewardItemCount3 = (int)reader["RewardItemCount3"];
            info.RewardItemCount4 = (int)reader["RewardItemCount4"];
            info.RewardItemCount5 = (int)reader["RewardItemCount5"];
            info.StrengthenLevel = (int)reader["StrengthenLevel"];
            info.AttackCompose = (int)reader["AttackCompose"];
            info.DefendCompose = (int)reader["DefendCompose"];
            info.AgilityCompose = (int)reader["AgilityCompose"];
            info.LuckCompose = (int)reader["LuckCompose"];
            info.IsCount = (bool)reader["IsCount"];
            info.IsBind = (bool)reader["IsBind"];
            return info;

        }

        /// <summary>
        /// 从任务条件表中读取数据
        /// </summary>
        /// <param name="reader">传入QuestGoods</param>
        /// <returns>任务条件表</returns>
        public QuestConditionInfo InitQuestCondiction(SqlDataReader reader)
        {
            QuestConditionInfo info = new QuestConditionInfo();
            info.QuestID = (int)reader["QuestID"];
            info.CondictionID = (int)reader["CondictionID"];
            info.CondictionTitle = reader["CondictionTitle"] == null ? "" : reader["CondictionTitle"].ToString();
            info.CondictionType = (int)reader["CondictionType"];
            info.Para1 = (int)reader["Para1"];
            info.Para2 = (int)reader["Para2"];
            info.isOpitional = (bool)reader["isOpitional"];
            return info;
        }
        public QuestRateInfo InitQuestRate(SqlDataReader reader)
        {
            QuestRateInfo info = new QuestRateInfo();
            info.BindMoneyRate = reader["BindMoneyRate"] == null ? "" : reader["BindMoneyRate"].ToString();
            info.ExpRate = reader["ExpRate"] == null ? "" : reader["ExpRate"].ToString();
            info.GoldRate = reader["GoldRate"] == null ? "" : reader["GoldRate"].ToString();
            info.ExploitRate = reader["ExploitRate"] == null ? "" : reader["ExploitRate"].ToString();
            info.CanOneKeyFinishTime = (int)reader["CanOneKeyFinishTime"];
            
            return info;
        }
        #endregion

        #region 掉落数据
        /// <summary>
        /// 获取所有掉落条件
        /// </summary>
        /// <returns></returns>
        public DropCondiction[] GetAllDropCondictions()
        {
            List<DropCondiction> infos = new List<DropCondiction>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Drop_Condiction_All");
                while (reader.Read())
                {
                    infos.Add(InitDropCondiction(reader));
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return infos.ToArray();
        }

        /// <summary>
        /// 获取所有掉落物品
        /// </summary>
        /// <returns></returns>
        public DropItem[] GetAllDropItems()
        {
            List<DropItem> infos = new List<DropItem>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Drop_Item_All");
                while (reader.Read())
                {
                    infos.Add(InitDropItem(reader));
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Init", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return infos.ToArray();
        }

        /// <summary>
        /// 掉落条件
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DropCondiction InitDropCondiction(SqlDataReader reader)
        {
            DropCondiction info = new DropCondiction();
            info.DropId = (int)reader["DropID"];
            info.CondictionType = (int)reader["CondictionType"];            
            info.Para1 = (string)reader["Para1"];
            info.Para2 = (string)reader["Para2"];
            return info;            
        }

        /// <summary>
        /// 掉落物品
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DropItem InitDropItem(SqlDataReader reader)
        {
            DropItem info = new DropItem();
            info.Id = (int)reader["Id"];
            info.DropId = (int)reader["DropId"];
            info.ItemId = (int)reader["ItemId"];
            info.ValueDate = (int)reader["ValueDate"];
            info.IsBind = (bool)reader["IsBind"];
            info.Random = (int)reader["Random"];
            info.BeginData = (int)reader["BeginData"];
            info.EndData = (int)reader["EndData"];
            return info;
        }
        #endregion

        public AASInfo[] GetAllAASInfo()
        {
            List<AASInfo> infos = new List<AASInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_AASInfo_All");

                while (reader.Read())
                {
                    AASInfo info = new AASInfo();
                    info.UserID = (int)reader["ID"];
                    info.Name = reader["Name"].ToString();
                    info.IDNumber = reader["IDNumber"].ToString();
                    info.State = (int)reader["State"];

                    infos.Add(info);
                }

            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("GetAllAASInfo", e);
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }

            return infos.ToArray();
        }

        public bool AddAASInfo(AASInfo info)
        {
            bool result = false;

            try
            {
                SqlParameter[] para = new SqlParameter[5];

                para[0] = new SqlParameter("@UserID", info.UserID);
                para[1] = new SqlParameter("@Name", info.Name);
                para[2] = new SqlParameter("@IDNumber", info.IDNumber);
                para[3] = new SqlParameter("@State", info.State);
                para[4] = new SqlParameter("@Result", System.Data.SqlDbType.Int);
                para[4].Direction = ParameterDirection.ReturnValue;

                db.RunProcedure("SP_ASSInfo_Add", para);
                result = (int)para[4].Value == 0;

            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("UpdateAASInfo", e);
                }
            }

            return result;
        }

        //public AASInfo GetASSInfoSingle(int UserID)
        //{
        //    SqlDataReader reader = null;

        //    try
        //    {
        //        SqlParameter[] para = new SqlParameter[2];
        //        para[0] = new SqlParameter("@UserID", UserID);
        //        para[1] = new SqlParameter("@Result",System.Data.SqlDbType.Int);
        //        para[1].Direction = ParameterDirection.ReturnValue;

        //        db.GetReader(ref reader,"SP_ASSInfo_Single",para);
        //        if ((int)para[1].Value == 0)
        //        { 
        //            if(reader.Read())
        //            {
        //                AASInfo info = new AASInfo();
        //                info.UserID = (int)reader["UserID"];
        //                info.Name = reader["Name"].ToString();
        //                info.IDNumber = reader["IDNumber"].ToString();
        //                info.State = (int)reader["State"];

        //                return info;
        //            }
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        if (log.IsErrorEnabled)
        //        {
        //            log.Error("GetASSInfoSingle",e); 
        //        }
        //    }

        //    return null;
        //}

        public string GetASSInfoSingle(int UserID)
        {
            SqlDataReader reader = null;
            try
            {

                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@UserID", UserID);
                // para[1] = new SqlParameter("@Result", System.Data.SqlDbType.Int);
                //para[1].Direction = ParameterDirection.ReturnValue;

                db.GetReader(ref reader, "SP_ASSInfo_Single", para);

                while (reader.Read())
                {
                    string ID = reader["IDNumber"].ToString();

                    return ID;
                }

            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("GetASSInfoSingle", e);
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }

            return "";
        }

        #region Award

        public DailyAwardInfo[] GetAllDailyAward()
        {
            List<DailyAwardInfo> infos = new List<DailyAwardInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Daily_Award_All");
                while (reader.Read())
                {
                    DailyAwardInfo info = new DailyAwardInfo();
                    info.Count = (int)reader["Count"];
                    info.ID = (int)reader["ID"];
                    info.IsBinds = (bool)reader["IsBinds"];
                    info.TemplateID = (int)reader["TemplateID"];
                    info.Type = (int)reader["Type"];
                    info.ValidDate = (int)reader["ValidDate"];
                    info.Sex = (int)reader["Sex"];
                    info.Remark = reader["Remark"] == null ? "" : reader["Remark"].ToString();
                    info.CountRemark = reader["CountRemark"] == null ? "" : reader["CountRemark"].ToString();
                    info.GetWay = (int)reader["GetWay"];
                    info.AwardDays = (int)reader["AwardDays"];
                    infos.Add(info);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("GetAllDaily", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }

        #endregion
        #region NPCInfo
        public NpcInfo[] GetAllNPCInfo()
        {
            List<NpcInfo> infos = new List<NpcInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_NPC_Info_All");
                while (reader.Read())
                {
                    NpcInfo info = new NpcInfo();
                    info.ID = (int)reader["ID"];
                    info.Name = reader["Name"] == null ? "" : reader["Name"].ToString();
                    info.Level = (int)reader["Level"];
                    info.Camp = (int)reader["Camp"];
                    info.Type = (int)reader["Type"];
                    info.Blood = (int)reader["Blood"];
                    info.X = (int)reader["X"];
                    info.Y = (int)reader["Y"];
                    info.Width = (int)reader["Width"];
                    info.Height = (int)reader["Height"];
                    info.MoveMin = (int)reader["MoveMin"];
                    info.MoveMax = (int)reader["MoveMax"];
                    info.BaseDamage = (int)reader["BaseDamage"];
                    info.BaseGuard = (int)reader["BaseGuard"];
                    info.Attack = (int)reader["Attack"];
                    info.Defence = (int)reader["Defence"];
                    info.Agility = (int)reader["Agility"];
                    info.Lucky = (int)reader["Lucky"];
                    info.ModelID = reader["ModelID"] == null ? "" : reader["ModelID"].ToString();
                    info.ResourcesPath = reader["ResourcesPath"] == null ? "" : reader["ResourcesPath"].ToString();
                    info.DropRate = reader["DropRate"] == null ? "" : reader["DropRate"].ToString();
                    info.Experience = (int)reader["Experience"];
                    info.Delay = (int)reader["Delay"];
                    info.Immunity = (int)reader["Immunity"];
                    info.Alert = (int)reader["Alert"];
                    info.Range = (int)reader["Range"];
                    info.Preserve = (int)reader["Preserve"];
                    info.Script = reader["Script"] == null ? "" : reader["Script"].ToString();
                    info.FireX = (int)reader["FireX"];
                    info.FireY = (int)reader["FireY"];
                    info.DropId = (int)reader["DropId"];
                    infos.Add(info);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("GetAllNPCInfo", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }


        #endregion

        #region
        public MissionInfo[] GetAllMissionInfo()
        {
            List<MissionInfo> infos = new List<MissionInfo>();
            SqlDataReader reader = null;
            try
            {
                db.GetReader(ref reader, "SP_Mission_Info_All");
                while (reader.Read())
                {
                    MissionInfo info = new MissionInfo();
                    info.Id = (int)reader["ID"];
                    info.Name = reader["Name"] == null ? "" : reader["Name"].ToString();
                    info.TotalCount = (int)reader["TotalCount"];
                    info.TotalTurn = (int)reader["TotalTurn"];
                    info.Script = reader["Script"] == null ? "" : reader["Script"].ToString();
                    info.Success = reader["Success"] == null ? "" : reader["Success"].ToString();
                    info.Failure = reader["Failure"] == null ? "" : reader["Failure"].ToString();
                    info.Description = reader["Description"] == null ? "" : reader["Description"].ToString();
                    info.IncrementDelay = (int)reader["IncrementDelay"];
                    info.Delay = (int)reader["Delay"];
                    info.Title = reader["Title"] == null ? "" : reader["Title"].ToString();
                    info.Param1 = (int)reader["Param1"];
                    info.Param2 = (int)reader["Param2"];
                    infos.Add(info);
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("GetAllMissionInfo", e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return infos.ToArray();
        }
        #endregion
    }
}
