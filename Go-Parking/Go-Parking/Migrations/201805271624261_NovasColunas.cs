namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NovasColunas : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reserva", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reserva", "VagaId", "dbo.Vaga");
            DropForeignKey("dbo.Reserva", "VeiculoId", "dbo.Veiculo");
            DropIndex("dbo.Reserva", new[] { "UserId" });
            DropIndex("dbo.Reserva", new[] { "VagaId" });
            DropIndex("dbo.Reserva", new[] { "VeiculoId" });
            AddColumn("dbo.Veiculo", "Porte", c => c.String());
            AddColumn("dbo.Vaga", "Porte", c => c.String());
            AlterColumn("dbo.Reserva", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reserva", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.Vaga", "Porte");
            DropColumn("dbo.Veiculo", "Porte");
            CreateIndex("dbo.Reserva", "VeiculoId");
            CreateIndex("dbo.Reserva", "VagaId");
            CreateIndex("dbo.Reserva", "UserId");
            AddForeignKey("dbo.Reserva", "VeiculoId", "dbo.Veiculo", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Reserva", "VagaId", "dbo.Vaga", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Reserva", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
