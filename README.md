<h1 align="center">Auction MarketApp</h1>
<h3 align="center">This is a backend C# application created with .Net8. The application allows users to create account, login, create auctions and palce bids.</h3>

# Features
1. Create users
2. Login users
3. Manage auctions through CRUD operations (Create, Read, Update, Delete).
4. Get realtime notifications during bidding process
5. Utilize MySQL database for data storage.
6. User authentication using JWT tokens
7. Swagger for Api documentation
8. RabbitMQ and websocket for messages

# Technologies Used
1. C#
2. .NET 8
3. Entity Framework
4. MySQL Database
5. RabbitMQ
6. Websocket
7. JWT auth
8. Swagger
9. Docker

# Prerequisites
Before running the application, make sure you have the following software installed:

.Net 8
<br/>
Visual studio
<br/>
Mysql server
<br/>
Microsoft sql management studio
<br/>
Docker
<br/>
RabbitMQ docker image

# Getting Started
<h3>To get started with the Application, follow these steps:

Clone the repository:
git clone https://github.com/IfedayoPeter/AuctionApp

using cmd navigave to project folder and run dotnet build to install the necessary dependencies

Restore NuGet packages

Install wscat:
npm install -g wscat

Ensure Microsoft SQL Server is running.

Update the connection string in appsettings.json:

 "ConnectionStrings": { "DefaultConnection": "Server=your_server;Database=AuctionAppDb;User Id=your_user;Password=your_password;" } 

Apply migrations

run your project with visual studio

using cmd, connect to web socket to stream data created in real time:
wscat -c ws://localhost:7270/ws

Access the application in your web browser:
http://localhost:7270/swagger/index.html<h3/>

# Running with Docker 

<h3>Ensure Docker is installed and running on your machine.<h3/>

<h3>Build the Docker image<h3/>


# Configuration
The application uses the default configuration provided by .Net. However, if you need to modify any settings, you can do so in the application.json file located in the root folder of the project.

# API Endpoints
The following API endpoints are available for interacting with the application:
<br>
POST /api/v{version}/auction/Auction/create_auction 
<br/>
GET /api/v{version}/auction/Auction/get_auction_by_code 
<br/>
GET /api/v{version}/auction/Auction/get_auctions 
<br/>
GET /api/v{version}/auction/Auction/get_active_auctions 
<br/>
GET /api/v{version}/auction/Auction/get_auction_results 
<br/>
GET /api/v{version}/auction/Auction/end_auctions 
<br/>
GET /api/v{version}/auction/Auction/start_auctions 
<br/>
PUT /api/v{version}/auction/Auction/update_auction Bid 
<br/>
POST /api/v{version}/bid/Bid/submit_bid 
<br/>
GET /api/v{version}/bid/Bid/get_bid_by_code 
<br/>
GET /api/v{version}/bid/Bid/get_bids 
<br/>
GET /api/v{version}/bid/Bid/get_highest_bids 
<br/>
PUT /api/v{version}/bid/Bid/update_bid BidRoom 
<br/>
POST /api/v{version}/bidRoom/BidRoom/create_participant 
<br/>
POST /api/v{version}/bidRoom/BidRoom/create_bidRoom 
<br/>
GET /api/v{version}/bidRoom/BidRoom/get_bidRoom_by_code 
<br/>
GET /api/v{version}/bidRoom/BidRoom/get_bidRooms 
<br/>
GET /api/v{version}/bidRoom/BidRoom/get_active_bidRooms 
<br/>
GET /api/v{version}/bidRoom/BidRoom/get_active_participants 
<br/>
POST /api/v{version}/bidRoom/BidRoom/exit_bidRoom 
<br/>
POST /api/v{version}/bidRoom/BidRoom/enter_bidRoom 
<br/>
POST /api/v{version}/bidRoom/BidRoom/update_bidRoom Login 
<br/>
POST /UserLogin Notification 
<br/>
GET /api/v{version}/notification/Notification/get_notification_by_user_code 
<br/>
GET /api/v{version}/notification/Notification/mark_as_read User
<br/>
POST /api/v{version}/user/User/create_buyer_account 
<br/>
POST /api/v{version}/user/User/create_seller_account 
<br/>
GET /api/v{version}/user/User/get_buyers GET /api/v{version}/user/User/get_sellers 
<br/>
GET /api/v{version}/user/User/get_user_by_code 
<br/>
GET /api/v{version}/user/User/get_user_by_username 
<br/>
DELETE /api/v{version}/user/User/delete_account 


# Contributing 

<h3>Fork the repository. 
Create a feature branch (git checkout -b feature/AmazingFeature). 
Commit your changes (git commit -m 'Add some AmazingFeature'). 
Push to the branch (git push origin feature/AmazingFeature). 
Open a Pull Request. <h3/>

License Distributed under the MIT License. See LICENSE for more information.

Contact Email - awoniyiifedayopeter@gmail.com
