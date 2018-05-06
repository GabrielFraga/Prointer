namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteraçãoNaClasseData : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vaga", "HoraEntrada", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.Vaga", "HoraSaida", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vaga", "HoraSaida", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Vaga", "HoraEntrada", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
    }
}
