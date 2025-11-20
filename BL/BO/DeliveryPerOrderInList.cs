namespace BO
{
    public class DeliveryPerOrderInList
    {
        public int DeliveryId { get; init; }
        public int CourierId { get; init; }
        public string? CourierName { get; init; }
        public EnumDeliveryMethod DeliveryMethod { get; init; }
        public DateTime DelCreationTime { get; init; }
        public EnumEndDeliveryStatus? EndDeliveryStatus { get; init; }
        public DateTime? EndDeliveryTime { get; init; }


    }
}
