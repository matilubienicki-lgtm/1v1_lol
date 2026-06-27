using UnityEngine;
using Photon.Pun2;

namespace Game.Networking
{
    /// <summary>
    /// Tracks player statistics (kills, deaths, score).
    /// </summary>
    public class PlayerStats : MonoBehaviourPunCallbacks
    {
        private int kills = 0;
        private int deaths = 0;
        private int score = 0;

        /// <summary>
        /// Records a kill for this player.
        /// </summary>
        public void AddKill()
        {
            kills++;
            score += 100;
            photonView.RPC(nameof(RPC_UpdateStats), RpcTarget.AllBuffered, kills, deaths, score);
            Debug.Log($"[PlayerStats] {photonView.Owner.NickName} killed someone! Kills: {kills}");
        }

        /// <summary>
        /// Records a death for this player.
        /// </summary>
        public void AddDeath()
        {
            deaths++;
            score -= 10;
            photonView.RPC(nameof(RPC_UpdateStats), RpcTarget.AllBuffered, kills, deaths, score);
            Debug.Log($"[PlayerStats] {photonView.Owner.NickName} died! Deaths: {deaths}");
        }

        public int GetKills() => kills;
        public int GetDeaths() => deaths;
        public int GetScore() => score;

        [PunRPC]
        private void RPC_UpdateStats(int newKills, int newDeaths, int newScore)
        {
            kills = newKills;
            deaths = newDeaths;
            score = newScore;
        }
    }
}
