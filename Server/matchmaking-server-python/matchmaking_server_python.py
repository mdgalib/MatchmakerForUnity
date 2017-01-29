import server

def start():
    # Start the server here!
    print('fredfishgames matchmaking server for unity.')
    s = server.server()
    s.start()
start()