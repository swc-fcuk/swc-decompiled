using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands;
using StaRTS.Main.Models.Commands.Squads.Requests;
using StaRTS.Main.Models.Commands.Squads.Responses;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Main.Utils.Chat;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Utils
{
	public static class SquadMsgUtils
	{
		public static PlayerIdRequest GeneratePlayerIdRequest(SquadMsg message)
		{
			return new PlayerIdRequest
			{
				PlayerId = message.OwnerData.PlayerId
			};
		}

		public static SquadIDRequest GenerateSquadIdRequest(SquadMsg message)
		{
			return new SquadIDRequest
			{
				SquadId = message.SquadData.Id,
				PlayerId = message.OwnerData.PlayerId
			};
		}

		public static MemberIdRequest GenerateMemberIdRequest(SquadMsg message)
		{
			return new MemberIdRequest
			{
				PlayerId = message.OwnerData.PlayerId,
				MemberId = message.MemberData.MemberId
			};
		}

		public static ApplyToSquadRequest GenerateApplyToSquadRequest(SquadMsg message)
		{
			return new ApplyToSquadRequest
			{
				SquadId = message.SquadData.Id,
				PlayerId = message.OwnerData.PlayerId,
				Message = WWW.EscapeURL(message.RequestData.Text)
			};
		}

		public static CreateSquadRequest GenerateCreateSquadRequest(SquadMsg message)
		{
			SqmSquadData squadData = message.SquadData;
			return new CreateSquadRequest(WWW.EscapeURL(squadData.Name), WWW.EscapeURL(squadData.Desc), squadData.Icon, squadData.MinTrophies, squadData.Open)
			{
				PlayerId = message.OwnerData.PlayerId
			};
		}

		public static EditSquadRequest GenerateEditSquadRequest(SquadMsg message)
		{
			SqmSquadData squadData = message.SquadData;
			return new EditSquadRequest
			{
				Desc = WWW.EscapeURL(squadData.Desc),
				Icon = squadData.Icon,
				OpenSquad = squadData.Open,
				MinTrophy = squadData.MinTrophies,
				PlayerId = message.OwnerData.PlayerId
			};
		}

		public static TroopSquadRequest GenerateTroopRequest(SquadMsg message)
		{
			return new TroopSquadRequest
			{
				PlayerId = message.OwnerData.PlayerId,
				PayToSkip = message.RequestData.PayToSkip,
				Message = WWW.EscapeURL(message.RequestData.Text)
			};
		}

		public static TroopDonateRequest GenerateTroopDonateRequest(SquadMsg message)
		{
			return new TroopDonateRequest
			{
				PlayerId = message.OwnerData.PlayerId,
				RecipientId = message.DonationData.RecipientId,
				RequestId = message.DonationData.RequestId,
				Donations = new Dictionary<string, int>(message.DonationData.Donations)
			};
		}

		public static ShareReplayRequest GenerateShareReplayRequest(SquadMsg message)
		{
			return new ShareReplayRequest
			{
				PlayerId = message.OwnerData.PlayerId,
				BattleId = message.ReplayData.BattleId,
				Message = WWW.EscapeURL(message.ReplayData.Text)
			};
		}

		public static ShareVideoRequest GenerateShareVideoRequest(SquadMsg message)
		{
			return new ShareVideoRequest
			{
				PlayerId = message.OwnerData.PlayerId,
				Url = message.VideoData.VideoId,
				Message = WWW.EscapeURL(message.VideoData.Text)
			};
		}

		public static SendSquadInviteRequest GenerateSendInviteRequest(SquadMsg message)
		{
			SqmFriendInviteData friendInviteData = message.FriendInviteData;
			return new SendSquadInviteRequest(message.OwnerData.PlayerId, friendInviteData.PlayerId, friendInviteData.FacebookFriendId, friendInviteData.FacebookAccessToken);
		}

		public static SquadInvite GenerateSquadInvite(SquadMsg message)
		{
			return new SquadInvite
			{
				SquadId = message.SquadData.Id,
				SenderId = message.FriendInviteData.SenderId,
				SenderName = message.FriendInviteData.SenderName
			};
		}

		public static SquadWarStartMatchmakingRequest GenerateStartWarMatchmakingRequest(SquadMsg message)
		{
			return new SquadWarStartMatchmakingRequest(message.WarParticipantData);
		}

		public static PlayerIdChecksumRequest GeneratePlayerIdChecksumRequest(SquadMsg message)
		{
			return new PlayerIdChecksumRequest();
		}

		public static SquadMsg GenerateMessageFromSquadResponse(SquadResponse response, LeaderboardController lbc)
		{
			SqmSquadData sqmSquadData = new SqmSquadData();
			sqmSquadData.Id = response.SquadId;
			Squad orCreateSquad = lbc.GetOrCreateSquad(sqmSquadData.Id);
			orCreateSquad.FromObject(response.SquadData);
			return new SquadMsg
			{
				SquadData = sqmSquadData,
				RespondedSquad = orCreateSquad
			};
		}

		public static SquadMsg GenerateMessageFromSquadMemberResponse(SquadMemberResponse response)
		{
			SquadMember squadMember = new SquadMember();
			squadMember.FromObject(response.SquadMemberData);
			return new SquadMsg
			{
				SquadMemberResponse = squadMember
			};
		}

		public static SquadMsg GenerateMessageFromTroopDonateResponse(TroopDonateResponse response)
		{
			SqmDonationData sqmDonationData = new SqmDonationData();
			sqmDonationData.Donations = response.TroopsDonated;
			return new SquadMsg
			{
				DonationData = sqmDonationData
			};
		}

		public static SquadMsg GenerateMessageFromGetSquadWarStatusResponse(GetSquadWarStatusResponse response)
		{
			SquadWarSquadData squadWarSquadData = new SquadWarSquadData();
			SquadWarSquadData squadWarSquadData2 = new SquadWarSquadData();
			List<SquadWarBuffBaseData> list = new List<SquadWarBuffBaseData>();
			squadWarSquadData.FromObject(response.Squad1Data);
			squadWarSquadData2.FromObject(response.Squad2Data);
			List<object> list2 = response.BuffBaseData as List<object>;
			int i = 0;
			int count = list2.Count;
			while (i < count)
			{
				SquadWarBuffBaseData squadWarBuffBaseData = new SquadWarBuffBaseData();
				squadWarBuffBaseData.FromObject(list2[i]);
				list.Add(squadWarBuffBaseData);
				i++;
			}
			SquadWarData squadWarData = new SquadWarData();
			squadWarData.WarId = response.Id;
			squadWarData.Squads[0] = squadWarSquadData;
			squadWarData.Squads[1] = squadWarSquadData2;
			squadWarData.BuffBases = list;
			squadWarData.PrepGraceStartTimeStamp = response.PrepGraceStartTimeStamp;
			squadWarData.PrepEndTimeStamp = response.PrepEndTimeStamp;
			squadWarData.ActionGraceStartTimeStamp = response.ActionGraceStartTimeStamp;
			squadWarData.ActionEndTimeStamp = response.ActionEndTimeStamp;
			squadWarData.StartTimeStamp = response.StartTimeStamp;
			squadWarData.CooldownEndTimeStamp = response.CooldownEndTimeStamp;
			squadWarData.RewardsProcessed = response.RewardsProcessed;
			return new SquadMsg
			{
				CurrentSquadWarData = squadWarData
			};
		}

		public static SquadMsg GenerateMessageFromNotifObject(object notif)
		{
			Dictionary<string, object> dictionary = notif as Dictionary<string, object>;
			if (dictionary == null)
			{
				return null;
			}
			SquadMsg squadMsg = new SquadMsg();
			if (dictionary.ContainsKey("id"))
			{
				squadMsg.NotifId = Convert.ToString(dictionary["id"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("date"))
			{
				squadMsg.TimeSent = Convert.ToUInt32(dictionary["date"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("type"))
			{
				string name = Convert.ToString(dictionary["type"], CultureInfo.InvariantCulture);
				squadMsg.Type = StringUtils.ParseEnum<SquadMsgType>(name);
			}
			if (dictionary.ContainsKey("playerId"))
			{
				SqmOwnerData sqmOwnerData = new SqmOwnerData();
				squadMsg.OwnerData = sqmOwnerData;
				sqmOwnerData.PlayerId = Convert.ToString(dictionary["playerId"], CultureInfo.InvariantCulture);
				if (dictionary.ContainsKey("name"))
				{
					sqmOwnerData.PlayerName = Convert.ToString(dictionary["name"], CultureInfo.InvariantCulture);
				}
			}
			if (dictionary.ContainsKey("message"))
			{
				SqmChatData sqmChatData = new SqmChatData();
				squadMsg.ChatData = sqmChatData;
				sqmChatData.Message = WWW.UnEscapeURL(Convert.ToString(dictionary["message"], CultureInfo.InvariantCulture));
			}
			if (dictionary.ContainsKey("data"))
			{
				Dictionary<string, object> dictionary2 = dictionary["data"] as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					if (dictionary2.ContainsKey("senderName"))
					{
						SqmFriendInviteData sqmFriendInviteData = new SqmFriendInviteData();
						squadMsg.FriendInviteData = sqmFriendInviteData;
						sqmFriendInviteData.SenderName = Convert.ToString(dictionary2["senderName"], CultureInfo.InvariantCulture);
					}
					if (dictionary2.ContainsKey("toRank"))
					{
						SqmMemberData sqmMemberData = new SqmMemberData();
						squadMsg.MemberData = sqmMemberData;
						string name2 = Convert.ToString(dictionary2["toRank"], CultureInfo.InvariantCulture);
						sqmMemberData.MemberRole = StringUtils.ParseEnum<SquadRole>(name2);
					}
					if (dictionary2.ContainsKey("acceptor"))
					{
						SqmApplyData sqmApplyData = new SqmApplyData();
						squadMsg.ApplyData = sqmApplyData;
						sqmApplyData.AcceptorId = Convert.ToString(dictionary2["acceptor"], CultureInfo.InvariantCulture);
					}
					if (dictionary2.ContainsKey("rejector"))
					{
						SqmApplyData sqmApplyData2 = new SqmApplyData();
						squadMsg.ApplyData = sqmApplyData2;
						sqmApplyData2.RejectorId = Convert.ToString(dictionary2["rejector"], CultureInfo.InvariantCulture);
					}
					if (dictionary2.ContainsKey("battleId"))
					{
						SqmReplayData sqmReplayData = new SqmReplayData();
						squadMsg.ReplayData = sqmReplayData;
						sqmReplayData.BattleId = Convert.ToString(dictionary2["battleId"], CultureInfo.InvariantCulture);
						if (dictionary2.ContainsKey("battleVersion"))
						{
							sqmReplayData.BattleVersion = Convert.ToString(dictionary2["battleVersion"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("cmsVersion"))
						{
							sqmReplayData.CMSVersion = Convert.ToString(dictionary2["cmsVersion"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("type"))
						{
							string name3 = Convert.ToString(dictionary2["type"], CultureInfo.InvariantCulture);
							sqmReplayData.BattleType = StringUtils.ParseEnum<SquadBattleReplayType>(name3);
						}
						if (dictionary2.ContainsKey("battleScoreDelta"))
						{
							object obj = dictionary2["battleScoreDelta"];
							if (obj != null)
							{
								sqmReplayData.MedalDelta = Convert.ToInt32(obj, CultureInfo.InvariantCulture);
							}
						}
						if (dictionary2.ContainsKey("damagePercent"))
						{
							sqmReplayData.DamagePercent = Convert.ToInt32(dictionary2["damagePercent"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("stars"))
						{
							sqmReplayData.Stars = Convert.ToInt32(dictionary2["stars"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("opponentId"))
						{
							sqmReplayData.OpponentId = Convert.ToString(dictionary2["opponentId"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("opponentName"))
						{
							sqmReplayData.OpponentName = Convert.ToString(dictionary2["opponentName"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("opponentFaction"))
						{
							string name4 = Convert.ToString(dictionary2["opponentFaction"], CultureInfo.InvariantCulture);
							sqmReplayData.OpponentFaction = StringUtils.ParseEnum<FactionType>(name4);
						}
						if (dictionary2.ContainsKey("faction"))
						{
							string name5 = Convert.ToString(dictionary2["faction"], CultureInfo.InvariantCulture);
							sqmReplayData.SharerFaction = StringUtils.ParseEnum<FactionType>(name5);
						}
					}
					if (dictionary2.ContainsKey("url"))
					{
						SqmVideoData sqmVideoData = new SqmVideoData();
						squadMsg.VideoData = sqmVideoData;
						sqmVideoData.VideoId = Convert.ToString(dictionary2["url"], CultureInfo.InvariantCulture);
					}
					if (dictionary2.ContainsKey("totalCapacity"))
					{
						SqmRequestData sqmRequestData = new SqmRequestData();
						squadMsg.RequestData = sqmRequestData;
						sqmRequestData.TotalCapacity = Convert.ToInt32(dictionary2["totalCapacity"], CultureInfo.InvariantCulture);
						if (dictionary2.ContainsKey("amount"))
						{
							sqmRequestData.StartingAvailableCapacity = Convert.ToInt32(dictionary2["amount"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("warId"))
						{
							sqmRequestData.WarId = Convert.ToString(dictionary2["warId"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("troopDonationLimit"))
						{
							sqmRequestData.TroopDonationLimit = Convert.ToInt32(dictionary2["troopDonationLimit"], CultureInfo.InvariantCulture);
						}
						else
						{
							Service.Get<StaRTSLogger>().Error("Missing troop request data param: troopDonationLimitdefaulting to " + GameConstants.MAX_PER_USER_TROOP_DONATION);
							sqmRequestData.TroopDonationLimit = GameConstants.MAX_PER_USER_TROOP_DONATION;
						}
					}
					if (dictionary2.ContainsKey("troopsDonated"))
					{
						SqmDonationData sqmDonationData = new SqmDonationData();
						squadMsg.DonationData = sqmDonationData;
						Dictionary<string, object> dictionary3 = dictionary2["troopsDonated"] as Dictionary<string, object>;
						if (dictionary3 != null)
						{
							sqmDonationData.Donations = new Dictionary<string, int>();
							foreach (KeyValuePair<string, object> current in dictionary3)
							{
								string key = current.get_Key();
								int value = Convert.ToInt32(current.get_Value(), CultureInfo.InvariantCulture);
								sqmDonationData.Donations.Add(key, value);
							}
						}
						if (dictionary2.ContainsKey("requestId"))
						{
							sqmDonationData.RequestId = Convert.ToString(dictionary2["requestId"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("recipientId"))
						{
							sqmDonationData.RecipientId = Convert.ToString(dictionary2["recipientId"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("amount"))
						{
							sqmDonationData.DonationCount = Convert.ToInt32(dictionary2["amount"], CultureInfo.InvariantCulture);
						}
					}
					if (dictionary2.ContainsKey("warId"))
					{
						SqmWarEventData sqmWarEventData = new SqmWarEventData();
						squadMsg.WarEventData = sqmWarEventData;
						sqmWarEventData.WarId = Convert.ToString(dictionary2["warId"], CultureInfo.InvariantCulture);
						if (dictionary2.ContainsKey("buffBaseUid"))
						{
							sqmWarEventData.BuffBaseUid = Convert.ToString(dictionary2["buffBaseUid"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("captured"))
						{
							sqmWarEventData.BuffBaseCaptured = Convert.ToBoolean(dictionary2["captured"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("opponentId"))
						{
							sqmWarEventData.OpponentId = Convert.ToString(dictionary2["opponentId"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("opponentName"))
						{
							sqmWarEventData.OpponentName = Convert.ToString(dictionary2["opponentName"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("stars"))
						{
							sqmWarEventData.StarsEarned = Convert.ToInt32(dictionary2["stars"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("victoryPoints"))
						{
							sqmWarEventData.VictoryPointsTaken = Convert.ToInt32(dictionary2["victoryPoints"], CultureInfo.InvariantCulture);
						}
						if (dictionary2.ContainsKey("attackExpirationDate"))
						{
							sqmWarEventData.AttackExpirationTime = Convert.ToUInt32(dictionary2["attackExpirationDate"], CultureInfo.InvariantCulture);
						}
					}
					if (dictionary2.ContainsKey("level") || dictionary2.ContainsKey("totalRepInvested"))
					{
						SquadMsgUtils.AddSquadLevelToSquadMessageData(dictionary2, squadMsg);
					}
					if (dictionary2.ContainsKey("perkId"))
					{
						SquadMsgUtils.AddPerkUnlockUpgrdeDataToSquadMessageData(dictionary2, squadMsg);
					}
				}
			}
			return squadMsg;
		}

		public static SquadMsg GenerateMessageFromServerMessageObject(object messageObj)
		{
			Dictionary<string, object> dictionary = messageObj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return null;
			}
			SquadMsg squadMsg = new SquadMsg();
			if (dictionary.ContainsKey("notification"))
			{
				squadMsg = SquadMsgUtils.GenerateMessageFromNotifObject(dictionary["notification"]);
				if (dictionary.ContainsKey("guildId"))
				{
					if (squadMsg.SquadData == null)
					{
						squadMsg.SquadData = new SqmSquadData();
					}
					squadMsg.SquadData.Id = Convert.ToString(dictionary["guildId"], CultureInfo.InvariantCulture);
					if (dictionary.ContainsKey("guildName"))
					{
						squadMsg.SquadData.Name = WWW.UnEscapeURL(Convert.ToString(dictionary["guildName"], CultureInfo.InvariantCulture));
					}
				}
				return squadMsg;
			}
			if (dictionary.ContainsKey("serverTime"))
			{
				squadMsg.TimeSent = Convert.ToUInt32(dictionary["serverTime"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("event"))
			{
				string name = Convert.ToString(dictionary["event"], CultureInfo.InvariantCulture);
				squadMsg.Type = StringUtils.ParseEnum<SquadMsgType>(name);
			}
			if (dictionary.ContainsKey("guildId"))
			{
				SqmSquadData sqmSquadData = new SqmSquadData();
				squadMsg.SquadData = sqmSquadData;
				sqmSquadData.Id = Convert.ToString(dictionary["guildId"], CultureInfo.InvariantCulture);
				if (dictionary.ContainsKey("guildName"))
				{
					sqmSquadData.Name = WWW.UnEscapeURL(Convert.ToString(dictionary["guildName"], CultureInfo.InvariantCulture));
				}
			}
			if (dictionary.ContainsKey("senderId"))
			{
				SqmFriendInviteData sqmFriendInviteData = new SqmFriendInviteData();
				squadMsg.FriendInviteData = sqmFriendInviteData;
				sqmFriendInviteData.SenderId = Convert.ToString(dictionary["senderId"], CultureInfo.InvariantCulture);
				if (dictionary.ContainsKey("senderName"))
				{
					sqmFriendInviteData.SenderName = Convert.ToString(dictionary["senderName"], CultureInfo.InvariantCulture);
				}
			}
			if (dictionary.ContainsKey("recipientId"))
			{
				if (squadMsg.FriendInviteData == null)
				{
					squadMsg.FriendInviteData = new SqmFriendInviteData();
				}
				squadMsg.FriendInviteData.PlayerId = Convert.ToString(dictionary["recipientId"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("warId"))
			{
				SqmWarEventData sqmWarEventData = new SqmWarEventData();
				squadMsg.WarEventData = sqmWarEventData;
				sqmWarEventData.WarId = Convert.ToString(dictionary["warId"], CultureInfo.InvariantCulture);
				if (dictionary.ContainsKey("empireName"))
				{
					sqmWarEventData.EmpireSquadName = Convert.ToString(dictionary["empireName"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("empireScore"))
				{
					sqmWarEventData.EmpireScore = Convert.ToInt32(dictionary["empireScore"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("rebelName"))
				{
					sqmWarEventData.RebelSquadName = Convert.ToString(dictionary["rebelName"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("rebelScore"))
				{
					sqmWarEventData.RebelScore = Convert.ToInt32(dictionary["rebelScore"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("buffBaseUid"))
				{
					sqmWarEventData.BuffBaseUid = Convert.ToString(dictionary["buffBaseUid"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("empireCrateTier"))
				{
					sqmWarEventData.EmpireCrateId = Convert.ToString(dictionary["empireCrateTier"], CultureInfo.InvariantCulture);
				}
				else if (dictionary.ContainsKey("empireCrateId"))
				{
					sqmWarEventData.EmpireCrateId = Convert.ToString(dictionary["empireCrateId"], CultureInfo.InvariantCulture);
				}
				if (dictionary.ContainsKey("rebelCrateTier"))
				{
					sqmWarEventData.RebelCrateId = Convert.ToString(dictionary["rebelCrateTier"], CultureInfo.InvariantCulture);
				}
				else if (dictionary.ContainsKey("rebelCrateId"))
				{
					sqmWarEventData.RebelCrateId = Convert.ToString(dictionary["rebelCrateId"], CultureInfo.InvariantCulture);
				}
			}
			if (dictionary.ContainsKey("level") || dictionary.ContainsKey("totalRepInvested"))
			{
				SquadMsgUtils.AddSquadLevelToSquadMessageData(dictionary, squadMsg);
			}
			if (dictionary.ContainsKey("perkId"))
			{
				SquadMsgUtils.AddPerkUnlockUpgrdeDataToSquadMessageData(dictionary, squadMsg);
			}
			return squadMsg;
		}

		private static void AddSquadLevelToSquadMessageData(Dictionary<string, object> dict, SquadMsg message)
		{
			if (message.SquadData == null)
			{
				message.SquadData = new SqmSquadData();
			}
			int num = 0;
			if (dict.ContainsKey("level"))
			{
				num = Convert.ToInt32(dict["level"], CultureInfo.InvariantCulture);
			}
			int num2 = 0;
			if (dict.ContainsKey("totalRepInvested"))
			{
				num2 = Convert.ToInt32(dict["totalRepInvested"], CultureInfo.InvariantCulture);
				message.SquadData.TotalRepInvested = num2;
			}
			if (num == 0 && num2 > 0)
			{
				num = GameUtils.GetSquadLevelFromInvestedRep(num2);
			}
			message.SquadData.Level = num;
		}

		private static void AddPerkUnlockUpgrdeDataToSquadMessageData(Dictionary<string, object> dict, SquadMsg message)
		{
			if (message.PerkData == null)
			{
				message.PerkData = new SqmPerkData();
			}
			message.PerkData.PerkUId = Convert.ToString(dict["perkId"], CultureInfo.InvariantCulture);
			if (dict.ContainsKey("perkInvestAmt"))
			{
				message.PerkData.PerkInvestedAmt = Convert.ToInt32(dict["perkInvestAmt"], CultureInfo.InvariantCulture);
			}
		}

		public static SquadMsg GenerateMessageFromChatObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return null;
			}
			SquadMsg squadMsg = new SquadMsg();
			squadMsg.Type = SquadMsgType.Chat;
			SqmChatData sqmChatData = new SqmChatData();
			squadMsg.ChatData = sqmChatData;
			if (dictionary.ContainsKey("text"))
			{
				string json = WWW.UnEscapeURL(Convert.ToString(dictionary["text"], CultureInfo.InvariantCulture));
				object obj2 = new JsonParser(json).Parse();
				Dictionary<string, object> dictionary2 = obj2 as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					SqmOwnerData sqmOwnerData = new SqmOwnerData();
					squadMsg.OwnerData = sqmOwnerData;
					if (dictionary2.ContainsKey("userId"))
					{
						sqmOwnerData.PlayerId = Convert.ToString(dictionary2["userId"], CultureInfo.InvariantCulture);
					}
					if (dictionary2.ContainsKey("userName"))
					{
						sqmOwnerData.PlayerName = Convert.ToString(dictionary2["userName"], CultureInfo.InvariantCulture);
					}
					if (dictionary2.ContainsKey("message"))
					{
						sqmChatData.Message = Convert.ToString(dictionary2["message"], CultureInfo.InvariantCulture);
					}
					if (dictionary2.ContainsKey("timestamp"))
					{
						squadMsg.TimeSent = Convert.ToUInt32(dictionary2["timestamp"], CultureInfo.InvariantCulture);
					}
				}
			}
			if (dictionary.ContainsKey("tag"))
			{
				sqmChatData.Tag = Convert.ToString(dictionary["tag"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("time"))
			{
				sqmChatData.Time = Convert.ToString(dictionary["time"], CultureInfo.InvariantCulture);
			}
			return squadMsg;
		}

		public static SquadMsg GenerateMessageFromChatMessage(string message)
		{
			return new SquadMsg
			{
				Type = SquadMsgType.Chat,
				TimeSent = ChatTimeConversionUtils.GetUnixTimestamp(),
				OwnerData = new SqmOwnerData
				{
					PlayerId = Service.Get<CurrentPlayer>().PlayerId,
					PlayerName = Service.Get<CurrentPlayer>().PlayerName
				},
				ChatData = new SqmChatData
				{
					Message = message
				}
			};
		}

		private static SquadMsg CreateActionMessage(SquadAction action, SquadController.ActionCallback callback, object cookie)
		{
			SqmActionData sqmActionData = new SqmActionData();
			sqmActionData.Type = action;
			sqmActionData.Callback = callback;
			sqmActionData.Cookie = cookie;
			SqmOwnerData sqmOwnerData = new SqmOwnerData();
			sqmOwnerData.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			sqmOwnerData.PlayerName = Service.Get<CurrentPlayer>().PlayerName;
			return new SquadMsg
			{
				OwnerData = sqmOwnerData,
				ActionData = sqmActionData
			};
		}

		private static SquadMsg CreateMemberIdMessage(string memberId, SquadAction action, SquadController.ActionCallback callback, object cookie)
		{
			SqmMemberData sqmMemberData = new SqmMemberData();
			sqmMemberData.MemberId = memberId;
			SquadMsg squadMsg = SquadMsgUtils.CreateActionMessage(action, callback, cookie);
			squadMsg.MemberData = sqmMemberData;
			return squadMsg;
		}

		public static SquadMsg CreateDemoteMemberMessage(string memberId, SquadController.ActionCallback callback, object cookie)
		{
			SquadMsg squadMsg = SquadMsgUtils.CreateMemberIdMessage(memberId, SquadAction.DemoteMember, callback, cookie);
			squadMsg.MemberData.MemberRole = SquadRole.Member;
			return squadMsg;
		}

		public static SquadMsg CreatePromoteMemberMessage(string memberId, SquadController.ActionCallback callback, object cookie)
		{
			SquadMsg squadMsg = SquadMsgUtils.CreateMemberIdMessage(memberId, SquadAction.PromoteMember, callback, cookie);
			squadMsg.MemberData.MemberRole = SquadRole.Officer;
			return squadMsg;
		}

		public static SquadMsg CreateRemoveMemberMessage(string memberId, SquadController.ActionCallback callback, object cookie)
		{
			return SquadMsgUtils.CreateMemberIdMessage(memberId, SquadAction.RemoveMember, callback, cookie);
		}

		public static SquadMsg CreateAcceptJoinRequestMessage(string requesterId, string biSource, SquadController.ActionCallback callback, object cookie)
		{
			SquadMsg squadMsg = SquadMsgUtils.CreateMemberIdMessage(requesterId, SquadAction.AcceptApplicationToJoin, callback, cookie);
			squadMsg.BISource = biSource;
			return squadMsg;
		}

		public static SquadMsg CreateRejectJoinRequestMessage(string requesterId, SquadController.ActionCallback callback, object cookie)
		{
			return SquadMsgUtils.CreateMemberIdMessage(requesterId, SquadAction.RejectApplicationToJoin, callback, cookie);
		}

		public static SquadMsg CreateWarDonateMessage(string recipientId, Dictionary<string, int> donations, int donationCount, string requestId, SquadController.ActionCallback callback, object cookie)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			SquadMsg squadMsg = SquadMsgUtils.CreateMemberIdMessage(currentPlayer.PlayerId, SquadAction.DonateWarTroops, callback, cookie);
			SqmDonationData sqmDonationData = new SqmDonationData();
			squadMsg.DonationData = sqmDonationData;
			sqmDonationData.RecipientId = recipientId;
			sqmDonationData.Donations = donations;
			sqmDonationData.DonationCount = donationCount;
			sqmDonationData.RequestId = requestId;
			return squadMsg;
		}

		public static SquadMsg CreateDonateMessage(string recipientId, Dictionary<string, int> donations, int donationCount, string requestId, SquadController.ActionCallback callback, object cookie)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			SquadMsg squadMsg = SquadMsgUtils.CreateMemberIdMessage(currentPlayer.PlayerId, SquadAction.DonateTroops, callback, cookie);
			SqmDonationData sqmDonationData = new SqmDonationData();
			squadMsg.DonationData = sqmDonationData;
			sqmDonationData.RecipientId = recipientId;
			sqmDonationData.Donations = donations;
			sqmDonationData.DonationCount = donationCount;
			sqmDonationData.RequestId = requestId;
			return squadMsg;
		}

		private static SquadMsg CreateSquadMessage(string name, string description, string symbolName, int scoreReq, bool openEnrollment, SquadAction action, SquadController.ActionCallback callback, object cookie)
		{
			SqmSquadData sqmSquadData = new SqmSquadData();
			sqmSquadData.Name = name;
			sqmSquadData.Desc = description;
			sqmSquadData.Icon = symbolName;
			sqmSquadData.MinTrophies = scoreReq;
			sqmSquadData.Open = openEnrollment;
			SquadMsg squadMsg = SquadMsgUtils.CreateActionMessage(action, callback, cookie);
			squadMsg.SquadData = sqmSquadData;
			return squadMsg;
		}

		private static SquadMsg CreateSquadMessage(string squadId, SquadAction action, SquadController.ActionCallback callback, object cookie)
		{
			SqmSquadData sqmSquadData = new SqmSquadData();
			sqmSquadData.Id = squadId;
			SquadMsg squadMsg = SquadMsgUtils.CreateActionMessage(action, callback, cookie);
			squadMsg.SquadData = sqmSquadData;
			return squadMsg;
		}

		public static SquadMsg CreateNewSquadMessage(string name, string description, string symbolName, int scoreReq, bool openEnrollment, SquadController.ActionCallback callback, object cookie)
		{
			return SquadMsgUtils.CreateSquadMessage(name, description, symbolName, scoreReq, openEnrollment, SquadAction.Create, callback, cookie);
		}

		public static SquadMsg CreateEditSquadMessage(string description, string symbolName, int scoreReq, bool openEnrollment, SquadController.ActionCallback callback, object cookie)
		{
			return SquadMsgUtils.CreateSquadMessage(null, description, symbolName, scoreReq, openEnrollment, SquadAction.Edit, callback, cookie);
		}

		public static SquadMsg CreateJoinSquadMessage(string squadId, string biSource, SquadController.ActionCallback callback, object cookie)
		{
			SquadMsg squadMsg = SquadMsgUtils.CreateSquadMessage(squadId, SquadAction.Join, callback, cookie);
			squadMsg.BISource = biSource;
			return squadMsg;
		}

		public static SquadMsg CreateApplyToJoinSquadMessage(string squadId, string message, SquadController.ActionCallback callback, object cookie)
		{
			SqmRequestData sqmRequestData = new SqmRequestData();
			sqmRequestData.Text = message;
			SquadMsg squadMsg = SquadMsgUtils.CreateSquadMessage(squadId, SquadAction.ApplyToJoin, callback, cookie);
			squadMsg.RequestData = sqmRequestData;
			return squadMsg;
		}

		public static SquadMsg CreateLeaveSquadMessage(SquadController.ActionCallback callback, object cookie)
		{
			return SquadMsgUtils.CreateActionMessage(SquadAction.Leave, callback, cookie);
		}

		public static SquadMsg CreateAcceptSquadInviteMessage(string squadId, SquadController.ActionCallback callback, object cookie)
		{
			return SquadMsgUtils.CreateSquadMessage(squadId, SquadAction.AcceptInviteToJoin, callback, cookie);
		}

		public static SquadMsg CreateRejectSquadInviteMessage(string squadId, SquadController.ActionCallback callback, object cookie)
		{
			return SquadMsgUtils.CreateSquadMessage(squadId, SquadAction.RejectInviteToJoin, callback, cookie);
		}

		public static SquadMsg CreateSendInviteMessage(string recipientId, string fbFriendId, string fbAccessToken, SquadController.ActionCallback callback, object cookie)
		{
			SqmFriendInviteData sqmFriendInviteData = new SqmFriendInviteData();
			sqmFriendInviteData.PlayerId = recipientId;
			sqmFriendInviteData.FacebookFriendId = fbFriendId;
			sqmFriendInviteData.FacebookAccessToken = fbAccessToken;
			SquadMsg squadMsg = SquadMsgUtils.CreateActionMessage(SquadAction.SendInviteToJoin, callback, cookie);
			squadMsg.FriendInviteData = sqmFriendInviteData;
			return squadMsg;
		}

		public static SquadMsg CreateRequestWarTroopsMessage(bool payToSkip, int resendCrystalCost, string message)
		{
			SqmRequestData sqmRequestData = new SqmRequestData();
			sqmRequestData.PayToSkip = payToSkip;
			sqmRequestData.ResendCrystalCost = resendCrystalCost;
			sqmRequestData.Text = message;
			SquadMsg squadMsg = SquadMsgUtils.CreateActionMessage(SquadAction.RequestWarTroops, null, null);
			squadMsg.RequestData = sqmRequestData;
			return squadMsg;
		}

		public static SquadMsg CreateRequestTroopsMessage(bool payToSkip, int resendCrystalCost, string message)
		{
			SqmRequestData sqmRequestData = new SqmRequestData();
			sqmRequestData.PayToSkip = payToSkip;
			sqmRequestData.ResendCrystalCost = resendCrystalCost;
			sqmRequestData.Text = message;
			SquadMsg squadMsg = SquadMsgUtils.CreateActionMessage(SquadAction.RequestTroops, null, null);
			squadMsg.RequestData = sqmRequestData;
			return squadMsg;
		}

		public static SquadMsg CreateSendReplayMessage(string battleId, string message)
		{
			SqmReplayData sqmReplayData = new SqmReplayData();
			sqmReplayData.BattleId = battleId;
			sqmReplayData.Text = message;
			SquadMsg squadMsg = SquadMsgUtils.CreateActionMessage(SquadAction.ShareReplay, null, null);
			squadMsg.ReplayData = sqmReplayData;
			return squadMsg;
		}

		public static SquadMsg CreateShareReplayMessage(string message, BattleEntry entry)
		{
			SqmReplayData sqmReplayData = new SqmReplayData();
			sqmReplayData.BattleId = entry.RecordID;
			sqmReplayData.Text = message;
			SquadMsg squadMsg = SquadMsgUtils.CreateActionMessage(SquadAction.ShareReplay, null, null);
			squadMsg.ReplayData = sqmReplayData;
			return squadMsg;
		}

		public static SquadMsg CreateShareVideoMessage(string videoId, string message)
		{
			SqmVideoData sqmVideoData = new SqmVideoData();
			sqmVideoData.VideoId = videoId;
			sqmVideoData.Text = message;
			SquadMsg squadMsg = SquadMsgUtils.CreateActionMessage(SquadAction.ShareVideo, null, null);
			squadMsg.VideoData = sqmVideoData;
			return squadMsg;
		}

		public static SquadMsg CreateStartMatchmakingMessage(List<string> memberIds, bool allowSameFaction)
		{
			SqmWarParticipantData sqmWarParticipantData = new SqmWarParticipantData();
			int i = 0;
			int count = memberIds.Count;
			while (i < count)
			{
				sqmWarParticipantData.Participants.Add(memberIds[i]);
				i++;
			}
			sqmWarParticipantData.AllowSameFactionMatchMaking = allowSameFaction;
			SquadMsg squadMsg = SquadMsgUtils.CreateActionMessage(SquadAction.StartWarMatchmaking, null, null);
			squadMsg.WarParticipantData = sqmWarParticipantData;
			return squadMsg;
		}

		public static SquadMsg CreateCancelMatchmakingMessage()
		{
			return SquadMsgUtils.CreateActionMessage(SquadAction.CancelWarMatchmaking, null, null);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			SquadMsgUtils.AddPerkUnlockUpgrdeDataToSquadMessageData((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args), (SquadMsg)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			SquadMsgUtils.AddSquadLevelToSquadMessageData((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args), (SquadMsg)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateAcceptJoinRequestMessage(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[2]), GCHandledObjects.GCHandleToObject(args[3])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateAcceptSquadInviteMessage(Marshal.PtrToStringUni(*(IntPtr*)args), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateActionMessage((SquadAction)(*(int*)args), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateApplyToJoinSquadMessage(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[2]), GCHandledObjects.GCHandleToObject(args[3])));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateCancelMatchmakingMessage());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateDemoteMemberMessage(Marshal.PtrToStringUni(*(IntPtr*)args), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateDonateMessage(Marshal.PtrToStringUni(*(IntPtr*)args), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[4]), GCHandledObjects.GCHandleToObject(args[5])));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateEditSquadMessage(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2), *(sbyte*)(args + 3) != 0, (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[4]), GCHandledObjects.GCHandleToObject(args[5])));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateJoinSquadMessage(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[2]), GCHandledObjects.GCHandleToObject(args[3])));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateLeaveSquadMessage((SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateMemberIdMessage(Marshal.PtrToStringUni(*(IntPtr*)args), (SquadAction)(*(int*)(args + 1)), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[2]), GCHandledObjects.GCHandleToObject(args[3])));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateNewSquadMessage(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), *(int*)(args + 3), *(sbyte*)(args + 4) != 0, (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[5]), GCHandledObjects.GCHandleToObject(args[6])));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreatePromoteMemberMessage(Marshal.PtrToStringUni(*(IntPtr*)args), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateRejectJoinRequestMessage(Marshal.PtrToStringUni(*(IntPtr*)args), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateRejectSquadInviteMessage(Marshal.PtrToStringUni(*(IntPtr*)args), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateRemoveMemberMessage(Marshal.PtrToStringUni(*(IntPtr*)args), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[1]), GCHandledObjects.GCHandleToObject(args[2])));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateRequestTroopsMessage(*(sbyte*)args != 0, *(int*)(args + 1), Marshal.PtrToStringUni(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateRequestWarTroopsMessage(*(sbyte*)args != 0, *(int*)(args + 1), Marshal.PtrToStringUni(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateSendInviteMessage(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[3]), GCHandledObjects.GCHandleToObject(args[4])));
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateSendReplayMessage(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateShareReplayMessage(Marshal.PtrToStringUni(*(IntPtr*)args), (BattleEntry)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateShareVideoMessage(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateSquadMessage(Marshal.PtrToStringUni(*(IntPtr*)args), (SquadAction)(*(int*)(args + 1)), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[2]), GCHandledObjects.GCHandleToObject(args[3])));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateSquadMessage(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), *(int*)(args + 3), *(sbyte*)(args + 4) != 0, (SquadAction)(*(int*)(args + 5)), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[6]), GCHandledObjects.GCHandleToObject(args[7])));
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateStartMatchmakingMessage((List<string>)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.CreateWarDonateMessage(Marshal.PtrToStringUni(*(IntPtr*)args), (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), (SquadController.ActionCallback)GCHandledObjects.GCHandleToObject(args[4]), GCHandledObjects.GCHandleToObject(args[5])));
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateApplyToSquadRequest((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateCreateSquadRequest((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateEditSquadRequest((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateMemberIdRequest((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateMessageFromChatMessage(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateMessageFromChatObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateMessageFromGetSquadWarStatusResponse((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateMessageFromNotifObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateMessageFromServerMessageObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateMessageFromSquadMemberResponse((SquadMemberResponse)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateMessageFromSquadResponse((SquadResponse)GCHandledObjects.GCHandleToObject(*args), (LeaderboardController)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateMessageFromTroopDonateResponse((TroopDonateResponse)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GeneratePlayerIdChecksumRequest((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GeneratePlayerIdRequest((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateSendInviteRequest((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateShareReplayRequest((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateShareVideoRequest((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateSquadIdRequest((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateSquadInvite((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateStartWarMatchmakingRequest((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateTroopDonateRequest((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadMsgUtils.GenerateTroopRequest((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
