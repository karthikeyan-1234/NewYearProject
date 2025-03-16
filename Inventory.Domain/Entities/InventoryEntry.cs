namespace Inventory.Domain.Entities
{
    /*
        One entry, per purchase detail id or sale detail id, will be added to the inventory.
        This entry will be used to track the quantity of the product in the inventory.
        New entries will be added to the inventory when a purchase is made or a sale is made.
        The quantity of the product will be updated in the inventory based on the type of order.
        ProductId refers to Master product id and can come from to either SaleDetail.ProductId or PurchaseDetail.ProductId, based on OrderType.
        OrderType can be either Purchase or Sale.

    1. New Sale detail added - Entry made into inventory {ProductId, SaleDetail.Id, SaleRefNo, Quantity, EntryDate, OrderType.Sale}
    2. New Purchase detail added - Entry made into inventory {ProductId, PurchaseDetail.Id, PurchaseRefNo, Quantity, EntryDate, OrderType.Purchase}
    3. Sale detail updated - Entry Quantity updated in the inventory based on the difference between the new quantity and the old quantity.
    4. Purchase detail updated - Entry Quantity updated in the inventory based on the difference between the new quantity and the old quantity.
    5. Sale detail deleted - Entry deleted from the inventory.
    6. Purchase detail deleted - Entry deleted from the inventory.
     */

    public class InventoryEntry // A+B+C is a unique key
    {
        public int Id { get; set; }
        public int ProductId { get; set; } //Master ProductId and can be either SaleDetail.ProductId or PurchaseDetail.ProductId - A
        public int OrderItemNo { get; set; } //This can be either SaleDetail.Id or PurchaseDetail.Id - B
        public string? OrderRefNo { get; set; } //This can be either PurchaseRefNo or SaleRefNo - C
        public float Quantity { get; set; }
        public DateTime EntryDate { get; set; } = DateTime.Now;
        public OrderType OrderType { get; set; } //This can be either Purchase or Sale
    }
}
