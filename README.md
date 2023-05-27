<h1>About</h1>

This project is implemented on the basis of gRPC and some services have the task of analyzing the information received from Kafka, which is exchanged between services after the necessary processing of this information.

<h3>Description</h3>

There are several projects in the solution that we explain their functions
The first project is called the `Chalesh`, in which there is the `MainController` file, which is the so-called central project.It has an API that has an input model and returns a json as an output

The next project is `Chalesh.Core`, which has Kafka models and service written in it. Kafka service has two methods, one is `Producer` and the other is `Consumer`, which are responsible for reading and writing in Kafka.

Another project is called `gRPCService1`. You have five classes. First class by name `FirstRequestService` This class checks a message whose input is a message about introducing service 2. The next class is called `HandeleRequestClient`, this class sends a message to the client and receives a response.

The next class is called `HandleRequestService`. After requesting another service, this class sends a series of processed information in response.

and the last class is the `HelathCheck` class. In this class, there is a method called SendRequestAsync that sends a request to the central server, which contains information such as ID, system time, and the number of connected clients. Also, if this method fails to communicate, it will Bar tries hard
And finally, the final service is `gRPCService2`,This service takes the data received from service 1 and after the necessary analysis, it responds to service 1.
