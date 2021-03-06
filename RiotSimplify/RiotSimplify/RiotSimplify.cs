﻿using RiotSimplify.Clients;
using RiotSimplify.Dto;
using RiotSimplify.Mappers;
using RiotSimplify.Publishers;
using RiotSimplify.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RiotSimplify
{
    public class RiotApiClient : MatchService, UserAccountService, IMatchListenerService
    {
        /* Clients */
        private MatchClient matchClient;
        private UserClient userClient;
        /* Important data */

        public RiotApiClient (string apiKey, string summonerName = null)
        {
            RiotApiUtils.ApiKey = apiKey;
            RiotApiUtils.SummonerName = summonerName;
            matchClient = new MatchClient();
            userClient = new UserClient();
        }

        public async Task<List<MatchResult>> GetMatchesFromSeason(int seasonId, string queue, int beginIndex = 0, int endIndex = 100, bool tooManyRequestsThrowException = true)
        {
            try
            {
                return await matchClient.GetMatchesBySeasonAsync(seasonId, queue, beginIndex, endIndex, tooManyRequestsThrowException); 

            } catch(Exception e)
            {
                throw new Exception(string.Format("Failed to get matches for season {0}", seasonId), e);
            }
        }

        public async Task<MatchResult> GetMatchDetails(int matchId, Dictionary<string, string> queryStringOptions = null)
        {
            try
            {               
                return await matchClient.GetMatchResultAsync(matchId, queryStringOptions);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Failed to get match with id {0}", matchId), e);
            }
        }

        public async Task<MatchlistDto> GetTotalMatches(int seasonId, string queue)
        {
            try
            {
                return await matchClient.GetTotalMatches(seasonId, queue);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Failed to get matches for season {0}", seasonId), e);
            }
        }

        public async Task<string> GetSummonerIcon()
        {
            try
            {
                return await userClient.GetSummonerIconPath(RiotApiUtils.SummonerName);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get summonner icon", e);
            }
        }

        public async Task<List<EntryDto>> GetEntries()
        {
            try
            {
                return await userClient.GetEntries();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get user entries", e);
            }
        }

        public void SubscribeForMatches(int season, string queue, MatchPublisher.MatchesReceivedEventHandler subscriberMethod)
        {
            var publisher = new MatchPublisher();
            publisher.MatchesReceived += subscriberMethod;
            publisher.Listen(season, queue);          
        }

    }
}
