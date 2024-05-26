using MassTransit;
using System.Reflection;
using System.Text;

namespace SagaStateMachineWorkerService.Models
{
    public class OrderStateInstance : SagaStateMachineInstance, ISagaVersion
    {
        public Guid CorrelationId { get; set; }

        public int Version { get; set; }

        public string CurrentState { get; set; }

        public string BuyerId { get; set; }

        public int OrderId { get; set; }

        public Payment Payment { get; set; }

        public override string ToString()
        {
            var properties = GetType().GetProperties();

            var stringBuilder = new StringBuilder();

            PropertiesToString(ref stringBuilder, properties, this);

            return stringBuilder.ToString();
        }

        private StringBuilder PropertiesToString(ref StringBuilder stringBuilder, IEnumerable<PropertyInfo> properties, object instance, string parentName = "")
        {
            foreach (var property in properties)
            {
                if (new Type[] { typeof(Payment) }.Contains(property.PropertyType))
                {
                    var innerProperties = property.PropertyType.GetProperties();
                    var innerInstance = property.GetValue(instance);
                    PropertiesToString(ref stringBuilder, innerProperties, innerInstance, property.Name);
                    continue;
                }

                string propertyName = string.IsNullOrEmpty(parentName) ? property.Name : $"{parentName}_{property.Name}";
                stringBuilder.AppendLine($"{propertyName}: {property.GetValue(instance)}");
            }

            return stringBuilder;
        }
    }
}
