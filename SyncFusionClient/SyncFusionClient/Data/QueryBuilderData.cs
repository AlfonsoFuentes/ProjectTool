using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncFusionClient.Data
{
    public class HardWareDetails
    {
        public HardWareDetails()
        {

        }
        public HardWareDetails(int TaskID, string Name, string AssignedTo, string Note, string Category, DateTime WEO, string SerialNo, string InvoiceNo, DateTime DOP, string Status)
        {
            this.TaskID = TaskID;
            this.Name = Name;
            this.Note = Note;
            this.AssignedTo = AssignedTo;
            this.WEO = WEO;
            this.Category = Category;
            this.SerialNo = SerialNo;
            this.InvoiceNo = InvoiceNo;
            this.DOP = DOP;
            this.Status = Status;
        }
        public static List<HardWareDetails> GetAllRecords()
        {
            List<HardWareDetails> order = new List<HardWareDetails>();
            int code = 1000;
            for (int i = 1; i < 10; i++)
            {
                order.Add(new HardWareDetails(code + 1, "Lenovo Yoga", "John Doe", "Remarks are noted", "Laptop", new DateTime(2018, 05, 15), "CB27932009", "INV-0984", new DateTime(2021, 7, 16), "Assigned"));
                order.Add(new HardWareDetails(code + 2, "Acer Aspire", "David Anto", "Remarks are noted", "Monitor", new DateTime(2018, 04, 04), "CB35728290", "INV-2712", new DateTime(2020, 9, 11), "In-repair"));
                order.Add(new HardWareDetails(code + 3, "Apple MacBook", "Mary Saveley", "Remarks are noted", "Tablet", new DateTime(2018, 11, 30), "CB35628728", "INV-3782", new DateTime(2021, 10, 7), "Pending"));
                order.Add(new HardWareDetails(code + 4, "Lenovo ThinkPad", "Pascale Cartrain", "Remarks are noted", "Others", new DateTime(2018, 10, 22), "CB56209872", "INV-2980", new DateTime(2021, 12, 30), "Ordered"));
                order.Add(new HardWareDetails(code + 5, "Dell Inspiron", "Paul Henriot", "Remarks are noted", "Laptop", new DateTime(2017, 02, 18), "CB56289036", "INV-2763", new DateTime(2021, 12, 3), "Assigned"));
                order.Add(new HardWareDetails(code + 6, "HP Pavilion", "Mary Saveley", "Remarks are noted", "Tablet", new DateTime(2017, 02, 18), "CB56289305", "INV-3456", new DateTime(2021, 12, 3), "Pending"));
                order.Add(new HardWareDetails(code + 7, "Asus ZenBook", "John Doe", "Remarks are noted", "Monitor", new DateTime(2019, 02, 18), "CB25637891", "INV-2878", new DateTime(2021, 12, 3), "In-repair"));
                code += 7;
            }
            return order;
        }

        public int TaskID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty;
        public DateTime WEO { get; set; }
        public string Category { get; set; } = string.Empty;
        public string SerialNo { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime DOP { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ShipAddress { get; set; } = string.Empty;
    }
    public class Orders
    {
        public Orders()
        {

        }
        public Orders(int OrderID, string CustomerId, int EmployeeId, decimal Freight, bool Verified, DateTime OrderDate, string ShipCity, string ShipName, string ShipCountry, DateTime ShippedDate, string ShipAddress)
        {
            this.OrderID = OrderID;
            CustomerID = CustomerId;
            EmployeeID = EmployeeId;
            this.Freight = Freight;
            this.ShipCity = ShipCity;
            this.Verified = Verified;
            this.OrderDate = OrderDate;
            this.ShipName = ShipName;
            this.ShipCountry = ShipCountry;
            this.ShippedDate = ShippedDate;
            this.ShipAddress = ShipAddress;
        }
        public static List<Orders> GetAllRecords()
        {
            List<Orders> orders = new List<Orders>();
            int code = 10000;
            for (int i = 1; i < 15; i++)
            {
                orders.Add(new Orders(code + 1, "ALFKI", i + 0, 2.32m * i, false, new DateTime(1991, 05, 15), "Berlin", "Simons bistro", "Denmark", new DateTime(1996, 7, 16), "Kirchgasse 6"));
                orders.Add(new Orders(code + 2, "ANATR", i + 2, 3.32m * i, true, new DateTime(1990, 04, 04), "Madrid", "Queen Cozinha", "Brazil", new DateTime(1996, 9, 11), "Avda. Azteca 123"));
                orders.Add(new Orders(code + 3, "ANTON", i + 1, 4.35m * i, true, new DateTime(1957, 11, 30), "Cholchester", "Frankenversand", "Germany", new DateTime(1996, 10, 7), "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"));
                orders.Add(new Orders(code + 4, "BLONP", i + 3, 5.38m * i, false, new DateTime(1930, 10, 22), "Marseille", "Ernst Handel", "Austria", new DateTime(1996, 12, 30), "Magazinweg 7"));
                orders.Add(new Orders(code + 5, "BOLID", i + 4, 6.35m * i, true, new DateTime(1953, 02, 18), "Tsawassen", "Hanari Carnes", "Switzerland", new DateTime(1997, 12, 3), "1029 - 12th Ave. S."));
                code += 5;
            }
            return orders;
        }

        public int? OrderID { get; set; }
        public string CustomerID { get; set; } = string.Empty;
        public int? EmployeeID { get; set; }
        public decimal? Freight { get; set; }
        public string ShipCity { get; set; } = string.Empty;
        public bool Verified { get; set; }
        public DateTime OrderDate { get; set; }

        public string ShipName { get; set; } = string.Empty;

        public string ShipCountry { get; set; } = string.Empty;

        public DateTime ShippedDate { get; set; }
        public string ShipAddress { get; set; } = string.Empty;
    }

}
