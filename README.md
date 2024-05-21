## About This Project

This project is a re-engineered version of an existing .NET application designed to manage a bus shuttle system. The primary goal is to enhance the system with new functionalities, improve its performance, and ensure a robust, scalable architecture. Key features include:

- **Manager Interface**: Allows managers to perform CRUD operations on various entities, view comprehensive reports, and manage user activations.
- **Driver Interface**: Enables drivers to select routes, log entries for each stop, and automatically advance the stop dropdown for efficient data submission.
- **Authentication and Authorization**: Implements role-based access to ensure that only authorized users can access specific parts of the system.
- **Persistence**: Data is stored in SQLite, with the option to switch to SQL Server if needed.
- **Testing and Logging**: Each layer of the application is independently tested, and extensive logging ensures that all system events are appropriately tracked.
- **Dependency Injection**: Enhances modularity and ease of testing by injecting dependencies where necessary.
- **User-friendly Design**: The application features a clean and intuitive interface for both managers and drivers.

Additional enhancements include publishing the application on an EC2 instance and integrating Google Maps to visualize bus routes and the most crowded stops.

## 🚧 Features Not Yet Added: 🚧  

- Testing    
- Adding Stops and Loops to Routes  
- Entry logging  
- Updated CSS styling  
- EC2 instance  
- Google Maps integration  

## Password Requirements ##  
- at least 8 characters  
- at least one uppercase letter  
- at least one numeral  
- at least one special character  

***Manager Login:***  
  nwheeler@email.com  
  Password123!  

***Driver Login:***  
  jdoe@email.com  
  Password123!  
