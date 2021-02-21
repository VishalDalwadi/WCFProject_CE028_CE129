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
        [FaultContract(typeof(GamesManagementFault))]
        void SaveGame(Game game, string token);

        [OperationContract]
        [FaultContract(typeof(GamesManagementFault))]
        List<Game> GetAllSavedGames(string token);

        [OperationContract]
        void DeleteGame(Game game, string token);
        
        [OperationContract]
        [FaultContract(typeof(GamesManagementFault))]
        string FindMatch(string token);
    }

    [DataContract]
    public class Game
    {
        public enum Player
        {
            White,
            Black
        }

        private Int64 _id;
        private Player _played_as;
        private string _game_string;

        [DataMember]
        public Int64 GameId
        {
            get { return _id; }
            set { _id = value; }
        }

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
        public Game(Int64 id, string game_string, Player played_as)
        {
            _id = id;
            _game_string = game_string;
            _played_as = played_as;
        }
    }

    [DataContract]
    public class GamesManagementFault
    {
        public enum GamesManagementFaultType
        {
            TokenExpired,
            InvalidSignature,
            ServerFault
        }

        private GamesManagementFaultType _faultType;

        [DataMember]
        public GamesManagementFaultType FaultType
        {
            get { return _faultType; }
        }

        public GamesManagementFault(GamesManagementFaultType type)
        {
            _faultType = type;
        }
    }
}
