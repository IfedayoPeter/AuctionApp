AuctionApp
Description
The online auction market requires an improved communication system to enable effective real-time interactions among users during bidding. The system allows users to enter a bidding room, submit bids, and notify the highest bidder at the end of the auction. Furthermore, it automatically creates an invoice for the highest bidder.

Technologies
.NET 8.0
Entity Framework
RabbitMQ
Docker
Microsoft SQL Database
WebSocket
Swagger
API Endpoints
Auction
POST /api/v{version}/auction/Auction/create_auction
GET /api/v{version}/auction/Auction/get_auction_by_code
GET /api/v{version}/auction/Auction/get_auctions
GET /api/v{version}/auction/Auction/get_active_auctions
GET /api/v{version}/auction/Auction/get_auction_results
GET /api/v{version}/auction/Auction/end_auctions
GET /api/v{version}/auction/Auction/start_auctions
PUT /api/v{version}/auction/Auction/update_auction
Bid
POST /api/v{version}/bid/Bid/submit_bid
GET /api/v{version}/bid/Bid/get_bid_by_code
GET /api/v{version}/bid/Bid/get_bids
GET /api/v{version}/bid/Bid/get_highest_bids
PUT /api/v{version}/bid/Bid/update_bid
BidRoom
POST /api/v{version}/bidRoom/BidRoom/create_participant
POST /api/v{version}/bidRoom/BidRoom/create_bidRoom
GET /api/v{version}/bidRoom/BidRoom/get_bidRoom_by_code
GET /api/v{version}/bidRoom/BidRoom/get_bidRooms
GET /api/v{version}/bidRoom/BidRoom/get_active_bidRooms
GET /api/v{version}/bidRoom/BidRoom/get_active_participants
POST /api/v{version}/bidRoom/BidRoom/exit_bidRoom
POST /api/v{version}/bidRoom/BidRoom/enter_bidRoom
POST /api/v{version}/bidRoom/BidRoom/update_bidRoom
Login
POST /UserLogin
Notification
GET /api/v{version}/notification/Notification/get_notification_by_user_code
GET /api/v{version}/notification/Notification/mark_as_read
User
POST /api/v{version}/user/User/create_buyer_account
POST /api/v{version}/user/User/create_seller_account
GET /api/v{version}/user/User/get_buyers
GET /api/v{version}/user/User/get_sellers
GET /api/v{version}/user/User/get_user_by_code
GET /api/v{version}/user/User/get_user_by_username
DELETE /api/v{version}/user/User/delete_account
Setting Up the Project
Cloning the GitHub Repository
Clone the repository:

bash
Copy code
git clone <repository-url>
cd AuctionApp
Open the project in your preferred IDE (e.g., Visual Studio, Visual Studio Code).

Restore NuGet packages:

bash
Copy code
dotnet restore
Set up the database:

Ensure Microsoft SQL Server is running.

Update the connection string in appsettings.json:

json
Copy code
"ConnectionStrings": {
  "DefaultConnection": "Server=your_server;Database=AuctionAppDb;User Id=your_user;Password=your_password;"
}
Apply migrations:

bash
Copy code
dotnet ef database update
Run the project:

bash
Copy code
dotnet run
The application should be running at https://localhost:7270.

Running with Docker
Ensure Docker is installed and running on your machine.

Build the Docker image:

bash
Copy code
docker build -t auctionapp .
Run the Docker container:

bash
Copy code
docker run -d -p 7270:80 --name auctionapp-container auctionapp
The application should be running at http://localhost:7270.

Swagger
To access the Swagger UI for API documentation, navigate to https://localhost:7270/swagger.

WebSocket
To connect to the WebSocket server, use a WebSocket client (e.g., wscat):

bash
Copy code
wscat -c ws://localhost:7270/ws
Docker Image Repository
The Docker image for this project can be pulled from the provided Docker image repository link.

Contributing
Fork the repository.
Create a feature branch (git checkout -b feature/AmazingFeature).
Commit your changes (git commit -m 'Add some AmazingFeature').
Push to the branch (git push origin feature/AmazingFeature).
Open a Pull Request.
License
Distributed under the MIT License. See LICENSE for more information.

Contact
Email - awoniyiifedayopeter@gmail.com

