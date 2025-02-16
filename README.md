# Task Management System

[![License](https://img.shields.io/github/license/potatoscript/TaskManagementSystem)](LICENSE)
[![Build Status](https://img.shields.io/github/actions/workflow/status/potatoscript/TaskManagementSystem/build.yml)](https://github.com/potatoscript/TaskManagementSystem/actions)
[![Contributors](https://img.shields.io/github/contributors/potatoscript/TaskManagementSystem)](https://github.com/potatoscript/TaskManagementSystem/graphs/contributors)

## Overview

The **Task Management System** is a robust, full-featured application designed to help teams and individuals efficiently manage and track tasks. It supports creating, assigning, updating, and monitoring tasks in real-time, making it a perfect solution for project management, agile workflows, and personal productivity.

## Features

- **Task Creation and Assignment**: Create tasks with detailed descriptions, priorities, and deadlines.
- **Team Collaboration**: Assign tasks to team members and track progress collaboratively.
- **Real-Time Updates**: Monitor task statuses in real-time.
- **Customizable Boards**: Organize tasks into customizable kanban-style boards.
- **User Authentication**: Secure login and user-specific task management.
- **Reporting and Analytics**: Gain insights into task performance and team productivity.
- **Responsive Design**: Accessible on desktop, tablet, and mobile devices.

## Demo

🎥 [Click here to watch the demo video](https://youtu.be/l6kjuRGdE9g?si=ZvcerMOhOQRrBZMA)  
📸 Screenshots are available in the [Wiki](https://github.com/potatoscript/TaskManagementSystem/wiki/Screenshots).

## Technology Stack

The project leverages modern technologies to ensure performance, scalability, and security:

### Backend
- **Language**: C#
- **Framework**: ASP.NET Core
- **Database**: SQL Server or SQLite (configurable)
- **API**: RESTful API with Swagger documentation

### Frontend
- **Framework**: React.js
- **Styling**: Tailwind CSS
- **State Management**: Redux Toolkit
- **Build Tool**: Vite.js

### DevOps
- **Containerization**: Docker
- **CI/CD**: GitHub Actions
- **Version Control**: Git

## Setup Guide

Follow these steps to set up the Task Management System locally:

### Prerequisites
- Node.js (v16+)
- .NET 6 SDK
- Docker (optional for containerized setup)
- SQL Server or SQLite

### Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/potatoscript/TaskManagementSystem.git
   cd TaskManagementSystem
   ```

2. **Backend Setup**
   - Navigate to the backend folder:
     ```bash
     cd backend
     ```
   - Restore dependencies:
     ```bash
     dotnet restore
     ```
   - Update the `appsettings.json` file with your database configuration.
   - Run the application:
     ```bash
     dotnet run
     ```

3. **Frontend Setup**
   - Navigate to the frontend folder:
     ```bash
     cd frontend
     ```
   - Install dependencies:
     ```bash
     npm install
     ```
   - Start the development server:
     ```bash
     npm run dev
     ```

4. **Access the Application**
   - Backend: [http://localhost:5000](http://localhost:5000)
   - Frontend: [http://localhost:5173](http://localhost:5173)

---

## Usage

1. **Create a New Account**: Sign up to get started.
2. **Create Tasks**: Use the task creation form to add tasks.
3. **Organize Tasks**: Drag and drop tasks into kanban columns.
4. **Assign Tasks**: Allocate tasks to specific team members.
5. **Track Progress**: Monitor task completion and deadlines.

---

## Contributing

Contributions are welcome! Please follow these steps to contribute:

1. Fork the repository.
2. Create a new branch:
   ```bash
   git checkout -b feature/my-new-feature
   ```
3. Commit your changes:
   ```bash
   git commit -m "Add new feature"
   ```
4. Push to the branch:
   ```bash
   git push origin feature/my-new-feature
   ```
5. Submit a pull request.

For detailed contribution guidelines, see [CONTRIBUTING.md](CONTRIBUTING.md).

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## Support

If you encounter any issues or have questions, feel free to:

- Open an issue on the [Issues page](https://github.com/potatoscript/TaskManagementSystem/issues)
- Reach out via email: [potatoscript@support.com](mailto:potatoscript@support.com)

---

## Roadmap

- **Phase 1**: Core Task Management Features ✅
- **Phase 2**: Analytics and Reporting ✅
- **Phase 3**: Real-Time Notifications ⏳
- **Phase 4**: Mobile Application Support ⏳

Check the [Project Board](https://github.com/potatoscript/TaskManagementSystem/projects) for updates.

---

## Acknowledgements

Special thanks to all contributors and the open-source community for making this project possible. ❤️

---

## Connect with Us

🌐 [Website](https://github.com/potatoscript)  
🐦 [Twitter](https://twitter.com/potatoscript)  
💻 [LinkedIn](https://linkedin.com/company/potatoscript)

