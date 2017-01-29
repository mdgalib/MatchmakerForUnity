import socket
import six.moves.configparser

class server(object) :
    PORT = 25570
    
    def start(self):
        self.CONFIG = six.moves.configparser.ConfigParser()
        self.CONFIG.read('settings.conf')
        self.PORT = int(self.CONFIG.get('settings', 'port'))
        self.GAME = self.CONFIG.get('settings', 'game')
        print('Starting {} matchmaking server on port {}'.format(self.GAME, self.PORT))

        self.serversocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.serversocket.bind((socket.gethostname(), 80))
        self.serversocket.listen(5)

        while 1:
            update()

    def update(self):
        (clientsocket, address) = self.serversocket.accept

        thread = client_thread(clientsocket)
        ct.run()

    def client_thread(self, clientSocket):
        print("Handling the client's request")