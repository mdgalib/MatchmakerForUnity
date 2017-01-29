import server

def start():
    startmessage = '''fredfishgames matchmaking server for unity
Licenced under the MIT licence
Make sure the configuration file has been set up before starting'''
    # Start the server here!
    print(startmessage)
    s = server.server()
    s.start()
start()