using System.Diagnostics.CodeAnalysis;

namespace MicroserviceProject.Shared.Enums;

public enum OrderStatus
{
    Pending = 1,
    Processing,
    Shipped,
    Delivered,
    Cancelled,
    Returned,
    OnHold,
    Completed,
    None
}

[SuppressMessage("ReSharper", "StringLiteralTypo")]
public static class OrderStatusHelper
{
    public static Dictionary<int, string> OrderStatusString;
    
    public static Dictionary<int, string> OrderStatusDescriptions;

    static OrderStatusHelper()
    {
        OrderStatusString = new Dictionary<int, string>()
        {
            { 1, "Pending" },
            { 2, "Processing" },
            { 3, "Shipped" },
            { 4, "Delivered" },
            { 5, "Cancelled" },
            { 6, "Returned" },
            { 7, "OnHold" },
            { 8, "Completed" },
            { 9, "None" }
        };
        
        OrderStatusDescriptions = new Dictionary<int, string>()
        {
            { 1, "Sipariş henüz işleme alınmadı." },
            { 2, "Sipariş işleme alındı ve hazırlanıyor." },
            { 3, "Sipariş gönderildi." },
            { 4, "Sipariş teslim edildi." },
            { 5, "Sipariş iptal edildi." },
            { 6, "İade edildi." },
            { 7, "Geçici bekleme durumu." },
            { 8, "Tüm işlemler tamamlandı. (Done=true)" },
            { 9, "Hiçbir duruma uymayan veya henüz belirlenmeyen durum." }
        };
    }
}