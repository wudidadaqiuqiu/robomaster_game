using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
namespace ROS.ROSConnect {
    public class ROSConnector
    {
        public static ROSConnection connection = ROSConnection.GetOrCreateInstance();

        public static void register_publisher<T>(string topic) where T : Message {
            connection.RegisterPublisher<T>(topic);
        }

        public static void publish(string rosTopicName, Message message) {
            connection.Publish(rosTopicName, message);
        }

    }
}