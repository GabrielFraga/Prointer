namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataHoraVagas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vaga", "HoraEntrada", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Vaga", "HoraSaida", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vaga", "HoraSaida");
            DropColumn("dbo.Vaga", "HoraEntrada");
        }
    }
}
