using System;
using System.Collections.Generic;
using Codenation.Challenge.Exceptions;
using Codenation.Models;
using System.Linq;

namespace Codenation.Challenge
{
    public class SoccerTeamsManager : IManageSoccerTeams
    {
        private IDictionary<long, Team> _teams = new Dictionary<long, Team>();
        private IDictionary<long, Player> _players = new Dictionary<long, Player>();

        public SoccerTeamsManager()
        {
        }

        public void AddTeam(long id, string name, DateTime createDate, string mainShirtColor, string secondaryShirtColor)
        {
            if(_teams.ContainsKey(id))
            {
                throw new UniqueIdentifierException("Time possui ID já cadastrado.");
            }
            _teams.Add
                (
                    id, 
                    new Team(id, name, createDate, mainShirtColor, secondaryShirtColor)
                );
        }

        public void AddPlayer(long id, long teamId, string name, DateTime birthDate, int skillLevel, decimal salary)
        {
            if (_players.ContainsKey(id))
            {
                throw new UniqueIdentifierException($"O ID {id} informado para o Jogador já está cadastrado.");
            }
            VerifyTeamIdExists(teamId);
            _players.Add
                (
                    id, 
                    new Player(id, teamId, name, birthDate, skillLevel, salary)
                );
        }

        public void SetCaptain(long playerId)
        {
            VerifyPlayerIdExists(playerId);
            var teamId = _players[playerId].TeamId;
            
            _teams[teamId].IdCaptain = playerId;

        }

        public long GetTeamCaptain(long teamId)
        {
            VerifyTeamIdExists(teamId);
            if (_teams[teamId].IdCaptain == 0)
            {
                throw new CaptainNotFoundException($"O id {teamId} do Time informado não possui um Capitão.");
            }
            return _teams[teamId].IdCaptain;
        }

        public string GetPlayerName(long playerId)
        {
            VerifyPlayerIdExists(playerId);
            return _players[playerId].Name;
        }

        public string GetTeamName(long teamId)
        {
            VerifyTeamIdExists(teamId);
            return _teams[teamId].Name;
        }

        public List<long> GetTeamPlayers(long teamId)
        {
            VerifyTeamIdExists(teamId);
            
            var teamPlayers = _players.Values.
                Where(player => player.TeamId == teamId).
                OrderBy(player => player.Id).
                Select(player => player.Id);
            
            return teamPlayers.ToList();
        }

        public long GetBestTeamPlayer(long teamId)
        {
            VerifyTeamIdExists(teamId);
            var bestSkill = 
                from player in _players.Values
                where player.TeamId == teamId
                orderby player.SkillLevel descending
                select player.Id;
            return bestSkill.First();
                 
        }

        public long GetOlderTeamPlayer(long teamId)
        {
            VerifyTeamIdExists(teamId);
            var olderPlayer = 
                from player in _players.Values
                where player.TeamId == teamId
                orderby player.BirthDate
                select player.Id;
            return olderPlayer.First();
        }

        public List<long> GetTeams()
        {
            SortedList<long, Team> sortedTeams = new SortedList<long, Team>(_teams);
            return sortedTeams != null ? sortedTeams.Keys.ToList() : new List<long>();
        }

        public long GetHigherSalaryPlayer(long teamId)
        {
            VerifyTeamIdExists(teamId);
            var higherSalaryPlayer = 
                from player in _players.Values
                where player.TeamId == teamId
                orderby player.Salary descending
                select player.Id;
            return higherSalaryPlayer.First();

        }

        public decimal GetPlayerSalary(long playerId)
        {
            VerifyPlayerIdExists(playerId);
            return _players[playerId].Salary;

        }

        public List<long> GetTopPlayers(int top)
        {
            var topPlayers = 
                from player in _players.Values
                orderby player.SkillLevel descending
                select player.Id;
            return topPlayers.Take(top).ToList();
        }

        public string GetVisitorShirtColor(long teamId, long visitorTeamId)
        {
            VerifyTeamIdExists(teamId);
            VerifyTeamIdExists(visitorTeamId);
            if(_teams[teamId].MainShirtColor == _teams[visitorTeamId].MainShirtColor)
            {
                return _teams[visitorTeamId].SecondaryShirtColor;
            }
            return _teams[visitorTeamId].MainShirtColor;
        }

        private void VerifyTeamIdExists(long id)
        {
            if (!_teams.Keys.Contains(id))
            {
                throw new TeamNotFoundException($"O ID {id} do Time informado não está cadastrado.");
            }
        }

         private void VerifyPlayerIdExists(long id)
        {
            if (!_players.Keys.Contains(id))
            {
                throw new PlayerNotFoundException($"Não existe jogador com o ID {id} cadastrado.");
            }
        }

    }
}
