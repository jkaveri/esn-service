﻿using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Summary description for RelationBLL
/// </summary>
/// 
namespace ESNLibrary
{
    public class RelationBLL
    {
        RelationDAL relDAL = new RelationDAL();
        AccountDAL accDAL = new AccountDAL();
        ActivityBLL actiBLL = new ActivityBLL();
        ActivityDAL actiDAL = new ActivityDAL();

        public RelationBLL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public List<AccountInfo> SearchFriend(string keyword, int userID, string type)
        {
            if (string.IsNullOrEmpty(keyword) == true)
            {
                return null;
            }
            else
            {
                return accDAL.SearchFriend(keyword, userID, type);
            }
        }

        public string SendFriendShipMessage(int userID, int friendID, string name)
        {
            if (accDAL.CheckExistingAccByID(friendID) == true)
            {
                List<Relation> listRel = relDAL.CheckExistingRelation(userID, friendID);
                List<Activity> listActi = actiDAL.CheckExistingFriendShipActivity(userID, friendID);

                if (listActi.Count != 0)
                {
                    return "Bạn đang có một yêu cầu kết bạn với " + name;
                }
                else
                {
                    if (listRel.Count == 0)
                    {
                        actiBLL.CreateNewActivity(userID, friendID, 1);
                        return "Đã gửi lời mời kết bạn đến " + name;
                    }
                    else
                    {
                        string msg = "";

                        if (listRel[0].Status == 0)
                        {
                            actiBLL.CreateNewActivity(userID, friendID, 1);
                            msg = "Đã gửi lời mời kết bạn đến " + name;
                        }
                        else if (listRel[0].Status == 1)
                        {
                            msg = "Bạn đã kết bạn với người " + name + " rồi";
                        }
                        return msg;
                    }
                }
            }
            else
            {
                return "Tài khoản này không còn tồn tại";
            }
        }


        public List<GetFriendListResult> GetFriendList(int userID)
        {
            return relDAL.GetFriendList(userID);
        }

        public bool UnFriend(int userID, int friendID)
        {
            bool rs = false;
            List<Relation> listRel = relDAL.CheckExistingRelation(userID, friendID);
            if (listRel.Count != 0)
            {
                if (listRel[0].Status == 1)
                {
                    relDAL.UpdateRelationStatus(listRel, 0);
                    actiBLL.CreateNewActivity(userID, friendID, 2);
                    rs = true;
                }
            }
            return rs;
        }

        public List<SearchFriendResult> SearchFriend(int accID, string keyword)
        {
            return relDAL.SearchFriend(accID, keyword);
        }

    }
}