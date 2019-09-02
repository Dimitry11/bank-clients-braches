namespace _33_Clien_brach_onWeb.Models
{
    using System;

    public partial class Client
    {
        public int id { get; set; }
        public string name { get; set; }
        public string card_number { get; set; } 
        public Guid number_acc { get; set; } = Guid.NewGuid();
        public DateTime date_register { get; set; } = DateTime.Now;
    }
}
