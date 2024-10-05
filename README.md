# 📦 Portfolio.Core

![License: Only for Interview Purposes](https://github.com/kmakatsoris/Portfolio.Core)

## 🌐 Live Demo

Not included, you need to clone the project -> Build it -> Run it -> Auto Navigation (Or url incl in launchSettings.json file) to Swagger UI interface

## 📝 Table of Contents

- [About the Project](#about-the-project)
  - [Requirements](#requirements)
  - [Optional](#optional)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Screenshots](#screenshots)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)
- [Acknowledgments](#acknowledgments)

## 📖 About the Project

Your task is to develop an API aggregation service that consolidates data from multiple external APIs and provides a unified endpoint to access the aggregated information. The goal is to create a scalable and efficient system that fetches data from various sources, aggregates it, and delivers it through a single API interface.

### 📖 Requirements

- Develop a .NET-based API aggregation service using ASP.NET Core.
- Design and implement an architecture that allows for easy integration of new APIs.
- Implement functionality to fetch data from at least three different external APIs simultaneously. You can choose any APIs you prefer (e.g., weather, news, social media, etc.).
- Create an API endpoint that allows users to retrieve the aggregated data.
- Provide the option to filter and sort the aggregated data based on specified parameters (e.g., date, relevance, category).
- Apply proper error handling and implement a fallback mechanism in case any of the external APIs are unavailable.
- Write unit tests to ensure the reliability and correctness of the codebase.
- Document the API aggregation service, including its endpoints, input/output formats, and any necessary setup or configuration instructions.

### 📖 Optional

- Utilize caching techniques to optimize performance and minimize redundant API calls.
- Utilize parallelism to decrease response times.
- Create an API endpoint to retrieve request statistics. Specifically, return for each API the total number of requests and the average response time, grouped in performance buckets (e.g. fast <100ms, average 100-200ms slow > 200ms – the values are indicative feel free to tweak). For the purposes of the exercise source data can be stored in memory.

### 🎯 Features

- Creating this application for interview purposes
- Updates Not Coming Because i am awaiting feedback to remove it from my GitHub Repositories.
- Easy integration of more External APIs changing only the appSetting.
- Easy way to check the caching mechanism just executing two times one endpoint from swagger or one of the two endpoints and then the other -> Checking times or debug to see that the cache includes items after the first execution of any of the two available endpoints.
- There is not simple way to remove multithreading -> I recommend to move between git commits or just remove 3 lines of code.

## 🛠 Tech Stack

### **Backend**

- **Languages:** C#
- **Frameworks:** .NET Core
- **Database:** None
- **API:** RESTful

## 🚀 Getting Started

### Prerequisites

- .NET SDK

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/kmakatsoris/AgileActors.AggregationApp.git
   cd AgileActors.AggregationApp
   ```

2. Build the application:

   ```bash
   dotnet build
   ```

3. Start the application:

   ```bash
   dotnet run
   ```

4. Open your browser and navigate to `http://localhost:5123/swagger/index.html`.

## 📸 Screenshots

![Portfolio Database](https://github.com/user-attachments/assets/2e99b732-bc76-41aa-83e5-adc294a35a9d)
![Solution Architecture](https://github.com/user-attachments/assets/749464e5-31fa-44c2-839a-c1f2605f0db0)

## 🤝 Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

## 📜 License

Distributed under the MAKATSORIS KONSTANTINOS & Agile Actors Greece License. Only for Interviews and MAKATSORIS KONSTANTINOS's Skills Preview Purposes.

## 📧 Contact

Email - [kmakatsoris@gmail.com](mailto:kmakatsoris@gmail.com)

Welcome Project Link: [Welcome Repository Link](https://github.com/kmakatsoris/AgileActors.AggregationApp)

## 🙏 Acknowledgments

---

### How to Use:

1. Only for Interviews and MAKATSORIS KONSTANTINOS's Skills Preview Purposes.

THANK YOU | Hope to see you soon | Be SAFE 🙏
