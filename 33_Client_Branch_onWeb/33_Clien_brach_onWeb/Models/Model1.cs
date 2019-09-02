namespace _33_Clien_brach_onWeb.Models
{
    using System.Data.Entity;

    public partial class Model1 : DbContext
    {
        public Model1() : base("name=Model11") { }

        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<m2m_bank_client> m2m_bank_client { get; set; }
    }
}
