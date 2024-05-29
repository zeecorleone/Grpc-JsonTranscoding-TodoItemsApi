Great! Let's update the README to reflect that this project uses .NET 8. Here's the revised version:

---

# Grpc-JsonTranscoding-TodoItemsApi ![gRPC](https://img.shields.io/badge/gRPC-Web-blue) ![Protobuf](https://img.shields.io/badge/Protobuf-3.0-lightgrey) ![License](https://img.shields.io/github/license/zeecorleone/Grpc-JsonTranscoding-TodoItemsApi)

## Introduction
Welcome to the **Grpc-JsonTranscoding-TodoItemsApi** repository! This project demonstrates how to create a ToDo API using gRPC web services and extend it with JSON Transcoding to make it accessible from browsers and other HTTP-based clients.

## Features
- **gRPC Web Service**: Leverage gRPC for efficient, high-performance communication.
- **JSON Transcoding**: Access the gRPC service from browsers and other HTTP clients using JSON.
- **Protobuf**: Use Protocol Buffers for defining the API contract.
- **.NET 8**: Utilize the latest features and improvements of .NET 8.

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ![Download .NET 8](https://img.shields.io/badge/Download-.NET%208-blue)
- [Protobuf Compiler](https://grpc.io/docs/protoc-installation/) ![Download Protobuf](https://img.shields.io/badge/Download-Protobuf-lightgrey)

### Installation
1. **Clone the repository**
    ```bash
    git clone https://github.com/zeecorleone/Grpc-JsonTranscoding-TodoItemsApi.git
    cd Grpc-JsonTranscoding-TodoItemsApi
    ```

2. **Restore dependencies**
    ```bash
    dotnet restore
    ```

3. **Run the application**
    ```bash
    dotnet run
    ```

### Configuration
This project uses `todo.proto` for defining the gRPC service contract. Make sure you have the Protobuf compiler installed to compile the `.proto` files if you make any changes.

#### Example `todo.proto` file:
```protobuf
syntax = "proto3";

option csharp_namespace = "TodoGrpc";
import "google/api/annotations.proto";

package todoit;

service ToDoIt {

    //Create
    rpc CreateToDo(CreateToDoRequest) returns (CreateToDoResponse) {
        option (google.api.http) = {
            post: "/v1/todo",
            body: "*"
        };
    }

    . . . . //other RPC methods

}

message CreateToDoRequest {
    string title = 1;
    string description = 2;
}

message CreateToDoResponse {
    int32 id = 1;
}


. . . //other messages

```

## Usage
Once the application is running, you can access the gRPC service via gRPC clients, and the JSON Transcoded endpoints can be accessed via HTTP clients like browsers, `curl`, or Postman.

## Repository Structure
```
Grpc-JsonTranscoding-TodoItemsApi/
├── Protos/
│   └── todo.proto
├── Services/
│   └── TodoService.cs
├── wwwroot/
├── appsettings.json
└── Program.cs
```

## Contributing
Contributions are welcome! Please submit a pull request or open an issue to discuss what you would like to change.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

