namespace _33_Clien_brach_onWeb.Models
{
    using System;

    public partial class Bank
    {
        public int id { get; set; }
        public string name { get; set; }
        public Guid? bank_acc { get; set; } = Guid.NewGuid();
        public int? branch_number { get; set; }
    }
}
