using System.Runtime.Serialization;

namespace APIRestaurante.Domain.Entities
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Cooking")]
        Cooking,

        [EnumMember(Value = "Ready")]
        Ready,

        [EnumMember(Value = "Delivered")]
        Delivered,

        [EnumMember(Value = "Cancelled")]
        Cancelled
    }
}
