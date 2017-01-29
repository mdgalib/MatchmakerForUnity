import socket
import six.moves.configparser
import _thread

class server(object) :
    PORT = 25570
    
    def start(self):
        self.CONFIG = six.moves.configparser.ConfigParser()
        self.CONFIG.read('settings.conf')
        self.SERVERS = []
        try:
            self.PORT = int(self.CONFIG.get('settings', 'port'))
            self.GAME = self.CONFIG.get('settings', 'game')
        except six.moves.configparser.NoOptionError as error:
            print('Config file at settings.conf not configured correctly.')
            print(error)
            return
        
        print('Starting {} matchmaking server on port {}'.format(self.GAME, self.PORT))

        self.serversocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.serversocket.bind((socket.gethostname(), self.PORT))
        self.serversocket.listen(5)

        while 1:
            self.update()

    def update(self):
        # Accepts a socket request
        (clientsocket, address) = self.serversocket.accept()
        # Creates a thread for the request to run on
        self.t = _thread.start_new_thread(self.client_thread, (clientsocket, address, ))

    def client_thread(self, clientSocket, Address):
        print("Player connected from {}".format(Address[0]))
        request = clientSocket.recv(1024).decode('utf-8').split(';')
        print('Recieved request {}'.format(request))
        if(request[0] == 'send'):
            # This request is when the client wants to register their game
            self.SERVERS.append(Address[0])
            print('Added address {} to the index'.format(Address[0]))
            clientSocket.send(b'done')
        elif(request[0] == 'get'):
            if(len(self.SERVERS) == 0):
                # This means that there are no servers on the list
                print('No servers...')
                clientSocket.send(b'none')
            else:
                # This means there are servers on the list
                # Sends back the address of the first server on the list
                clientSocket.send(self.SERVERS[0].encode('utf-8'))
        elif(request[0] == 'full'):
            # This means that the server should be removed from the list
            if(len(self.SERVERS)>0):
                if(request[1] == '\r\n'):
                    adrs = Address[0]
                else:
                    adrs = request[1]
                index = 0
                deleted = 0
                while(index < len(self.SERVERS)):
                    if(self.SERVERS[index] == adrs):
                        deleted += 1
                        self.SERVERS.pop(index)
                        index -= 1
                    index += 1
                print('cleared {} occurences of {}'.format(deleted, adrs))
        elif(request[0] == 'status'):
            clientSocket.send(b'yes')
        clientSocket.close()
        