# fredfishgames unity matchmaker
This is an alternative to the unity matchmaker, which manages multiplayer and puts players into lobbies.

**Please note, some C#, unity, git and networking knowledge is required.**

# Client setup
Download the /Client/Unity/MatchmakerForUnityXX.unitypackage, and open it while your multiplayer project is open.
For basic use, only edit the Matchmaking.cs MonoBehaviour script.
To configure, open /Scripts/Matchmaking.cs, and edit the `OnGettingStatus`, `OnStartClient`, `OnStartServer` and `OnClearServer` methods. These are called when the request has been sent, and can be used to open any loading UI. Note: all the code in these methods is not important, and can be changed to whatever you want. I've just left my use of them as an example.

Next, edit the `GetServerCallback`, `SendServerCallback`, `GetStatusCallback` and `ClearServerCallback` methods. The `args` parameter is set to zero if it failed, and to one if the request succeded, with the exception of the `address` parameter, which is the IP address recieved.

Finally, set the `Address` and `Port` variables in the unity editor, to the address and port of your server.

# Server setup
Clone the repo into your home directory.
## Linux
Create a `start.sh` script in your home directory, which contains `sudo python3 /home/USERNAME/MatchmakerForUnity/Server/matchmaking-server-python/matchmaking_server_python.py & ` replace `USERNAME` with your username.
Next, start the server by running the command `sh start.sh` in your home directory.
An error message should appear, and the script should create a `ffgmsconfig.conf` file in your home directory. Edit this to match your needs.
Reboot your system.

**If you would like your server to be used outside of the local network, ensure that the port has been forwarded on your router**
