using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GamesManagementService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGamesManagementService" in both code and config file together.
    [ServiceContract]
    public interface IGamesManagementService
    {
        [OperationContract]
        void SaveGame(Game game, string token);

        [OperationContract]
        List<Game> GetAllSavedGames(string token);
        string AddPlayer(string token);
    }

    [DataContract]
    public class Game
    {
        public enum Player
        {
            White,
            Black
        }

        private Player _played_as;
        private string _game_string;

        [DataMember]
        public Player PlayedAs
        {
            get { return _played_as; }
            set { _played_as = value; }
        }

        [DataMember]
        public string GameString
        {
            get { return _game_string; }
            set { _game_string = value; }
        }

        public Game() { }
        public Game(string game_string, Player played_as)
        {
            _game_string = game_string;
            _played_as = played_as;
        }
    }
}
