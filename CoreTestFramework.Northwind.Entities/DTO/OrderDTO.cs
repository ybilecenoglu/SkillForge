namespace CoreTestFramework.Northwind.Entities.DTO
{
    public class OrderDTO 
    {
        public int OrderId { get; set; }
        public string CustomerName {get; set;}
        public string EmployeeName {get; set;}
        public string ShipperName {get; set;}
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public decimal Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
    }
}