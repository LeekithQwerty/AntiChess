# AntiChess
A multiplayer variant of chess 


Sem 5- MINI PROJECT Grade-A+ 
This game is based the variant of chess which involves sacrificing chess pieces to win the game. It requires high stratergy play. Created in SEM 5 this game is still being developed to add single player feature. Unfortunately, the authentication method provided by google was deprecated in October 2022, hence I will be incorporating the Loopback API method soon. But you can enjoy playing the game as 2 player local player till I resolve the issue here: 
https://likhitjha.itch.io/anti-chess-beta-version


Core Game Features
1) All chess pieces have traditional moves.
2) King has no special power. Checks and checkmate doesn't exist 
3) Rules like promotion, time etc exists in the game 
4) A player must eliminate a piece if they have the ability to do so, he cannot move other pieces. 


Authentication
Google Proivder oAuth2.0 using REST client
REST client was used intead of Firebase.Auth.FirebaseAuth as authentication can be doen through any application.
Custom Copy/Paste Method [Deprecated] Removed in Oct 2022
Steps: 
1) Get Google code
2) Choose an account
3) Copy Authorization code manually 
4) Paste the code in the game & sign in. (Exchange code for Token)
5) ID Token Generated to call API 

Database
Firebase (NOSQL) database: To store user credentials and game data 
Game Data:
1) UserName
2) Score
3) MatchesWon
4) Enemy
5) Moves

Libraries used:
REST Client
FullSerializer







