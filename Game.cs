using SplashKitSDK;
using System;
using System.Collections.Generic;

public class Game
{
    private Player _player;
    public bool OnlineGame { get; private set; }
    public bool IsServer { get; private set; }
    public GamePeer ThisPeer { get; private set; }
    private string _otherPlayerMsg;

    private List<OtherPlayer> _otherPlayers = new List<OtherPlayer>();
    private List<string> _otherNetworkNames = new List<string>();

    private string name = "";
    private double x = 0, y = 0, radius = 0;


    public bool Quit
    {
        get
        {
            return _player.Quit;
        }
    }

    public Game(Window window)
    {

        string answer;
        Console.Write("What is your name: ");
        string name = Console.ReadLine();

        do
        {
            Console.Write("Do you want to play it online? (Y/N) ");
            answer = Console.ReadLine();
        }
        while (answer.ToUpper() != "Y" && answer.ToUpper() != "N");

        if (answer.ToUpper() == "N") // Not online game, offline game
        {
            OnlineGame = false;
        }
        else if (answer.ToUpper() == "Y") // Online game
        {
            OnlineGame = true;
            Console.Write("Which port to run at: ");
            ushort port = Convert.ToUInt16(Console.ReadLine());
            GamePeer peer = new GamePeer(port) { Name = name };
            ThisPeer = peer;

            string isHost;
            do
            {
                Console.Write("Is this the host? (Y/N) ");
                isHost = Console.ReadLine();
            } while (isHost.ToUpper() != "Y" && isHost.ToUpper() != "N");

            if (isHost.ToUpper() == "N") // Not host server, select server to connect to
            {
                IsServer = false;
                MakeNewConnection(peer);
            }
            else if (isHost.ToUpper() == "Y") // Be the host server
            {
                IsServer = true;
            }
        }
        

        Player Player = new Player(window, name);
        _player = Player;

        Console.WriteLine("Please switch back to game window!");
    }

    public void HandleInput()
    {
        _player.HandleInput();
    }

    public void Draw()
    {
        _player.Draw();

        if (_otherPlayers.Count > 0)
        {
            foreach (OtherPlayer op in _otherPlayers)
            {
                op.Draw();
            }
        }
    }

    public void Update()
    {
        if (OnlineGame)
        {
            UpdateNetworkInfo();
            UpdateOtherPlayerInfo();
        }

        CheckCollisions();
    }

    private void CheckCollisions()
    {
        foreach (OtherPlayer op in _otherPlayers)
        {
            if (_player.CollidedWithPlayer(op))
            {
                Console.WriteLine("Collided at "+ DateTime.Now);
            }
        }
    }



    #region Network, OtherPlayer
    public void UpdateNetworkInfo()
    {
        if (IsServer)
        {
            SplashKit.AcceptAllNewConnections();
        }
        

        _otherPlayerMsg = ThisPeer.GetNewMessages();

        UpdateOtherPlayer();
        
        BroadcastMessage();
    }

    public void UpdateOtherPlayer()
    {
        if (_otherPlayerMsg != null && _otherPlayerMsg.Length > 0)
        {
            name = _otherPlayerMsg.Split(',')[0];
            x = Convert.ToDouble(_otherPlayerMsg.Split(',')[1]);
            y = Convert.ToDouble(_otherPlayerMsg.Split(',')[2]);
            radius = Convert.ToDouble(_otherPlayerMsg.Split(',')[3]);
        }

        if (name.Length > 0)
        {
            if (!_otherNetworkNames.Contains(name))
            {
                _otherNetworkNames.Add(name);
                _otherPlayers.Add(new OtherPlayer(name, x, y, radius));
            }
        }
    }


    public void UpdateOtherPlayerInfo()
    {
        foreach (OtherPlayer op in _otherPlayers)
        {
            op.X = x;
            op.Y = y;
            op.Radius = radius;
        }
    }

    private void BroadcastMessage()
    {
        ThisPeer.Broadcast($"{_player.Name},{_player.X},{_player.Y},{_player.Radius}");
    }

    private void MakeNewConnection(GamePeer peer)
    {
        string address;
        ushort port;

        Console.Write("Enter Host Server address: ");
        address = Console.ReadLine();

        Console.Write("Enter Host Server port: ");
        port = Convert.ToUInt16(Console.ReadLine());

        peer.ConnectToGamePeer(address, port);
    }

    #endregion
}
