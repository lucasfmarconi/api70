# API70

API70 is a project to perform some proof of concepts. It's written to follow the Clean Architecture software pattern.<br>
This is a RESTful API that provides the base API from the .NET framework project template. It is built using .NET Core 7.0 and utilizes
a RabbitMQ broker to publish messages. The application requires a RabbitMQ instance to be running, which can be provided by the 
docker-compose.yml file written with the Docker Compose for Visual Studio support.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)

## Installation

To install and run the API locally, follow these steps:

1. Clone the repository: `git clone https://github.com/lucasfmarconi/api70.git`
2. Navigate to the `api70` directory: `cd api70`
3. Start the RabbitMQ instance using Docker Compose: `docker-compose up -d`
4. Build the application: `dotnet build`
5. Start the application: `dotnet run`

The server should now be running on `http://localhost:5000`.

## Usage

API70 provides a Swagger which helps with the Web API usage:

For example, using the docker-compose project [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html)

You can use tools like Postman or CURL to interact with the API.

## API Documentation

API documentation is available by opening `index.html` Swagger in your web browser.

## Contributing

Contributions to API70 are welcome! To contribute, please follow these steps:

1. Fork the repository.
2. Create a new branch: `git checkout -b my-branch-name`
3. Make your changes and commit them: `git commit -m 'Add some feature'`
4. Push to the branch: `git push origin my-branch-name`
5. Submit a pull request.

## License

API70 is licensed under the MIT license. See the `LICENSE` file for more information.
