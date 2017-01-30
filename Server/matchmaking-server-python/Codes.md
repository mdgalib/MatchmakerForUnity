# fredfishgames matchmaking protocol codes

These are the only way of communication between the server and the client. Each request must return either one or zero, to indicate whether the previous command was successful.

| Code | Title | Description |
| --- | --- | --- |
| 0 | Failed | Sent from the server to the client, when the previous command failed. |
| 1 | Successful | Sent from the server to the client, when the previous command succeeded. |
| 2 | Get Status | Sent from the client to the server, requesting the status of the server. |
| 3 | Get Server | Sent from the client to the server, requesting a server to play on. |
| 4 | Register server | Sent from the client to the server, telling the server to add the client's game to the game index. |
| 5 | Clear server | Sent from the client to the server, telling the server to clear the client's game from the game index. |

A full command is structured as an 8 bit section (representing one of the codes above) Then 4 8 bit sections, which can represent an IP address.

| Code | Binary |
| --- | --- |
| 001255255255255 | 0000 0001 1111 1111 1111 1111 1111 1111 1111 1111 |

This tells the client that there is a server of the ip 255.255.255.255 that it should join.

| | Command | Data | | | |
| --- | --- | --- | --- | --- | --- |
| Code | 1 | 255 | 255 | 255 | 255
| Binary | 00000001 | 11111111 | 11111111 | 11111111 | 11111111 |

Overall, the length of one request / response is 5 bytes.
