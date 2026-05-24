# StyleLoft - Fashion Marketplace Platform
# Members

FEVE LANCE BACLAYONFEVE LANCE BACLAYON
MARK EDZEL BACOCANAGMARK EDZEL BACOCANAG  =====group leader=====
JOREN JAY BULAHANJOREN JAY BULAHAN
JANIEL SENADORJANIEL SENADOR
CLYDE CEDRICK TORRES


## What is StyleLoft?

StyleLoft is a full-stack e-commerce marketplace built with ASP.NET Core MVC that connects fashion designers (sellers/ateliers) with customers. Designers can create an atelier, list their products, manage orders, and update order statuses. Customers can browse products, add them to a cart, checkout, and track their orders.

## Key Features

### For Customers
- User registration and authentication
- Browse products by category and collection
- View product details
- Add products to shopping cart
- Update quantities or remove items from cart
- Checkout with shipping address
- View order history and order details
- Edit user profile and shipping address
- Add profile picture

### For Sellers/Ateliers
- Become an atelier from user profile
- Create and publish products with images, descriptions, and pricing
- Edit existing products
- View all orders containing their products
- See seller-specific order totals (not the entire order total if multiple sellers are involved)
- Update order statuses (Pending → Processing → Shipped → Delivered)
- Stop being an atelier at any time
- Product images on order cards for easy identification

### System Features
- SQL Server database with Entity Framework Core
- ASP.NET Core Identity for user management
- Image upload functionality for products and profile pictures
- Toast notifications for user feedback
- Responsive design
- Free shipping for orders over ₱5,000
- Order number generation with unique identifiers

## System Flow

### Customer Flow
1. **Register/Login**: Create an account or sign in
2. **Browse Products**: Explore the wardrobe (product catalog)
3. **View Product Details**: Click any product to see more information
4. **Add to Cart**: Add products to your shopping bag
5. **Manage Cart**: Update quantities or remove items
6. **Checkout**: Enter or confirm shipping address and place order
7. **Order History**: View past orders and their statuses

### Seller Flow
1. **Become Atelier**: Go to Edit Profile and click "Become Atelier"
2. **Atelier Dashboard**: Access the Seller Atelier to manage products
3. **Create Products**: Add new products with images, descriptions, prices, and stock
4. **Edit Products**: Update existing product information
5. **View Orders**: Go to "View Orders" to see all orders containing your products
6. **Manage Orders**: Click on the status badge to update order status (Pending → Processing → Shipped → Delivered)
7. **Stop Being Atelier**: Go to Edit Profile to stop being a seller (your products will no longer be visible)

## Setup Instructions

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server Express LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (usually included with Visual Studio)
- Visual Studio 2022 or later (recommended) or Visual Studio Code

### Step 1: Clone or Download the Project
Download or clone the StyleLoft repository to your local machine.

### Step 2: Open the Project
Open `StyleLoft.slnx` in Visual Studio, or open the project folder in Visual Studio Code.

### Step 3: Restore Dependencies
The project should automatically restore NuGet packages when opened. If not, run:
```bash
dotnet restore
```

### Step 4: Apply Database Migrations
The project uses Entity Framework Core migrations to create the database. Open Package Manager Console in Visual Studio and run:
```powershell
Update-Database
```

Or run from the command line in the project directory:
```bash
dotnet ef database update
```

This will create the `StyleLoft` database in your LocalDB instance.

### Step 5: Configure User Secrets (Optional)
For production, you should configure proper secrets, but for development, the default connection string in `appsettings.json` should work:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StyleLoft;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### Step 6: Run the Application
Press `F5` in Visual Studio or run from the command line:
```bash
dotnet run
```

The application will start at `https://localhost:5001` (or `http://localhost:5000`).

### Step 7: Create an Account
1. Navigate to the application in your browser
2. Click "Register" to create a new account
3. (Optional) To test seller features, register a second account and become an atelier

## Project Structure


## Technologies Used
- **ASP.NET Core MVC 10** - Web framework
- **Entity Framework Core 10** - ORM for database access
- **ASP.NET Core Identity** - User authentication and authorization
- **SQL Server Express LocalDB** - Database
- **Razor Views** - Server-side HTML rendering
- **C#** - Programming language

## Default Shipping Rules
- **Shipping Cost**: ₱120 flat rate
- **Free Shipping**: For orders with subtotal over ₱5,000

## Troubleshooting
- **Database not found**: Make sure LocalDB is installed and running. Try re-running `Update-Database`
- **Can't upload images**: Ensure the `wwwroot/images` folders have write permissions
- **Build errors**: Make sure .NET 10 SDK is installed
- **App won't start**: Check that no other process is using port 5000 or 5001

## License
This project is for educational and demonstration purposes.